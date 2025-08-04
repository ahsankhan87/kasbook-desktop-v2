//using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using POS.BLL;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace pos
{
    public partial class frm_sales_invoice : Form
    {
        DataTable _dt;
        bool _isPrint = false;
        bool _isPrintProductCode = false;

        public frm_sales_invoice(DataTable sales_detail,bool isPrint, bool isPrintProductCode=false)
        {
            InitializeComponent();
            _dt = sales_detail;
            _isPrintProductCode = isPrintProductCode;
            _isPrint = isPrint;
        }

        private void frm_sales_invoice_Load(object sender, EventArgs e)
        {
            load_print();
        }
        public void load_print()
        {
            double total_amount = 0;
            double total_tax = 0;
            double total_discount = 0;
            double net_total = 0;
            string sale_date = "";
            byte[] zatca_qrcode_phase2 = null;

            foreach (DataRow dr in _dt.Rows)
            {
                total_amount = Convert.ToDouble(dr["total_amount"]);
                total_tax = Convert.ToDouble(dr["total_tax"]);
                total_discount = Convert.ToDouble(dr["total_discount"]);
                sale_date = dr["sale_time"].ToString();
                zatca_qrcode_phase2 =  (dr["zatca_qrcode_phase2"] == DBNull.Value ? null : (byte[])dr["zatca_qrcode_phase2"]);
            }
            net_total = total_amount - total_discount + total_tax;
            string s_date = Convert.ToDateTime(sale_date).ToString("yyyy-MM-ddTHH:mm:ss");

            CompaniesBLL company_obj = new CompaniesBLL();
            DataTable company_dt = company_obj.GetCompany();
            string company_name = "";
            string company_address = "";
            string company_email = "";
            string company_vat_no = "";
            string company_contact_no = "";
            
            foreach (DataRow dr_company in company_dt.Rows)
            {
                company_name = dr_company["name"].ToString();
                company_address = dr_company["address"].ToString();
                company_email = dr_company["email"].ToString();
                company_vat_no = dr_company["vat_no"].ToString();
                company_contact_no = dr_company["contact_no"].ToString();
            }

            string SallerName = gethexstring(1, company_name); //Tag1
            string VATReg = gethexstring(2, company_vat_no); //Tag2
            string DateTimeStr = gethexstring(3, s_date); //Tage3
            string TotalAmt = gethexstring(4, net_total.ToString()); //Tag4
            string VatAmt = gethexstring(5, total_tax.ToString()); //Tag5
            string qtcode_String = SallerName + VATReg + DateTimeStr + TotalAmt + VatAmt;


            byte[] imageData = GenerateQrCode(HexToBase64(qtcode_String));//GIVE DATA TO FUNCTION AND GET QRCODE
            byte[] imageData_phase2 = GeneratePhase2QrCode(zatca_qrcode_phase2);  //GIVE DATA TO FUNCTION AND GET PHASE 2 QRCODE
            _dt.Columns.Add("qrcode_image", typeof(byte[]));// INSERT QRCODE DATA TO DATATABLE
            _dt.Columns.Add("qrcode_image_phase2", typeof(byte[]));// INSERT QRCODE DATA TO DATATABLE
            foreach (DataRow dr in _dt.Rows)
            {
                dr["qrcode_image"] = imageData;
                dr["qrcode_image_phase2"] = imageData_phase2;
            }

            string appPath = Path.GetDirectoryName(Application.ExecutablePath);
            ReportDocument rptDoc = new ReportDocument();
            if(_isPrintProductCode)
            {
                rptDoc.Load(appPath + @"\\reports\\sales_invoice.rpt");
            }
            else
            {
                rptDoc.Load(appPath + @"\\reports\\sales_invoice_sans_code.rpt");
            }
            
            rptDoc.SetDataSource(_dt);
            //rptDoc.SetParameterValue("username", username);
            rptDoc.SetParameterValue("company_name", company_name);
            rptDoc.SetParameterValue("company_vat", company_vat_no);
            rptDoc.SetParameterValue("company_address", company_address);
            rptDoc.SetParameterValue("company_contact", company_contact_no);

            rptDoc.SetParameterValue("subtotal", total_amount);
            rptDoc.SetParameterValue("total_discount", total_discount);
            rptDoc.SetParameterValue("total_vat", total_tax);
            rptDoc.SetParameterValue("net_total", net_total);
            rptDoc.SetParameterValue("company_email", company_email);

            crystalReportViewer_sales_invoice.ReportSource = rptDoc;

           
            if (_isPrint)
            {
                rptDoc.PrintToPrinter(1, true, 0, 0);
            }
            
        }

        private byte[] GenerateQrCode(string qrmsg)
        {
            QRCoder.QRCodeGenerator qRCodeGenerator = new QRCoder.QRCodeGenerator();
            QRCoder.QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(qrmsg, QRCoder.QRCodeGenerator.ECCLevel.Q);
            QRCoder.QRCode qRCode = new QRCoder.QRCode(qRCodeData);

            using (Bitmap bmp = qRCode.GetGraphic(20))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    
                    bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] byteImage = ms.ToArray();
                    return byteImage;
                    //FileStream fileStream = new FileStream(appPath + "qrcode.jpg", FileMode.Open);

                    //BinaryReader binaryReader = new BinaryReader(fileStream);
                    //return binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
                    //Image image = Image.FromStream(ms);
                    //return image;
                }
            }
        }
        private byte[] GeneratePhase2QrCode(byte[] qrBytes)
        {
            byte[] byteImage = null;
            if (qrBytes != null)
            {
                string base64String = Convert.ToBase64String(qrBytes);

                QRCoder.QRCodeGenerator qRCodeGenerator = new QRCoder.QRCodeGenerator();
                QRCoder.QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(base64String, QRCoder.QRCodeGenerator.ECCLevel.Q);
                QRCoder.QRCode qRCode = new QRCoder.QRCode(qRCodeData);

                using (Bitmap bmp = qRCode.GetGraphic(20))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {

                        bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        byteImage = ms.ToArray();
                        
                        //FileStream fileStream = new FileStream(appPath + "qrcode.jpg", FileMode.Open);

                        //BinaryReader binaryReader = new BinaryReader(fileStream);
                        //return binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
                        //Image image = Image.FromStream(ms);
                        //return image;
                    }
                }
            }
            return byteImage;
        }

        static string gethexstring(Int32 TagNo, string TagValue)
        {
          
            string decString = TagValue;
            byte[] bytes = Encoding.UTF8.GetBytes(decString);
            string hexString = BitConverter.ToString(bytes);

            string StrTagNo = String.Format("0{0:X}", TagNo);
            String TagNoVal = StrTagNo.Substring(StrTagNo.Length - 2, 2);

            string StrTagValue_Length = String.Format("0{0:X}", bytes.Length);
            String TagValue_LengthVal = StrTagValue_Length.Substring(StrTagValue_Length.Length - 2, 2);


            hexString = TagNoVal + TagValue_LengthVal + hexString.Replace("-", "");
            return hexString;
        }
       
        static string gethexDec(Int32 TagValue)
        {
            string hxint = String.Format("0{0:X}", TagValue);
            return hxint.Substring(hxint.Length - 2, 2);

        }
        public static string HexToBase64(string strInput)
        {
            try
            {
                var bytes = new byte[strInput.Length / 2];
                for (var i = 0; i < bytes.Length; i++)
                {
                    bytes[i] = Convert.ToByte(strInput.Substring(i * 2, 2), 16);
                }
                return Convert.ToBase64String(bytes);
            }
            catch (Exception)
            {
                return "-1";
            }
        }

        private string StringToHex(string hexstring)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char t in hexstring)
            {
                //Note: X for upper, x for lower case letters
                sb.Append(Convert.ToInt32(t).ToString("x"));
            }
            return sb.ToString();
        }

        private void crystalReportViewer_sales_invoice_Load(object sender, EventArgs e)
        {

        }
    }
}
