using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using POS.BLL;
using pos.UI;

namespace pos
{
    public partial class frm_estimates_detail : Form
    {
        int _sale_id;
        string _invoice_no;

        public frm_estimates_detail(int sale_id,string invoice_no)
        {
            InitializeComponent();
            _sale_id = sale_id;
            _invoice_no = invoice_no;
        }


        public void frm_estimates_detail_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            StyleForm();

            try
            {
                // Display conversion status if applicable
                DisplayEstimateConversionStatus();

                double _total_qty = 0;
                double _total_cost = 0;
                double _total_vat = 0;
                double _total_discount = 0;
                double _grand_total = 0;

                grid_estimates_detail.DataSource = null;
                grid_estimates_detail.AutoGenerateColumns = false;

                EstimatesBLL objestimatesBLL = new EstimatesBLL();
                DataTable dt = objestimatesBLL.GetAllEstimatesItems(_sale_id);

                foreach (DataRow dr in dt.Rows)
                {

                    string[] row00 = {
                        dr["id"].ToString(),
                        dr["invoice_no"].ToString(),
                        dr["item_code"].ToString(),
                        dr["product_name"].ToString(),
                        dr["loc_code"].ToString(),
                        Math.Round(Convert.ToDouble(dr["quantity_sold"]),2).ToString(),
                        Math.Round(Convert.ToDouble(dr["unit_price"]),2).ToString(),
                        Math.Round(Convert.ToDouble(dr["discount_value"]),2).ToString(),
                        Math.Round(Convert.ToDouble(dr["vat"]),2).ToString(),
                        Math.Round(Convert.ToDouble(dr["net_total"]),2).ToString()

                    };
                    _total_qty += Convert.ToDouble(dr["quantity_sold"].ToString());
                    _total_cost += Convert.ToDouble(dr["unit_price"].ToString());
                    _total_discount += Convert.ToDouble(dr["discount_value"].ToString());
                    _total_vat += Convert.ToDouble(dr["vat"].ToString());
                    _grand_total += Convert.ToDouble(dr["net_total"].ToString());

                    grid_estimates_detail.Rows.Add(row00);

                }
                string[] row12 = { "", "", "", "", "Total", _total_qty.ToString("N2"), _total_cost.ToString("N2"), _total_discount.ToString("N2"), _total_vat.ToString("N2"), _grand_total.ToString("N2") };
                grid_estimates_detail.Rows.Add(row12);

                CustomizeDataGridView();
            }
             catch (Exception ex)
             {
                 MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

             }
        }

        private void DisplayEstimateConversionStatus()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(POS.DLL.dbConnection.ConnectionString))
                {
                    cn.Open();
                    string query = @"SELECT status, linked_sales_invoice_no, converted_date 
                                    FROM pos_estimates 
                                    WHERE invoice_no = @invoiceNo";

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@invoiceNo", _invoice_no);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int status = Convert.ToInt32(reader["status"]);
                                if (status == 1) // Converted
                                {
                                    string linkedInvoiceNo = reader["linked_sales_invoice_no"] != DBNull.Value
                                        ? reader["linked_sales_invoice_no"].ToString()
                                        : "Unknown";
                                    DateTime convertedDate = reader["converted_date"] != DBNull.Value
                                        ? Convert.ToDateTime(reader["converted_date"])
                                        : DateTime.MinValue;

                                    string message = $"This estimate was converted to Sales Invoice: {linkedInvoiceNo}";
                                    if (convertedDate != DateTime.MinValue)
                                        message += $" on {convertedDate:yyyy-MM-dd HH:mm}";

                                    UiMessages.ShowInfo(
                                        message,
                                        $"تم تحويل عرض السعر هذا إلى فاتورة المبيعات: {linkedInvoiceNo}",
                                        "Estimate Status",
                                        "حالة عرض السعر"
                                    );
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                // Silently handle if columns don't exist yet (backward compatibility)
            }
        }

        private void StyleForm()
        {
            AppTheme.ApplyListFormStyle(panel2, lbl_taxes_title, panel1, grid_estimates_detail, id, invoice_no);
            txt_close.FlatStyle = FlatStyle.System;
            txt_close.Font = AppTheme.FontButton;
        }

        public DataTable load_estimates_detail_grid()
        {
              
            //bind data in data grid view  
            EstimatesBLL objestimatesBLL = new EstimatesBLL();
            DataTable dt = objestimatesBLL.GetAllEstimatesItems(_sale_id);
            return dt;
            
        }

       
        private void txt_search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) // Enter
            {
                txt_close.PerformClick();
            }
        }

        private void frm_estimates_detail_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

        private void txt_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CustomizeDataGridView()
        {
            if (grid_estimates_detail.Rows.Count == 0) return;

            DataGridViewRow lastRow = grid_estimates_detail.Rows[grid_estimates_detail.Rows.Count - 1];

            foreach (DataGridViewCell cell in lastRow.Cells)
            {
                cell.Style = new DataGridViewCellStyle(cell.Style)
                {
                    Font = AppTheme.FontSemiBold,
                    BackColor = SystemColors.ControlDark,
                    ForeColor = SystemColors.ControlText,
                    SelectionBackColor = SystemColors.ControlDark,
                    SelectionForeColor = SystemColors.ControlText
                };
            }
        }

    }
}
