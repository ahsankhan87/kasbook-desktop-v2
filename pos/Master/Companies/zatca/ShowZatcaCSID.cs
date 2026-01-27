using pos.UI;
using pos.UI.Busy;
using POS.BLL;
using POS.Core;
using System;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace pos.Master.Companies.zatca
{
    public partial class ShowZatcaCSID : Form
    {
        public ShowZatcaCSID()
        {
            InitializeComponent();
        }

        private void grid_fiscal_years_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (grid_zatca_csids.CurrentRow == null) return;
            if (e.ColumnIndex < 0) return;

            string name = grid_zatca_csids.Columns[e.ColumnIndex].Name;
            if (name == "activate")
            {
                string id = Convert.ToString(grid_zatca_csids.CurrentRow.Cells["id"].Value);

                if (string.IsNullOrWhiteSpace(id))
                {
                    UiMessages.ShowWarning(
                        "Please select a valid record.",
                        "يرجى اختيار سجل صالح.",
                        captionEn: "ZATCA",
                        captionAr: "زاتكا");
                    return;
                }

                var result = UiMessages.ConfirmYesNo(
                    "Do you want to activate the selected ZATCA environment?",
                    "هل تريد تفعيل بيئة زاتكا المحددة؟",
                    captionEn: "Activate",
                    captionAr: "تفعيل",
                    defaultButton: MessageBoxDefaultButton.Button2);

                if (result == DialogResult.Yes)
                {
                    using (BusyScope.Show(this, UiMessages.T("Activating...", "جارٍ التفعيل...")))
                    {
                        try
                        {
                            ZatcaInvoiceGenerator.UpdateZatcaStatus(int.Parse(id));

                            UiMessages.ShowInfo(
                                "Environment activated successfully.",
                                "تم تفعيل البيئة بنجاح.",
                                captionEn: "ZATCA",
                                captionAr: "زاتكا");

                            LoadZatcaCSIDGrid();
                        }
                        catch (Exception ex)
                        {
                            UiMessages.ShowError(
                                "Unable to activate the selected environment.\n" + ex.Message,
                                "تعذر تفعيل البيئة المحددة.\n" + ex.Message,
                                captionEn: "ZATCA",
                                captionAr: "زاتكا");
                        }
                    }
                }
            }
        }

        private void ShowZatcaCSID_Load(object sender, EventArgs e)
        {
            LoadZatcaCSIDGrid();
        }

        protected void LoadZatcaCSIDGrid()
        {
            using (BusyScope.Show(this, UiMessages.T("Loading credentials...", "جارٍ تحميل بيانات الاعتماد...")))
            {
                try
                {
                    GeneralBLL objBLL = new GeneralBLL();
                    grid_zatca_csids.AutoGenerateColumns = false;
                    string keyword = "*";
                    string table = "zatca_credentials";
                    grid_zatca_csids.DataSource = objBLL.GetRecord(keyword, table);
                }
                catch (Exception ex)
                {
                    UiMessages.ShowError(
                        "Unable to load ZATCA credentials.\n" + ex.Message,
                        "تعذر تحميل بيانات اعتماد زاتكا.\n" + ex.Message,
                        captionEn: "ZATCA",
                        captionAr: "زاتكا");
                }
            }
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            LoadZatcaCSIDGrid();
        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            using (var autoGenerateCSR = new AutoGenerateCSID())
            {
                autoGenerateCSR.ShowDialog(this);
            }

            // reload after closing (in case user saved new credentials)
            LoadZatcaCSIDGrid();
        }

        private void btn_generatePCSID_Click(object sender, EventArgs e)
        {
            try
            {
                if (grid_zatca_csids.CurrentRow == null)
                {
                    UiMessages.ShowWarning(
                        "Please select a CSID record first.",
                        "يرجى اختيار سجل CSID أولاً.",
                        captionEn: "ZATCA",
                        captionAr: "زاتكا");
                    return;
                }

                string id = Convert.ToString(grid_zatca_csids.CurrentRow.Cells["id"].Value);
                if (string.IsNullOrWhiteSpace(id))
                {
                    UiMessages.ShowWarning(
                        "The selected record is not valid.",
                        "السجل المحدد غير صالح.",
                        captionEn: "ZATCA",
                        captionAr: "زاتكا");
                    return;
                }

                using (var generatePCSID = new GeneratePCSID(int.Parse(id)))
                {
                    generatePCSID.ShowDialog(this);
                }

                LoadZatcaCSIDGrid();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, "خطأ", "Error", "خطأ");
            }
        }

        private void btn_renew_PCSID_Click(object sender, EventArgs e)
        {
            try
            {
                if (grid_zatca_csids.CurrentRow == null)
                {
                    UiMessages.ShowWarning(
                        "Please select a CSID record first.",
                        "يرجى اختيار سجل CSID أولاً.",
                        captionEn: "ZATCA",
                        captionAr: "زاتكا");
                    return;
                }

                string id = Convert.ToString(grid_zatca_csids.CurrentRow.Cells["id"].Value);
                if (string.IsNullOrWhiteSpace(id))
                {
                    UiMessages.ShowWarning(
                        "The selected record is not valid.",
                        "السجل المحدد غير صالح.",
                        captionEn: "ZATCA",
                        captionAr: "زاتكا");
                    return;
                }

                using (var renewPCSID = new RenewPCSID(int.Parse(id)))
                {
                    renewPCSID.ShowDialog(this);
                }

                LoadZatcaCSIDGrid();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, "خطأ", "Error", "خطأ");
            }
        }

        private void btn_info_Click(object sender, EventArgs e)
        {
            if (grid_zatca_csids.CurrentRow == null)
            {
                UiMessages.ShowWarning(
                    "Please select a record first.",
                    "يرجى اختيار سجل أولاً.",
                    captionEn: "ZATCA",
                    captionAr: "زاتكا");
                return;
            }

            string publicKey = Convert.ToString(grid_zatca_csids.CurrentRow.Cells["cert_base64"].Value);

            if (!string.IsNullOrWhiteSpace(publicKey))
            {
                GetCertInfo(publicKey);
            }
            else
            {
                UiMessages.ShowInfo(
                    "No certificate data is available for the selected record.",
                    "لا توجد بيانات شهادة للسجل المحدد.",
                    captionEn: "ZATCA",
                    captionAr: "زاتكا");
            }
        }

        private void GetCertInfo(string publicKey)
        {
            if (string.IsNullOrWhiteSpace(publicKey)) return;

            try
            {
                string pemText = publicKey.Trim();

                string base64 = pemText
                    .Replace("-----BEGIN CERTIFICATE-----", "")
                    .Replace("-----END CERTIFICATE-----", "")
                    .Replace("\r", "")
                    .Replace("\n", "")
                    .Trim();

                byte[] certBytes = Convert.FromBase64String(base64);
                var certificate = new X509Certificate2(certBytes);

                string info = "";
                info = UiMessages.T("Subject: ", "الموضوع: ") + certificate.Subject + "\n";
                info += UiMessages.T("Issuer: ", "المصدر: ") + certificate.Issuer + "\n";
                info += UiMessages.T("Valid From: ", "صالح من: ") + certificate.NotBefore + "\n";
                info += UiMessages.T("Valid To: ", "صالح حتى: ") + certificate.NotAfter + "\n";

                MessageBox.Show(info, UiMessages.T("Certificate Information", "معلومات الشهادة"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    "Unable to read certificate information.\n" + ex.Message,
                    "تعذر قراءة معلومات الشهادة.\n" + ex.Message,
                    captionEn: "ZATCA",
                    captionAr: "زاتكا");
            }
        }
    }
}
