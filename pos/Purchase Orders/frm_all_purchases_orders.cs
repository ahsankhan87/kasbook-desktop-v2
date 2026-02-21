using pos.Security.Authorization;
using pos.UI;
using POS.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos
{
    public partial class frm_all_purchases_orders : Form
    {
        Purchases_orderBLL objPurchases_orderBLL = new Purchases_orderBLL();
        
        // Use centralized, DB-backed authorization and current user
        private readonly IAuthorizationService _auth = AppSecurityContext.Auth;
        private UserIdentity _currentUser = AppSecurityContext.User;

        public frm_all_purchases_orders()
        {
            InitializeComponent();
        }

        public void frm_all_purchases_orders_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            StyleForm();
            load_all_purchases_orders_grid();

            grid_all_purchases_orders.Columns["delete"].Visible = _auth.HasPermission(_currentUser, Permissions.PurchaseOrders_Delete);
        }

        private void StyleForm()
        {
            // ── Header panel ──────────────────────────────────────────
            panel2.BackColor = AppTheme.PrimaryDark;
            panel2.ForeColor = Color.White;
            lbl_taxes_title.Font = AppTheme.FontHeader;
            lbl_taxes_title.ForeColor = Color.White;

            // ── Body panel ────────────────────────────────────────────
            panel1.BackColor = SystemColors.Control;

            // ── Grid ──────────────────────────────────────────────────
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.SetProperty,
                null, grid_all_purchases_orders, new object[] { true });

            grid_all_purchases_orders.BackgroundColor = SystemColors.AppWorkspace;
            grid_all_purchases_orders.RowHeadersVisible = false;
            grid_all_purchases_orders.ColumnHeadersHeight = 36;
            grid_all_purchases_orders.RowTemplate.Height = 30;
            grid_all_purchases_orders.DefaultCellStyle.Font = AppTheme.FontGrid;
            grid_all_purchases_orders.DefaultCellStyle.ForeColor = SystemColors.ControlText;
            grid_all_purchases_orders.DefaultCellStyle.BackColor = SystemColors.Window;
            grid_all_purchases_orders.ColumnHeadersDefaultCellStyle.Font = AppTheme.FontGridHeader;
            grid_all_purchases_orders.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.ControlText;
            grid_all_purchases_orders.AlternatingRowsDefaultCellStyle.BackColor = SystemColors.ControlLight;
            grid_all_purchases_orders.AlternatingRowsDefaultCellStyle.ForeColor = SystemColors.ControlText;

            // Hide internal id column
            id.Visible = false;
        }

        public void load_all_purchases_orders_grid()
        {
            try
            {
                grid_all_purchases_orders.DataSource = null;
                grid_all_purchases_orders.Rows.Clear();
                grid_all_purchases_orders.Refresh();
                //bind data in data grid view  
                grid_all_purchases_orders.AutoGenerateColumns = false;

                //String keyword = "id,name,date_created";
               // String table = "pos_all_purchases_orders";
                grid_all_purchases_orders.DataSource = objPurchases_orderBLL.GetAllPurchasesOrders();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }

      

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            load_all_purchases_orders_grid();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                grid_all_purchases_orders.DataSource = null;
                grid_all_purchases_orders.Rows.Clear();
                grid_all_purchases_orders.Refresh();
                String condition = txt_search.Text;
                grid_all_purchases_orders.DataSource = objPurchases_orderBLL.SearchRecord(condition);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void txt_search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) // Enter
            {
                btn_search.PerformClick();
            }
        }

        private void frm_all_purchases_orders_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void grid_all_purchases_orders_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var sale_id = grid_all_purchases_orders.CurrentRow.Cells["id"].Value.ToString(); // retreive the current row

                load_purchases_items_detail(Convert.ToInt16(sale_id));
            }
        }

        private void grid_all_purchases_orders_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var sale_id = grid_all_purchases_orders.CurrentRow.Cells["id"].Value.ToString(); // retreive the current row

            load_purchases_items_detail(Convert.ToInt16(sale_id));
        }

        private void load_purchases_items_detail(int sale_id)
        {
            frm_purchases_orders_detail frm_purchases_detail_obj = new frm_purchases_orders_detail();
            frm_purchases_detail_obj.load_purchases_orders_detail_grid(sale_id);
            frm_purchases_detail_obj.ShowDialog();
        }

        private void grid_all_purchases_orders_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string name = grid_all_purchases_orders.Columns[e.ColumnIndex].Name;
                if (name == "detail")
                {
                    var sale_id = grid_all_purchases_orders.CurrentRow.Cells["id"].Value.ToString(); // retreive the current row

                    load_purchases_items_detail(Convert.ToInt16(sale_id));
                }
                if (name == "delete")
                {
                    
                    DialogResult result = MessageBox.Show("Are you sure you want to delete", "Purchase Order", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {

                        var id = grid_all_purchases_orders.CurrentRow.Cells["id"].Value.ToString(); // retreive the current row
                        var invoice_no = grid_all_purchases_orders.CurrentRow.Cells["invoice_no"].Value.ToString(); // retreive the current row
                        Purchases_orderBLL purchases_OrderBLL = new Purchases_orderBLL();
                        
                        int qresult = purchases_OrderBLL.DeletePurchasesOrder(invoice_no);
                        if (qresult > 0)
                        {
                            MessageBox.Show("Purchase order deleted successfully", "Purchase Order", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            load_all_purchases_orders_grid();
                        }
                        else
                        {
                            MessageBox.Show(invoice_no + " not deleted, please try again", "Delete Purchase Order", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Purchase Order", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            
        }

        private void btn_print_invoice_Click(object sender, EventArgs e)
        {
            if (grid_all_purchases_orders.Rows.Count > 0)
            {
                string invoice_no = grid_all_purchases_orders.CurrentRow.Cells["invoice_no"].Value.ToString();
                if (!string.IsNullOrEmpty(invoice_no))
                {
                    using (frm_purchase_order_report obj = new frm_purchase_order_report(invoice_no, false))
                    {
                        obj.ShowDialog();
                    }
                }
            }
        }

        public DataTable load_sales_receipt()
        {
            DataTable dt = new DataTable();
            if (grid_all_purchases_orders.Rows.Count > 0)
            {
                var invoice_no = grid_all_purchases_orders.CurrentRow.Cells["invoice_no"].Value.ToString();
                //bind data in data grid view  
                dt = objPurchases_orderBLL.GetAllPurchaseOrder(invoice_no);
            }
            return dt;

        }

    }
}
