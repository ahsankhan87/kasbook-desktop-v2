using pos.Security.Authorization;
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
    public partial class frm_all_purchases : Form
    {
        PurchasesBLL objBLL = new PurchasesBLL();

        // Use centralized, DB-backed authorization and current user
        private readonly IAuthorizationService _auth = AppSecurityContext.Auth;
        private UserIdentity _currentUser = AppSecurityContext.User;

        public frm_all_purchases()
        {
            InitializeComponent();
        }


        public void frm_all_purchases_Load(object sender, EventArgs e)
        {
            load_all_purchases_grid();
        }

        public void load_all_purchases_grid()
        {
            try
            {
                grid_all_purchases.DataSource = null;

                //bind data in data grid view  
                PurchasesBLL objpurchasesBLL = new PurchasesBLL();
                grid_all_purchases.AutoGenerateColumns = false;

                //String keyword = "id,name,date_created";
               // String table = "pos_all_purchases";
                grid_all_purchases.DataSource = objpurchasesBLL.GetAllPurchases();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }


        private void btn_refresh_Click(object sender, EventArgs e)
        {
            load_all_purchases_grid();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                    
                String condition = txt_search.Text;
                if(!string.IsNullOrEmpty(condition))
                {
                    grid_all_purchases.DataSource = objBLL.SearchRecord(condition);
                }
                    

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

        private void frm_all_purchases_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void grid_all_purchases_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var invoice_no = grid_all_purchases.CurrentRow.Cells["invoice_no"].Value.ToString(); // retreive the current row

                load_purchases_items_detail(invoice_no);
            }
        }

        private void grid_all_purchases_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var invoice_no = grid_all_purchases.CurrentRow.Cells["invoice_no"].Value.ToString(); // retreive the current row

            load_purchases_items_detail(invoice_no);
        }

        private void load_purchases_items_detail(string invoice_no)
        {
            frm_purchases_detail frm_purchases_detail_obj = new frm_purchases_detail();
            frm_purchases_detail_obj.load_purchases_detail_grid(invoice_no);
            frm_purchases_detail_obj.ShowDialog();
        }

        private void grid_all_purchases_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string name = grid_all_purchases.Columns[e.ColumnIndex].Name;
                if (name == "detail")
                {
                    var sale_id = grid_all_purchases.CurrentRow.Cells["id"].Value.ToString(); // retreive the current row
                    var invoice_no = grid_all_purchases.CurrentRow.Cells["invoice_no"].Value.ToString();

                    load_purchases_items_detail(invoice_no);

                }
                if (name == "btn_delete")
                {
                    // Permission check
                    if(!_auth.HasPermission(_currentUser, Permissions.Purchases_Delete))
                    {
                        MessageBox.Show("You do not have permission to delete purchases.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    DialogResult result = MessageBox.Show("Are you sure you want to delete", "Sale Transaction", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {

                        var invoice_no = grid_all_purchases.CurrentRow.Cells["invoice_no"].Value.ToString();

                        int qresult = objBLL.DeletePurchases(invoice_no);
                        if (qresult > 0)
                        {
                            MessageBox.Show(invoice_no + " deleted successfully", "Delete Sales", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            load_all_purchases_grid();
                        }
                        else
                        {
                            MessageBox.Show(invoice_no + " not deleted, please try again", "Delete Sales", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_print_invoice_Click(object sender, EventArgs e)
        {
            if(grid_all_purchases.Rows.Count > 0)
            {
                // permission check
                if(!_auth.HasPermission(_currentUser, Permissions.Purchases_Print))
                {
                    MessageBox.Show("You do not have permission to print purchase invoices.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (frm_purchase_invoice obj = new frm_purchase_invoice(load_purchase_receipt(), false))
                {
                    obj.ShowDialog();
                }
            }
            
        }

       
        public DataTable load_purchase_receipt()
        {
            //DataTable purchase_dt = new DataTable();
            
            if (grid_all_purchases.Rows.Count > 0)
            {
                var invoice_no = grid_all_purchases.CurrentRow.Cells["invoice_no"].Value.ToString();
                //bind data in data grid view  
                return objBLL.PurchaseReceipt(invoice_no);
                
            }
            return null;

        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(grid_all_purchases.CurrentRow.Cells["invoice_no"].Value.ToString());

        }

        private void BtnSupplierNameChange_Click(object sender, EventArgs e)
        {
            try
            {
                if (grid_all_purchases.Rows.Count > 0)
                {
                    string invoiceNo = grid_all_purchases.CurrentRow.Cells["invoice_no"].Value.ToString();
                    pos.Suppliers.ChangeSupplierName supplierNameChange = new Suppliers.ChangeSupplierName(invoiceNo);
                    supplierNameChange.ShowDialog();
                    load_all_purchases_grid();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
