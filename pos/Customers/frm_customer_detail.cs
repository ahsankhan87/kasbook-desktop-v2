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

namespace pos
{
    public partial class frm_customer_detail : Form
    {
        public int _customer_id;
        public string _customer_name;

        public frm_customer_detail(int customer_id,string customer_name)
        {
            _customer_id = customer_id;
            _customer_name = customer_name;
            InitializeComponent();
        }

        public frm_customer_detail()
        {
            InitializeComponent();
        }

        public void frm_customer_detail_Load(object sender, EventArgs e)
        {
            lbl_title.Text = "Customer Detail: "+_customer_name;
            load_customer_detail_grid(_customer_id);

            ViewTotalInLastRow();

        }

        public void load_customer_detail_grid(int customer_id)
        {
            try
            {
                grid_customer_detail.DataSource = null;

                //bind data in data grid view  
                GeneralBLL objBLL = new GeneralBLL();
                grid_customer_detail.AutoGenerateColumns = false;

                String keyword = "id,invoice_no,debit,credit,(debit-credit) AS balance,description,entry_date,account_id,account_name";
                String table = "pos_customers_payments WHERE customer_id = "+customer_id+"";
                
                DataTable dt = new DataTable();
                dt = objBLL.GetRecord(keyword, table);

                double _dr_total = 0;
                double _cr_total = 0;
                
                foreach (DataRow dr in dt.Rows)
                {
                    _dr_total += Convert.ToDouble(dr["debit"].ToString());
                    _cr_total += Convert.ToDouble(dr["credit"].ToString());
                    
                }

                DataRow newRow = dt.NewRow();
                newRow[8] = "Total";
                newRow[2] = _dr_total;
                newRow[3] = _cr_total;
                newRow[4] = (_dr_total-_cr_total);
                dt.Rows.InsertAt(newRow, dt.Rows.Count);
                
                grid_customer_detail.DataSource = dt;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }
        
        private void ViewTotalInLastRow()
        {
            grid_customer_detail.Rows[grid_customer_detail.Rows.Count - 1].Cells["invoice_no"].Style.BackColor = Color.LightGray;
            grid_customer_detail.Rows[grid_customer_detail.Rows.Count - 1].Cells["entry_date"].Style.BackColor = Color.LightGray;
            grid_customer_detail.Rows[grid_customer_detail.Rows.Count - 1].Cells["account_name"].Style.BackColor = Color.LightGray;
            grid_customer_detail.Rows[grid_customer_detail.Rows.Count - 1].Cells["debit"].Style.BackColor = Color.LightGray;
            grid_customer_detail.Rows[grid_customer_detail.Rows.Count - 1].Cells["credit"].Style.BackColor = Color.LightGray;
            grid_customer_detail.Rows[grid_customer_detail.Rows.Count - 1].Cells["balance"].Style.BackColor = Color.LightGray;
            grid_customer_detail.Rows[grid_customer_detail.Rows.Count - 1].Cells["description"].Style.BackColor = Color.LightGray;

            
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            load_customer_detail_grid(_customer_id);
            ViewTotalInLastRow();
        }

        
        private void frm_customer_detail_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void btn_payment_Click(object sender, EventArgs e)
        {
            //frm_customer_payment obj = new frm_customer_payment(this,_customer_id);
            //obj.ShowDialog();
            ViewTotalInLastRow();
        }

    }
}
