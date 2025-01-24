using POS.BLL;
using POS.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos.Customers
{
    public partial class CustomerNameChange : Form
    {
        public string _invoiceNo;

        public CustomerNameChange(string invoiceNo)
        {
            InitializeComponent();
            _invoiceNo = invoiceNo;
        }

        private void Btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Btn_ok_Click(object sender, EventArgs e)
        {
           
            try
            {
                SalesBLL salesBLL = new SalesBLL();
                int result = salesBLL.UpdateCustomerInSales(_invoiceNo, cmb_customers.SelectedValue.ToString());
                if(result > 0)
                {
                    MessageBox.Show("Customer updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Customer not updated", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void CustomerNameChange_Load(object sender, EventArgs e)
        {
            this.ActiveControl = cmb_customers;
            lbl_invoice_no.Text = _invoiceNo;
            Get_customers_DDL();
        }
        public void Get_customers_DDL()
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "id,first_name";
            string table = "pos_customers WHERE branch_id = " + UsersModal.logged_in_branch_id + "";

            DataTable banks_DDL = generalBLL_obj.GetRecord(keyword, table);
            //DataRow emptyRow = banks_DDL.NewRow();
            //emptyRow[0] = 0;              // Set Column Value
            //emptyRow[1] = "";              // Set Column Value
            //banks_DDL.Rows.InsertAt(emptyRow, 0);

            cmb_customers.DisplayMember = "first_name";
            cmb_customers.ValueMember = "id";
            cmb_customers.DataSource = banks_DDL;

        }
    }
}
