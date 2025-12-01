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
    public partial class frm_search_users : Form
    {
        private frm_adduser mainForm;
        
        string _search = "";
        
        public bool _returnStatus = false;

        public frm_search_users(frm_adduser mainForm, string search)
        {
            this.mainForm = mainForm;
            
            _search = search;
            
            InitializeComponent();
        }

        public frm_search_users()
        {
            InitializeComponent();

        }

        private void frm_search_users_Load(object sender, EventArgs e)
        {
            txt_search.Text = _search;
            load_users_grid();
            grid_search_users.Focus();
        }

        public void load_users_grid()
        {
            try
            {
                grid_search_users.DataSource = null;

                //bind data in data grid view  
                UsersBLL objBLL = new UsersBLL();
                grid_search_users.AutoGenerateColumns = false;

                String condition = txt_search.Text.Trim();

                //if (condition != "")
                //{
                    grid_search_users.DataSource = objBLL.SearchRecord(condition);

                //}


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (grid_search_users.SelectedCells.Count > 0)
            {
                string user_id = grid_search_users.CurrentRow.Cells["id"].Value.ToString();
                
                
                if(mainForm != null)
                {
                    mainForm.load_user_detail(int.Parse(user_id));
                    mainForm.load_user_rights(int.Parse(user_id));
                    mainForm.load_user_commission_grid(int.Parse(user_id));
                    mainForm.check_all_modules(int.Parse(user_id));
                    
                }

                
                this.Close();
            }
            else
            {
                MessageBox.Show("Please select record", "users", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void grid_search_users_KeyDown(object sender, KeyEventArgs e)
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

        private void grid_search_users_DoubleClick(object sender, EventArgs e)
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
                    load_users_grid();

                }
                

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void frm_search_users_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Down)
            {
                grid_search_users.Focus();
            }
        }
        
    }
}
     