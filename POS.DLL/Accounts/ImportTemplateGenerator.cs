using System;
using System.Data;
using System.IO;

namespace POS.DLL.Accounts
{
    /// <summary>
    /// Generates Excel import templates for the Accounting Import module.
    /// Note: This uses simple HTML-based Excel generation (no EPPlus dependency required).
    /// Templates open in Excel as .xls files with formatting, validation instructions, and sample data.
    /// </summary>
    public static class ImportTemplateGenerator
    {
        /// <summary>
        /// Creates an import template file for the specified import type
        /// </summary>
        /// <param name="importType">Type of import: COA, OPENING_BALANCE, CUSTOMER_BALANCES, SUPPLIER_BALANCES, JOURNAL_HISTORY</param>
        /// <returns>Full path to the created template file in user's temp directory</returns>
        public static string CreateImportTemplate(string importType)
        {
            if (string.IsNullOrWhiteSpace(importType))
                throw new ArgumentNullException(nameof(importType));

            importType = importType.ToUpperInvariant();

            DataTable templateData;
            string instructions;

            switch (importType)
            {
                case "COA":
                    templateData = BuildChartOfAccountsTemplate(out instructions);
                    break;

                case "OPENING_BALANCE":
                    templateData = BuildOpeningBalanceTemplate(out instructions);
                    break;

                case "CUSTOMER_BALANCES":
                    templateData = BuildCustomerBalancesTemplate(out instructions);
                    break;

                case "SUPPLIER_BALANCES":
                    templateData = BuildSupplierBalancesTemplate(out instructions);
                    break;

                case "JOURNAL_HISTORY":
                    templateData = BuildJournalHistoryTemplate(out instructions);
                    break;

                default:
                    throw new ArgumentException($"Invalid import type: {importType}. Valid types: COA, OPENING_BALANCE, CUSTOMER_BALANCES, SUPPLIER_BALANCES, JOURNAL_HISTORY");
            }

            string fileName = $"Import_Template_{importType}_{DateTime.Now:yyyyMMdd_HHmmss}.xls";
            string filePath = Path.Combine(Path.GetTempPath(), fileName);

            // Generate HTML-based Excel file with instructions sheet
            GenerateExcelFile(filePath, templateData, instructions, importType);

            return filePath;
        }

        private static DataTable BuildChartOfAccountsTemplate(out string instructions)
        {
            var dt = new DataTable("ChartOfAccounts");

            // Define columns
            dt.Columns.Add("Account Code*", typeof(string));
            dt.Columns.Add("Account Name*", typeof(string));
            dt.Columns.Add("Account Type*", typeof(string));
            dt.Columns.Add("Parent Code", typeof(string));
            dt.Columns.Add("Description", typeof(string));
            dt.Columns.Add("Is Active", typeof(string));

            // Add sample rows
            dt.Rows.Add("1000", "Assets", "Asset", "", "Main assets account", "Yes");
            dt.Rows.Add("1100", "Current Assets", "Asset", "1000", "Short-term assets", "Yes");
            dt.Rows.Add("1110", "Cash", "Asset", "1100", "Cash on hand and in bank", "Yes");

            instructions = @"CHART OF ACCOUNTS IMPORT INSTRUCTIONS

REQUIRED COLUMNS (* required):
- Account Code*: Unique code for the account (e.g., 1000, 1100)
- Account Name*: Full name of the account
- Account Type*: Asset, Liability, Equity, Revenue, Expense
- Parent Code: Code of parent account (for hierarchical structure)
- Description: Optional description
- Is Active: Yes/No (default: Yes)

VALIDATION RULES:
1. Account Code must be unique
2. Account Type must be one of: Asset, Liability, Equity, Revenue, Expense
3. If Parent Code is provided, it must exist in the system or in this import
4. Accounts should be ordered so parent accounts appear before child accounts

SAMPLE DATA:
The first 3 rows contain sample data (highlighted in gray).
Delete these rows and replace with your actual data.";

            return dt;
        }

