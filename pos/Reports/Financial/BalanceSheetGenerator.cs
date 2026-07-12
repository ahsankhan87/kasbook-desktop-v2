using POS.BLL;
using System;
using System.Data;
using System.Linq;

namespace pos.Reports.Financial
{
    public static class BalanceSheetGenerator
    {
        public static BalanceSheetReportModel Build(AccountsBLL accountsBll, DateTime asOfDate, DateTime? comparisonDate, bool detailed, int branchId)
        {
            if (accountsBll == null)
            {
                throw new ArgumentNullException(nameof(accountsBll));
            }

            BalanceSheetReportModel model = new BalanceSheetReportModel
            {
                AsOfDate = asOfDate.Date,
                ComparisonDate = comparisonDate
            };

            DateTime fiscalStart = new DateTime(asOfDate.Year, 1, 1);
            DateTime priorFiscalEnd = fiscalStart.AddDays(-1);

            decimal comparisonCash = comparisonDate.HasValue ? GetBalance(accountsBll, comparisonDate.Value, "cash") : 0m;
            decimal comparisonBank = comparisonDate.HasValue ? GetBalance(accountsBll, comparisonDate.Value, "bank") : 0m;
            decimal comparisonAr = comparisonDate.HasValue ? GetBalance(accountsBll, comparisonDate.Value, "accounts receivable", "trade receivable", "debtors") : 0m;
            decimal comparisonInventory = comparisonDate.HasValue ? accountsBll.GetInventoryValueAsOf(comparisonDate.Value.Date) : 0m;
            decimal comparisonPrepaid = comparisonDate.HasValue ? GetBalance(accountsBll, comparisonDate.Value, "prepaid") : 0m;
            decimal comparisonCurrentAssets = comparisonCash + comparisonBank + comparisonAr + comparisonInventory + comparisonPrepaid;

            decimal comparisonPpe = comparisonDate.HasValue ? GetBalance(accountsBll, comparisonDate.Value, "property", "plant", "equipment", "machinery", "furniture", "fixed asset") : 0m;
            decimal comparisonAccumDep = comparisonDate.HasValue ? GetBalance(accountsBll, comparisonDate.Value, "accumulated depreciation", "depreciation") : 0m;
            decimal comparisonNetFixedAssets = comparisonPpe + comparisonAccumDep;
            decimal comparisonTotalAssets = comparisonCurrentAssets + comparisonNetFixedAssets;

            decimal comparisonAp = comparisonDate.HasValue ? GetBalance(accountsBll, comparisonDate.Value, "accounts payable", "trade payable", "creditors") : 0m;
            decimal comparisonShortLoans = comparisonDate.HasValue ? GetBalance(accountsBll, comparisonDate.Value, "short term loan", "short-term loan", "current portion loan") : 0m;
            decimal comparisonTax = comparisonDate.HasValue ? GetBalance(accountsBll, comparisonDate.Value, "tax payable", "sales tax payable", "vat payable") : 0m;
            decimal comparisonCurrentLiabilities = comparisonAp + comparisonShortLoans + comparisonTax;
            decimal comparisonLongLoans = comparisonDate.HasValue ? GetBalance(accountsBll, comparisonDate.Value, "long term loan", "long-term loan", "lease liability") : 0m;
            decimal comparisonTotalLiabilities = comparisonCurrentLiabilities + comparisonLongLoans;

            decimal comparisonCapital = comparisonDate.HasValue ? GetBalance(accountsBll, comparisonDate.Value, "owner capital", "share capital", "capital", "paid in capital") : 0m;
            decimal comparisonRetained = comparisonDate.HasValue ? GetHistoricalRetainedEarnings(accountsBll, comparisonDate.Value.Date) : 0m;
            decimal comparisonNetProfit = comparisonDate.HasValue ? GetCurrentPeriodNetProfit(accountsBll, comparisonDate.Value.Date) : 0m;
            decimal comparisonTotalEquity = comparisonCapital + comparisonRetained + comparisonNetProfit;
            decimal comparisonLiabilitiesAndEquity = comparisonTotalLiabilities + comparisonTotalEquity;

            decimal cash = GetBalance(accountsBll, asOfDate, "cash");
            decimal bank = GetBalance(accountsBll, asOfDate, "bank");
            decimal accountsReceivable = GetBalance(accountsBll, asOfDate, "accounts receivable", "trade receivable", "debtors");
            decimal inventory = accountsBll.GetInventoryValueAsOf(asOfDate.Date);
            decimal prepaid = GetBalance(accountsBll, asOfDate, "prepaid");
            decimal totalCurrentAssets = cash + bank + accountsReceivable + inventory + prepaid;

            decimal propertyPlantEquipment = GetBalance(accountsBll, asOfDate, "property", "plant", "equipment", "machinery", "furniture", "fixed asset");
            decimal accumulatedDepreciation = GetBalance(accountsBll, asOfDate, "accumulated depreciation", "depreciation");
            decimal netFixedAssets = propertyPlantEquipment + accumulatedDepreciation;
            decimal totalAssets = totalCurrentAssets + netFixedAssets;

            decimal accountsPayable = GetBalance(accountsBll, asOfDate, "accounts payable", "trade payable", "creditors");
            decimal shortTermLoans = GetBalance(accountsBll, asOfDate, "short term loan", "short-term loan", "current portion loan");
            decimal taxPayable = GetBalance(accountsBll, asOfDate, "tax payable", "sales tax payable", "vat payable");
            decimal totalCurrentLiabilities = accountsPayable + shortTermLoans + taxPayable;

            decimal longTermLiabilities = GetBalance(accountsBll, asOfDate, "long term loan", "long-term loan", "lease liability");
            decimal totalLiabilities = totalCurrentLiabilities + longTermLiabilities;

            decimal ownerCapital = GetBalance(accountsBll, asOfDate, "owner capital", "share capital", "capital", "paid in capital");
            decimal retainedEarnings = GetHistoricalRetainedEarnings(accountsBll, asOfDate.Date);
            decimal currentPeriodNetProfit = GetCurrentPeriodNetProfit(accountsBll, asOfDate.Date);
            decimal totalEquity = ownerCapital + retainedEarnings + currentPeriodNetProfit;

            decimal liabilitiesAndEquityTotal = totalLiabilities + totalEquity;
            decimal difference = totalAssets - liabilitiesAndEquityTotal;

            model.AssetsTotal = totalAssets;
            model.LiabilitiesAndEquityTotal = liabilitiesAndEquityTotal;
            model.Difference = difference;

            BuildAssets(model, detailed, cash, bank, accountsReceivable, inventory, prepaid, totalCurrentAssets, propertyPlantEquipment, accumulatedDepreciation, netFixedAssets, totalAssets, comparisonCash, comparisonBank, comparisonAr, comparisonInventory, comparisonPrepaid, comparisonPpe, comparisonAccumDep, comparisonDate, comparisonCurrentAssets, comparisonNetFixedAssets, comparisonTotalAssets);
            BuildLiabilitiesAndEquity(model, detailed, accountsPayable, shortTermLoans, taxPayable, totalCurrentLiabilities, longTermLiabilities, totalLiabilities, ownerCapital, retainedEarnings, currentPeriodNetProfit, totalEquity, liabilitiesAndEquityTotal, comparisonAp, comparisonShortLoans, comparisonTax, comparisonCurrentLiabilities, comparisonLongLoans, comparisonTotalLiabilities, comparisonCapital, comparisonRetained, comparisonNetProfit, comparisonTotalEquity, comparisonLiabilitiesAndEquity);
            BuildNotes(model, asOfDate, fiscalStart, priorFiscalEnd, cash, bank, accountsReceivable, inventory, prepaid, propertyPlantEquipment, accumulatedDepreciation, accountsPayable, shortTermLoans, taxPayable, longTermLiabilities, ownerCapital, retainedEarnings, currentPeriodNetProfit);
            BuildDiagnostics(model, comparisonDate, cash, bank, accountsReceivable, inventory, prepaid, totalCurrentAssets, totalAssets, accountsPayable, shortTermLoans, taxPayable, totalCurrentLiabilities, longTermLiabilities, totalLiabilities, ownerCapital, retainedEarnings, currentPeriodNetProfit, totalEquity);

            return model;
        }

