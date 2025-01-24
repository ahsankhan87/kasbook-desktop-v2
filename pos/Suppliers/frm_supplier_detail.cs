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
    public partial class frm_supplier_detail : Form
    {
        public int _supplier_id;
        public string _supplier_name;

        public frm_supplier_detail(int supplier_id,string supplier_name)
        {
            _supplier_id = supplier_id;
            _supplier_name = supplier_name;
            InitializeComponent();
        }

        public frm_supplier_detail()
        {
            InitializeComponent();
        }

        public void frm_supplier_detail_Load(object sender, EventArgs e)
        {
            lbl_title.Text = "Supplier Detail: "+_supplier_name;
            load_supplier_detail_grid(_supplier_id);

            ViewTotalInLastRow();

        }

        public void load_supplier_detail_grid(int supplier_id)
        {
            try
            {
                grid_supplier_detail.DataSource = null;

                //bind data in data grid view  
                GeneralBLL objBLL = new GeneralBLL();
                grid_supplier_detail.AutoGenerateColumns = false;

                String keyword = "id,invoice_no,debit,credit,(debit-credit) AS balance,description,entry_date,account_id,account_name";
                String table = "pos_suppliers_payments WHERE supplier_id = "+supplier_id+"";
                
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
                
                grid_supplier_detail.DataSource = dt;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }
        
        private void ViewTotalInLastRow()
        {
            grid_supplier_detail.Rows[grid_supplier_detail.Rows.Count - 1].Cells["invoice_no"].Style.BackColor = Color.LightGray;
            grid_supplier_detail.Rows[grid_supplier_detail.Rows.Count - 1].Cells["entry_date"].Style.BackColor = Color.LightGray;
            grid_supplier_detail.Rows[grid_supplier_detail.Rows.Count - 1].Cells["account_name"].Style.BackColor = Color.LightGray;
            grid_supplier_detail.Rows[grid_supplier_detail.Rows.Count - 1].Cells["debit"].Style.BackColor = Color.LightGray;
            grid_supplier_detail.Rows[grid_supplier_detail.Rows.Count - 1].Cells["credit"].Style.BackColor = Color.LightGray;
            grid_supplier_detail.Rows[grid_supplier_detail.Rows.Count - 1].Cells["balance"].Style.BackColor = Color.LightGray;
            grid_supplier_detail.Rows[grid_supplier_detail.Rows.Count - 1].Cells["description"].Style.BackColor = Color.LightGray;

            
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            load_supplier_detail_grid(_supplier_id);
            ViewTotalInLastRow();
        }

        
        private void frm_supplier_detail_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void btn_payment_Click(object sender, EventArgs e)
        {
            //frm_supplier_payment obj = new frm_supplier_payment(this,_supplier_id);
            //obj.ShowDialog();
            //ViewTotalInLastRow();
        }

    }
}
