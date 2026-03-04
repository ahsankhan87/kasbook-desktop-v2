using POS.BLL;
using POS.Core;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using pos.UI;
using pos.UI.Busy;

namespace pos
{
    public partial class frm_productsMovements : Form
    {
        private readonly string _item_number;
        private readonly string _product_name;

        public frm_productsMovements(string item_number, string product_name = "")
        {
            InitializeComponent();
            _item_number = item_number;
            _product_name = product_name;
        }

        private void frm_productsMovements_Load(object sender, EventArgs e)
        {
            lbl_productName.Text = string.IsNullOrWhiteSpace(_product_name)
                ? UiMessages.T("Item: " + _item_number, "الصنف: " + _item_number)
                : _product_name;

            if (grid_search_products.Columns.Contains("product_name"))
                grid_search_products.Columns["product_name"].Visible = false;

            load_Products_grid();
        }

        public void load_Products_grid()
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Loading movements...", "جاري تحميل الحركات...")))
                {
                    load_product_movements();
                }
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

                grid_search_products.AutoGenerateColumns = false;

                DataTable product_dt = new DataTable();
                using (SqlConnection cn = new SqlConnection(POS.DLL.dbConnection.ConnectionString))
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandText = @"SELECT
                                            I.id,
                                            I.item_code,
                                            I.item_number,
                                            I.qty,
                                            I.unit_price,
                                            I.cost_price,
                                            I.invoice_no,
                                            I.description,
                                            I.trans_date,
                                            C.first_name AS customer,
                                            CONCAT(S.first_name,' ',S.last_name) AS supplier,
                                            COALESCE(U.name,U.username,  '') AS username,
                                            SUM(I.qty) OVER (ORDER BY I.id ASC ROWS UNBOUNDED PRECEDING) AS balance_qty
                                        FROM pos_inventory I
                                        LEFT JOIN pos_customers C ON C.id = I.customer_id
                                        LEFT JOIN pos_suppliers S ON S.id = I.supplier_id
                                        LEFT JOIN pos_users U ON U.id = I.user_id
                                        WHERE I.item_number = @item_number
                                          AND I.branch_id = @branch_id
                                        ORDER BY I.id DESC";
                    cmd.Parameters.AddWithValue("@item_number", _item_number);
                    cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(product_dt);
                    }
                }

                if (product_dt.Rows.Count <= 0)
                {
                    UiMessages.ShowInfo(
                        "No movements found for this item.",
                        "لا توجد حركات لهذا الصنف.",
                        captionEn: "Movements",
                        captionAr: "الحركات");
                    return;
                }

                int RowIndex = 0;
                foreach (DataRow row in product_dt.Rows)
                {
                    int id = Convert.ToInt32(row["id"]);
                    string invoice_no = Convert.ToString(row["invoice_no"]);
                    string name = _product_name;
                    string qty = Convert.ToDecimal(row["qty"]).ToString("N2");
                    string balance = Convert.ToDecimal(row["balance_qty"]).ToString("N2");
                    string cost_price = Convert.ToDecimal(row["cost_price"]).ToString("N2");
                    string unit_price = Convert.ToDecimal(row["unit_price"]).ToString("N2");
                    string description = Convert.ToString(row["description"]);
                    string supplier = Convert.ToString(row["supplier"]);
                    string customer = Convert.ToString(row["customer"]);
                    string date = Convert.ToString(row["trans_date"]);
                    string username = Convert.ToString(row["username"]);

                    string[] row0 = {
                        id.ToString(), invoice_no, name, qty, balance,
                        cost_price, unit_price,
                        description, supplier, customer, date,username
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
