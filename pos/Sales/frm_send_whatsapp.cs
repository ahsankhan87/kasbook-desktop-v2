using CrystalDecisions.CrystalReports.Engine;
using POS.BLL;
using POS.Core;
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
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
                }else
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
                string appPath = Path.GetDirectoryName(Application.ExecutablePath);
                ReportDocument rpt = new ReportDocument();
                rpt.Load(appPath + (chkIncludeCode.Checked ? "\\reports\\sales_invoice.rpt" : "\\reports\\sales_invoice_sans_code.rpt"));
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
    }
}