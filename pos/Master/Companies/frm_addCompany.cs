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
    public partial class frm_addCompany : Form
    {
        public static frm_addCompany instance;
        public TextBox tb_id;
        public TextBox tb_currency_id;
        public TextBox tb_name;
        public TextBox tb_address;
        public TextBox tb_email;
        public TextBox tb_contact_no;
        public TextBox tb_image;
        public TextBox tb_vat_no;
        public Label tb_lbl_is_edit;
        private frm_companies mainForm;
        
        public frm_addCompany(frm_companies mainForm): this()
        {
            this.mainForm = mainForm;
        }

        public frm_addCompany()
        {
            InitializeComponent();
            instance = this;
            tb_id = txt_id;
            tb_name = txt_name;
            tb_currency_id = txt_currency_id;
            tb_address = txt_address;
            tb_email = txt_email;
            tb_contact_no = txt_contact_no;
            tb_image = txt_image;
            tb_vat_no = txt_vat_no;
            
            tb_lbl_is_edit = lbl_edit_status;

        }

        public void frm_addCompany_Load(object sender, EventArgs e)
        {
            if (lbl_edit_status.Text == "true")
            {
                btn_save.Text = "Update";
                lbl_header_title.Text = "Update companies";
                
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
                    CompaniesModal info = new CompaniesModal();
                    info.name = txt_name.Text;
                    info.vat_no = txt_vat_no.Text;
                    info.address = txt_address.Text;
                    info.contact_no = txt_contact_no.Text;
                    info.currency_id = 0; // Convert.ToInt16(txt_currency_id.Text);
                    info.image = txt_image.Text;
                    info.email = txt_email.Text;
                    
                    CompaniesBLL objBLL = new CompaniesBLL();
                    
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
                    frm_companies obj_frm_cust = new frm_companies();
                    //obj_frm_cust.Close();
                    //obj_frm_cust.ShowDialog();
                    mainForm.load_companies_grid();
                    //obj_frm_cust.load_companiess_grid();
                    //obj_frm_cust.frm_companies_Load(sender,e);

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

        private void frm_addCompany_KeyDown(object sender, KeyEventArgs e)
        {
            //when you enter in textbox it will goto next textbox, work like TAB key
            if (e.KeyData == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
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
