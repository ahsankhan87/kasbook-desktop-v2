using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Transactions;
using POS.BLL;
using POS.Core;

namespace POS.BLL.FixedAssets
{
    public class DepreciationEngine
    {
        private readonly FixedAssetBLL _assetBll;
        private readonly JournalsBLL _journalsBll;

        public DepreciationEngine()
        {
            _assetBll = new FixedAssetBLL();
            _journalsBll = new JournalsBLL();
        }

        public decimal CalculateStraightLine(FixedAssetModel asset, DateTime periodDate)
        {
            ValidateAsset(asset);
            DateTime normalizedPeriod = FixedAssetBLL.NormalizePeriodDate(periodDate);
            if (normalizedPeriod < asset.PurchaseDate.Date)
            {
                return 0m;
            }

            decimal depreciableBase = Math.Max(0m, asset.Cost - asset.ResidualValue);
            if (depreciableBase <= 0m || asset.UsefulLifeMonths <= 0)
            {
                return 0m;
            }

            decimal monthlyAmount = RoundMoney(depreciableBase / asset.UsefulLifeMonths);
            decimal remaining = GetRemainingDepreciableAmount(asset);
            if (remaining <= 0m)
            {
                return 0m;
            }

            return Math.Min(monthlyAmount, remaining);
        }

        public decimal CalculateReducingBalance(FixedAssetModel asset, DateTime periodDate)
        {
            ValidateAsset(asset);
            DateTime normalizedPeriod = FixedAssetBLL.NormalizePeriodDate(periodDate);
            if (normalizedPeriod < asset.PurchaseDate.Date)
            {
                return 0m;
            }

            decimal currentWdv = GetCurrentWdv(asset);
            decimal residual = Math.Max(0m, asset.ResidualValue);
            if (currentWdv <= residual || asset.DepRate <= 0m)
            {
                return 0m;
            }

            decimal rate = asset.DepRate / 100m;
            decimal monthlyAmount = RoundMoney(currentWdv * (rate / 12m));
            decimal maxAllowable = currentWdv - residual;
            return Math.Min(monthlyAmount, Math.Max(0m, maxAllowable));
        }

        public decimal CalculateProRata(FixedAssetModel asset, DateTime acquisitionDate, DateTime periodDate)
        {
            ValidateAsset(asset);
            DateTime normalizedPeriod = FixedAssetBLL.NormalizePeriodDate(periodDate);
            DateTime normalizedAcquisition = acquisitionDate.Date;
            if (normalizedPeriod < normalizedAcquisition)
            {
                return 0m;
            }

            int monthsSinceAcquisition = ((normalizedPeriod.Year - normalizedAcquisition.Year) * 12) + (normalizedPeriod.Month - normalizedAcquisition.Month);
            if (monthsSinceAcquisition < 0 || monthsSinceAcquisition >= asset.UsefulLifeMonths)
            {
                return 0m;
            }

            decimal depreciableBase = Math.Max(0m, asset.Cost - asset.ResidualValue);
            if (depreciableBase <= 0m || asset.UsefulLifeMonths <= 0)
            {
                return 0m;
            }

            decimal monthlyAmount = RoundMoney(depreciableBase / asset.UsefulLifeMonths);
            decimal depAmount;

            if (monthsSinceAcquisition == 0)
            {
                int daysInMonth = DateTime.DaysInMonth(normalizedAcquisition.Year, normalizedAcquisition.Month);
                int heldDays = (normalizedPeriod.Day - normalizedAcquisition.Day) + 1;
                heldDays = Math.Max(0, Math.Min(daysInMonth, heldDays));
                depAmount = RoundMoney(monthlyAmount * heldDays / daysInMonth);
            }
            else
            {
                depAmount = monthlyAmount;
            }

            decimal remaining = GetRemainingDepreciableAmount(asset);
            if (remaining <= 0m)
            {
                return 0m;
            }

            return Math.Min(depAmount, remaining);
        }