        private static DataTable BuildOpeningBalanceTemplate(out string instructions)
        {
            var dt = new DataTable("OpeningBalances");

            dt.Columns.Add("Account Code*", typeof(string));
            dt.Columns.Add("Account Name", typeof(string));
            dt.Columns.Add("Debit Amount", typeof(string));
            dt.Columns.Add("Credit Amount", typeof(string));
            dt.Columns.Add("Remarks", typeof(string));

            // Sample data
            dt.Rows.Add("1110", "Cash", "50000.00", "", "Opening cash balance");
            dt.Rows.Add("2100", "Accounts Payable", "", "35000.00", "Opening payables");
            dt.Rows.Add("3000", "Owner's Equity", "", "15000.00", "Balancing equity");

            instructions = @"OPENING BALANCE IMPORT INSTRUCTIONS

REQUIRED COLUMNS (* required):
- Account Code*: Must exist in Chart of Accounts
- Account Name: For reference only (not validated)
- Debit Amount: Enter debit balance (leave blank if credit)
- Credit Amount: Enter credit balance (leave blank if debit)
- Remarks: Optional notes for the entry

VALIDATION RULES:
1. Each account can have EITHER debit OR credit (not both)
2. Total Debits MUST equal Total Credits
3. All account codes must exist in the system
4. Amounts must be positive numbers
5. No duplicate account codes allowed

IMPORTANT:
- Leave Debit Amount blank if account has credit balance
- Leave Credit Amount blank if account has debit balance
- Trial Balance MUST balance (Dr = Cr) before import
- Sample data rows (gray) should be deleted

SAMPLE DATA:
Rows 1-3 are samples. Delete and replace with your data.";

            return dt;
        }

        private static DataTable BuildCustomerBalancesTemplate(out string instructions)
        {
            var dt = new DataTable("CustomerBalances");

            dt.Columns.Add("Customer Code*", typeof(string));
            dt.Columns.Add("Customer Name", typeof(string));
            dt.Columns.Add("Balance Amount*", typeof(string));
            dt.Columns.Add("Balance Type*", typeof(string));
            dt.Columns.Add("Invoice Reference", typeof(string));
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Remarks", typeof(string));

            dt.Rows.Add("CUST001", "ABC Trading Co.", "15000.00", "Debit", "INV-2024-001", "2024-01-01", "Opening receivable");
            dt.Rows.Add("CUST002", "XYZ Corporation", "8500.50", "Debit", "INV-2024-002", "2024-01-01", "Outstanding invoice");

            instructions = @"CUSTOMER BALANCES IMPORT INSTRUCTIONS

REQUIRED COLUMNS (* required):
- Customer Code*: Must exist in customers master
- Customer Name: For reference (not validated)
- Balance Amount*: Outstanding amount
- Balance Type*: Debit (customer owes you) or Credit (you owe customer)
- Invoice Reference: Optional invoice number
- Date: Balance as of date (format: YYYY-MM-DD)
- Remarks: Optional notes

VALIDATION RULES:
1. Customer Code must exist in the system
2. Balance Amount must be positive
3. Balance Type must be 'Debit' or 'Credit'
4. Date format: YYYY-MM-DD (e.g., 2024-01-01)

TYPICAL SCENARIO:
Most customer balances are 'Debit' (customer owes money).
Use 'Credit' only for customer advance payments or refunds due.";

            return dt;
        }

