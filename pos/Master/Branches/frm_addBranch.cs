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
    public partial class frm_addBranch : Form
    {
        public static frm_addBranch instance;
        public TextBox tb_id;
        public TextBox tb_name;
        public Label tb_lbl_is_edit;
        private frm_branches mainForm;
        
        public frm_addBranch(frm_branches mainForm): this()
        {
            this.mainForm = mainForm;
        }

        public frm_addBranch()
        {
            InitializeComponent();
            instance = this;
            tb_id = txt_id;
            tb_name = txt_name;
            
            tb_lbl_is_edit = lbl_edit_status;

        }

        public void frm_addBranch_Load(object sender, EventArgs e)
        {
            if (lbl_edit_status.Text == "true")
            {
                btn_save.Text = "Update";
                lbl_header_title.Text = "Update Branches";
                
            }
            else
            {
                btn_save.Text = "Save";
            }
        }
        
        private void btn_save_Click(object sender, EventArgs e)
        {
            
                
                if (txt_name.Text != string.Empty)
                {
                    BranchesModal info = new BranchesModal();
                    info.name = txt_name.Text;
                    info.description = txt_description.Text;
                    
                    BranchesBLL objBLL = new BranchesBLL();
                    
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
                    
                    if(mainForm != null)
                    {
                        mainForm.load_branches_grid();

                    }

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

        private void frm_addBranch_KeyDown(object sender, KeyEventArgs e)
        {
            //when you enter in textbox it will goto next textbox, work like TAB key
            if (e.KeyData == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        
    }
}
