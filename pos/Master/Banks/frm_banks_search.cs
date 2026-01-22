using POS.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos.Master.Banks
{
    public partial class frm_banks_search : Form
    {
        private frm_banks mainForm;
        string _search = "";

        public bool _returnStatus = false;

        public frm_banks_search(frm_banks mainForm, string search)
        {
            this.mainForm = mainForm;
            _search = search;

            InitializeComponent();
        }

        public frm_banks_search()
        {
            InitializeComponent();
        }

        private void frm_banks_search_Load(object sender, EventArgs e)
        {
            txt_search.Text = _search;
            load_customers_grid();
            grid_search_banks.Focus();
        }

        public void load_customers_grid()
        {
            try
            {
                grid_search_banks.DataSource = null;

                //bind data in data grid view  
                BankBLL objBLL = new BankBLL();
                grid_search_banks.AutoGenerateColumns = false;

                String condition = txt_search.Text.Trim();

                grid_search_banks.DataSource = objBLL.SearchRecord(condition);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (grid_search_banks.SelectedCells.Count > 0)
            {
                string customer_id = grid_search_banks.CurrentRow.Cells["id"].Value.ToString();


                if (mainForm != null)
                {
                    mainForm.load_bank_detail(int.Parse(customer_id));
                    mainForm.load_banks_transactions_grid(int.Parse(customer_id));
                }


                this.Close();
            }
            else
            {
                MessageBox.Show("Please select record", "Customers", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txt_search_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (txt_search.Text != "")
                {
                    //bind data in data grid view  
                    BankBLL objBLL = new BankBLL();
                    grid_search_banks.AutoGenerateColumns = false;

                    String condition = txt_search.Text.Trim();
                    grid_search_banks.DataSource = objBLL.SearchRecord(condition);

                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void grid_search_customers_DoubleClick(object sender, EventArgs e)
        {
            btn_ok.PerformClick();
        }

        private void grid_search_customers_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_ok.PerformClick();
            }
        }

        private void frm_banks_search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Down)
            {
                grid_search_banks.Focus();
            }
            if(e.KeyData == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
