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
    public partial class frm_register_company : Form
    {
        
        public frm_register_company()
        {
            InitializeComponent();
            
        }

        public void frm_register_company_Load(object sender, EventArgs e)
        {
            get_branches_dropdownlist();
            get_language_dropdownlist();
            get_userrole_dropdownlist();
            
            HardwareIdentifier systemID_obj = new HardwareIdentifier();
            string systemID = systemID_obj.GetUniqueHardwareId();
            txtSystemID.Text = systemID;

            txt_name.Focus();
        }
        
        private void btn_save_Click(object sender, EventArgs e)
        {
            if (txt_name.Text != string.Empty && txt_username.Text != string.Empty && txt_password.Text != string.Empty)
            {
                if (txt_password.Text == txt_confirm_pwd.Text)
                {
                    CompaniesModal info = new CompaniesModal();
                    info.name = txt_name.Text;
                    info.vat_no = txt_vat_no.Text;
                    info.address = txt_address.Text;
                    info.contact_no = txt_contact_no.Text;
                    info.currency_id = 0; // Convert.ToInt16(txt_currency_id.Text);
                    //info.image = txt_image.Text;
                    info.email = txt_email.Text;
                    info.locked = 0;
                    info.systemID = txtSystemID.Text;

                    var date = DateTime.Now;
                    DateTime nextMonth = date.AddDays(1).AddMonths(2).AddDays(-1);
                    info.expiry_date = nextMonth;

                    //Subscription sub = new Subscription();
                    //double convertToDays = Convert.ToDouble(14);
                    //int userID = Convert.ToInt32(txtUserID.Text);
                    //DateTime newExpiryDate = DateTime.Today.AddDays(+convertToDays);

                    //string subscriptionKey = sub.GenerateRenewalKey(userID, nextMonth, txtSystemID.Text);
                    
                    info.subscriptionKey = txtSubscriptionKey.Text;
                    CompaniesBLL companyobjBLL = new CompaniesBLL();

                    int companyID = companyobjBLL.Register(info);

                    UsersModal userinfo = new UsersModal();
                    userinfo.companyID = companyID;
                    userinfo.name = txt_full_name.Text;
                    userinfo.username = txt_username.Text;
                    userinfo.password = txt_password.Text;
                    userinfo.branch_id = Convert.ToInt32(cmb_branches.SelectedValue);
                    userinfo.language = cmb_lang.SelectedValue.ToString();
                    userinfo.user_role = cmb_user_role.Text;
                    userinfo.user_level = Convert.ToInt32(cmb_user_role.SelectedValue);

                    UsersBLL userBLL = new UsersBLL();
                    int result = userBLL.Insert(userinfo); // result is user_id

                    if (result > 0)
                    {
                        MessageBox.Show("New Account has registered successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Record not saved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }


                    this.Close();

                }
                else
                {
                    MessageBox.Show("Password do not match", "Confirm Password", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    txt_password.Focus();
                }
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

        private void frm_register_company_KeyDown(object sender, KeyEventArgs e)
        {
            //when you enter in textbox it will goto next textbox, work like TAB key
            if (e.KeyData == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        public void get_branches_dropdownlist()
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "id,name";
            string table = "pos_branches";
            DataTable branches = generalBLL_obj.GetRecord(keyword, table);

            //DataRow emptyRow = branches.NewRow();
            //emptyRow[0] = 0;              // Set Column Value
            //emptyRow[1] = "Please Select";              // Set Column Value
            //branches.Rows.InsertAt(emptyRow, 0);

            cmb_branches.DisplayMember = "name";
            cmb_branches.ValueMember = "id";
            cmb_branches.DataSource = branches;

        }

        private void get_language_dropdownlist()
        {
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("id");
            dt.Columns.Add("name");
            DataRow _row_1 = dt.NewRow();
            _row_1["id"] = "en-US";
            _row_1["name"] = "English";
            dt.Rows.Add(_row_1);

            DataRow _row = dt.NewRow();
            _row["id"] = "ar-SA";
            _row["name"] = "Arabic";
            dt.Rows.Add(_row);


            cmb_lang.DisplayMember = "name";
            cmb_lang.ValueMember = "id";
            cmb_lang.DataSource = dt;

        }

        private void get_userrole_dropdownlist()
        {
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("id");
            dt.Columns.Add("name");
            DataRow _row_1 = dt.NewRow();
            _row_1["id"] = "1";
            _row_1["name"] = "Administrator";
            dt.Rows.Add(_row_1);

            //DataRow _row = dt.NewRow();
            //_row["id"] = "2";
            //_row["name"] = "User";
            //dt.Rows.Add(_row);


            cmb_user_role.DisplayMember = "name";
            cmb_user_role.ValueMember = "id";
            cmb_user_role.DataSource = dt;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Image data = GenerateQrCode("Ahsan Khan");
            //pictureBox1.Image = data;
        }


        private Image GenerateQrCode(string qrmsg)
        {
            QRCoder.QRCodeGenerator qRCodeGenerator = new QRCoder.QRCodeGenerator();
            QRCoder.QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(qrmsg, QRCoder.QRCodeGenerator.ECCLevel.Q);
            QRCoder.QRCode qRCode = new QRCoder.QRCode(qRCodeData);
            //Bitmap bmp = qRCode.GetGraphic(5);

            using (Bitmap bmp = qRCode.GetGraphic(5))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bmp.Save(ms, ImageFormat.Bmp);
                    //byte[] byteImage = ms.ToArray();
                    Image image = Image.FromStream(ms);
                    return image;
                }
            }
        }
    }
}