        private static DataTable BuildSupplierBalancesTemplate(out string instructions)
        {
            var dt = new DataTable("SupplierBalances");

            dt.Columns.Add("Supplier Code*", typeof(string));
            dt.Columns.Add("Supplier Name", typeof(string));
            dt.Columns.Add("Balance Amount*", typeof(string));
            dt.Columns.Add("Balance Type*", typeof(string));
            dt.Columns.Add("Invoice Reference", typeof(string));
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Remarks", typeof(string));

            dt.Rows.Add("SUPP001", "Global Suppliers Ltd.", "25000.00", "Credit", "BILL-2024-001", "2024-01-01", "Opening payable");
            dt.Rows.Add("SUPP002", "Tech Parts Inc.", "12300.75", "Credit", "BILL-2024-002", "2024-01-01", "Outstanding bill");

            instructions = @"SUPPLIER BALANCES IMPORT INSTRUCTIONS

REQUIRED COLUMNS (* required):
- Supplier Code*: Must exist in suppliers master
- Supplier Name: For reference (not validated)
- Balance Amount*: Outstanding amount
- Balance Type*: Credit (you owe supplier) or Debit (supplier owes you)
- Invoice Reference: Optional bill/invoice number
- Date: Balance as of date (format: YYYY-MM-DD)
- Remarks: Optional notes

VALIDATION RULES:
1. Supplier Code must exist in the system
2. Balance Amount must be positive
3. Balance Type must be 'Credit' or 'Debit'
4. Date format: YYYY-MM-DD (e.g., 2024-01-01)

TYPICAL SCENARIO:
Most supplier balances are 'Credit' (you owe money to supplier).
Use 'Debit' only for supplier advance payments or refunds due to you.";

            return dt;
        }

        private static DataTable BuildJournalHistoryTemplate(out string instructions)
        {
            var dt = new DataTable("JournalHistory");

            dt.Columns.Add("Voucher No*", typeof(string));
            dt.Columns.Add("Voucher Date*", typeof(string));
            dt.Columns.Add("Account Code*", typeof(string));
            dt.Columns.Add("Debit Amount", typeof(string));
            dt.Columns.Add("Credit Amount", typeof(string));
            dt.Columns.Add("Narration", typeof(string));
            dt.Columns.Add("Reference No", typeof(string));

            dt.Rows.Add("JV-001", "2024-01-05", "1110", "10000.00", "", "Cash received", "");
            dt.Rows.Add("JV-001", "2024-01-05", "4100", "", "10000.00", "Sales revenue", "");
            dt.Rows.Add("JV-002", "2024-01-10", "5100", "5000.00", "", "Purchase expense", "");
            dt.Rows.Add("JV-002", "2024-01-10", "1110", "", "5000.00", "Cash paid", "");

            instructions = @"JOURNAL VOUCHERS HISTORY IMPORT INSTRUCTIONS

REQUIRED COLUMNS (* required):
- Voucher No*: Unique voucher number (multiple rows with same number = one voucher)
- Voucher Date*: Date of transaction (format: YYYY-MM-DD)
- Account Code*: Must exist in Chart of Accounts
- Debit Amount: Debit entry (leave blank if credit)
- Credit Amount: Credit entry (leave blank if debit)
- Narration: Description of the entry
- Reference No: Optional external reference

VALIDATION RULES:
1. Each voucher (same Voucher No) must have balanced Dr and Cr
2. All Account Codes must exist in system
3. Each row can have EITHER Debit OR Credit (not both)
4. Date format: YYYY-MM-DD
5. Voucher must have at least 2 lines (one Dr, one Cr minimum)

STRUCTURE:
- Group entries by Voucher No
- Each voucher must balance (Total Dr = Total Cr)
- Typical voucher has 2+ lines

EXAMPLE:
JV-001 has 2 lines: Dr 1110 (Cash), Cr 4100 (Revenue)
JV-002 has 2 lines: Dr 5100 (Expense), Cr 1110 (Cash)";

            return dt;
        }

