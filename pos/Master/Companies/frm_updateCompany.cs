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
using System.IO;
using System.Drawing.Imaging;

namespace pos
{
    public partial class frm_updateCompany : Form
    {
         
        public frm_updateCompany()
        {
            InitializeComponent();
            
        }

        
        public void frm_updateCompany_Load(object sender, EventArgs e)
         {
            //DataTable accountDT = get_GL_accounts_dt();

            //LOAD ACCOUNT DROPDOWN LIST
            fill_cash_acc_ddl(get_GL_accounts_dt());
            fill_receivable_acc_ddl(get_GL_accounts_dt());
            fill_payable_acc_ddl(get_GL_accounts_dt());
            fill_sales_acc_ddl(get_GL_accounts_dt());
            fill_inventory_acc_ddl(get_GL_accounts_dt());
            fill_purchases_acc_id_ddl(get_GL_accounts_dt());
            fill_sales_return_acc_ddl(get_GL_accounts_dt());
            fill_sales_disc_acc_ddl(get_GL_accounts_dt());
            fill_purchases_return_acc_ddl(get_GL_accounts_dt());
            fill_purchases_disc_acc_ddl(get_GL_accounts_dt());
            fill_tax_acc_ddl(get_GL_accounts_dt());
            fill_commission_acc_ddl(get_GL_accounts_dt());
            fill_item_variance_acc_ddl(get_GL_accounts_dt());
            ////////////////////////////
            load_company_detail();
            
            

        }
        
        private void load_company_detail()
        {
            GeneralBLL objBLL = new GeneralBLL();

            String keyword = "TOP 1 *";
            String table = "pos_companies";
            DataTable companies_dt = objBLL.GetRecord(keyword, table);
            foreach (DataRow dr in companies_dt.Rows)
            {
                txt_id.Text = dr["id"].ToString();
                txt_name.Text = dr["name"].ToString();
                txt_address.Text = dr["address"].ToString();
                txt_contact_no.Text = dr["contact_no"].ToString();
                txt_email.Text = dr["email"].ToString();
                txt_vat_no.Text = dr["vat_no"].ToString();

                //HardwareIdentifier systemID_obj = new HardwareIdentifier();
                //string systemID = systemID_obj.GetUniqueHardwareId();

                Subscription subcription_obj = new Subscription();
                DateTime expiryDate = DateTime.Now;
                bool verifySubcriptionKey = subcription_obj.VerifySubscriptionKey((int)dr["id"], dr["subscriptionKey"].ToString(), out expiryDate, dr["systemID"].ToString());

                DateTime currentDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                TimeSpan t = expiryDate - currentDate;
                double no_of_days = t.TotalDays;

                lblExpiryDate.Text = "Software will expire on "+expiryDate.Date.ToShortDateString() + ". Number of days left: "+no_of_days;

                cmb_cash_acc_id.SelectedValue =  dr["cash_acc_id"].ToString();
                cmb_tax_acc_id.SelectedValue = dr["tax_acc_id"].ToString();
                cmb_receivable_acc_id.SelectedValue = dr["receivable_acc_id"].ToString();
                cmb_payable_acc_id.SelectedValue = dr["payable_acc_id"].ToString();
                cmb_sales_acc_id.SelectedValue = dr["sales_acc_id"].ToString();
                cmb_inventory_acc_id.SelectedValue = dr["inventory_acc_id"].ToString();
                cmb_sales_return_acc_id.SelectedValue = dr["sales_return_acc_id"].ToString();
                cmb_sales_disc_acc_id.SelectedValue = dr["sales_discount_acc_id"].ToString();
                cmb_purchases_acc_id.SelectedValue = dr["purchases_acc_id"].ToString();
                cmb_purchases_return_acc_id.SelectedValue = dr["purchases_return_acc_id"].ToString();
                cmb_purchases_disc_acc_id.SelectedValue = dr["purchases_discount_acc_id"].ToString();
                cmb_item_variance_acc_id.SelectedValue = dr["item_variance_acc_id"].ToString();
                cmb_commission_acc_id.SelectedValue = dr["commission_acc_id"].ToString();
                
            }
        }


