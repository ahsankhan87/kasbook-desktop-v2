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
    public partial class frm_search_suppliers : Form
    {
        private frm_addSupplier mainForm;
        
        string _search = "";
        
        public bool _returnStatus = false;

        public frm_search_suppliers(frm_addSupplier mainForm, string search)
        {
            this.mainForm = mainForm;
            
            _search = search;
            
            InitializeComponent();
        }

        public frm_search_suppliers()
        {
            InitializeComponent();

        }

        private void frm_search_suppliers_Load(object sender, EventArgs e)
        {
            txt_search.Text = _search;
            load_suppliers_grid();
            grid_search_suppliers.Focus();
        }

        public void load_suppliers_grid()
        {
            try
            {
                grid_search_suppliers.DataSource = null;

                //bind data in data grid view  
                SupplierBLL objBLL = new SupplierBLL();
                grid_search_suppliers.AutoGenerateColumns = false;

                String condition = txt_search.Text.Trim();

                //if (condition != "")
                //{
                    grid_search_suppliers.DataSource = objBLL.SearchRecord(condition);

                //}


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (grid_search_suppliers.SelectedCells.Count > 0)
            {
                string Supplier_id = grid_search_suppliers.CurrentRow.Cells["id"].Value.ToString();
                
                
                if(mainForm != null)
                {
                    mainForm.load_detail(int.Parse(Supplier_id));
                    mainForm.load_transactions_grid(int.Parse(Supplier_id));
                }

                
                this.Close();
            }
            else
            {
                MessageBox.Show("Please select record", "suppliers", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void grid_search_suppliers_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_ok.PerformClick();
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void grid_search_suppliers_DoubleClick(object sender, EventArgs e)
        {
            btn_ok.PerformClick();
        }


        private void txt_search_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if(txt_search.Text != "")
                {
                    //bind data in data grid view  
                    SupplierBLL objBLL = new SupplierBLL();
                    grid_search_suppliers.AutoGenerateColumns = false;

                    String condition = txt_search.Text.Trim();
                    grid_search_suppliers.DataSource = objBLL.SearchRecord(condition);

                }
                

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void frm_search_suppliers_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Down)
            {
                grid_search_suppliers.Focus();
            }
        }
        
    }
}
     