        public List<DepScheduleLine> GenerateDepreciationSchedule(FixedAssetModel asset)
        {
            ValidateAsset(asset);

            List<DepScheduleLine> schedule = new List<DepScheduleLine>();
            DateTime firstPeriod = new DateTime(asset.PurchaseDate.Year, asset.PurchaseDate.Month, 1);
            decimal openingWdv = asset.Cost;
            decimal accumulatedDep = 0m;

            for (int monthIndex = 0; monthIndex < asset.UsefulLifeMonths; monthIndex++)
            {
                DateTime periodDate = FixedAssetBLL.NormalizePeriodDate(firstPeriod.AddMonths(monthIndex));
                decimal depAmount = CalculateScheduledDepreciation(asset, asset.PurchaseDate, periodDate, openingWdv, accumulatedDep);
                if (depAmount <= 0m && monthIndex > 0)
                {
                    break;
                }

                decimal closingWdv = Math.Max(asset.ResidualValue, RoundMoney(openingWdv - depAmount));
                accumulatedDep = RoundMoney(accumulatedDep + depAmount);

                schedule.Add(new DepScheduleLine
                {
                    AssetId = asset.AssetId,
                    AssetCode = asset.AssetCode,
                    AssetName = asset.AssetName,
                    DepMethod = NormalizeMethod(asset.DepMethod),
                    PeriodDate = periodDate,
                    OpeningWDV = RoundMoney(openingWdv),
                    DepreciationAmount = depAmount,
                    AccumulatedDepreciation = accumulatedDep,
                    ClosingWDV = closingWdv
                });

                openingWdv = closingWdv;
                if (openingWdv <= asset.ResidualValue)
                {
                    break;
                }
            }

            return schedule;
        }

        public List<DepScheduleLine> BuildMonthlyPreview(DateTime periodDate)
        {
            DateTime normalizedPeriod = FixedAssetBLL.NormalizePeriodDate(periodDate);
            List<FixedAssetModel> assets = _assetBll.GetEligibleAssetsForDepreciation(normalizedPeriod);
            List<DepScheduleLine> preview = new List<DepScheduleLine>();

            foreach (FixedAssetModel asset in assets)
            {
                decimal openingWdv = GetCurrentWdv(asset);
                decimal depAmount = CalculateAssetDepreciation(asset, normalizedPeriod);
                decimal closingWdv = Math.Max(asset.ResidualValue, RoundMoney(openingWdv - depAmount));

                preview.Add(new DepScheduleLine
                {
                    AssetId = asset.AssetId,
                    AssetCode = asset.AssetCode,
                    AssetName = asset.AssetName,
                    DepMethod = NormalizeMethod(asset.DepMethod),
                    PeriodDate = normalizedPeriod,
                    OpeningWDV = RoundMoney(openingWdv),
                    DepreciationAmount = depAmount,
                    AccumulatedDepreciation = RoundMoney(asset.AccumulatedDepreciation + depAmount),
                    ClosingWDV = closingWdv
                });
            }

            return preview;
        }