        private static void BuildAssets(BalanceSheetReportModel model, bool detailed, decimal cash, decimal bank, decimal accountsReceivable, decimal inventory, decimal prepaid, decimal totalCurrentAssets, decimal ppe, decimal accumulatedDepreciation, decimal netFixedAssets, decimal totalAssets, decimal comparisonCash, decimal comparisonBank, decimal comparisonAr, decimal comparisonInventory, decimal comparisonPrepaid, decimal comparisonPpe, decimal comparisonAccumDep, DateTime? comparisonDate, decimal comparisonCurrentAssetsTotal, decimal comparisonNetFixed, decimal comparisonTotalAssets)
        {
            AddHeading(model.Assets, "Current Assets");
            if (detailed)
            {
                AddLine(model.Assets, "Cash in Hand", cash, comparisonDate.HasValue ? comparisonCash : (decimal?)null, "All cash accounts", false, false, 1);
                AddLine(model.Assets, "Bank Accounts", bank, comparisonDate.HasValue ? comparisonBank : (decimal?)null, "All bank accounts", false, false, 1);
                AddLine(model.Assets, "Accounts Receivable", accountsReceivable, comparisonDate.HasValue ? comparisonAr : (decimal?)null, "Customer sub-ledger total", false, false, 1);
                AddLine(model.Assets, "Inventory / Stock", inventory, comparisonDate.HasValue ? comparisonInventory : (decimal?)null, "As-of stock valuation", false, false, 1);
                AddLine(model.Assets, "Prepaid Expenses", prepaid, comparisonDate.HasValue ? comparisonPrepaid : (decimal?)null, "Deferred expense balances", false, false, 1);
            }
            AddLine(model.Assets, "Total Current Assets", totalCurrentAssets, comparisonDate.HasValue ? comparisonCurrentAssetsTotal : (decimal?)null, string.Empty, true, false, 0);

            AddHeading(model.Assets, "Fixed Assets");
            if (detailed)
            {
                AddLine(model.Assets, "Property, Plant & Equipment", ppe, comparisonDate.HasValue ? comparisonPpe : (decimal?)null, "Gross PPE balances", false, false, 1);
                AddLine(model.Assets, "Less: Accumulated Depreciation", accumulatedDepreciation, comparisonDate.HasValue ? comparisonAccumDep : (decimal?)null, "Contra-asset balance", false, false, 1);
            }
            AddLine(model.Assets, "Net Fixed Assets", netFixedAssets, comparisonDate.HasValue ? comparisonNetFixed : (decimal?)null, string.Empty, true, false, 0);
            AddLine(model.Assets, "TOTAL ASSETS", totalAssets, comparisonDate.HasValue ? comparisonTotalAssets : (decimal?)null, string.Empty, true, false, 0);
        }

