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
    public partial class frm_purchases_detail : Form
    {

        public frm_purchases_detail()
        {
            InitializeComponent();
        }


        public void frm_purchases_detail_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            StyleForm();
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
                null, grid_purchases_detail, new object[] { true });

            grid_purchases_detail.BackgroundColor = SystemColors.AppWorkspace;
            grid_purchases_detail.RowHeadersVisible = false;
            grid_purchases_detail.ColumnHeadersHeight = 36;
            grid_purchases_detail.RowTemplate.Height = 30;
            grid_purchases_detail.DefaultCellStyle.Font = AppTheme.FontGrid;
            grid_purchases_detail.DefaultCellStyle.ForeColor = SystemColors.ControlText;
            grid_purchases_detail.DefaultCellStyle.BackColor = SystemColors.Window;
            grid_purchases_detail.ColumnHeadersDefaultCellStyle.Font = AppTheme.FontGridHeader;
            grid_purchases_detail.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.ControlText;
            grid_purchases_detail.AlternatingRowsDefaultCellStyle.BackColor = SystemColors.ControlLight;
            grid_purchases_detail.AlternatingRowsDefaultCellStyle.ForeColor = SystemColors.ControlText;

            // Hide internal data columns from the user
            id.Visible = false;
            invoice_no.Visible = false;
        }

        public void load_purchases_detail_grid(string invoice_no)
        {
            try
            {
                double _total_qty = 0;
                double _total_cost = 0;
                double _total_vat = 0;
                double _total_discount = 0;
                double _grand_total = 0;

                grid_purchases_detail.DataSource = null;

                //bind data in data grid view  
                PurchasesBLL objpurchasesBLL = new PurchasesBLL();
                grid_purchases_detail.AutoGenerateColumns = false;

                //String keyword = "id,name,date_created";
               // String table = "pos_purchases_detail";
                DataTable dt = objpurchasesBLL.GetAllPurchasesItems(invoice_no);
                
                foreach (DataRow dr in dt.Rows)
                {
                    
                    string[] row00 = {
                        dr["id"].ToString(),
                        dr["invoice_no"].ToString(),
                        dr["item_code"].ToString(),
                        dr["product_name"].ToString(),
                        dr["loc_code"].ToString(),
                        Math.Round(Convert.ToDouble(dr["quantity"]),2).ToString(),
                        Math.Round(Convert.ToDouble(dr["cost_price"]),2).ToString(),
                        Math.Round(Convert.ToDouble(dr["discount_value"]),2).ToString(),
                        Math.Round(Convert.ToDouble(dr["vat"]),2).ToString(),
                        Math.Round(Convert.ToDouble(dr["net_total"]),2).ToString()
                    };

                    _total_qty += Convert.ToDouble(dr["quantity"].ToString());
                    _total_cost += Convert.ToDouble(dr["cost_price"].ToString());
                    _total_discount += Convert.ToDouble(dr["discount_value"].ToString());
                    _total_vat += Convert.ToDouble(dr["vat"].ToString());
                    _grand_total += Convert.ToDouble(dr["net_total"].ToString());
                    
                    grid_purchases_detail.Rows.Add(row00);

                }
                string[] row12 = { "","","","","Total", _total_qty.ToString("N2"), _total_cost.ToString("N2"), _total_discount.ToString("N2"), _total_vat.ToString("N2"), _grand_total.ToString("N2") };
                grid_purchases_detail.Rows.Add(row12);
                CustomizeDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }
        private void CustomizeDataGridView()
        {
            if (grid_purchases_detail.Rows.Count == 0) return;

            DataGridViewRow lastRow = grid_purchases_detail.Rows[grid_purchases_detail.Rows.Count - 1];

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

        private void frm_purchases_detail_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

        private void txt_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
