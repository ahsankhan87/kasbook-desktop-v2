using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace pos.About
{
    public partial class frm_about : Form
    {
        private PictureBox picLogo;
        private PictureBox picSisterLogo;
        private Label lblCompanyName;
        private Label lblLocations;
        private Label lblContactTitle;
        private LinkLabel lnkWhatsappSa;
        private LinkLabel lnkWhatsappPk;
        private Label lblEmail;
        private Label lblWebsite;
        private Label lblSisterTitle;
        private Label lblSisterWebsite;
        private Label lblSisterLocation;
        private TextBox txtDescription;
        private Button btnClose;
        private TableLayoutPanel sisterPanel;
        private TableLayoutPanel mainPanel;

        public frm_about()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            var isArabic = System.Globalization.CultureInfo.CurrentUICulture.IetfLanguageTag == "ar-SA";

            this.Text = isArabic ? "حول - شركة نوزم للتقنيات" : "About - NOZUM Technologies";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ClientSize = new Size(780, 520);
            this.Font = new Font("Segoe UI", 9F);

            // Main company panel (logo + info stacked)
            mainPanel = new TableLayoutPanel
            {
                Location = new Point(20, 20),
                AutoSize = true,
                ColumnCount = 2,
                RowCount = 7,
            };
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130));
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            // Main company logo
            picLogo = new PictureBox
            {
                Size = new Size(120, 120),
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle
            };
            TryLoadImage(picLogo, Path.Combine(Application.StartupPath, "nozum.png"));

            // Main company labels
            lblCompanyName = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                Text = isArabic ? "شركة نوزم للتقنيات" : "NOZUM Technologies",
                Anchor = AnchorStyles.Left
            };

            lblLocations = new Label
            {
                AutoSize = true,
                Text = isArabic ? "الدمام، الرياض، جدة - المملكة العربية السعودية" : "Dammam, Riyadh, Jeddah, Saudi Arabia",
                Anchor = AnchorStyles.Left
            };

            lblContactTitle = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Text = isArabic ? "التواصل" : "Contact",
                Anchor = AnchorStyles.Left
            };

            lnkWhatsappSa = new LinkLabel
            {
                AutoSize = true,
                Text = isArabic ? "واتساب (السعودية): +966 56 155 6977" : "WhatsApp (SA): +966 56 155 6977",
                Anchor = AnchorStyles.Left
            };
            lnkWhatsappSa.LinkClicked += (s, e) => OpenWhatsApp("966561556977");

            lnkWhatsappPk = new LinkLabel
            {
                AutoSize = true,
                Text = isArabic ? "واتساب (باكستان): +92 345 9079213" : "WhatsApp (PK): +92 345 9079213",
                Anchor = AnchorStyles.Left
            };
            lnkWhatsappPk.LinkClicked += (s, e) => OpenWhatsApp("923459079213");

            lblEmail = new Label
            {
                AutoSize = true,
                Text = isArabic ? "البريد الإلكتروني: info@nozumtech.com" : "Email: info@nozumtech.com",
                Anchor = AnchorStyles.Left
            };

            lblWebsite = new Label
            {
                AutoSize = true,
                ForeColor = Color.Blue,
                Cursor = Cursors.Hand,
                Text = isArabic ? "الموقع: https://www.nozumtech.com" : "Website: https://www.nozumtech.com",
                Anchor = AnchorStyles.Left
            };
            lblWebsite.Click += (s, e) => OpenUrl("https://www.nozumtech.com");

            // Add main components
            mainPanel.Controls.Add(picLogo, 0, 0);
            mainPanel.SetRowSpan(picLogo, 7);
            mainPanel.Controls.Add(lblCompanyName, 1, 0);
            mainPanel.Controls.Add(lblLocations, 1, 1);
            mainPanel.Controls.Add(lblContactTitle, 1, 2);
            mainPanel.Controls.Add(lnkWhatsappSa, 1, 3);
            mainPanel.Controls.Add(lnkWhatsappPk, 1, 4);
            mainPanel.Controls.Add(lblEmail, 1, 5);
            mainPanel.Controls.Add(lblWebsite, 1, 6);

            // Sister company title
            lblSisterTitle = new Label
            {
                AutoSize = true,
                Location = new Point(20, mainPanel.Bottom + 10),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Text = isArabic ? "الشركة الشقيقة" : "Sister Company"
            };

            // Sister company area
            sisterPanel = new TableLayoutPanel
            {
                Location = new Point(20, lblSisterTitle.Bottom + 8),
                AutoSize = true,
                ColumnCount = 2,
                RowCount = 2,
            };
            sisterPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130));
            sisterPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            sisterPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            sisterPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            picSisterLogo = new PictureBox
            {
                Size = new Size(120, 120),
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle
            };
            TryLoadImage(picSisterLogo, Path.Combine(Application.StartupPath, "khybersoft.png"));

            lblSisterLocation = new Label
            {
                AutoSize = true,
                Text = isArabic ? "شركة خيبرسوفت — بيشاور، باكستان" : "KhyberSoft — Peshawar, Pakistan",
                Anchor = AnchorStyles.Left
            };

            lblSisterWebsite = new Label
            {
                AutoSize = true,
                ForeColor = Color.Blue,
                Cursor = Cursors.Hand,
                Text = isArabic ? "الموقع: https://khybersoft.com" : "Website: https://khybersoft.com",
                Anchor = AnchorStyles.Left
            };
            lblSisterWebsite.Click += (s, e) => OpenUrl("https://khybersoft.com");

            sisterPanel.Controls.Add(picSisterLogo, 0, 0);
            sisterPanel.SetRowSpan(picSisterLogo, 2);
            sisterPanel.Controls.Add(lblSisterLocation, 1, 0);
            sisterPanel.Controls.Add(lblSisterWebsite, 1, 1);

            // Description
            txtDescription = new TextBox
            {
                Location = new Point(20, sisterPanel.Bottom + 40),
                Size = new Size(740, 140),
                Multiline = true,
                ReadOnly = true,
                BorderStyle = BorderStyle.FixedSingle,
                ScrollBars = ScrollBars.Vertical,
                Text = isArabic
                    ? "نوزم للتقنيات تقدم حلول نقاط البيع والبرمجيات المؤسسية.\r\n" +
                      "لدعم العملاء، يرجى التواصل عبر واتساب أو البريد الإلكتروني.\r\n\r\n" +
                      "خيبرسوفت (الشركة الشقيقة) تقدم حلولاً في بيشاور، باكستان."
                    : "NOZUM Technologies provides POS and enterprise software solutions.\r\n" +
                      "For support, please reach us on WhatsApp or email.\r\n\r\n" +
                      "KhyberSoft (sister company) focuses on solutions based in Peshawar, Pakistan."
            };

            btnClose = new Button
            {
                Text = isArabic ? "حسناً" : "OK",
                DialogResult = DialogResult.OK,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                Location = new Point(this.ClientSize.Width - 100, this.ClientSize.Height - 50),
                Size = new Size(80, 28)
            };
            btnClose.Click += (s, e) => this.Close();

            // Add to form
            this.Controls.Add(mainPanel);
            this.Controls.Add(lblSisterTitle);
            this.Controls.Add(sisterPanel);
            this.Controls.Add(txtDescription);
            this.Controls.Add(btnClose);
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
                // Ignore load failures; keep placeholder border
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
                MessageBox.Show((System.Globalization.CultureInfo.CurrentUICulture.IetfLanguageTag == "ar-SA" ? "تعذر فتح الرابط." : "Unable to open link.") + "\n" + ex.Message, 
                    System.Globalization.CultureInfo.CurrentUICulture.IetfLanguageTag == "ar-SA" ? "حول" : "About", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private static void OpenWhatsApp(string internationalNumberDigitsOnly)
        {
            var url = $"https://wa.me/{internationalNumberDigitsOnly}";
            OpenUrl(url);
        }
    }
}