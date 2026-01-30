using BarcodeStandard;
using CrystalDecisions.CrystalReports.Engine;
using SkiaSharp;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using pos.UI;
using pos.UI.Busy;

namespace pos
{
    public partial class frm_ProductLabelReport : Form
    {
        private DataTable _dt;

        public frm_ProductLabelReport(DataTable sales_detail)
        {
            _dt = sales_detail;
            InitializeComponent();
        }

        public frm_ProductLabelReport()
        {
            InitializeComponent();
        }

        private void frm_ProductLabelReport_Load(object sender, EventArgs e)
        {
            using (BusyScope.Show(this, UiMessages.T("Preparing report...", "جاري تجهيز التقرير...")))
            {
                load_print();
            }
        }

        public void load_print()
        {
            try
            {
                if (_dt == null || _dt.Rows.Count == 0)
                {
                    UiMessages.ShowWarning(
                        "No data to print.",
                        "لا توجد بيانات للطباعة.",
                        captionEn: "Labels",
                        captionAr: "الملصقات");
                    return;
                }

                DataTable new_dt = new DataTable();
                new_dt.Columns.Add("id", typeof(int));
                new_dt.Columns.Add("code", typeof(string));
                new_dt.Columns.Add("name", typeof(string));
                new_dt.Columns.Add("qty", typeof(string));
                new_dt.Columns.Add("unit_price", typeof(string));
                // Crystal expects field type = BLOB (byte[]) for images
                new_dt.Columns.Add("barcode", typeof(byte[]));
                new_dt.Columns.Add("location_code", typeof(string));

                foreach (DataRow dr in _dt.Rows)
                {
                    // label_qty comes from grid; default to 0
                    decimal labelQty = 0;
                    decimal.TryParse(Convert.ToString(dr["label_qty"]), out labelQty);
                    if (labelQty <= 0)
                        continue;

                    int id = Convert.ToInt32(dr["id"]);
                    string code = Convert.ToString(dr["code"]);
                    string name = Convert.ToString(dr["name"]);
                    string qty = Convert.ToString(dr["qty"]);

                    decimal up;
                    decimal.TryParse(Convert.ToString(dr["unit_price"]), out up);
                    string unit_price = Math.Round(up, 2).ToString();

                    string barcodeText = (Convert.ToString(dr["barcode"]) ?? string.Empty).Trim();
                    string location_code = Convert.ToString(dr["location_code"]);

                    byte[] barcodeImage = GenerateBarcodePngBytes(barcodeText);

                    for (int i = 0; i < labelQty; i++)
                    {
                        var row = new_dt.NewRow();
                        row["id"] = id;
                        row["code"] = code;
                        row["name"] = name;
                        row["qty"] = qty;
                        row["unit_price"] = unit_price;
                        row["barcode"] = barcodeImage; // byte[]
                        row["location_code"] = location_code;
                        new_dt.Rows.Add(row);
                    }
                }

                if (new_dt.Rows.Count == 0)
                {
                    UiMessages.ShowWarning(
                        "Nothing to print. Please enter label quantities.",
                        "لا يوجد ما يمكن طباعته. يرجى إدخال عدد الملصقات.",
                        captionEn: "Labels",
                        captionAr: "الملصقات");
                    return;
                }

                string appPath = Path.GetDirectoryName(Application.ExecutablePath);
                var rptPath = Path.Combine(appPath, "reports", "ProductLabel_2.rpt");

                ReportDocument rptDoc = new ReportDocument();
                rptDoc.Load(rptPath);
                rptDoc.SetDataSource(new_dt);

                crystalReportViewer1.ReportSource = rptDoc;
                crystalReportViewer1.Refresh();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, captionEn: "Error", captionAr: "خطأ");
            }
        }

        /// <summary>
        /// Generate barcode image as PNG bytes for Crystal Reports BLOB field.
        /// NOTE: Ensure your .rpt field type is BLOB and placed as a Picture.
        /// </summary>
        private static byte[] GenerateBarcodePngBytes(string barcodeText)
        {
            // If invalid/empty barcode, return a transparent 1x1 PNG to avoid Crystal errors.
            if (string.IsNullOrWhiteSpace(barcodeText))
                return CreateTransparentPngBytes(1, 1);

            // BarcodeStandard will throw for unsupported formats/lengths depending on type.
            // Use Code128 for general alphanumeric; switch if you need EAN/UPC only.
            var barcode = new Barcode
            {
                IncludeLabel = true
            };

            SKImage img;
            try
            {
                img = barcode.Encode(BarcodeStandard.Type.Code128, barcodeText, SKColors.Black, SKColors.White, 290, 120);
            }
            catch
            {
                // fallback: return placeholder rather than crashing
                return CreateTransparentPngBytes(1, 1);
            }

            using (img)
            using (var data = img.Encode(SKEncodedImageFormat.Png, 100))
            {
                return data.ToArray();
            }
        }

        private static byte[] CreateTransparentPngBytes(int width, int height)
        {
            using (var surface = SKSurface.Create(new SKImageInfo(width, height, SKColorType.Rgba8888, SKAlphaType.Premul)))
            {
                surface.Canvas.Clear(SKColors.Transparent);
                using (var image = surface.Snapshot())
                using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                {
                    return data.ToArray();
                }
            }
        }
    }
}
