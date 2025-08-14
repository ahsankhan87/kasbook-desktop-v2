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
    public partial class frm_search_estimates : Form
    {
        private frm_sales salesForm;
        
        public frm_search_estimates(frm_sales salesForm)
        {
            
            InitializeComponent();
            this.salesForm = salesForm;

        }

        public frm_search_estimates()
        {
            InitializeComponent();

        }

        private void frm_search_estimates_Load(object sender, EventArgs e)
        {
            load_estimates_grid(); 
            grid_search_estimates.Focus();
            
        }

        public void load_estimates_grid()
        {
            try
            {
                grid_search_estimates.DataSource = null;

                EstimatesBLL objBLL = new EstimatesBLL();
                grid_search_estimates.AutoGenerateColumns = false;

                grid_search_estimates.DataSource = objBLL.GetAllActiveEstimates();
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
                SalesBLL salesObj = new SalesBLL();
                DataTable estimates_dt = new DataTable();
                string inv_no = "";

                if (grid_search_estimates.SelectedCells.Count > 0)
                {
                    inv_no = grid_search_estimates.CurrentRow.Cells["invoice_no"].Value.ToString();

                    estimates_dt = salesObj.GetEstimatesAndEstimatesItems(inv_no);

                    salesForm.Load_products_to_grid_by_invoiceno(estimates_dt, inv_no);
                    
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Please select record", "estimates", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
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

        private void grid_search_estimates_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_ok.PerformClick();
            }
        }

        private void grid_search_estimates_DoubleClick(object sender, EventArgs e)
        {
            btn_ok.PerformClick();
        }

        private void txt_search_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                grid_search_estimates.DataSource = null;

                //bind data in data grid view  
                EstimatesBLL objBLL = new EstimatesBLL();
                grid_search_estimates.AutoGenerateColumns = false;

                String condition = txt_search.Text.Trim();
                grid_search_estimates.DataSource = objBLL.SearchRecord(condition);


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
     