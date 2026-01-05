using CrystalDecisions.CrystalReports.Engine;
using POS.BLL;
using POS.Core;
using QRCoder;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions; // added for phone normalization
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos
{
    public class frm_send_whatsapp : Form
    {
        private TextBox txtPhone;
        private Button btnSend;
        private Button btnCancel;
        private CheckBox chkIncludeCode;
        private Label lblInfo;
        private GroupBox grpMode;
        private RadioButton rbDesktop;
        private RadioButton rbWeb;
        private readonly string _invoiceNo;
        private readonly bool _isEstimate;
        private readonly SalesBLL _salesBll = new SalesBLL();
        private readonly EstimatesBLL _estimatesBll = new EstimatesBLL();
        private string lang = (UsersModal.logged_in_lang.Length > 0 ? UsersModal.logged_in_lang : "en-US");

        public frm_send_whatsapp(string invoiceNo, bool isEstimate = false)
        {
            _invoiceNo = invoiceNo;
            _isEstimate = isEstimate;
            Init();
        }

        private void Init()
        {
            this.Text = (lang == "en-US" ? "Send Invoice via WhatsApp" : "إرسال الفاتورة عبر WhatsApp");
            this.Size = new Size(430, 250);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            lblInfo = new Label
            {
                Left = 20,
                Top = 15,
                Width = 380,
                Text = (lang == "en-US" ? "Customer WhatsApp Number (countrycode+number):" : "رقم واتساب )العميل (رمز الدولة + الرقم):")
            };
            txtPhone = new TextBox
            {
                Left = 20,
                Top = 40,
                Width = 380,
                Text = ""
            };
            chkIncludeCode = new CheckBox
            {
                Left = 20,
                Top = 70,
                Width = 200,
                Checked = true,
                Text = (lang == "en-US" ? "Include product codes" : "تضمين رموز المنتج")
            };
            grpMode = new GroupBox
            {
                Left = 20,
                Top = 100,
                Width = 200,
                Height = 70,
                Text = "Send Mode"
            };
            rbDesktop = new RadioButton
            {
                Left = 10,
                Top = 20,
                Width = 120,
                Text = "Desktop",
                Checked = true
            };
            rbWeb = new RadioButton
            {
                Left = 10,
                Top = 40,
                Width = 120,
                Text = "Web"
            };
            grpMode.Controls.Add(rbDesktop);
            grpMode.Controls.Add(rbWeb);

            btnSend = new Button
            {
                Left = 250,
                Top = 115,
                Width = 150,
                Text = "Send"
            };
            btnCancel = new Button
            {
                Left = 250,
                Top = 155,
                Width = 150,
                Text = "Cancel"
            };

            btnSend.Click += async (s, e) => await SendAsync();
            btnCancel.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] { lblInfo, txtPhone, chkIncludeCode, grpMode, btnSend, btnCancel });

            this.AcceptButton = btnSend;
            this.CancelButton = btnCancel;

            TryAutoFillCustomerPhone(); // auto fill after controls created
        }

        private void TryAutoFillCustomerPhone()
        {
            try
            {
                DataTable dt = null;
                // Get customer ID from invoice
                if (_isEstimate)
                {
                    dt = _estimatesBll.SaleReceipt(_invoiceNo);
                }
                else
                {
                    dt = _salesBll.SaleReceipt(_invoiceNo);

                }
                if (dt == null || dt.Rows.Count == 0) return;
                DataRow r = dt.Rows[0];
                int customerId = 0;
                if (r.Table.Columns.Contains("customer_id") && r["customer_id"] != DBNull.Value)
                {
                    int.TryParse(r["customer_id"].ToString(), out customerId);
                }
                if (customerId <= 0) return; // walk-in
                CustomerBLL custBll = new CustomerBLL();
                DataTable custDt = custBll.SearchRecordByCustomerID(customerId);
                if (custDt.Rows.Count == 0) return;
                string rawPhone = Convert.ToString(custDt.Rows[0]["contact_no"]);
                if (string.IsNullOrWhiteSpace(rawPhone)) return;
                // Normalize: remove non digits, remove leading +, keep country code if present
                string digits = Regex.Replace(rawPhone, "[^0-9]", "");
                if (digits.StartsWith("00")) digits = digits.Substring(2); // replace international 00 prefix
                // If user already entered something don't overwrite
                if (string.IsNullOrWhiteSpace(txtPhone.Text)) txtPhone.Text = digits;
            }
            catch { /* silent */ }
        }

        private bool ValidatePhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return false;
            if (phone.StartsWith("+")) phone = phone.Substring(1);
            foreach (char c in phone)
            {
                if (!char.IsDigit(c)) return false;
            }
            return phone.Length >= 8;
        }

        private async Task SendAsync()
        {
            string phone = txtPhone.Text.Trim();
            if (phone.StartsWith("+")) phone = phone.Substring(1);
            if (!ValidatePhone(phone))
            {
                MessageBox.Show("Invalid phone number.", "WhatsApp", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                return;
            }
            try
            {
                //DataTable dt = _salesBll.SaleReceipt(_invoiceNo);
                DataTable dt = null;
                // Get customer ID from invoice
                if (_isEstimate)
                {
                    dt = _estimatesBll.SaleReceipt(_invoiceNo);
                }
                else
                {
                    dt = _salesBll.SaleReceipt(_invoiceNo);

                }
                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("Invoice data not found.", "WhatsApp", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                double total_amount = 0, total_tax = 0, total_discount = 0;

                foreach (DataRow dr in dt.Rows)
                {
                    total_amount = Convert.ToDouble(dr["total_amount"]);
                    total_tax = Convert.ToDouble(dr["total_tax"]);
                    total_discount = Convert.ToDouble(dr["total_discount"]);

                }
                double net_total = total_amount - total_discount + total_tax;
                CompaniesBLL company_obj = new CompaniesBLL();
                DataTable company_dt = company_obj.GetCompany();
                string company_name = "", company_address = "", company_email = "", company_vat_no = "", company_contact_no = "", StreetName = "", BuildingNumber = "", CitySubdivisionName = "", CityName = "", Postalcode = "", CountryName = "";
                foreach (DataRow cdr in company_dt.Rows)
                {
                    company_name = cdr["name"].ToString();
                    company_address = cdr["address"].ToString();
                    company_email = cdr["email"].ToString();
                    company_vat_no = cdr["vat_no"].ToString();
                    company_contact_no = cdr["contact_no"].ToString();
                    StreetName = cdr["StreetName"].ToString();
                    BuildingNumber = cdr["BuildingNumber"].ToString();
                    CitySubdivisionName = cdr["CitySubdivisionName"].ToString();
                    CityName = cdr["CityName"].ToString();
                    Postalcode = cdr["Postalcode"].ToString();
                    CountryName = cdr["CountryName"].ToString();
                }
                string saleTimeStr = dt.Rows[0].Table.Columns.Contains("sale_time") ? Convert.ToString(dt.Rows[0]["sale_time"]) : null;
                DateTime saleTime = DateTime.Now;
                DateTime parsedSaleTime;
                if (!string.IsNullOrWhiteSpace(saleTimeStr) && DateTime.TryParse(saleTimeStr, out parsedSaleTime))
                {
                    saleTime = parsedSaleTime;
                }

                // IMPORTANT: add qrcode_image + qrcode_image_phase2 columns BEFORE binding report
                EnsureInvoiceQrColumns(dt, net_total, total_tax, saleTime, company_name, company_vat_no);

                string appPath = Path.GetDirectoryName(Application.ExecutablePath);
                ReportDocument rpt = new ReportDocument();
                rpt.Load(appPath + (chkIncludeCode.Checked ? "\\reports\\sales_invoice.rpt" : "\\reports\\sales_invoice_sans_code.rpt"));

                // Bind AFTER we add QR columns
                rpt.SetDataSource(dt);

                rpt.SetParameterValue("company_name", company_name);
                rpt.SetParameterValue("company_vat", company_vat_no);
                rpt.SetParameterValue("company_address", company_address);
                rpt.SetParameterValue("company_contact", company_contact_no);
                rpt.SetParameterValue("StreetName", StreetName);
                rpt.SetParameterValue("BuildingNumber", BuildingNumber);
                rpt.SetParameterValue("CitySubdivisionName", CitySubdivisionName);
                rpt.SetParameterValue("CityName", CityName);
                rpt.SetParameterValue("Postalcode", Postalcode);
                rpt.SetParameterValue("CountryName", CountryName);
                rpt.SetParameterValue("subtotal", total_amount);
                rpt.SetParameterValue("total_discount", total_discount);
                rpt.SetParameterValue("total_vat", total_tax);
                rpt.SetParameterValue("net_total", net_total);
                rpt.SetParameterValue("company_email", company_email);


                btnSend.Enabled = false;
                var mode = rbDesktop.Checked ? WhatsAppInvoiceSender.WhatsAppSendMode.Desktop : WhatsAppInvoiceSender.WhatsAppSendMode.Web;
                SetKeyboardToEnglish();
                await WhatsAppInvoiceSender.SendInvoicePdfAsync(_invoiceNo, phone, rpt, $"Invoice {_invoiceNo} - Total {net_total:N2}", mode);
                MessageBox.Show("Invoice sent (or attempted) to WhatsApp.", "WhatsApp", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Send failed: " + ex.Message, "WhatsApp", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSend.Enabled = true;
            }
        }
        public static void SetKeyboardToEnglish()
        {
            try
            {
                // Create a CultureInfo object for English (United States)
                CultureInfo englishCulture = new CultureInfo("en-US");

                // Get the InputLanguage corresponding to the English culture
                InputLanguage englishInputLanguage = InputLanguage.FromCulture(englishCulture);

                // Set the current input language to English
                InputLanguage.CurrentInputLanguage = englishInputLanguage;

                //MessageBox.Show("Keyboard layout changed to English (US).");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error changing keyboard layout: {ex.Message}");
            }
        }
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // frm_send_whatsapp
            // 
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Name = "frm_send_whatsapp";
            this.Load += new System.EventHandler(this.frm_send_whatsapp_Load);
            this.ResumeLayout(false);

        }

        private void frm_send_whatsapp_Load(object sender, EventArgs e)
        {

        }

        private static byte[] GenerateQrCodeImage(string qrmsg, int pixelsPerModule)
        {
            QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
            QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(qrmsg, QRCodeGenerator.ECCLevel.Q);
            QRCode qRCode = new QRCode(qRCodeData);

            using (Bitmap bmp = qRCode.GetGraphic(pixelsPerModule))
            using (MemoryStream ms = new MemoryStream())
            {
                bmp.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        private static byte[] GeneratePhase2QrCodeImage(byte[] qrBytes, int pixelsPerModule)
        {
            if (qrBytes == null || qrBytes.Length == 0) return null;

            string base64String = Convert.ToBase64String(qrBytes);
            return GenerateQrCodeImage(base64String, pixelsPerModule);
        }

        private static void EnsureInvoiceQrColumns(DataTable dt, double netTotal, double totalTax, DateTime saleTime, string companyName, string companyVatNo)
        {
            // Phase-1 TLV QR (always)
            string s_date = saleTime.ToString("yyyy-MM-ddTHH:mm:ss");
            string SallerName = gethexstring(1, companyName);
            string VATReg = gethexstring(2, companyVatNo);
            string DateTimeStr = gethexstring(3, s_date);
            string TotalAmt = gethexstring(4, netTotal.ToString());
            string VatAmt = gethexstring(5, totalTax.ToString());
            string qtcode_String = SallerName + VATReg + DateTimeStr + TotalAmt + VatAmt;

            byte[] imageData = GenerateQrCodeImage(HexToBase64(qtcode_String), pixelsPerModule: 20);

            // Phase-2 QR (from DB column: zatca_qrcode_phase2)
            byte[] zatca_qrcode_phase2 = null;
            if (dt.Columns.Contains("zatca_qrcode_phase2") && dt.Rows.Count > 0)
                zatca_qrcode_phase2 = (dt.Rows[0]["zatca_qrcode_phase2"] == DBNull.Value ? null : (byte[])dt.Rows[0]["zatca_qrcode_phase2"]);

            byte[] imageData_phase2 = GeneratePhase2QrCodeImage(zatca_qrcode_phase2, pixelsPerModule: 20);

            if (!dt.Columns.Contains("qrcode_image"))
                dt.Columns.Add("qrcode_image", typeof(byte[]));
            if (!dt.Columns.Contains("qrcode_image_phase2"))
                dt.Columns.Add("qrcode_image_phase2", typeof(byte[]));

            foreach (DataRow r in dt.Rows)
            {
                r["qrcode_image"] = imageData;
                r["qrcode_image_phase2"] = (object)imageData_phase2 ?? DBNull.Value;
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