        private static void BuildLiabilitiesAndEquity(BalanceSheetReportModel model, bool detailed, decimal accountsPayable, decimal shortTermLoans, decimal taxPayable, decimal totalCurrentLiabilities, decimal longTermLiabilities, decimal totalLiabilities, decimal ownerCapital, decimal retainedEarnings, decimal currentPeriodNetProfit, decimal totalEquity, decimal liabilitiesAndEquityTotal, decimal comparisonAp, decimal comparisonShortLoans, decimal comparisonTax, decimal comparisonCurrentLiabilities, decimal comparisonLongTermLiabilities, decimal comparisonTotalLiabilities, decimal comparisonCapital, decimal comparisonRetained, decimal comparisonNetProfit, decimal comparisonTotalEquity, decimal comparisonLiabilitiesAndEquity)
        {
            AddHeading(model.LiabilitiesAndEquity, "Current Liabilities");
            if (detailed)
            {
                AddLine(model.LiabilitiesAndEquity, "Accounts Payable", accountsPayable, model.ComparisonDate.HasValue ? comparisonAp : (decimal?)null, "Supplier sub-ledger total", false, false, 1);
                AddLine(model.LiabilitiesAndEquity, "Short-term Loans", shortTermLoans, model.ComparisonDate.HasValue ? comparisonShortLoans : (decimal?)null, "Current portion of borrowings", false, false, 1);
                AddLine(model.LiabilitiesAndEquity, "Tax Payable", taxPayable, model.ComparisonDate.HasValue ? comparisonTax : (decimal?)null, "Sales/VAT and income tax payables", false, false, 1);
            }
            AddLine(model.LiabilitiesAndEquity, "Total Current Liabilities", totalCurrentLiabilities, model.ComparisonDate.HasValue ? comparisonCurrentLiabilities : (decimal?)null, string.Empty, true, false, 0);

            AddHeading(model.LiabilitiesAndEquity, "Long-term Liabilities");
            if (detailed)
            {
                AddLine(model.LiabilitiesAndEquity, "Long-term Liabilities", longTermLiabilities, model.ComparisonDate.HasValue ? comparisonLongTermLiabilities : (decimal?)null, "Loans and obligations due after 12 months", false, false, 1);
            }
            AddLine(model.LiabilitiesAndEquity, "Total Liabilities", totalLiabilities, model.ComparisonDate.HasValue ? comparisonTotalLiabilities : (decimal?)null, string.Empty, true, false, 0);

            AddHeading(model.LiabilitiesAndEquity, "Equity");
            if (detailed)
            {
                AddLine(model.LiabilitiesAndEquity, "Owner's Capital / Share Capital", ownerCapital, model.ComparisonDate.HasValue ? comparisonCapital : (decimal?)null, "Paid-in capital balances", false, false, 1);
                AddLine(model.LiabilitiesAndEquity, "Retained Earnings", retainedEarnings, model.ComparisonDate.HasValue ? comparisonRetained : (decimal?)null, "Cumulative historical P&L before current year", false, false, 1);
                AddLine(model.LiabilitiesAndEquity, "Current Period Net Profit", currentPeriodNetProfit, model.ComparisonDate.HasValue ? comparisonNetProfit : (decimal?)null, "Pulled from Profit & Loss", false, false, 1);
            }
            AddLine(model.LiabilitiesAndEquity, "Total Equity", totalEquity, model.ComparisonDate.HasValue ? comparisonTotalEquity : (decimal?)null, string.Empty, true, false, 0);
            AddLine(model.LiabilitiesAndEquity, "TOTAL LIABILITIES + EQUITY", liabilitiesAndEquityTotal, model.ComparisonDate.HasValue ? comparisonLiabilitiesAndEquity : (decimal?)null, string.Empty, true, false, 0);
        }

