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
    public partial class frm_addEmployee : Form
    {
        public static frm_addEmployee instance;
        public TextBox tb_id;
        public TextBox tb_first_name;
        public TextBox tb_last_name;
        public TextBox tb_address;
        public TextBox tb_vat_no;
        public TextBox tb_contact_no;
        public TextBox tb_email;
        public TextBox tb_commission;
        public Label tb_lbl_is_edit;
        private frm_employees mainForm;
        
        int _employee_id = 0;
        
        public frm_addEmployee(frm_employees mainForm): this()
        {
            this.mainForm = mainForm;
        }

        public frm_addEmployee()
        {
            InitializeComponent();
            instance = this;
            tb_id = txt_id;
            tb_first_name = txt_first_name;
            tb_last_name = txt_last_name;
            tb_address = txt_address;
            tb_vat_no = txt_vatno;
            tb_contact_no = txt_contact_no;
            tb_email = txt_email;
            tb_commission = txt_commission_percent;
            tb_lbl_is_edit = lbl_edit_status;

        }
        
        public void frm_addEmployee_Load(object sender, EventArgs e)
        {
            
            if (lbl_edit_status.Text == "true")
            {
                btn_save.Text = "Update";
                lbl_header_title.Text = "Update Employee";
                _employee_id = int.Parse(txt_id.Text);
                load_employee_commission_grid(_employee_id);

            }
            else
            {
                btn_save.Text = "Save";
            }
        }
        
        private void btn_save_Click(object sender, EventArgs e)
        {
                
                if (txt_first_name.Text != string.Empty && txt_last_name.Text != string.Empty)
                {
                    EmployeeModal info = new EmployeeModal();
                    info.first_name = txt_first_name.Text.Trim();
                    info.last_name = txt_last_name.Text.Trim();
                    info.email = txt_email.Text.Trim();
                    info.vat_no = txt_vatno.Text.Trim();
                    info.address = txt_address.Text.Trim();
                    info.contact_no = txt_contact_no.Text.Trim();
                    info.commission_percent = (txt_commission_percent.Text.Trim().Length == 0 ? 0 : int.Parse(txt_commission_percent.Text));

                    EmployeeBLL objBLL = new EmployeeBLL();
                    
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
                    frm_employees obj_frm_cust = new frm_employees();
                    //obj_frm_cust.Close();
                    //obj_frm_cust.ShowDialog();
                    mainForm.load_Employees_grid();
                    //obj_frm_cust.load_Employees_grid();
                    //obj_frm_cust.frm_employees_Load(sender,e);

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

        private void frm_addEmployee_KeyDown(object sender, KeyEventArgs e)
        {
            //when you enter in textbox it will goto next textbox, work like TAB key
            if (e.KeyData == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void txt_commission_percent_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        public void load_employee_commission_grid(int employee_id)
        {
            try
            {
                grid_commission.DataSource = null;

                //bind data in data grid view  
                GeneralBLL objBLL = new GeneralBLL();
                grid_commission.AutoGenerateColumns = false;

                String keyword = "id,entry_date,invoice_no,debit,credit,(debit-credit) AS balance,description";
                String table = "pos_employees_commission WHERE employee_id = " + employee_id + "";

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
                newRow[2] = "Total";
                newRow[3] = _dr_total;
                newRow[4] = _cr_total;
                newRow[5] = (_dr_total - _cr_total);
                dt.Rows.InsertAt(newRow, dt.Rows.Count);

                grid_commission.DataSource = dt;
                ViewTotalInLastRow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }

        private void ViewTotalInLastRow()
        {
            grid_commission.Rows[grid_commission.Rows.Count - 1].Cells["invoice_no"].Style.BackColor = Color.LightGray;
            grid_commission.Rows[grid_commission.Rows.Count - 1].Cells["entry_date"].Style.BackColor = Color.LightGray;
            //grid_commission.Rows[grid_commission.Rows.Count - 1].Cells["account_name"].Style.BackColor = Color.LightGray;
            grid_commission.Rows[grid_commission.Rows.Count - 1].Cells["debit"].Style.BackColor = Color.LightGray;
            grid_commission.Rows[grid_commission.Rows.Count - 1].Cells["credit"].Style.BackColor = Color.LightGray;
            grid_commission.Rows[grid_commission.Rows.Count - 1].Cells["balance"].Style.BackColor = Color.LightGray;
            grid_commission.Rows[grid_commission.Rows.Count - 1].Cells["description"].Style.BackColor = Color.LightGray;


        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            load_employee_commission_grid(_employee_id);
            ViewTotalInLastRow();
        }

        private void btn_payment_Click(object sender, EventArgs e)
        {
            frm_emp_commission_payment obj = new frm_emp_commission_payment(this, _employee_id);
            obj.ShowDialog();
            ViewTotalInLastRow();
        }

        
    }
}
