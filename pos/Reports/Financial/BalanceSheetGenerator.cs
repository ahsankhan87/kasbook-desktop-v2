using POS.BLL;
using System;
using System.Data;
using System.Linq;
using pos.UI;

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
            decimal comparisonInventory = comparisonDate.HasValue ? GetBalance(accountsBll, asOfDate, "inventory asset", "stocks", "stock", "merchandise")  : 0m;
            decimal comparisonPrepaid = comparisonDate.HasValue ? GetBalance(accountsBll, comparisonDate.Value, "prepaid") : 0m;
            decimal comparisonCurrentAssets = comparisonCash + comparisonBank + comparisonAr + comparisonInventory + comparisonPrepaid;

            decimal comparisonPpe = comparisonDate.HasValue ? GetBalance(accountsBll, comparisonDate.Value, "property", "plant", "equipment", "machinery", "furniture", "fixed asset") : 0m;
            decimal comparisonAccumDep = comparisonDate.HasValue ? GetBalance(accountsBll, comparisonDate.Value, "accumulated depreciation", "depreciation") : 0m;
            decimal comparisonNetFixedAssets = comparisonPpe + comparisonAccumDep;
            decimal comparisonTotalAssets = comparisonCurrentAssets + comparisonNetFixedAssets;

            decimal comparisonAp = comparisonDate.HasValue ? GetBalanceWithType(accountsBll, comparisonDate.Value, "LIABILITY", "accounts payable", "trade payable", "creditors") : 0m;
            decimal comparisonShortLoans = comparisonDate.HasValue ? GetBalanceWithType(accountsBll, comparisonDate.Value, "LIABILITY", "short term loan", "short-term loan", "current portion loan") : 0m;
            decimal comparisonTax = comparisonDate.HasValue ? GetBalanceWithType(accountsBll, comparisonDate.Value, "LIABILITY", "tax payable", "sales tax payable", "vat payable") : 0m;
            decimal comparisonCurrentLiabilities = comparisonAp + comparisonShortLoans + comparisonTax;
            decimal comparisonLongLoans = comparisonDate.HasValue ? GetBalanceWithType(accountsBll, comparisonDate.Value, "LIABILITY", "long term loan", "long-term loan", "lease liability") : 0m;
            decimal comparisonTotalLiabilities = comparisonCurrentLiabilities + comparisonLongLoans;

            decimal comparisonCapital = comparisonDate.HasValue ? GetBalanceWithType(accountsBll, comparisonDate.Value, "EQUITY", "owner capital", "share capital", "capital", "paid in capital") : 0m;
            decimal comparisonRetained = comparisonDate.HasValue ? GetHistoricalRetainedEarnings(accountsBll, comparisonDate.Value.Date) : 0m;
            decimal comparisonNetProfit = comparisonDate.HasValue ? GetCurrentPeriodNetProfit(accountsBll, comparisonDate.Value.Date) : 0m;
            decimal comparisonTotalEquity = comparisonCapital + comparisonRetained + comparisonNetProfit;
            decimal comparisonLiabilitiesAndEquity = comparisonTotalLiabilities + comparisonTotalEquity;

            decimal cash = GetBalance(accountsBll, asOfDate, "cash");
            decimal bank = GetBalance(accountsBll, asOfDate, "bank");
            decimal accountsReceivable = GetBalance(accountsBll, asOfDate, "accounts receivable", "trade receivable", "debtors");
            decimal inventory = GetBalance(accountsBll, asOfDate, "inventory asset", "stocks","stock", "merchandise"); // accountsBll.GetInventoryValueAsOf(asOfDate.Date);
            decimal prepaid = GetBalance(accountsBll, asOfDate, "prepaid");
            decimal totalCurrentAssets = cash + bank + accountsReceivable + inventory + prepaid;

            decimal propertyPlantEquipment = GetBalance(accountsBll, asOfDate, "property", "plant", "equipment", "machinery", "furniture", "fixed asset");
            decimal accumulatedDepreciation = GetBalance(accountsBll, asOfDate, "accumulated depreciation", "depreciation");
            decimal netFixedAssets = propertyPlantEquipment + accumulatedDepreciation;
            decimal totalAssets = totalCurrentAssets + netFixedAssets;

            decimal accountsPayable = GetBalanceWithType(accountsBll, asOfDate, "LIABILITY", "accounts payable", "trade payable", "creditors");
            decimal shortTermLoans = GetBalanceWithType(accountsBll, asOfDate, "LIABILITY", "short term loan", "short-term loan", "current portion loan");
            decimal taxPayable = GetBalanceWithType(accountsBll, asOfDate, "LIABILITY", "tax payable", "sales tax payable", "vat payable");
            decimal totalCurrentLiabilities = accountsPayable + shortTermLoans + taxPayable;

            decimal longTermLiabilities = GetBalanceWithType(accountsBll, asOfDate, "LIABILITY", "long term loan", "long-term loan", "lease liability");
            decimal totalLiabilities = totalCurrentLiabilities + longTermLiabilities;

            decimal ownerCapital = GetBalanceWithType(accountsBll, asOfDate, "EQUITY", "owner capital", "share capital", "capital", "paid in capital");
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
            AddHeading(model.Assets, UiMessages.T("Current Assets", "الأصول المتداولة"));
            if (detailed)
            {
                AddLine(model.Assets, UiMessages.T("Cash in Hand", "النقد في الصندوق"), cash, comparisonDate.HasValue ? comparisonCash : (decimal?)null, UiMessages.T("All cash accounts", "جميع حسابات النقد"), false, false, 1);
                AddLine(model.Assets, UiMessages.T("Bank Accounts", "الحسابات البنكية"), bank, comparisonDate.HasValue ? comparisonBank : (decimal?)null, UiMessages.T("All bank accounts", "جميع الحسابات البنكية"), false, false, 1);
                AddLine(model.Assets, UiMessages.T("Accounts Receivable", "الذمم المدينة"), accountsReceivable, comparisonDate.HasValue ? comparisonAr : (decimal?)null, UiMessages.T("Customer sub-ledger total", "إجمالي دفتر العملاء"), false, false, 1);
                AddLine(model.Assets, UiMessages.T("Inventory / Stock", "المخزون"), inventory, comparisonDate.HasValue ? comparisonInventory : (decimal?)null, UiMessages.T("As-of stock valuation", "تقييم المخزون"), false, false, 1);
                AddLine(model.Assets, UiMessages.T("Prepaid Expenses", "المصروفات المقدمة"), prepaid, comparisonDate.HasValue ? comparisonPrepaid : (decimal?)null, UiMessages.T("Deferred expense balances", "أرصدة المصروفات المؤجلة"), false, false, 1);
            }
            AddLine(model.Assets, UiMessages.T("Total Current Assets", "إجمالي الأصول المتداولة"), totalCurrentAssets, comparisonDate.HasValue ? comparisonCurrentAssetsTotal : (decimal?)null, string.Empty, true, false, 0);

            AddHeading(model.Assets, UiMessages.T("Fixed Assets", "الأصول الثابتة"));
            if (detailed)
            {
                AddLine(model.Assets, UiMessages.T("Property, Plant & Equipment", "الممتلكات والمنشآت والمعدات"), ppe, comparisonDate.HasValue ? comparisonPpe : (decimal?)null, UiMessages.T("Gross PPE balances", "إجمالي الممتلكات والمنشآت والمعدات"), false, false, 1);
                AddLine(model.Assets, UiMessages.T("Less: Accumulated Depreciation", "يطرح: الاستهلاك المتراكم"), accumulatedDepreciation, comparisonDate.HasValue ? comparisonAccumDep : (decimal?)null, UiMessages.T("Contra-asset balance", "رصيد الأصل المقابل"), false, false, 1);
            }
            AddLine(model.Assets, UiMessages.T("Net Fixed Assets", "صافي الأصول الثابتة"), netFixedAssets, comparisonDate.HasValue ? comparisonNetFixed : (decimal?)null, string.Empty, true, false, 0);
            AddLine(model.Assets, UiMessages.T("TOTAL ASSETS", "إجمالي الأصول"), totalAssets, comparisonDate.HasValue ? comparisonTotalAssets : (decimal?)null, string.Empty, true, false, 0);
        }

        private static void BuildLiabilitiesAndEquity(BalanceSheetReportModel model, bool detailed, decimal accountsPayable, decimal shortTermLoans, decimal taxPayable, decimal totalCurrentLiabilities, decimal longTermLiabilities, decimal totalLiabilities, decimal ownerCapital, decimal retainedEarnings, decimal currentPeriodNetProfit, decimal totalEquity, decimal liabilitiesAndEquityTotal, decimal comparisonAp, decimal comparisonShortLoans, decimal comparisonTax, decimal comparisonCurrentLiabilities, decimal comparisonLongTermLiabilities, decimal comparisonTotalLiabilities, decimal comparisonCapital, decimal comparisonRetained, decimal comparisonNetProfit, decimal comparisonTotalEquity, decimal comparisonLiabilitiesAndEquity)
        {
            AddHeading(model.LiabilitiesAndEquity, UiMessages.T("Current Liabilities", "الخصوم المتداولة"));
            if (detailed)
            {
                AddLine(model.LiabilitiesAndEquity, UiMessages.T("Accounts Payable", "الذمم الدائنة"), accountsPayable, model.ComparisonDate.HasValue ? comparisonAp : (decimal?)null, UiMessages.T("Supplier sub-ledger total", "إجمالي دفتر الموردين"), false, false, 1);
                AddLine(model.LiabilitiesAndEquity, UiMessages.T("Short-term Loans", "القروض قصيرة الأجل"), shortTermLoans, model.ComparisonDate.HasValue ? comparisonShortLoans : (decimal?)null, UiMessages.T("Current portion of borrowings", "الجزء الحالي من الاقتراض"), false, false, 1);
                AddLine(model.LiabilitiesAndEquity, UiMessages.T("Tax Payable", "الضرائب المستحقة"), taxPayable, model.ComparisonDate.HasValue ? comparisonTax : (decimal?)null, UiMessages.T("Sales/VAT and income tax payables", "ضريبة المبيعات والضريبة على الدخل المستحقة"), false, false, 1);
            }
            AddLine(model.LiabilitiesAndEquity, UiMessages.T("Total Current Liabilities", "إجمالي الخصوم المتداولة"), totalCurrentLiabilities, model.ComparisonDate.HasValue ? comparisonCurrentLiabilities : (decimal?)null, string.Empty, true, false, 0);

            AddHeading(model.LiabilitiesAndEquity, UiMessages.T("Long-term Liabilities", "الخصوم طويلة الأجل"));
            if (detailed)
            {
                AddLine(model.LiabilitiesAndEquity, UiMessages.T("Long-term Liabilities", "الخصوم طويلة الأجل"), longTermLiabilities, model.ComparisonDate.HasValue ? comparisonLongTermLiabilities : (decimal?)null, UiMessages.T("Loans and obligations due after 12 months", "القروض والالتزامات المستحقة بعد 12 شهر"), false, false, 1);
            }
            AddLine(model.LiabilitiesAndEquity, UiMessages.T("Total Liabilities", "إجمالي الخصوم"), totalLiabilities, model.ComparisonDate.HasValue ? comparisonTotalLiabilities : (decimal?)null, string.Empty, true, false, 0);

            AddHeading(model.LiabilitiesAndEquity, UiMessages.T("Equity", "حقوق المالكين"));
            if (detailed)
            {
                AddLine(model.LiabilitiesAndEquity, UiMessages.T("Owner's Capital / Share Capital", "رأس مال المالك / رأس المال"), ownerCapital, model.ComparisonDate.HasValue ? comparisonCapital : (decimal?)null, UiMessages.T("Paid-in capital balances", "أرصدة رأس المال المدفوع"), false, false, 1);
                AddLine(model.LiabilitiesAndEquity, UiMessages.T("Retained Earnings", "الأرباح المحتفظ بها"), retainedEarnings, model.ComparisonDate.HasValue ? comparisonRetained : (decimal?)null, UiMessages.T("Cumulative historical P&L before current year", "الأرباح والخسائر التراكمية قبل السنة الحالية"), false, false, 1);
                AddLine(model.LiabilitiesAndEquity, UiMessages.T("Current Period Net Profit", "صافي ربح الفترة الحالية"), currentPeriodNetProfit, model.ComparisonDate.HasValue ? comparisonNetProfit : (decimal?)null, UiMessages.T("Pulled from Profit & Loss", "مسحوب من قائمة الأرباح والخسائر"), false, false, 1);
            }
            AddLine(model.LiabilitiesAndEquity, UiMessages.T("Total Equity", "إجمالي حقوق المالكين"), totalEquity, model.ComparisonDate.HasValue ? comparisonTotalEquity : (decimal?)null, string.Empty, true, false, 0);
            AddLine(model.LiabilitiesAndEquity, UiMessages.T("TOTAL LIABILITIES + EQUITY", "إجمالي الخصوم + حقوق المالكين"), liabilitiesAndEquityTotal, model.ComparisonDate.HasValue ? comparisonLiabilitiesAndEquity : (decimal?)null, string.Empty, true, false, 0);
        }

        private static void BuildNotes(BalanceSheetReportModel model, DateTime asOfDate, DateTime fiscalStart, DateTime priorFiscalEnd, decimal cash, decimal bank, decimal accountsReceivable, decimal inventory, decimal prepaid, decimal ppe, decimal accumulatedDepreciation, decimal accountsPayable, decimal shortTermLoans, decimal taxPayable, decimal longTermLiabilities, decimal ownerCapital, decimal retainedEarnings, decimal currentPeriodNetProfit)
        {
            model.Notes.Add(new BalanceNote { Title = UiMessages.T("As-of Date", "تاريخ الإعداد"), Summary = asOfDate.ToString("dd-MMM-yyyy"), Details = { UiMessages.T("Balance sheet is a point-in-time statement.", "الميزانية العمومية بيان لفترة معينة من الزمن.") } });
            model.Notes.Add(new BalanceNote { Title = UiMessages.T("Inventory Valuation", "تقييم المخزون"), Summary = inventory.ToString("N2"), Details = { UiMessages.T("Fetched from stock tables using item quantities multiplied by average cost.", "تم استخراجها من جداول المخزون باستخدام كميات العناصر مضروبة في متوسط التكلفة."), UiMessages.T("Method: pos_inventory joined to pos_products.avg_cost.", "الطريقة: pos_inventory في pos_products.avg_cost.") } });
            model.Notes.Add(new BalanceNote { Title = UiMessages.T("Retained Earnings", "الأرباح المحتفظ بها"), Summary = retainedEarnings.ToString("N2"), Details = { UiMessages.T("Calculated as cumulative historical P&L before the current fiscal year.", "محسوب كأرباح وخسائر تراكمية تاريخية قبل السنة المالية الحالية."), UiMessages.T("Historical P&L window: ", "نافذة الأرباح والخسائر التاريخية: ") + DateTime.MinValue.ToString("yyyy") + UiMessages.T(" to ", " إلى ") + priorFiscalEnd.ToString("dd-MMM-yyyy") } });
            model.Notes.Add(new BalanceNote { Title = UiMessages.T("Current Period Net Profit", "صافي ربح الفترة الحالية"), Summary = currentPeriodNetProfit.ToString("N2"), Details = { UiMessages.T("Pulled automatically from Profit & Loss.", "تم سحبها تلقائياً من قائمة الأرباح والخسائر."), UiMessages.T("Current fiscal year window: ", "نافذة السنة المالية الحالية: ") + fiscalStart.ToString("dd-MMM-yyyy") + UiMessages.T(" to ", " إلى ") + asOfDate.ToString("dd-MMM-yyyy") } });
            model.Notes.Add(new BalanceNote { Title = UiMessages.T("Key Balances", "الأرصدة الرئيسية"), Summary = string.Format(UiMessages.T("Cash {0:N2} | Bank {1:N2} | AR {2:N2} | AP {3:N2}", "نقد {0:N2} | بنك {1:N2} | ذمم {2:N2} | دائنون {3:N2}"), cash, bank, accountsReceivable, accountsPayable), Details = { UiMessages.T("Prepaid: ", "مقدم: ") + prepaid.ToString("N2"), UiMessages.T("PPE: ", "الممتلكات والمنشآت والمعدات: ") + ppe.ToString("N2"), UiMessages.T("Accumulated Depreciation: ", "الاستهلاك المتراكم: ") + accumulatedDepreciation.ToString("N2"), UiMessages.T("Short-term loans: ", "القروض قصيرة الأجل: ") + shortTermLoans.ToString("N2"), UiMessages.T("Tax payable: ", "الضرائب المستحقة: ") + taxPayable.ToString("N2"), UiMessages.T("Long-term liabilities: ", "الخصوم طويلة الأجل: ") + longTermLiabilities.ToString("N2"), UiMessages.T("Owner capital: ", "رأس مال المالك: ") + ownerCapital.ToString("N2") } });
        }

        private static void BuildDiagnostics(BalanceSheetReportModel model, DateTime? comparisonDate, decimal cash, decimal bank, decimal accountsReceivable, decimal inventory, decimal prepaid, decimal totalCurrentAssets, decimal totalAssets, decimal accountsPayable, decimal shortTermLoans, decimal taxPayable, decimal totalCurrentLiabilities, decimal longTermLiabilities, decimal totalLiabilities, decimal ownerCapital, decimal retainedEarnings, decimal currentPeriodNetProfit, decimal totalEquity)
        {
            AddDiagnostic(model, UiMessages.T("Current Assets", "الأصول المتداولة"), totalCurrentAssets, comparisonDate.HasValue ? totalCurrentAssets : 0m, UiMessages.T("Asset subtotal check", "فحص إجمالي الأصول الفرعي"));
            AddDiagnostic(model, UiMessages.T("Total Assets", "إجمالي الأصول"), totalAssets, comparisonDate.HasValue ? totalAssets : 0m, UiMessages.T("Final asset total", "إجمالي الأصول النهائي"));
            AddDiagnostic(model, UiMessages.T("Current Liabilities", "الخصوم المتداولة"), totalCurrentLiabilities, comparisonDate.HasValue ? totalCurrentLiabilities : 0m, UiMessages.T("Liability subtotal check", "فحص إجمالي الخصوم الفرعي"));
            AddDiagnostic(model, UiMessages.T("Total Liabilities", "إجمالي الخصوم"), totalLiabilities, comparisonDate.HasValue ? totalLiabilities : 0m, UiMessages.T("Liability total", "إجمالي الخصوم"));
            AddDiagnostic(model, UiMessages.T("Total Equity", "إجمالي حقوق المالكين"), totalEquity, comparisonDate.HasValue ? totalEquity : 0m, UiMessages.T("Equity total", "إجمالي حقوق المالكين"));
            AddDiagnostic(model, UiMessages.T("AR / AP Sanity", "معقولية الذمم"), accountsReceivable, accountsPayable, UiMessages.T("Comparative control accounts", "حسابات التحكم المقارنة"));
            AddDiagnostic(model, UiMessages.T("Cash vs Bank", "نقد مقابل بنك"), cash, bank, UiMessages.T("Liquidity split", "تقسيم السيولة"));
            AddDiagnostic(model, UiMessages.T("Inventory vs Prepaids", "مخزون مقابل مقدمات"), inventory, prepaid, UiMessages.T("Working capital split", "تقسيم رأس المال العامل"));
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

        private static decimal GetBalanceWithType(AccountsBLL accountsBll, DateTime asOfDate, string accountType, params string[] patterns)
        {
            decimal value = accountsBll.GetBalanceByAccountNamePatternsAsOfWithAccountType(asOfDate.Date, accountType, patterns);
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
