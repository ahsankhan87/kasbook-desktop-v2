using POS.BLL;
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
using pos.UI;

namespace pos
{
    public partial class frmRenewSubscrption : Form
    {
        public frmRenewSubscrption()
        {
            InitializeComponent();
        }

        private void RenewSubscrption_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            int userID = GetUserID();
            txtUserID.Text = userID.ToString();

            HardwareIdentifier systemID_obj = new HardwareIdentifier();
            string systemID = systemID_obj.GetUniqueHardwareId();
            txtSystemID.Text = systemID;
        }

        private int GetUserID()
        {
            int company_id = 0;

            CompaniesBLL company_obj = new CompaniesBLL();
            DataTable dt = company_obj.GetCompany();

            foreach (DataRow myProductView in dt.Rows)
            {
                company_id = Convert.ToInt32(myProductView["id"]);
            }

            return company_id;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string userID = txtUserID.Text;
                string subscriptionKey = txtSubscriptionKey.Text;

                if (userID != string.Empty && subscriptionKey != string.Empty)
                {
                    CompaniesBLL company_obj = new CompaniesBLL();
                    int affected = company_obj.UpdateSubscriptionKeyToDatabase(Convert.ToInt32(userID), subscriptionKey);

                    if (affected > 0)
                    {
                        MessageBox.Show("Account renewed successfully.", "Record Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Please enter value in required (*) fields", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error: "+ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSubscriptionKey_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
