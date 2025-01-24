using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using POS.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos.Reports.Sales.New
{
    public partial class SaleInvoiceReport : Form
    {
        public SaleInvoiceReport()
        {
            InitializeComponent();
        }

        private void SaleInvoiceReport_Load(object sender, EventArgs e)
        {
            
        }
        public void LoadReport(string invoiceNo, string sale_date,double netTotal,double totalTax, bool _isPrint, bool isPrintInvoiceWithCode,string QrCode ="")
        {
            // Create an instance of your report
            string appPath = Path.GetDirectoryName(Application.ExecutablePath);
            ReportDocument reportDoc = new ReportDocument();
            if (isPrintInvoiceWithCode)
            {
                reportDoc.Load(appPath + @"\\reports\\New\\sales_invoice.rpt");
            }
            else
            {
                reportDoc.Load(appPath + @"\\reports\\New\\sales_invoice.rpt");
            }


            // Use the centralized connection method
            ReportConnectionManager.SetDatabaseLogon(reportDoc);


            CompaniesBLL company_obj = new CompaniesBLL();
            DataTable company_dt = company_obj.GetCompany();
            string company_name = "";
            string company_address = "";
            string company_email = "";
            string company_vat_no = "";
            string company_contact_no = "";

            string s_date = Convert.ToDateTime(sale_date).ToString("yyyy-MM-ddTHH:mm:ss");

            foreach (DataRow dr_company in company_dt.Rows)
            {
                company_name = dr_company["name"].ToString();
                company_address = dr_company["address"].ToString();
                company_email = dr_company["email"].ToString();
                company_vat_no = dr_company["vat_no"].ToString();
                company_contact_no = dr_company["contact_no"].ToString();
            }

            if (string.IsNullOrEmpty(QrCode))
            {
                string SallerName = gethexstring(1, company_name); //Tag1
                string VATReg = gethexstring(2, company_vat_no); //Tag2
                string DateTimeStr = gethexstring(3, s_date); //Tage3
                string TotalAmt = gethexstring(4, netTotal.ToString()); //Tag4
                string VatAmt = gethexstring(5, totalTax.ToString()); //Tag5
                string qtcode_String = SallerName + VATReg + DateTimeStr + TotalAmt + VatAmt;

                byte[] imageData = GenerateQrCode(HexToBase64(qtcode_String));//GIVE DATA TO FUNCTION AND GET QRCODE

                SalesBLL salesBLL = new SalesBLL();
                salesBLL.UpdateZetcaQrcodeInSales(invoiceNo, imageData);
            }


            // Set the parameter value
            reportDoc.SetParameterValue("InvoiceNo", invoiceNo);
            reportDoc.SetParameterValue("CompanyName", company_name);
            reportDoc.SetParameterValue("CompanyVat", company_vat_no);
            reportDoc.SetParameterValue("CompanyAddress", company_address);
            reportDoc.SetParameterValue("PhoneNumber", company_contact_no);
            reportDoc.SetParameterValue("CompanyEmail", company_email);
            //reportDoc.SetParameterValue("QrCode", imageData);

            // Set the report source for the CrystalReportViewer
            crystalReportViewer1.ReportSource = reportDoc;

            if (_isPrint)
            {
                reportDoc.PrintToPrinter(1, true, 0, 0);
            }
        }

        private byte[] GenerateQrCode(string qrmsg)
        {
            QRCoder.QRCodeGenerator qRCodeGenerator = new QRCoder.QRCodeGenerator();
            QRCoder.QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(qrmsg, QRCoder.QRCodeGenerator.ECCLevel.Q);
            QRCoder.QRCode qRCode = new QRCoder.QRCode(qRCodeData);

            using (Bitmap bmp = qRCode.GetGraphic(5))
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
    }
}