        private static void BuildNotes(BalanceSheetReportModel model, DateTime asOfDate, DateTime fiscalStart, DateTime priorFiscalEnd, decimal cash, decimal bank, decimal accountsReceivable, decimal inventory, decimal prepaid, decimal ppe, decimal accumulatedDepreciation, decimal accountsPayable, decimal shortTermLoans, decimal taxPayable, decimal longTermLiabilities, decimal ownerCapital, decimal retainedEarnings, decimal currentPeriodNetProfit)
        {
            model.Notes.Add(new BalanceNote { Title = "As-of Date", Summary = asOfDate.ToString("dd-MMM-yyyy"), Details = { "Balance sheet is a point-in-time statement." } });
            model.Notes.Add(new BalanceNote { Title = "Inventory Valuation", Summary = inventory.ToString("N2"), Details = { "Fetched from stock tables using item quantities multiplied by average cost.", "Method: pos_inventory joined to pos_products.avg_cost." } });
            model.Notes.Add(new BalanceNote { Title = "Retained Earnings", Summary = retainedEarnings.ToString("N2"), Details = { "Calculated as cumulative historical P&L before the current fiscal year.", "Historical P&L window: " + DateTime.MinValue.ToString("yyyy") + " to " + priorFiscalEnd.ToString("dd-MMM-yyyy") } });
            model.Notes.Add(new BalanceNote { Title = "Current Period Net Profit", Summary = currentPeriodNetProfit.ToString("N2"), Details = { "Pulled automatically from Profit & Loss.", "Current fiscal year window: " + fiscalStart.ToString("dd-MMM-yyyy") + " to " + asOfDate.ToString("dd-MMM-yyyy") } });
            model.Notes.Add(new BalanceNote { Title = "Key Balances", Summary = string.Format("Cash {0:N2} | Bank {1:N2} | AR {2:N2} | AP {3:N2}", cash, bank, accountsReceivable, accountsPayable), Details = { "Prepaid: " + prepaid.ToString("N2"), "PPE: " + ppe.ToString("N2"), "Accumulated Depreciation: " + accumulatedDepreciation.ToString("N2"), "Short-term loans: " + shortTermLoans.ToString("N2"), "Tax payable: " + taxPayable.ToString("N2"), "Long-term liabilities: " + longTermLiabilities.ToString("N2"), "Owner capital: " + ownerCapital.ToString("N2") } });
        }

