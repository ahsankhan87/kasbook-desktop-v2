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
using CrystalDecisions.CrystalReports.Engine;

namespace pos
{
    public partial class frm_all_estimates : Form
    {

        public frm_all_estimates()
        {
            InitializeComponent();
        }

        public void frm_all_estimates_Load(object sender, EventArgs e)
        {
            load_all_estimates_grid();
        }

        public void load_all_estimates_grid()
        {
            try
            {
                grid_all_estimates.DataSource = null;

                //bind data in data grid view  
                EstimatesBLL objEstimatesBLL = new EstimatesBLL();
                grid_all_estimates.AutoGenerateColumns = false;

                //String keyword = "id,name,date_created";
               // String table = "pos_all_estimates";
                grid_all_estimates.DataSource = objEstimatesBLL.GetAllEstimates();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            load_all_estimates_grid();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                EstimatesBLL objBLL = new EstimatesBLL();
                    
                String condition = txt_search.Text;
                grid_all_estimates.DataSource = objBLL.SearchRecord(condition);

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

        private void grid_all_estimates_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var sale_id = grid_all_estimates.CurrentRow.Cells["id"].Value.ToString(); // retreive the current row
                var invoice_no = grid_all_estimates.CurrentRow.Cells["invoice_no"].Value.ToString();
                
                load_estimates_items_detail(Convert.ToInt16(sale_id), invoice_no);
            }
        }

        private void grid_all_estimates_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var sale_id = grid_all_estimates.CurrentRow.Cells["id"].Value.ToString(); // retreive the current row
            var invoice_no = grid_all_estimates.CurrentRow.Cells["invoice_no"].Value.ToString();
            
            load_estimates_items_detail(Convert.ToInt16(sale_id), invoice_no);
        }

        private void load_estimates_items_detail(int sale_id, string invoice_no)
        {
            frm_estimates_detail frm_estimates_detail_obj = new frm_estimates_detail(sale_id, invoice_no);
            frm_estimates_detail_obj.load_estimates_detail_grid();
            frm_estimates_detail_obj.ShowDialog();
        }

        private void frm_all_estimates_KeyDown(object sender, KeyEventArgs e)
        {

        }

        public DataTable load_estimates_receipt()
        {
           
                var invoice_no = grid_all_estimates.CurrentRow.Cells["invoice_no"].Value.ToString();
                //bind data in data grid view  
                EstimatesBLL objEstimatesBLL = new EstimatesBLL();
                DataTable dt = objEstimatesBLL.SaleReceipt(invoice_no);
                return dt;

            
        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            using (frm_sales_receipt obj = new frm_sales_receipt(load_estimates_receipt()))
            {
                obj.ShowDialog();
            }
        }

        private void btn_print_invoice_Click(object sender, EventArgs e)
        {
            if (grid_all_estimates.Rows.Count > 0)
            {
                // PRINT INVOICE
                DialogResult result1 = MessageBox.Show("Print invoice with product code?", "Print Invoice", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

                bool isPrintInvoiceCode = false;
                if (result1 == DialogResult.Yes)
                {
                    isPrintInvoiceCode = true;
                }

                using (frm_sales_invoice obj = new frm_sales_invoice(load_estimates_receipt(), false, isPrintInvoiceCode))
                {
                    obj.ShowDialog();
                }
            }
        }

        private void grid_all_estimates_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string name = grid_all_estimates.Columns[e.ColumnIndex].Name;
            if (name == "detail")
            {
                var sale_id = grid_all_estimates.CurrentRow.Cells["id"].Value.ToString(); // retreive the current row
                var invoice_no = grid_all_estimates.CurrentRow.Cells["invoice_no"].Value.ToString();

                load_estimates_items_detail(Convert.ToInt16(sale_id), invoice_no);

            }
            if (name == "btn_delete")
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete", "Delete Transaction", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    var invoice_no = grid_all_estimates.CurrentRow.Cells["invoice_no"].Value.ToString();

                    EstimatesBLL estimatesBLL = new EstimatesBLL();
                    int qresult = estimatesBLL.DeleteEstimates(invoice_no);
                    if (qresult > 0)
                    {
                        MessageBox.Show(invoice_no + " deleted successfully", "Delete Estimates", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        load_all_estimates_grid();
                    }
                    else
                    {
                        MessageBox.Show(invoice_no + " not deleted, please try again", "Delete Estimates", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                }
            }
        }

        
    }
}