        private void btn_save_Click(object sender, EventArgs e)
        {

            if (txt_name.Text != string.Empty)
            {
                CompaniesModal info = new CompaniesModal();
                info.name = txt_name.Text;
                info.vat_no = txt_vat_no.Text;
                info.address = txt_address.Text;
                info.contact_no = txt_contact_no.Text;
                info.email = txt_email.Text;
                info.tax_acc_id = Convert.ToInt32(cmb_tax_acc_id.SelectedValue.ToString());
                info.cash_acc_id = Convert.ToInt32(cmb_cash_acc_id.SelectedValue.ToString());
                info.inventory_acc_id = Convert.ToInt32(cmb_inventory_acc_id.SelectedValue.ToString());
                info.payable_acc_id = Convert.ToInt32(cmb_payable_acc_id.SelectedValue.ToString());
                info.purchases_acc_id = Convert.ToInt32(cmb_purchases_acc_id.SelectedValue.ToString());
                info.purchases_discount_acc_id = Convert.ToInt32(cmb_purchases_disc_acc_id.SelectedValue.ToString());
                info.purchases_return_acc_id = Convert.ToInt32(cmb_purchases_return_acc_id.SelectedValue.ToString());
                info.receivable_acc_id = Convert.ToInt32(cmb_receivable_acc_id.SelectedValue.ToString());
                info.sales_acc_id = Convert.ToInt32(cmb_sales_acc_id.SelectedValue.ToString());
                info.sales_discount_acc_id = Convert.ToInt32(cmb_sales_disc_acc_id.SelectedValue.ToString());
                info.sales_return_acc_id = Convert.ToInt32(cmb_sales_return_acc_id.SelectedValue.ToString());
                info.sales_return_acc_id = Convert.ToInt32(cmb_sales_return_acc_id.SelectedValue.ToString());
                info.sales_return_acc_id = Convert.ToInt32(cmb_sales_return_acc_id.SelectedValue.ToString());
                info.item_variance_acc_id = Convert.ToInt32(cmb_item_variance_acc_id.SelectedValue.ToString());
                info.commission_acc_id = Convert.ToInt32(cmb_commission_acc_id.SelectedValue.ToString());

                CompaniesBLL objBLL = new CompaniesBLL();


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



                this.Close();

            }
            else
            {
                MessageBox.Show("Please enter value in field", "Invalid Data", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            }

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
            return  dt;
        }

        private void fill_tax_acc_ddl(DataTable dt)
        {
            cmb_tax_acc_id.DataSource = dt;

            cmb_tax_acc_id.DisplayMember = "name";
            cmb_tax_acc_id.ValueMember = "id";
        }

        private void fill_cash_acc_ddl(DataTable dt)
        {
            cmb_cash_acc_id.DataSource = dt;

            cmb_cash_acc_id.DisplayMember = "name";
            cmb_cash_acc_id.ValueMember = "id";
        }
        private void fill_receivable_acc_ddl(DataTable dt)
        {
            cmb_receivable_acc_id.DataSource = dt;
            cmb_receivable_acc_id.DisplayMember = "name";
            cmb_receivable_acc_id.ValueMember = "id";
        }
        private void fill_payable_acc_ddl(DataTable dt)
        {
            cmb_payable_acc_id.DataSource = dt;
            cmb_payable_acc_id.DisplayMember = "name";
            cmb_payable_acc_id.ValueMember = "id";
        }

        private void fill_sales_acc_ddl(DataTable dt)
        {
            cmb_sales_acc_id.DataSource = dt;

            cmb_sales_acc_id.DisplayMember = "name";
            cmb_sales_acc_id.ValueMember = "id";
        }
        private void fill_inventory_acc_ddl(DataTable dt)
        {
            cmb_inventory_acc_id.DataSource = dt;
            cmb_inventory_acc_id.DisplayMember = "name";
            cmb_inventory_acc_id.ValueMember = "id";
        }
        
        private void fill_sales_return_acc_ddl(DataTable dt)
        {
            cmb_sales_return_acc_id.DataSource = dt;
            cmb_sales_return_acc_id.DisplayMember = "name";
            cmb_sales_return_acc_id.ValueMember = "id";
        }
        private void fill_sales_disc_acc_ddl(DataTable dt)
        {
            cmb_sales_disc_acc_id.DataSource = dt;
            cmb_sales_disc_acc_id.DisplayMember = "name";
            cmb_sales_disc_acc_id.ValueMember = "id";

        }
        private void fill_purchases_acc_id_ddl(DataTable dt)
        {
            cmb_purchases_acc_id.DataSource = dt;
            cmb_purchases_acc_id.DisplayMember = "name";
            cmb_purchases_acc_id.ValueMember = "id";
        }

        private void fill_purchases_return_acc_ddl(DataTable dt)
        {
            cmb_purchases_return_acc_id.DataSource = dt;
            cmb_purchases_return_acc_id.DisplayMember = "name";
            cmb_purchases_return_acc_id.ValueMember = "id";
        }
        private void fill_purchases_disc_acc_ddl(DataTable dt)
        {
            cmb_purchases_disc_acc_id.DataSource = dt;
            cmb_purchases_disc_acc_id.DisplayMember = "name";
            cmb_purchases_disc_acc_id.ValueMember = "id";

        }

        private void fill_item_variance_acc_ddl(DataTable dt)
        {
            cmb_item_variance_acc_id.DataSource = dt;
            cmb_item_variance_acc_id.DisplayMember = "name";
            cmb_item_variance_acc_id.ValueMember = "id";

        }

        private void fill_commission_acc_ddl(DataTable dt)
        {
            cmb_commission_acc_id.DataSource = dt;
            cmb_commission_acc_id.DisplayMember = "name";
            cmb_commission_acc_id.ValueMember = "id";

        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            //this.Dispose(); 
            this.Close();
        }

        private void frm_updateCompany_KeyDown(object sender, KeyEventArgs e)
        {
            //when you enter in textbox it will goto next textbox, work like TAB key
            if (e.KeyData == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

    }
}
