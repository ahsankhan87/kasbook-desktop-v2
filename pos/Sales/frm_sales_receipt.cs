//using Microsoft.Reporting.WinForms;
using POS.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos
{
    public partial class frm_sales_receipt : Form
    {
        DataTable _dt;
        
        public frm_sales_receipt(DataTable sales_detail)
        {
            InitializeComponent();
            _dt = sales_detail;
        }

        private void frm_sales_receipt_Load(object sender, EventArgs e)
        {
            //this.reportViewer_sales.LocalReport.EnableExternalImages = true;
            double total_amount = 0;
            double total_tax = 0;
            double total_discount = 0;
            double net_total = 0;
            string sale_date = "";
            string contact_no = "";

            foreach (DataRow dr in _dt.Rows)
            {
                total_amount += Convert.ToDouble(dr["total"]);
                total_tax += Convert.ToDouble(dr["vat"]);
                total_discount += Convert.ToDouble(dr["discount_value"]);
                sale_date = dr["sale_time"].ToString();
                //sale_date_1 = sale_date.Date.ToString("d");
            }
            net_total = total_amount - total_discount + total_tax;
            string s_date = Convert.ToDateTime(sale_date).ToString("yyyy-MM-ddTHH:mm:ss");


            CompaniesBLL company_obj = new CompaniesBLL();
            DataTable company_dt = company_obj.GetCompany();
            string company_name = "";
            string address="";
            string vat_no = "";
            foreach (DataRow dr_company in company_dt.Rows)
            {
                company_name = dr_company["name"].ToString();
                address = dr_company["address"].ToString();
                vat_no = dr_company["vat_no"].ToString();
                contact_no = dr_company["contact_no"].ToString();
            }

            string SallerName = gethexstring(1, company_name); //Tag1
            string VATReg = gethexstring(2, vat_no); //Tag2
            string DateTimeStr = gethexstring(3, s_date); //Tage3
            string TotalAmt = gethexstring(4, net_total.ToString()); //Tag4
            string VatAmt = gethexstring(5, total_tax.ToString()); //Tag5
            string qtcode_String = SallerName + VATReg + DateTimeStr + TotalAmt + VatAmt;


            byte[] imageData = GenerateQrCode(HexToBase64(qtcode_String));//GIVE DATA TO FUNCTION AND GET QRCODE
            _dt.Columns.Add("qrcode_image", typeof(byte[]));// INSERT QRCODE DATA TO DATATABLE
            foreach (DataRow dr in _dt.Rows)
            {
                dr["qrcode_image"] = imageData;
            }

            //ReportDataSource datasource_rpt = new ReportDataSource("ds_sales_receipt", _dt);

            //ReportParameter rp1 = new ReportParameter("company_name", company_name);
            //ReportParameter rp2 = new ReportParameter("address", address);
            //ReportParameter rp3 = new ReportParameter("vat_no", vat_no);
            //ReportParameter rp4 = new ReportParameter("contact_no", contact_no);
            //this.reportViewer_sales.LocalReport.SetParameters(new ReportParameter[] { rp1, rp2,rp3,rp4 });
               
            //this.reportViewer_sales.LocalReport.DataSources.Clear();
            //this.reportViewer_sales.LocalReport.DataSources.Add(datasource_rpt);
            //reportViewer_sales.RefreshReport();
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
                    bmp.Save(ms, ImageFormat.Bmp);
                    byte[] byteImage = ms.ToArray();
                    return byteImage;
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

    }

}

