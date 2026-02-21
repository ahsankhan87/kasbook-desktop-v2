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
    public partial class frm_purchases_orders_detail : Form
    {

        public frm_purchases_orders_detail()
        {
            InitializeComponent();
        }


        public void frm_purchases_orders_detail_Load(object sender, EventArgs e)
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
                null, grid_purchases_orders_detail, new object[] { true });

            grid_purchases_orders_detail.BackgroundColor = SystemColors.AppWorkspace;
            grid_purchases_orders_detail.RowHeadersVisible = false;
            grid_purchases_orders_detail.ColumnHeadersHeight = 36;
            grid_purchases_orders_detail.RowTemplate.Height = 30;
            grid_purchases_orders_detail.DefaultCellStyle.Font = AppTheme.FontGrid;
            grid_purchases_orders_detail.DefaultCellStyle.ForeColor = SystemColors.ControlText;
            grid_purchases_orders_detail.DefaultCellStyle.BackColor = SystemColors.Window;
            grid_purchases_orders_detail.ColumnHeadersDefaultCellStyle.Font = AppTheme.FontGridHeader;
            grid_purchases_orders_detail.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.ControlText;
            grid_purchases_orders_detail.AlternatingRowsDefaultCellStyle.BackColor = SystemColors.ControlLight;
            grid_purchases_orders_detail.AlternatingRowsDefaultCellStyle.ForeColor = SystemColors.ControlText;

            // Hide internal data columns from the user
            id.Visible = false;
            invoice_no.Visible = false;
        }

        public void load_purchases_orders_detail_grid(int sale_id)
        {
            try
            {
                grid_purchases_orders_detail.DataSource = null;

                //bind data in data grid view  
                Purchases_orderBLL objPurchases_orderBLL = new Purchases_orderBLL();
                grid_purchases_orders_detail.AutoGenerateColumns = false;

                //String keyword = "id,name,date_created";
                // String table = "pos_purchases_orders_detail";
                var dt = objPurchases_orderBLL.GetAllPurchases_orderItems(sale_id); // Assuming GetAllPurchasesOrders returns a DataTable

                if (dt != null && dt.Rows.Count > 0)
                {
                    // Add grand total row to the DataTable itself
                    AddGrandTotalRowToDataTable(dt);
                    grid_purchases_orders_detail.DataSource = dt;
                    MakeLastRowBold();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }
        private void AddGrandTotalRowToDataTable(DataTable dt)
        {
            decimal totalQty = 0;
            decimal totalCostPrice = 0;
            decimal totalTax = 0;
            decimal grandTotal = 0;

            foreach (DataRow row in dt.Rows)
            {
                totalQty += Convert.ToDecimal(row["quantity"] ?? 0);
                totalCostPrice += Convert.ToDecimal(row["cost_price"] ?? 0);
                totalTax += Convert.ToDecimal(row["tax"] ?? 0);

                decimal lineTotal = (Convert.ToDecimal(row["cost_price"] ?? 0) +
                                    Convert.ToDecimal(row["tax"] ?? 0)) *
                                    Convert.ToDecimal(row["quantity"] ?? 0); 
                grandTotal += lineTotal;
            }

            // Create a new row
            DataRow totalRow = dt.NewRow();
            totalRow["product_name"] = "Grand Total:";
            totalRow["quantity"] = totalQty;
            totalRow["cost_price"] = totalCostPrice;
            totalRow["tax"] = totalTax;
            totalRow["total"] = grandTotal;

            dt.Rows.Add(totalRow);
        }
        private void MakeLastRowBold()
        {
            if (grid_purchases_orders_detail.Rows.Count == 0) return;

            DataGridViewRow lastRow = grid_purchases_orders_detail.Rows[grid_purchases_orders_detail.Rows.Count - 1];

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

        private void frm_purchases_orders_detail_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

        private void txt_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