        public DepreciationRunSummary RunMonthlyDepreciation(DateTime period, int userId)
        {
            DateTime normalizedPeriod = FixedAssetBLL.NormalizePeriodDate(period);
            List<FixedAssetModel> assets = _assetBll.GetEligibleAssetsForDepreciation(normalizedPeriod);
            DepreciationRunSummary summary = new DepreciationRunSummary
            {
                PeriodDate = normalizedPeriod,
                EvaluatedAssets = assets.Count
            };

            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadCommitted
                }))
                {
                    foreach (FixedAssetModel asset in assets)
                    {
                        if (_assetBll.DepreciationRunExists(asset.AssetId, normalizedPeriod))
                        {
                            summary.SkippedCount++;
                            summary.Messages.Add(string.Format(CultureInfo.InvariantCulture, "Asset {0} skipped: depreciation already run for {1:yyyy-MM}.", asset.AssetCode, normalizedPeriod));
                            continue;
                        }

                        decimal openingWdv = GetCurrentWdv(asset);
                        decimal depAmount = CalculateAssetDepreciation(asset, normalizedPeriod);
                        if (depAmount <= 0m)
                        {
                            summary.SkippedCount++;
                            summary.Messages.Add(string.Format(CultureInfo.InvariantCulture, "Asset {0} skipped: no depreciation due for {1:yyyy-MM}.", asset.AssetCode, normalizedPeriod));
                            continue;
                        }

                        decimal closingWdv = Math.Max(asset.ResidualValue, RoundMoney(openingWdv - depAmount));
                        AutoJVModel autoJournal = CreateAutoJournal(asset, depAmount, normalizedPeriod);
                        PostResult journalResult = _journalsBll.PostAutoJournalEntry(autoJournal, userId);
                        if (journalResult == null || !journalResult.Success || journalResult.EntryIds.Count == 0)
                        {
                            string journalError = GetPostResultError(journalResult);
                            throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Failed to post journal for asset {0}: {1}", asset.AssetCode, journalError));
                        }

                        int runId = _assetBll.InsertDepreciationRun(asset.AssetId, normalizedPeriod, openingWdv, depAmount, closingWdv, journalResult.VoucherId, userId, DateTime.Now);
                        if (runId <= 0)
                        {
                            throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Failed to insert depreciation run for asset {0}.", asset.AssetCode));
                        }

                        decimal newAccumulated = RoundMoney(asset.AccumulatedDepreciation + depAmount);
                        string newStatus = closingWdv <= asset.ResidualValue ? "Fully Depreciated" : asset.Status;
                        _assetBll.UpdateAssetDepreciationState(asset.AssetId, newAccumulated, closingWdv, normalizedPeriod, newStatus);

                        summary.PostedCount++;
                        summary.TotalDepreciation = RoundMoney(summary.TotalDepreciation + depAmount);
                        summary.PostedLines.Add(new DepScheduleLine
                        {
                            AssetId = asset.AssetId,
                            AssetCode = asset.AssetCode,
                            AssetName = asset.AssetName,
                            DepMethod = NormalizeMethod(asset.DepMethod),
                            PeriodDate = normalizedPeriod,
                            OpeningWDV = RoundMoney(openingWdv),
                            DepreciationAmount = depAmount,
                            AccumulatedDepreciation = newAccumulated,
                            ClosingWDV = closingWdv
                        });
                    }

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                summary.PostedCount = 0;
                summary.SkippedCount = 0;
                summary.TotalDepreciation = 0m;
                summary.PostedLines.Clear();
                summary.ErrorCount++;
                summary.Messages.Add(ex.Message);
            }

            return summary;
        }

        private decimal CalculateAssetDepreciation(FixedAssetModel asset, DateTime periodDate)
        {
            string method = NormalizeMethod(asset.DepMethod);
            if (method == "REDUCING_BALANCE" || method == "REDUCING" || method == "DB")
            {
                return CalculateReducingBalance(asset, periodDate);
            }

            if (method == "PRO_RATA" || method == "PRORATA" || method == "PRO-RATA")
            {
                return CalculateProRata(asset, asset.PurchaseDate, periodDate);
            }

            return CalculateStraightLine(asset, periodDate);
        }

        private decimal CalculateScheduledDepreciation(FixedAssetModel asset, DateTime acquisitionDate, DateTime periodDate, decimal openingWdv, decimal accumulatedDepreciation)
        {
            string method = NormalizeMethod(asset.DepMethod);
            decimal remaining = Math.Max(0m, asset.Cost - asset.ResidualValue - accumulatedDepreciation);
            if (remaining <= 0m || openingWdv <= asset.ResidualValue)
            {
                return 0m;
            }

            if (method == "REDUCING_BALANCE" || method == "REDUCING" || method == "DB")
            {
                decimal rate = asset.DepRate / 100m;
                decimal dep = RoundMoney(openingWdv * (rate / 12m));
                return Math.Min(dep, remaining);
            }

            if (method == "PRO_RATA" || method == "PRORATA" || method == "PRO-RATA")
            {
                return CalculateProRataForSchedule(asset, acquisitionDate, periodDate, openingWdv, remaining);
            }

            decimal monthly = RoundMoney(Math.Max(0m, asset.Cost - asset.ResidualValue) / asset.UsefulLifeMonths);
            return Math.Min(monthly, remaining);
        }

        private decimal CalculateProRataForSchedule(FixedAssetModel asset, DateTime acquisitionDate, DateTime periodDate, decimal openingWdv, decimal remaining)
        {
            DateTime normalizedAcquisition = acquisitionDate.Date;
            DateTime normalizedPeriod = periodDate.Date;
            if (normalizedPeriod < normalizedAcquisition)
            {
                return 0m;
            }

            int monthsSinceAcquisition = ((normalizedPeriod.Year - normalizedAcquisition.Year) * 12) + (normalizedPeriod.Month - normalizedAcquisition.Month);
            if (monthsSinceAcquisition >= asset.UsefulLifeMonths)
            {
                return 0m;
            }

            decimal monthly = RoundMoney(Math.Max(0m, asset.Cost - asset.ResidualValue) / asset.UsefulLifeMonths);
            decimal amount;
            if (monthsSinceAcquisition == 0)
            {
                int daysInMonth = DateTime.DaysInMonth(normalizedAcquisition.Year, normalizedAcquisition.Month);
                int heldDays = (DateTime.DaysInMonth(normalizedPeriod.Year, normalizedPeriod.Month) - normalizedAcquisition.Day) + 1;
                heldDays = Math.Max(0, Math.Min(daysInMonth, heldDays));
                amount = RoundMoney(monthly * heldDays / daysInMonth);
            }
            else
            {
                amount = monthly;
            }

            decimal maxAllowed = Math.Min(remaining, Math.Max(0m, openingWdv - asset.ResidualValue));
            return Math.Min(amount, maxAllowed);
        }

        private AutoJVModel CreateAutoJournal(FixedAssetModel asset, decimal depAmount, DateTime periodDate)
        {
            if (asset.DepAccountId <= 0 || asset.AccumDepAccountId <= 0)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Asset {0} is missing depreciation account mapping.", asset.AssetCode));
            }

            string narration = string.Format(CultureInfo.InvariantCulture, "Depreciation for {0} - {1:yyyy-MM}", asset.AssetCode, periodDate);
            AutoJVModel model = new AutoJVModel
            {
                ModuleName = "FixedAssets",
                RefModule = "FA_DEPRECIATION",
                RefId = asset.AssetId,
                VoucherDate = periodDate,
                ReferenceNo = asset.AssetCode,
                Narration = narration,
                IsAutoPosted = true
            };

            model.Lines.Add(new JVLineModel
            {
                LineNo = 1,
                AccountId = asset.DepAccountId,
                Debit = depAmount,
                Credit = 0m,
                Narration = narration,
                ModuleName = "FixedAssets",
                RefId = asset.AssetId
            });

            model.Lines.Add(new JVLineModel
            {
                LineNo = 2,
                AccountId = asset.AccumDepAccountId,
                Debit = 0m,
                Credit = depAmount,
                Narration = narration,
                ModuleName = "FixedAssets",
                RefId = asset.AssetId
            });

            return model;
        }

        private static void ValidateAsset(FixedAssetModel asset)
        {
            if (asset == null)
            {
                throw new ArgumentNullException("asset");
            }

            if (asset.Cost < 0m)
            {
                throw new ArgumentException("Asset cost cannot be negative.", "asset");
            }

            if (asset.UsefulLifeMonths <= 0)
            {
                throw new ArgumentException("Useful life months must be greater than zero.", "asset");
            }
        }

        private static decimal GetCurrentWdv(FixedAssetModel asset)
        {
            decimal wdv = asset.CurrentWDV > 0m ? asset.CurrentWDV : asset.Cost - asset.AccumulatedDepreciation;
            return RoundMoney(Math.Max(asset.ResidualValue, wdv));
        }

        private static decimal GetRemainingDepreciableAmount(FixedAssetModel asset)
        {
            decimal recoverableBase = Math.Max(0m, asset.Cost - asset.ResidualValue);
            decimal remaining = recoverableBase - asset.AccumulatedDepreciation;
            return RoundMoney(Math.Max(0m, remaining));
        }

        private static decimal RoundMoney(decimal value)
        {
            return Math.Round(value, 2, MidpointRounding.AwayFromZero);
        }

        private static string NormalizeMethod(string method)
        {
            return string.IsNullOrWhiteSpace(method) ? "STRAIGHT_LINE" : method.Trim().ToUpperInvariant().Replace(' ', '_');
        }

        private static string GetPostResultError(PostResult result)
        {
            if (result == null || result.Messages == null || result.Messages.Count == 0)
            {
                return "Unknown journal posting error.";
            }

            List<string> errors = new List<string>();
            foreach (ValidationError error in result.Messages)
            {
                if (error != null)
                {
                    errors.Add(error.Message);
                }
            }

            return errors.Count == 0 ? "Unknown journal posting error." : string.Join("; ", errors);
        }
    }
}