        private static void GenerateExcelFile(string filePath, DataTable data, string instructions, string importType)
        {
            var html = new System.Text.StringBuilder();

            // Build HTML with Excel XML namespace for better formatting
            html.AppendLine("<?xml version=\"1.0\"?>");
            html.AppendLine("<?mso-application progid=\"Excel.Sheet\"?>");
            html.AppendLine("<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\"");
            html.AppendLine(" xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\">");

            // Styles
            html.AppendLine("<Styles>");

            // Header style - Bold, background color, borders
            html.AppendLine("<Style ss:ID=\"Header\">");
            html.AppendLine(" <Font ss:Bold=\"1\" ss:Size=\"11\"/>");
            html.AppendLine(" <Interior ss:Color=\"#4472C4\" ss:Pattern=\"Solid\"/>");
            html.AppendLine(" <Font ss:Color=\"#FFFFFF\"/>");
            html.AppendLine(" <Borders>");
            html.AppendLine("  <Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>");
            html.AppendLine(" </Borders>");
            html.AppendLine("</Style>");

            // Sample data style - Light gray background
            html.AppendLine("<Style ss:ID=\"Sample\">");
            html.AppendLine(" <Interior ss:Color=\"#E7E6E6\" ss:Pattern=\"Solid\"/>");
            html.AppendLine("</Style>");

            // Instruction style - Wrapped text
            html.AppendLine("<Style ss:ID=\"Instruction\">");
            html.AppendLine(" <Alignment ss:Vertical=\"Top\" ss:WrapText=\"1\"/>");
            html.AppendLine(" <Font ss:Size=\"10\" ss:FontName=\"Courier New\"/>");
            html.AppendLine("</Style>");

            html.AppendLine("</Styles>");

            // Data worksheet
            html.AppendLine($"<Worksheet ss:Name=\"{importType}_Data\">");
            html.AppendLine("<Table>");

            // Set column widths
            for (int i = 0; i < data.Columns.Count; i++)
            {
                html.AppendLine("<Column ss:Width=\"120\"/>");
            }

            // Header row
            html.AppendLine("<Row ss:Height=\"25\">");
            foreach (DataColumn col in data.Columns)
            {
                html.AppendLine($" <Cell ss:StyleID=\"Header\"><Data ss:Type=\"String\">{System.Security.SecurityElement.Escape(col.ColumnName)}</Data></Cell>");
            }
            html.AppendLine("</Row>");

            // Column description row
            html.AppendLine("<Row>");
            foreach (DataColumn col in data.Columns)
            {
                string description = GetColumnDescription(col.ColumnName);
                html.AppendLine($" <Cell><Data ss:Type=\"String\">{System.Security.SecurityElement.Escape(description)}</Data></Cell>");
            }
            html.AppendLine("</Row>");

            // Sample data rows (highlighted)
            foreach (DataRow row in data.Rows)
            {
                html.AppendLine("<Row>");
                foreach (var item in row.ItemArray)
                {
                    string value = item?.ToString() ?? "";
                    html.AppendLine($" <Cell ss:StyleID=\"Sample\"><Data ss:Type=\"String\">{System.Security.SecurityElement.Escape(value)}</Data></Cell>");
                }
                html.AppendLine("</Row>");
            }

            html.AppendLine("</Table>");
            html.AppendLine("</Worksheet>");

            // Instructions worksheet
            html.AppendLine("<Worksheet ss:Name=\"INSTRUCTIONS\">");
            html.AppendLine("<Table>");
            html.AppendLine("<Column ss:Width=\"600\"/>");

            // Split instructions into lines
            var instructionLines = instructions.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            foreach (var line in instructionLines)
            {
                html.AppendLine("<Row ss:Height=\"18\">");
                html.AppendLine($" <Cell ss:StyleID=\"Instruction\"><Data ss:Type=\"String\">{System.Security.SecurityElement.Escape(line)}</Data></Cell>");
                html.AppendLine("</Row>");
            }

            html.AppendLine("</Table>");
            html.AppendLine("</Worksheet>");

            html.AppendLine("</Workbook>");

            // Write to file
            File.WriteAllText(filePath, html.ToString(), System.Text.Encoding.UTF8);
        }

        private static string GetColumnDescription(string columnName)
        {
            // Return brief descriptions for column headers
            if (columnName.Contains("*"))
                return "(Required field)";
            else if (columnName.Contains("Code"))
                return "(Unique identifier)";
            else if (columnName.Contains("Amount"))
                return "(Numeric, 2 decimals)";
            else if (columnName.Contains("Date"))
                return "(Format: YYYY-MM-DD)";
            else if (columnName.Contains("Type"))
                return "(See instructions)";
            else
                return "(Optional)";
        }
    }
}
