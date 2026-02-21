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
    public partial class frm_sales_detail : Form
    {
        int _sale_id;
        string _invoice_no;

        public frm_sales_detail(int sale_id,string invoice_no)
        {
            InitializeComponent();
            _sale_id = sale_id;
            _invoice_no = invoice_no;
        }


        public void frm_sales_detail_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            StyleForm();

            //load_sales_detail_grid(sale_id);
            try
            {
                double _total_qty = 0;
                double _total_cost = 0;
                double _total_vat = 0;
                double _total_discount = 0;
                double _grand_total = 0;

                grid_sales_detail.DataSource = null;
                grid_sales_detail.AutoGenerateColumns = false;
                SalesBLL objSalesBLL = new SalesBLL();
                DataTable dt = objSalesBLL.GetAllSalesItems(_invoice_no);

                foreach (DataRow dr in dt.Rows)
                {

                    string[] row00 = {
                        dr["id"].ToString(),
                        dr["invoice_no"].ToString(),
                        dr["item_code"].ToString(),
                        dr["item_name"].ToString(),
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

                    grid_sales_detail.Rows.Add(row00);

                }
                string[] row12 = { "", "", "", "", "Total", _total_qty.ToString("N2"), _total_cost.ToString("N2"), _total_discount.ToString("N2"), _total_vat.ToString("N2"), _grand_total.ToString("N2") };
                grid_sales_detail.Rows.Add(row12);

                CustomizeDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void StyleForm()
        {
            // ── Header panel ──────────────────────────────────────────
            panel2.BackColor = AppTheme.PrimaryDark;
            panel2.ForeColor = Color.White;
            lbl_taxes_title.Font = AppTheme.FontHeader;
            lbl_taxes_title.ForeColor = Color.White;

            txt_close.FlatStyle = FlatStyle.System;
            txt_close.Font = AppTheme.FontButton;

            // ── Body panel ────────────────────────────────────────────
            panel1.BackColor = SystemColors.Control;

            // ── Grid ──────────────────────────────────────────────────
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.SetProperty,
                null, grid_sales_detail, new object[] { true });

            grid_sales_detail.BackgroundColor = SystemColors.AppWorkspace;
            grid_sales_detail.RowHeadersVisible = false;
            grid_sales_detail.ColumnHeadersHeight = 36;
            grid_sales_detail.RowTemplate.Height = 30;
            grid_sales_detail.DefaultCellStyle.Font = AppTheme.FontGrid;
            grid_sales_detail.DefaultCellStyle.ForeColor = SystemColors.ControlText;
            grid_sales_detail.DefaultCellStyle.BackColor = SystemColors.Window;
            grid_sales_detail.ColumnHeadersDefaultCellStyle.Font = AppTheme.FontGridHeader;
            grid_sales_detail.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.ControlText;
            grid_sales_detail.AlternatingRowsDefaultCellStyle.BackColor = SystemColors.ControlLight;
            grid_sales_detail.AlternatingRowsDefaultCellStyle.ForeColor = SystemColors.ControlText;

            // Hide internal columns
            id.Visible = false;
            invoice_no.Visible = false;
        }
        private void CustomizeDataGridView()
        {
            if (grid_sales_detail.Rows.Count == 0) return;

            DataGridViewRow lastRow = grid_sales_detail.Rows[grid_sales_detail.Rows.Count - 1];

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

        private void txt_search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) // Enter
            {
                txt_close.PerformClick();
            }
        }

        private void frm_sales_detail_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

        private void txt_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
    }
}
