using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;

namespace pos.Reports.Common
{
    public sealed class ProductExcelImportRow
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal? Qty { get; set; }
        public decimal? Price { get; set; }
    }

    public static class ProductExcelImportHelper
    {
        public static DataTable BuildTemplate()
        {
            var dt = new DataTable();
            dt.Columns.Add("Product Code");
            dt.Columns.Add("Name");
            dt.Columns.Add("Qty");
            dt.Columns.Add("Price");

            dt.Rows.Add("PRD-001", "Sample Product 1", "2", "25.50");
            dt.Rows.Add("PRD-002", "Sample Product 2", "1", "100.00");

            return dt;
        }

        public static DataTable ReadExcel(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLowerInvariant();
            string connectionString;

            if (extension == ".xlsx")
            {
                connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";";
            }
            else if (extension == ".xls")
            {
                connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\";";
            }
            else
            {
                throw new InvalidOperationException("Only Excel files (.xlsx, .xls) are supported.");
            }

            using (var cn = new OleDbConnection(connectionString))
            {
                cn.Open();

                var schema = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (schema == null || schema.Rows.Count == 0)
                    throw new InvalidOperationException("No worksheet was found in the selected Excel file.");

                string sheetName = string.Empty;
                foreach (DataRow row in schema.Rows)
                {
                    var tableName = Convert.ToString(row["TABLE_NAME"]);
                    if (!string.IsNullOrWhiteSpace(tableName) && tableName.EndsWith("$"))
                    {
                        sheetName = tableName;
                        break;
                    }
                }

                if (string.IsNullOrWhiteSpace(sheetName))
                    sheetName = Convert.ToString(schema.Rows[0]["TABLE_NAME"]);

                using (var cmd = new OleDbCommand("SELECT * FROM [" + sheetName + "]", cn))
                using (var da = new OleDbDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public static List<ProductExcelImportRow> ParseRows(DataTable source)
        {
            if (source == null || source.Rows.Count == 0)
                return new List<ProductExcelImportRow>();

            string codeColumn = FindImportColumn(source, "productcode", "product_code", "product code", "code", "itemcode", "item code");
            string nameColumn = FindImportColumn(source, "productname", "product_name", "product name", "name", "itemname", "item name");
            string qtyColumn = FindImportColumn(source, "qty", "quantity");
            string priceColumn = FindImportColumn(source, "price", "unitprice", "unit_price", "unit price", "saleprice", "sale price", "costprice", "cost price");

            if (string.IsNullOrWhiteSpace(codeColumn) && string.IsNullOrWhiteSpace(nameColumn))
                throw new InvalidOperationException("Excel must contain either Product Code or Name column.");

            var rows = new List<ProductExcelImportRow>();
            foreach (DataRow row in source.Rows)
            {
                string productCode = GetImportCellString(row, codeColumn);
                string productName = GetImportCellString(row, nameColumn);

                if (string.IsNullOrWhiteSpace(productCode) && string.IsNullOrWhiteSpace(productName))
                    continue;

                decimal? qty = GetOptionalImportDecimal(row, qtyColumn);
                if (qty.HasValue && qty.Value <= 0)
                    continue;

                decimal? price = GetOptionalImportDecimal(row, priceColumn);
                if (price.HasValue && price.Value < 0)
                    continue;

                rows.Add(new ProductExcelImportRow
                {
                    ProductCode = productCode,
                    ProductName = productName,
                    Qty = qty,
                    Price = price
                });
            }

            return rows;
        }

        private static string FindImportColumn(DataTable dt, params string[] aliases)
        {
            foreach (DataColumn column in dt.Columns)
            {
                string normalizedColumn = NormalizeImportColumnName(column.ColumnName);
                if (aliases.Any(alias => normalizedColumn == NormalizeImportColumnName(alias)))
                    return column.ColumnName;
            }

            return null;
        }

        private static string NormalizeImportColumnName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            return new string(value.Where(char.IsLetterOrDigit).ToArray()).ToLowerInvariant();
        }

        private static string GetImportCellString(DataRow row, string columnName)
        {
            if (row == null || string.IsNullOrWhiteSpace(columnName) || !row.Table.Columns.Contains(columnName))
                return string.Empty;

            return Convert.ToString(row[columnName]).Trim();
        }

        private static decimal? GetOptionalImportDecimal(DataRow row, string columnName)
        {
            if (string.IsNullOrWhiteSpace(columnName))
                return null;

            string text = GetImportCellString(row, columnName);
            if (string.IsNullOrWhiteSpace(text))
                return null;

            decimal value;
            return decimal.TryParse(text, out value) ? value : (decimal?)null;
        }
    }
}
