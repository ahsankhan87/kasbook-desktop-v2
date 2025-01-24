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
    public partial class frm_addProductGroup : Form
    {
        public static frm_addProductGroup instance;
        public TextBox tb_id;
        public TextBox tb_name;
        public TextBox tb_code;
        public Label tb_lbl_is_edit;
        private frm_product_groups mainForm;

        public frm_addProductGroup(frm_product_groups mainForm)
            : this()
        {
            this.mainForm = mainForm;
            
        }

        public frm_addProductGroup()
        {
            InitializeComponent();
            instance = this;
            tb_id = txt_id;
            tb_name = txt_name;
            tb_code = txt_code;
            
            tb_lbl_is_edit = lbl_edit_status;

        }

        public void frm_addProductGroup_Load(object sender, EventArgs e)
        {
            if (lbl_edit_status.Text == "true")
            {
                btn_save.Text = "Update";
                lbl_header_title.Text = "Update Product Groups";
                
            }
            else
            {
                btn_save.Text = "Save";
            }
        }
        
        private void btn_save_Click(object sender, EventArgs e)
        {


            if (txt_code.Text != string.Empty && txt_name.Text != string.Empty)
                {
                    ProductGroupsModal info = new ProductGroupsModal();
                    info.code = txt_code.Text;
                    info.name = txt_name.Text;
                    
                    ProductGroupsBLL objBLL = new ProductGroupsBLL();
                    
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
                        mainForm.load_ProductGroups_grid();

                    }
                    
                    this.Close();

                }
                else
                {
                    MessageBox.Show("Please enter code and name", "Invalid Data", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                }
            
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Dispose(); 
            this.Close();
        }

        private void frm_addProductGroup_KeyDown(object sender, KeyEventArgs e)
        {
            //when you enter in textbox it will goto next textbox, work like TAB key
            if (e.KeyData == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        
    }
}
