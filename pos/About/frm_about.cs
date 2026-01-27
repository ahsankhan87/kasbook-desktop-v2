using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace pos.About
{
    public partial class frm_about : Form
    {
        private PictureBox picLogo;
        private PictureBox picSisterLogo;

        private Label lblHeaderTitle;
        private Label lblHeaderSubtitle;
        private Label lblVersion;

        private GroupBox grpMain;
        private GroupBox grpSister;

        private LinkLabel lnkWhatsappSa;
        private LinkLabel lnkWhatsappPk;
        private LinkLabel lnkWebsite;
        private LinkLabel lnkSisterWebsite;

        private TextBox txtDescription;
        private Button btnClose;

        public frm_about()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            var isArabic = System.Globalization.CultureInfo.CurrentUICulture.IetfLanguageTag == "ar-SA";

            Text = isArabic ? "حول" : "About";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            BackColor = Color.White;
            Font = new Font("Segoe UI", 9F);
            ClientSize = new Size(860, 560);

            // RTL support
            if (isArabic)
            {
                RightToLeft = RightToLeft.Yes;
                RightToLeftLayout = true;
            }

            // Header
            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 74,
                BackColor = Color.White,
                Padding = new Padding(18, 14, 18, 10)
            };

            lblHeaderTitle = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                Text = isArabic ? "معلومات الشركة" : "Company Information",
                ForeColor = Color.FromArgb(25, 25, 25),
                TextAlign = isArabic ? ContentAlignment.MiddleRight : ContentAlignment.MiddleLeft
            };

            lblHeaderSubtitle = new Label
            {
                AutoSize = true,
                Top = 42,
                ForeColor = Color.DimGray,
                TextAlign = isArabic ? ContentAlignment.MiddleRight : ContentAlignment.MiddleLeft,
                Text = isArabic
                    ? "تفاصيل الاتصال والمواقع الخاصة بالشركات"
                    : "Company contact, locations and websites"
            };

            pnlHeader.Controls.Add(lblHeaderTitle);
            pnlHeader.Controls.Add(lblHeaderSubtitle);

            var pnlDivider = new Panel
            {
                Dock = DockStyle.Top,
                Height = 1,
                BackColor = Color.Gainsboro
            };

            // Body
            var pnlBody = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(18, 14, 18, 14)
            };

            // Position groups based on RTL
            var leftGroupX = isArabic ? 436 : 18;
            var rightGroupX = isArabic ? 18 : 436;

            grpMain = new GroupBox
            {
                Text = isArabic ? "الشركة الرئيسية" : "Main Company",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(45, 45, 45),
                Location = new Point(leftGroupX, 10),
                Size = new Size(400, 250),
                RightToLeft = isArabic ? RightToLeft.Yes : RightToLeft.No
            };

            var mainTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 8,
                Padding = new Padding(12),
                RightToLeft = isArabic ? RightToLeft.Yes : RightToLeft.No
            };
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            picLogo = new PictureBox
            {
                Size = new Size(104, 104),
                Dock = DockStyle.Top,
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };
            TryLoadImage(picLogo, Path.Combine(Application.StartupPath, "nozum.png"));

            var lblMainName = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Text = isArabic ? "شركة نوزم للتقنيات" : "NOZUM Technologies",
                Margin = new Padding(3, 2, 3, 6),
                TextAlign = isArabic ? ContentAlignment.MiddleRight : ContentAlignment.MiddleLeft
            };

            var lblMainLocationsTitle = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(70, 70, 70),
                Text = isArabic ? "المواقع" : "Locations",
                TextAlign = isArabic ? ContentAlignment.MiddleRight : ContentAlignment.MiddleLeft
            };

            var lblMainLocations = new Label
            {
                AutoSize = true,
                ForeColor = Color.FromArgb(45, 45, 45),
                TextAlign = isArabic ? ContentAlignment.MiddleRight : ContentAlignment.MiddleLeft,
                Text = isArabic
                    ? "الدمام، الرياض، جدة — المملكة العربية السعودية"
                    : "Dammam, Riyadh, Jeddah — Saudi Arabia"
            };

            var lblMainContactTitle = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(70, 70, 70),
                Text = isArabic ? "التواصل" : "Contact",
                TextAlign = isArabic ? ContentAlignment.MiddleRight : ContentAlignment.MiddleLeft
            };

            lnkWhatsappSa = new LinkLabel
            {
                AutoSize = true,
                LinkColor = Color.FromArgb(0, 102, 204),
                ActiveLinkColor = Color.FromArgb(0, 102, 204),
                VisitedLinkColor = Color.FromArgb(0, 102, 204),
                TextAlign = isArabic ? ContentAlignment.MiddleRight : ContentAlignment.MiddleLeft,
                Text = isArabic ? "واتساب (السعودية): +966 56 155 6977" : "WhatsApp (SA): +966 56 155 6977"
            };
            lnkWhatsappSa.LinkClicked += (s, e) => OpenWhatsApp("966561556977");

            lnkWhatsappPk = new LinkLabel
            {
                AutoSize = true,
                LinkColor = Color.FromArgb(0, 102, 204),
                ActiveLinkColor = Color.FromArgb(0, 102, 204),
                VisitedLinkColor = Color.FromArgb(0, 102, 204),
                TextAlign = isArabic ? ContentAlignment.MiddleRight : ContentAlignment.MiddleLeft,
                Text = isArabic ? "واتساب (باكستان): +92 345 9079213" : "WhatsApp (PK): +92 345 9079213"
            };
            lnkWhatsappPk.LinkClicked += (s, e) => OpenWhatsApp("923459079213");

            var lblEmail = new Label
            {
                AutoSize = true,
                ForeColor = Color.FromArgb(45, 45, 45),
                TextAlign = isArabic ? ContentAlignment.MiddleRight : ContentAlignment.MiddleLeft,
                Text = isArabic ? "البريد الإلكتروني: info@nozumtech.com" : "Email: info@nozumtech.com"
            };

            lnkWebsite = new LinkLabel
            {
                AutoSize = true,
                LinkColor = Color.FromArgb(0, 102, 204),
                ActiveLinkColor = Color.FromArgb(0, 102, 204),
                VisitedLinkColor = Color.FromArgb(0, 102, 204),
                TextAlign = isArabic ? ContentAlignment.MiddleRight : ContentAlignment.MiddleLeft,
                Text = isArabic ? "الموقع: www.nozumtech.com" : "Website: www.nozumtech.com"
            };
            lnkWebsite.LinkClicked += (s, e) => OpenUrl("https://www.nozumtech.com");

            mainTable.Controls.Add(picLogo, 0, 0);
            mainTable.SetRowSpan(picLogo, 3);
            mainTable.Controls.Add(lblMainName, 1, 0);
            mainTable.Controls.Add(lblMainLocationsTitle, 1, 1);
            mainTable.Controls.Add(lblMainLocations, 1, 2);
            mainTable.Controls.Add(lblMainContactTitle, 1, 3);
            mainTable.Controls.Add(lnkWhatsappSa, 1, 4);
            mainTable.Controls.Add(lnkWhatsappPk, 1, 5);
            mainTable.Controls.Add(lblEmail, 1, 6);
            mainTable.Controls.Add(lnkWebsite, 1, 7);

            grpMain.Controls.Add(mainTable);

            grpSister = new GroupBox
            {
                Text = isArabic ? "الشركة الشقيقة" : "Sister Company",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(45, 45, 45),
                Location = new Point(rightGroupX, 10),
                Size = new Size(400, 250),
                RightToLeft = isArabic ? RightToLeft.Yes : RightToLeft.No
            };

            var sisterTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 4,
                Padding = new Padding(12),
                RightToLeft = isArabic ? RightToLeft.Yes : RightToLeft.No
            };
            sisterTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            sisterTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            picSisterLogo = new PictureBox
            {
                Size = new Size(104, 104),
                Dock = DockStyle.Top,
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };
            TryLoadImage(picSisterLogo, Path.Combine(Application.StartupPath, "khybersoft.png"));

            var lblSisterName = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Text = isArabic ? "خيبرسوفت" : "KhyberSoft",
                Margin = new Padding(3, 2, 3, 6),
                TextAlign = isArabic ? ContentAlignment.MiddleRight : ContentAlignment.MiddleLeft
            };

            var lblSisterLocationTitle = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(70, 70, 70),
                Text = isArabic ? "الموقع" : "Location",
                TextAlign = isArabic ? ContentAlignment.MiddleRight : ContentAlignment.MiddleLeft
            };

            var lblSisterLocation = new Label
            {
                AutoSize = true,
                ForeColor = Color.FromArgb(45, 45, 45),
                TextAlign = isArabic ? ContentAlignment.MiddleRight : ContentAlignment.MiddleLeft,
                Text = isArabic ? "بيشاور — باكستان" : "Peshawar — Pakistan"
            };

            lnkSisterWebsite = new LinkLabel
            {
                AutoSize = true,
                LinkColor = Color.FromArgb(0, 102, 204),
                ActiveLinkColor = Color.FromArgb(0, 102, 204),
                VisitedLinkColor = Color.FromArgb(0, 102, 204),
                TextAlign = isArabic ? ContentAlignment.MiddleRight : ContentAlignment.MiddleLeft,
                Text = isArabic ? "الموقع: khybersoft.com" : "Website: khybersoft.com"
            };
            lnkSisterWebsite.LinkClicked += (s, e) => OpenUrl("https://khybersoft.com");

            sisterTable.Controls.Add(picSisterLogo, 0, 0);
            sisterTable.SetRowSpan(picSisterLogo, 3);
            sisterTable.Controls.Add(lblSisterName, 1, 0);
            sisterTable.Controls.Add(lblSisterLocationTitle, 1, 1);
            sisterTable.Controls.Add(lblSisterLocation, 1, 2);
            sisterTable.Controls.Add(lnkSisterWebsite, 1, 3);

            grpSister.Controls.Add(sisterTable);

            txtDescription = new TextBox
            {
                Location = new Point(18, 274),
                Size = new Size(818, 210),
                Multiline = true,
                ReadOnly = true,
                BorderStyle = BorderStyle.FixedSingle,
                ScrollBars = ScrollBars.Vertical,
                BackColor = Color.White,
                RightToLeft = isArabic ? RightToLeft.Yes : RightToLeft.No,
                Text = isArabic
                    ? "نوزم للتقنيات تقدم حلول نقاط البيع والبرمجيات المؤسسية.\r\n" +
                      "للدعم الفني، تواصل معنا عبر واتساب أو البريد الإلكتروني.\r\n\r\n" +
                      "خيبرسوفت (الشركة الشقيقة) تقدم حلولاً في بيشاور، باكستان."
                    : "NOZUM Technologies provides POS and enterprise software solutions.\r\n" +
                      "For support, please reach us on WhatsApp or email.\r\n\r\n" +
                      "KhyberSoft (sister company) provides solutions based in Peshawar, Pakistan."
            };

            // Footer
            var pnlFooter = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 58,
                Padding = new Padding(18, 10, 18, 10),
                BackColor = Color.White
            };

            lblVersion = new Label
            {
                AutoSize = true,
                ForeColor = Color.DimGray,
                Location = new Point(18, 18),
                TextAlign = isArabic ? ContentAlignment.MiddleRight : ContentAlignment.MiddleLeft,
                Text = (isArabic ? "الإصدار" : "Version") + ": " + GetAppVersion()
            };

            btnClose = new Button
            {
                Text = isArabic ? "إغلاق" : "Close",
                DialogResult = DialogResult.OK,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                Size = new Size(100, 30),
                Location = new Point(ClientSize.Width - 18 - 100, 14)
            };
            btnClose.Click += (s, e) => Close();

            pnlFooter.Controls.Add(lblVersion);
            pnlFooter.Controls.Add(btnClose);

            pnlBody.Controls.Add(grpMain);
            pnlBody.Controls.Add(grpSister);
            pnlBody.Controls.Add(txtDescription);

            Controls.Add(pnlBody);
            Controls.Add(pnlFooter);
            Controls.Add(pnlDivider);
            Controls.Add(pnlHeader);

            AcceptButton = btnClose;
        }

        private static string GetAppVersion()
        {
            try
            {
                var v = Assembly.GetEntryAssembly()?.GetName()?.Version;
                if (v == null) return "1.0.0";
                return v.Major + "." + v.Minor + "." + v.Build;
            }
            catch
            {
                return "1.0.0";
            }
        }

        private static void TryLoadImage(PictureBox pb, string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    pb.Image = Image.FromFile(path);
                }
            }
            catch
            {
                // Ignore load failures
            }
        }

        private static void OpenUrl(string url)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                var isArabic = System.Globalization.CultureInfo.CurrentUICulture.IetfLanguageTag == "ar-SA";
                MessageBox.Show(
                    (isArabic ? "تعذر فتح الرابط." : "Unable to open link.") + "\n" + ex.Message,
                    isArabic ? "حول" : "About",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private static void OpenWhatsApp(string internationalNumberDigitsOnly)
        {
            OpenUrl($"https://wa.me/{internationalNumberDigitsOnly}");
        }
    }
}