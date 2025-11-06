using CrystalDecisions.CrystalReports.Engine;
using pos.Reports;
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

namespace pos.Customers.Customer_Ledger_Report
{
    public partial class Frm_customerPaymentReceipt : Form
    {
        string _payment_id;

        public Frm_customerPaymentReceipt(string payment_id)
        {
            _payment_id = payment_id;
            InitializeComponent();
        }

        private void Frm_customerPaymentReceipt_Load(object sender, EventArgs e)
        {
            LoadReport(_payment_id, false);
        }
        public void LoadReport(string payment_id, bool _isPrint)
        {
            // Create an instance of your report
            string appPath = Path.GetDirectoryName(Application.ExecutablePath);
            ReportDocument reportDoc = new ReportDocument();
            reportDoc.Load(appPath + @"\\reports\\Accounts\\\Customers\\CustomerPayment.rpt");

            // Use the centralized connection method
            ReportConnectionManager.SetDatabaseLogon(reportDoc);

            CompaniesBLL company_obj = new CompaniesBLL();
            DataTable company_dt = company_obj.GetCompany();
            string company_name = "";

            foreach (DataRow dr_company in company_dt.Rows)
            {
                company_name = dr_company["name"].ToString();

            }

            // Set the parameter value
            reportDoc.SetParameterValue("PaymentID", payment_id);
            reportDoc.SetParameterValue("company_name", company_name);
            //reportDoc.SetParameterValue("end_date", toDate);

            // Set the report source for the CrystalReportViewer
            crystalReportViewer1.ReportSource = reportDoc;

            if (_isPrint)
            {
                reportDoc.PrintToPrinter(1, true, 0, 0);
            }
        }

        private void Frm_customerPaymentReceipt_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
