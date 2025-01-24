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
    public partial class frm_employees : Form
    {
        
        public frm_employees()
        {
            InitializeComponent();
        }

        
        public void frm_employees_Load(object sender, EventArgs e)
        {
            load_Employees_grid();
        }

        public void load_Employees_grid()
        {
            try
            {
                grid_employees.DataSource = null;

                //bind data in data grid view  
                //GeneralBLL objBLL = new GeneralBLL();

                EmployeeBLL objBLL = new EmployeeBLL();
                grid_employees.AutoGenerateColumns = false;

                //String keyword = "id,first_name,last_name,email,vat_no,address,contact_no,date_created,commission_percent";
                //String table = "pos_employees WHERE  branch_id=@branch_id ";
                grid_employees.DataSource = objBLL.GetAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
            
        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            frm_addEmployee frm_addEmployee_obj = new frm_addEmployee(this);
            frm_addEmployee.instance.tb_lbl_is_edit.Text = "false";

            frm_addEmployee_obj.ShowDialog();
        }
        
        private void btn_update_Click(object sender, EventArgs e)
        {
            if(grid_employees.RowCount > 0)
            {
                string id = grid_employees.CurrentRow.Cells[0].Value.ToString();
                string first_name = grid_employees.CurrentRow.Cells[1].Value.ToString();
                string last_name = grid_employees.CurrentRow.Cells[2].Value.ToString();
                string email = grid_employees.CurrentRow.Cells[3].Value.ToString();
                string vat_no = grid_employees.CurrentRow.Cells[4].Value.ToString();
                string contact_no = grid_employees.CurrentRow.Cells[5].Value.ToString();
                string address = grid_employees.CurrentRow.Cells[6].Value.ToString();
                string commission_percent = grid_employees.CurrentRow.Cells["commission_percent"].Value.ToString();

                frm_addEmployee frm_addEmployee_obj = new frm_addEmployee(this);
                frm_addEmployee.instance.tb_lbl_is_edit.Text = "true";

                frm_addEmployee.instance.tb_id.Text = id;
                frm_addEmployee.instance.tb_first_name.Text = first_name;
                frm_addEmployee.instance.tb_last_name.Text = last_name;
                frm_addEmployee.instance.tb_email.Text = email;
                frm_addEmployee.instance.tb_vat_no.Text = vat_no;
                frm_addEmployee.instance.tb_address.Text = address;
                frm_addEmployee.instance.tb_contact_no.Text = contact_no;
                frm_addEmployee.instance.tb_commission.Text = commission_percent;

                frm_addEmployee.instance.Show();
            }
            
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (grid_employees.RowCount > 0)
            {

                string id = grid_employees.CurrentRow.Cells[0].Value.ToString();

                DialogResult result = MessageBox.Show("Are you sure you want to delete", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {

                    EmployeeBLL objBLL = new EmployeeBLL();
                    objBLL.Delete(int.Parse(id));

                    MessageBox.Show("Record deleted successfully.", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    load_Employees_grid();
                }
                else
                {
                    MessageBox.Show("Please select record", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            load_Employees_grid();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                  //grid_employees.DataSource = null;

                    //bind data in data grid view  
                    EmployeeBLL objBLL = new EmployeeBLL();
                    //grid_employees.AutoGenerateColumns = false;

                    String condition = txt_search.Text;
                    grid_employees.DataSource = objBLL.SearchRecord(condition);
                    
                    //txt_search.Text = "";
                
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

        private void frm_employees_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.N)
            {
                btn_new.PerformClick();
                
            }

            if (e.Control == true && e.KeyCode == Keys.U)
            {
                btn_update.PerformClick();
            }
            
            if (e.Control == true && e.KeyCode == Keys.D)
            {
                btn_delete.PerformClick();

            }
        }  
    }
}
