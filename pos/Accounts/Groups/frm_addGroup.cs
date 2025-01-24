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
using POS.BLL;
using POS.Core;

namespace pos
{
    public partial class frm_addGroup : Form
    {
        public static frm_addGroup instance;
        public TextBox tb_id;
        public TextBox tb_name;
        public TextBox tb_name_2;
        public ComboBox tb_account_type;
        public ComboBox tb_parent_id;
        public Label tb_lbl_is_edit;
        private frm_groups mainForm;
        
        public frm_addGroup(frm_groups mainForm): this()
        {
            this.mainForm = mainForm;
        }

        public frm_addGroup()
        {
            InitializeComponent();
            instance = this;
            
            get_account_types_dropdownlist();
            get_parent_group_dropdownlist();
            
            tb_id = txt_id;
            tb_name = txt_name;
            tb_name_2 = txt_name_2;
            tb_account_type = cmb_account_types;
            tb_parent_id = cmb_parent_id;

            tb_lbl_is_edit = lbl_edit_status;

        }

        public void frm_addGroup_Load(object sender, EventArgs e)
        {
            
            if (lbl_edit_status.Text == "true")
            {
                btn_save.Text = "Update";
                lbl_header_title.Text = "Update Groupes";
                
            }
            else
            {
                btn_save.Text = "Save";
            }
        }

        public void get_account_types_dropdownlist()
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "id,name";
            string table = "acc_account_type";

            DataTable dt = generalBLL_obj.GetRecord(keyword, table);
            //DataRow emptyRow = taxes.NewRow();
            //emptyRow[0] = Convert.ToInt32("0");              // Set Column Value
            //taxes.Rows.InsertAt(emptyRow, 0);

            cmb_account_types.DataSource = dt;

            cmb_account_types.DisplayMember = "name";
            cmb_account_types.ValueMember = "id";
        }


        public void get_parent_group_dropdownlist()
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "id,name";
            string table = "acc_groups WHERE parent_id = 0";

            DataTable dt = generalBLL_obj.GetRecord(keyword, table);
            DataRow emptyRow = dt.NewRow();
            emptyRow[0] = 0;              // Set Column Value
            emptyRow[1] = "root";              // Set Column Value
            dt.Rows.InsertAt(emptyRow, 0);

            cmb_parent_id.DataSource = dt;

            cmb_parent_id.DisplayMember = "name";
            cmb_parent_id.ValueMember = "id";
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            
                
            if (txt_name.Text != string.Empty)
            {
                GroupsModal info = new GroupsModal();
                info.name = txt_name.Text;
                info.name_2 = txt_name_2.Text;
                info.description = txt_description.Text;
                info.code = txt_group_code.Text;
                info.account_type_id = Convert.ToInt32(cmb_account_types.SelectedValue.ToString());
                info.parent_id = Convert.ToInt32(cmb_parent_id.SelectedValue.ToString()); 
                    
                GroupsBLL objBLL = new GroupsBLL();
                    
                if (lbl_edit_status.Text == "true")
                {
                    info.id = int.Parse(txt_id.Text);
                    
                    int result = objBLL.Update(info);
                    if (result > 0)
                    {
                        MessageBox.Show("Record updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Record not saved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    int result = objBLL.Insert(info);
                    if (result > 0)
                    {
                        MessageBox.Show("Record updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Record not saved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                frm_groups obj_frm_cust = new frm_groups();
                //obj_frm_cust.Close();
                //obj_frm_cust.ShowDialog();
                mainForm.load_groups_grid();
                //obj_frm_cust.load_Groupess_grid();
                //obj_frm_cust.frm_groups_Load(sender,e);

                this.Close();

            }
            else
            {
                MessageBox.Show("Please enter value in field", "Invalid Data", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            }
            
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Dispose(); 
            this.Close();
        }

        private void frm_addGroup_KeyDown(object sender, KeyEventArgs e)
        {
            //when you enter in textbox it will goto next textbox, work like TAB key
            if (e.KeyData == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        
    }
}
