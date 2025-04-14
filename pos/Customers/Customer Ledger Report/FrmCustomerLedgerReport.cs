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
    public partial class FrmCustomerLedgerReport : Form
    {
        string _customer_id;

        public FrmCustomerLedgerReport(string customer_id)
        {
            _customer_id = customer_id;
            InitializeComponent();
        }

        public FrmCustomerLedgerReport()
        {
            InitializeComponent();
        }

        private void FrmCustomerLedgerReport_Load(object sender, EventArgs e)
        {
            CmbCondition.Items.AddRange(new string[]
           {
                "Custom", "Today", "Yesterday", "This Week", "Last Week",
                "This Month", "Last Month", "This Quarter", "Last Quarter",
                "This Year", "Last Year", "Year to Date (YTD)", "Last 7 Days",
                "Last 30 Days", "Last 90 Days", "Last 6 Months",
                "Previous Fiscal Year", "Next Fiscal Year"
           });
            CmbCondition.SelectedIndex = 0;
        }

        public void LoadReport(string customer_id, string fromDate, string toDate, bool _isPrint)
        {
            // Create an instance of your report
            string appPath = Path.GetDirectoryName(Application.ExecutablePath);
            ReportDocument reportDoc = new ReportDocument();
            reportDoc.Load(appPath + @"\\reports\\Accounts\\\Customers\\CustomerLedger.rpt");

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
            reportDoc.SetParameterValue("customer_id", customer_id);
            reportDoc.SetParameterValue("start_date", fromDate);
            reportDoc.SetParameterValue("end_date", toDate);

            // Set the report source for the CrystalReportViewer
            crystalReportViewer1.ReportSource = reportDoc;

            if (_isPrint)
            {
                reportDoc.PrintToPrinter(1, true, 0, 0);
            }
        }

        private void CmbCondition_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime today = DateTime.Today;
            DateTime startDate = today;
            DateTime endDate = today;

            switch (CmbCondition.SelectedItem.ToString())
            {
                case "Custom":
                    return;

                case "Today":
                    startDate = endDate = today;
                    break;

                case "Yesterday":
                    startDate = endDate = today.AddDays(-1);
                    break;

                case "This Week":
                    startDate = today.AddDays(-(int)today.DayOfWeek);
                    endDate = startDate.AddDays(6);
                    break;

                case "Last Week":
                    startDate = today.AddDays(-(int)today.DayOfWeek - 7);
                    endDate = startDate.AddDays(6);
                    break;

                case "This Month":
                    startDate = new DateTime(today.Year, today.Month, 1);
                    endDate = startDate.AddMonths(1).AddDays(-1);
                    break;

                case "Last Month":
                    startDate = new DateTime(today.Year, today.Month, 1).AddMonths(-1);
                    endDate = startDate.AddMonths(1).AddDays(-1);
                    break;

                case "This Quarter":
                    startDate = new DateTime(today.Year, ((today.Month - 1) / 3) * 3 + 1, 1);
                    endDate = startDate.AddMonths(3).AddDays(-1);
                    break;

                case "Last Quarter":
                    startDate = new DateTime(today.Year, ((today.Month - 1) / 3) * 3 + 1, 1).AddMonths(-3);
                    endDate = startDate.AddMonths(3).AddDays(-1);
                    break;

                case "Year to Date (YTD)":
                    startDate = new DateTime(today.Year, 1, 1);
                    endDate = today;
                    break;

                case "Last 7 Days":
                    startDate = today.AddDays(-6);
                    break;

                case "Last 30 Days":
                    startDate = today.AddDays(-29);
                    break;

                case "Last 90 Days":
                    startDate = today.AddDays(-89);
                    break;

                case "Last 6 Months":
                    startDate = today.AddMonths(-6);
                    break;

                case "This Year":
                    startDate = new DateTime(today.Year, 1, 1);
                    endDate = new DateTime(today.Year, 12, 31);
                    break;

                case "Last Year":
                    startDate = new DateTime(today.Year - 1, 1, 1);
                    endDate = new DateTime(today.Year - 1, 12, 31);
                    break;

                case "Previous Fiscal Year":
                    startDate = new DateTime(today.Year - 1, 1, 1);
                    endDate = new DateTime(today.Year - 1, 12, 31);
                    break;

                case "Next Fiscal Year":
                    startDate = new DateTime(today.Year + 1, 1, 1);
                    endDate = new DateTime(today.Year + 1, 12, 31);
                    break;
            }

            txt_from_date.Value = startDate;
            txt_to_date.Value = endDate;
        }

        private void FrmCustomerLedgerReport_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //when you enter in textbox it will goto next textbox, work like TAB key
                if (e.KeyData == Keys.Enter)
                {
                    SendKeys.Send("{TAB}");
                }
                if (e.KeyCode == Keys.F3)
                {
                    BtnSubmit.PerformClick();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            LoadReport(_customer_id, txt_from_date.Value.ToShortDateString(), txt_to_date.Value.ToShortDateString(), false);
        }
    }
}