        private static void BuildDiagnostics(BalanceSheetReportModel model, DateTime? comparisonDate, decimal cash, decimal bank, decimal accountsReceivable, decimal inventory, decimal prepaid, decimal totalCurrentAssets, decimal totalAssets, decimal accountsPayable, decimal shortTermLoans, decimal taxPayable, decimal totalCurrentLiabilities, decimal longTermLiabilities, decimal totalLiabilities, decimal ownerCapital, decimal retainedEarnings, decimal currentPeriodNetProfit, decimal totalEquity)
        {
            AddDiagnostic(model, "Current Assets", totalCurrentAssets, comparisonDate.HasValue ? totalCurrentAssets : 0m, "Asset subtotal check");
            AddDiagnostic(model, "Total Assets", totalAssets, comparisonDate.HasValue ? totalAssets : 0m, "Final asset total");
            AddDiagnostic(model, "Current Liabilities", totalCurrentLiabilities, comparisonDate.HasValue ? totalCurrentLiabilities : 0m, "Liability subtotal check");
            AddDiagnostic(model, "Total Liabilities", totalLiabilities, comparisonDate.HasValue ? totalLiabilities : 0m, "Liability total");
            AddDiagnostic(model, "Total Equity", totalEquity, comparisonDate.HasValue ? totalEquity : 0m, "Equity total");
            AddDiagnostic(model, "AR / AP Sanity", accountsReceivable, accountsPayable, "Comparative control accounts");
            AddDiagnostic(model, "Cash vs Bank", cash, bank, "Liquidity split");
            AddDiagnostic(model, "Inventory vs Prepaids", inventory, prepaid, "Working capital split");
        }

        private static void AddDiagnostic(BalanceSheetReportModel model, string groupName, decimal currentAmount, decimal comparisonAmount, string note)
        {
            DataRow row = model.Diagnostics.NewRow();
            row["GroupName"] = groupName;
            row["CurrentAmount"] = currentAmount;
            row["ComparisonAmount"] = comparisonAmount;
            row["Difference"] = currentAmount - comparisonAmount;
            row["Notes"] = note;
            model.Diagnostics.Rows.Add(row);
        }

        private static decimal GetBalance(AccountsBLL accountsBll, DateTime asOfDate, params string[] patterns)
        {
            decimal value = accountsBll.GetBalanceByAccountNamePatternsAsOf(asOfDate.Date, patterns);
            return value;
        }

        private static decimal GetHistoricalRetainedEarnings(AccountsBLL accountsBll, DateTime asOfDate)
        {
            DateTime fiscalStart = new DateTime(asOfDate.Year, 1, 1);
            DateTime historyEnd = fiscalStart.AddDays(-1);
            if (historyEnd.Year < 1900)
            {
                return 0m;
            }

            return accountsBll.GetNetProfitBetween(new DateTime(1900, 1, 1), historyEnd);
        }

        private static decimal GetCurrentPeriodNetProfit(AccountsBLL accountsBll, DateTime asOfDate)
        {
            DateTime fiscalStart = new DateTime(asOfDate.Year, 1, 1);
            return accountsBll.GetNetProfitBetween(fiscalStart, asOfDate.Date);
        }

        private static void AddHeading(DataTable table, string title)
        {
            DataRow row = table.NewRow();
            row["LineItem"] = title;
            row["Amount"] = DBNull.Value;
            row["Comparison"] = DBNull.Value;
            row["Notes"] = string.Empty;
            row["IsTotal"] = false;
            row["IsHeading"] = true;
            row["IndentLevel"] = 0;
            table.Rows.Add(row);
        }

        private static void AddLine(DataTable table, string title, decimal amount, decimal? comparisonAmount, string notes, bool isTotal, bool isHeading, int indentLevel)
        {
            DataRow row = table.NewRow();
            row["LineItem"] = new string(' ', Math.Max(0, indentLevel * 2)) + title;
            row["Amount"] = amount;
            row["Comparison"] = comparisonAmount.HasValue ? (object)comparisonAmount.Value : DBNull.Value;
            row["Notes"] = notes ?? string.Empty;
            row["IsTotal"] = isTotal;
            row["IsHeading"] = isHeading;
            row["IndentLevel"] = indentLevel;
            table.Rows.Add(row);
        }
    }
}
