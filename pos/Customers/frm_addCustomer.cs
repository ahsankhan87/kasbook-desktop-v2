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
    public partial class frm_addCustomer : Form
    {
        
        public frm_addCustomer()
        {
            InitializeComponent();
           
        }
        
        public void frm_addCustomer_Load(object sender, EventArgs e)
        {
            txt_search.Focus();
            this.ActiveControl = txt_search;
        }

        public void load_customer_detail(int customer_id)
        {
            CustomerBLL objBLL = new CustomerBLL();
            DataTable dt = objBLL.SearchRecordByCustomerID(customer_id);
            foreach (DataRow myProductView in dt.Rows)
            {
                txt_id.Text = myProductView["id"].ToString();
                txt_first_name.Text = myProductView["first_name"].ToString();
                txt_last_name.Text = myProductView["last_name"].ToString();
                txt_address.Text = myProductView["address"].ToString();
                txt_vatno.Text = myProductView["vat_no"].ToString();
                txt_contact_no.Text = myProductView["contact_no"].ToString();
                txt_email.Text = myProductView["email"].ToString();
                txt_vin_no.Text = myProductView["vin_no"].ToString();
                txt_car_name.Text = myProductView["car_name"].ToString();
                
            }
            lbl_customer_name.Visible = true;
            lbl_customer_name.Text = txt_first_name.Text + ' ' + txt_last_name.Text;
        }

        public DataTable get_GL_accounts_dt()
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "id,name";
            string table = "acc_accounts";

            DataTable dt = generalBLL_obj.GetRecord(keyword, table);
            DataRow emptyRow = dt.NewRow();
            emptyRow[0] = 0;              // Set Column Value
            emptyRow[1] = "Select Account";              // Set Column Value
            dt.Rows.InsertAt(emptyRow, 0);
            return dt;
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_first_name.Text != string.Empty && txt_last_name.Text != string.Empty)
                {
                    CustomerModal info = new CustomerModal
                    {
                        first_name = txt_first_name.Text,
                        last_name = txt_last_name.Text,
                        email = txt_email.Text,
                        vat_no = txt_vatno.Text,
                        address = txt_address.Text,
                        contact_no = txt_contact_no.Text,
                        vin_no = txt_vin_no.Text,
                        car_name = txt_car_name.Text,
                        credit_limit = (txt_credit_limit.Text != "" ? Convert.ToDouble(txt_credit_limit.Text) : 0)
                    };

                    CustomerBLL objBLL = new CustomerBLL();
                    int result = objBLL.Insert(info);
                    if (result > 0)
                    {
                        MessageBox.Show("Record created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clear_all();
                    }
                    else
                    {
                        MessageBox.Show("Record not saved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }


                }
                else
                {
                    MessageBox.Show("Please enter value in field", "Invalid Data", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            //this.Dispose(); 
            this.Close();
        }

        private void frm_addCustomer_KeyDown(object sender, KeyEventArgs e)
        {
            //when you enter in textbox it will goto next textbox, work like TAB key
            if (e.KeyData == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
            if (e.KeyData == Keys.F3)
            {
                btn_save.PerformClick();
            }
            if (e.KeyData == Keys.F4)
            {
                btn_update.PerformClick();
            }
            if (e.KeyData == Keys.F5)
            {
                btn_refresh.PerformClick();
            }
            
        }

        private void clear_all()
        {
            txt_id.Text = "";
            txt_first_name.Text = "";
            txt_last_name.Text = "";
            txt_address.Text = "";
            txt_vatno.Text = "";
            txt_contact_no.Text = "";
            txt_email.Text = "";
            txt_vin_no.Text = "";
            txt_car_name.Text = "";
            lbl_customer_name.Text = "";
            grid_customer_transactions.DataSource = null;
            txt_credit_limit.Text = "";

        }

        private void btn_blank_Click(object sender, EventArgs e)
        {
            clear_all();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(txt_id.Text))
                {
                    MessageBox.Show("Record not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (txt_first_name.Text != string.Empty && txt_last_name.Text != string.Empty)
                {
                    CustomerModal info = new CustomerModal
                    {
                        first_name = txt_first_name.Text,
                        last_name = txt_last_name.Text,
                        email = txt_email.Text,
                        vat_no = txt_vatno.Text,
                        address = txt_address.Text,
                        contact_no = txt_contact_no.Text,
                        vin_no = txt_vin_no.Text,
                        car_name = txt_car_name.Text,
                        credit_limit = (txt_credit_limit.Text != "" ? Convert.ToDouble(txt_credit_limit.Text) : 0)
                    };

                    CustomerBLL objBLL = new CustomerBLL();

                    info.id = int.Parse(txt_id.Text);

                    int result = objBLL.Update(info);
                    if (result > 0)
                    {
                        MessageBox.Show("Record updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clear_all();
                    }
                    else
                    {
                        MessageBox.Show("Record not saved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
}

        private void btn_search_Click(object sender, EventArgs e)
        {
            frm_search_customers search_obj = new frm_search_customers(this, txt_search.Text);
            search_obj.ShowDialog();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            try
            {
                string id = txt_id.Text;

                if (id != "")
                {
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show("Are you sure you want to delete", "Delete Record", buttons, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        CustomerBLL objBLL = new CustomerBLL();
                        int resulte = objBLL.Delete(int.Parse(id));

                        MessageBox.Show("Record deleted successfully.", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clear_all();
                    }
                    else
                    {
                        MessageBox.Show("Please select record", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            string customer_id = txt_id.Text;

            if (customer_id != "")
            {
                load_customer_detail(int.Parse(customer_id));
                
            }

        }

        private void btn_trans_refresh_Click(object sender, EventArgs e)
        {
            string customer_id = txt_id.Text;
            if (customer_id != "")
            {
                load_customer_transactions_grid(int.Parse(customer_id));
            }
        }

        private void btn_payment_Click(object sender, EventArgs e)
        {
            string customer_id = txt_id.Text;
            if (customer_id != "")
            {
                frm_customer_payment obj = new frm_customer_payment(this, int.Parse(customer_id));
                obj.ShowDialog();
               
            }
        }
        private void CustomizeDataGridView()
        {
            // Get the last row in the DataGridView
            DataGridViewRow lastRow = grid_customer_transactions.Rows[grid_customer_transactions.Rows.Count - 1];

            // Loop through all cells in the row
            foreach (DataGridViewCell cell in lastRow.Cells)
            {
                DataGridViewCellStyle style = new DataGridViewCellStyle(cell.Style);

                // Set the font to bold
                style.Font = new Font(grid_customer_transactions.Font, FontStyle.Bold);

                // Set the background color
                style.BackColor = Color.LightGray;

                // Apply the style to the current cell
                cell.Style = style;
            }

        }

        public void load_customer_transactions_grid(int customer_id)
        {
            try
            {
                grid_customer_transactions.DataSource = null;

                //bind data in data grid view  
                GeneralBLL objBLL = new GeneralBLL();
                grid_customer_transactions.AutoGenerateColumns = false;

                String keyword = "id,invoice_no,debit,credit,(debit-credit) AS balance,description,entry_date,account_id,account_name";
                String table = "pos_customers_payments WHERE customer_id = " + customer_id + "";

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
                newRow[4] = (_dr_total - _cr_total);
                dt.Rows.InsertAt(newRow, dt.Rows.Count);

                grid_customer_transactions.DataSource = dt;
                CustomizeDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }

        private void Btn_ledger_report_Click(object sender, EventArgs e)
        {
            string customer_id = txt_id.Text;
            if (customer_id != "")
            {
                pos.Customers.Customer_Ledger_Report.FrmCustomerLedgerReport obj = new Customers.Customer_Ledger_Report.FrmCustomerLedgerReport(customer_id);
                obj.ShowDialog();

            }
        }

        private void Btn_report_ledger_Click(object sender, EventArgs e)
        {
            string customer_id = txt_id.Text;
            if (customer_id != "")
            {
                pos.Customers.Customer_Ledger_Report.FrmCustomerLedgerReport obj = new Customers.Customer_Ledger_Report.FrmCustomerLedgerReport(customer_id);
                obj.ShowDialog();

            }
        }
    }
}
