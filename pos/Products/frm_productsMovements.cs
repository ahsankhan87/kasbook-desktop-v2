using POS.BLL;
using POS.Core;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using pos.UI;
using pos.UI.Busy;

namespace pos
{
    public partial class frm_productsMovements : Form
    {
        private readonly string _item_number;

        public frm_productsMovements(string item_number)
        {
            InitializeComponent();
            _item_number = item_number;
        }

        private void frm_productsMovements_Load(object sender, EventArgs e)
        {
            using (BusyScope.Show(this, UiMessages.T("Loading movements...", "جاري تحميل الحركات...")))
            {
                load_Products_grid();
            }
        }

        public void load_Products_grid()
        {
            try
            {
                load_product_movements();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, captionEn: "Error", captionAr: "خطأ");
            }
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            // No UI action currently, keep for future filters.
            UiMessages.ShowInfo(
                "Search filters are not implemented on this screen.",
                "خيارات البحث غير مفعلة في هذه الشاشة.",
                captionEn: "Movements",
                captionAr: "الحركات");
        }

        private void load_product_movements()
        {
            try
            {
                grid_search_products.Rows.Clear();

                if (string.IsNullOrWhiteSpace(_item_number))
                {
                    UiMessages.ShowWarning(
                        "No item selected.",
                        "لم يتم اختيار صنف.",
                        captionEn: "Movements",
                        captionAr: "الحركات");
                    return;
                }

                GeneralBLL objBLL = new GeneralBLL();
                grid_search_products.AutoGenerateColumns = false;

                string keyword = "I.id,P.name AS product_name,I.item_code,I.item_number,I.qty,I.unit_price,I.cost_price,I.invoice_no,I.description,trans_date,C.first_name AS customer,S.first_name AS supplier";
                string table = "pos_inventory I " +
                               "LEFT JOIN pos_products P ON P.code = I.item_code " +
                               "LEFT JOIN pos_customers C ON C.id = I.customer_id " +
                               "LEFT JOIN pos_suppliers S ON S.id = I.supplier_id " +
                               "WHERE I.item_number = '" + _item_number.Replace("'", "''") + "' AND I.branch_id = " + UsersModal.logged_in_branch_id + " " +
                               "ORDER BY I.id ASC";

                DataTable product_dt = objBLL.GetRecord(keyword, table);

                if (product_dt.Rows.Count <= 0)
                {
                    UiMessages.ShowInfo(
                        "No movements found for this item.",
                        "لا توجد حركات لهذا الصنف.",
                        captionEn: "Movements",
                        captionAr: "الحركات");
                    return;
                }

                // Add balance_qty column manually
                if (!product_dt.Columns.Contains("balance_qty"))
                    product_dt.Columns.Add("balance_qty", typeof(double));

                // Calculate running balance
                double balance_qty = 0;
                foreach (DataRow row in product_dt.Rows)
                {
                    balance_qty += Convert.ToDouble(row["qty"]);
                    row["balance_qty"] = balance_qty;
                }

                // Display in DESC order
                int RowIndex = 0;
                foreach (DataRow row in product_dt.Select("", "id DESC"))
                {
                    int id = Convert.ToInt32(row["id"]);
                    string invoice_no = Convert.ToString(row["invoice_no"]);
                    string name = Convert.ToString(row["product_name"]);
                    string qty = Convert.ToString(row["qty"]);
                    string balance = Convert.ToString(row["balance_qty"]);
                    double cost_price = Convert.ToDouble(row["cost_price"]);
                    double unit_price = Convert.ToDouble(row["unit_price"]);
                    string description = Convert.ToString(row["description"]);
                    string supplier = Convert.ToString(row["supplier"]);
                    string customer = Convert.ToString(row["customer"]);
                    string date = Convert.ToString(row["trans_date"]);

                    string[] row0 = {
                        id.ToString(), invoice_no, name, qty, balance,
                        cost_price.ToString(), unit_price.ToString(),
                        description, supplier, customer, date
                    };

                    grid_search_products.Rows.Add(row0);

                    if (description == "Sale")
                        grid_search_products.Rows[RowIndex].DefaultCellStyle.BackColor = Color.LightBlue;
                    else if (description == "Purchase")
                        grid_search_products.Rows[RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
                    else if (description == "Adjustment")
                        grid_search_products.Rows[RowIndex].DefaultCellStyle.BackColor = Color.Yellow;

                    RowIndex++;
                }
            }
            catch
            {
                // Let caller show the message
                throw;
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (grid_search_products.CurrentRow == null || grid_search_products.CurrentRow.Cells["invoice_no"] == null)
                {
                    UiMessages.ShowWarning(
                        "Please select a row to copy.",
                        "يرجى اختيار سطر للنسخ.",
                        captionEn: "Copy",
                        captionAr: "نسخ");
                    return;
                }

                var invoiceNo = Convert.ToString(grid_search_products.CurrentRow.Cells["invoice_no"].Value);
                if (string.IsNullOrWhiteSpace(invoiceNo))
                {
                    UiMessages.ShowWarning(
                        "Selected row has no invoice number.",
                        "السطر المحدد لا يحتوي على رقم فاتورة.",
                        captionEn: "Copy",
                        captionAr: "نسخ");
                    return;
                }

                Clipboard.SetText(invoiceNo);
                UiMessages.ShowInfo(
                    "Invoice number copied.",
                    "تم نسخ رقم الفاتورة.",
                    captionEn: "Copy",
                    captionAr: "نسخ");
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, captionEn: "Error", captionAr: "خطأ");
            }
        }
    }
}
