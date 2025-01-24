using POS.BLL;
using POS.DLL;
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
    public partial class frm_search_porder : Form
    {
        private frm_purchases purchasesForm;
        private frm_purchases_order purchase_order_form;
        
        public frm_search_porder(frm_purchases purchasesForm,frm_purchases_order purchase_order_form=null)
        {
            this.purchasesForm = purchasesForm;
            this.purchase_order_form = purchase_order_form;
            
            InitializeComponent();
        }

        public frm_search_porder()
        {
            InitializeComponent();

        }

        private void frm_search_porder_Load(object sender, EventArgs e)
        {
            load_porder_grid(); 
            grid_search_porder.Focus();
            
        }

        public void load_porder_grid()
        {
            try
            {
                grid_search_porder.DataSource = null;

                Purchases_orderBLL objBLL = new Purchases_orderBLL();
                grid_search_porder.AutoGenerateColumns = false;

                grid_search_porder.DataSource = objBLL.GetAllActivePOrder();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }
         
        private void btn_ok_Click(object sender, EventArgs e)
        {
            try
            {
                Purchases_orderBLL purchasesObj = new Purchases_orderBLL();
                DataTable porder_dt = new DataTable();
                string inv_no = "";

                if (grid_search_porder.SelectedCells.Count > 0)
                {
                    inv_no = grid_search_porder.CurrentRow.Cells["invoice_no"].Value.ToString();

                    porder_dt = purchasesObj.GetAllPurchaseOrder(inv_no);

                    if (purchasesForm != null)
                    {
                        purchasesForm.Load_products_to_grid_by_invoiceno(porder_dt, inv_no);
                    
                    }else if(purchase_order_form != null)
                    {
                        purchase_order_form.Load_products_to_grid_by_invoiceno(porder_dt, inv_no);
                    
                    }
                    
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Please select record", "porder", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                grid_search_porder.DataSource = null;

                //bind data in data grid view  
                Purchases_orderBLL objBLL = new Purchases_orderBLL();
                grid_search_porder.AutoGenerateColumns = false;

                String condition = txt_search.Text.Trim();
                grid_search_porder.DataSource = objBLL.SearchRecord(condition);


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void grid_search_porder_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_ok.PerformClick();
            }
        }

        private void grid_search_porder_DoubleClick(object sender, EventArgs e)
        {
            btn_ok.PerformClick();
        }

        private void txt_search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) // Enter
            {
                btn_search.PerformClick();
            }
        }

    }
}
     