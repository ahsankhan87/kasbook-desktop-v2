using CrystalDecisions.CrystalReports.Engine;
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

namespace pos.Reports.Purchases.Report_Viewer
{
    public partial class frm_purchase_report_viewer : Form
    {
        DataTable _dt;
        bool _isPrint = false;
        string _date_range;
        string _purchase_type;
        string _employee;

        public frm_purchase_report_viewer(DataTable purchase_detail, string date_range, string purchase_type, string employee, bool isPrint)
        {
            _dt = purchase_detail;
            _isPrint = isPrint;
            _date_range = date_range;
            _purchase_type = purchase_type;
            _employee = employee;
            InitializeComponent();
        }
        public frm_purchase_report_viewer()
        {
            InitializeComponent();
        }

        private void frm_purchase_report_viewer_Load(object sender, EventArgs e)
        {
            load_print();
        }
        public void load_print()
        {
            string appPath = Path.GetDirectoryName(Application.ExecutablePath);
            ReportDocument rptDoc = new ReportDocument();
            rptDoc.Load(appPath + @"\\Reports\\Accounts\\Purchases\\PurchasesReport.rpt");

            // Make a copy and remove the last row (e.g., the "Total" row appended for grid display)
            DataTable dtForReport = _dt != null ? _dt.Copy() : new DataTable();
            if (dtForReport.Rows.Count > 0)
            {
                dtForReport.Rows.RemoveAt(dtForReport.Rows.Count - 1);
                dtForReport.AcceptChanges();
            }

            rptDoc.SetDataSource(dtForReport);
            crystalReportViewer1.ReportSource = rptDoc;

            CompaniesBLL company_obj = new CompaniesBLL();
            DataTable company_dt = company_obj.GetCompany();
            string company_name = "";
            string company_address = "";
            string company_vat_no = "";
            string company_contact_no = "";

            foreach (DataRow dr_company in company_dt.Rows)
            {
                company_name = dr_company["name"].ToString();
                company_address = dr_company["address"].ToString();
                company_vat_no = dr_company["vat_no"].ToString();
                company_contact_no = dr_company["contact_no"].ToString();
            }

            rptDoc.SetParameterValue("company_name", company_name);
            rptDoc.SetParameterValue("date_range", _date_range);
            rptDoc.SetParameterValue("purchase_type", _purchase_type);
            rptDoc.SetParameterValue("employee", _employee);

            if (_isPrint)
            {
                rptDoc.PrintToPrinter(1, true, 0, 0);
            }
        }

    }
}
