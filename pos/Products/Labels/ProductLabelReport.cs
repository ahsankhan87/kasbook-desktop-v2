using CrystalDecisions.CrystalReports.Engine;
using System;

using System.Data;

using System.IO;

using System.Windows.Forms;

namespace pos
{
    public partial class frm_ProductLabelReport : Form
    {
        DataTable _dt;

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
            load_print();
        }

        public void load_print()
        {
            DataTable new_dt = new DataTable();

            new_dt.Columns.Add("id");
            new_dt.Columns.Add("code");
            new_dt.Columns.Add("name");
            new_dt.Columns.Add("qty");
            new_dt.Columns.Add("unit_price");
            new_dt.Columns.Add("barcode");
            new_dt.Columns.Add("location_code");
            
            foreach (DataRow dr in _dt.Rows)
            {
                for (int i = 0; i < int.Parse(dr["label_qty"].ToString()); i++)
                {
                    int id = Convert.ToInt32(dr["id"]);
                    string code = dr["code"].ToString();
                    string name = dr["name"].ToString();
                    string qty = dr["qty"].ToString();
                    string unit_price = Math.Round(decimal.Parse(dr["unit_price"].ToString()),2).ToString();
                    string barcode = dr["barcode"].ToString();
                    string location_code = dr["location_code"].ToString();
                    
                    string[] row0 = { id.ToString(), code, name, qty, unit_price, barcode, location_code };

                    new_dt.Rows.Add(row0);
                }
            }

            string appPath = Path.GetDirectoryName(Application.ExecutablePath);
            ReportDocument rptDoc = new ReportDocument();
            rptDoc.Load(appPath + @"\\reports\\ProductLabel_2.rpt");
            rptDoc.SetDataSource(new_dt);
            //rptDoc.PrintToPrinter(1, true, 0, 0); // direct print without loading viewer
            crystalReportViewer1.ReportSource = rptDoc;
            crystalReportViewer1.Refresh();
        }
    }
}
