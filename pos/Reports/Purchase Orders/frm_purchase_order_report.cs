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

namespace pos
{
    public partial class frm_purchase_order_report : Form
    {
        string _InvoiceNo;
        bool _isDirectPrint=false;

        public frm_purchase_order_report(string InvoiceNo,bool DirectPrint)
        {
            InitializeComponent();
            _InvoiceNo = InvoiceNo;
            _isDirectPrint = DirectPrint;
        }

        private void frm_purchase_order_report_Load(object sender, EventArgs e)
        {
            load_print(_InvoiceNo, _isDirectPrint);
        }

        public void load_print(string InvoiceNo, bool IsDirectPrint)
        {
            
            CompaniesBLL company_obj = new CompaniesBLL();
            DataTable company_dt = company_obj.GetCompany();
            string company_name = "";
            string company_address = "";
            string company_vat_no = "";
            string company_contact_no = "";
            string company_email = "";

            foreach (DataRow dr_company in company_dt.Rows)
            {
                company_name = dr_company["name"].ToString();
                company_address = dr_company["address"].ToString();
                company_vat_no = dr_company["vat_no"].ToString();
                company_contact_no = dr_company["contact_no"].ToString();
                company_email = dr_company["email"].ToString();
            }

            // Create an instance of your report
            string appPath = Path.GetDirectoryName(Application.ExecutablePath);
            ReportDocument rptDoc = new ReportDocument();
            rptDoc.Load(appPath + @"\\reports\\Accounts\\Purchases\\Orders\\PurchaseOrder.rpt");

            // Use the centralized connection method
            ReportConnectionManager.SetDatabaseLogon(rptDoc);

            rptDoc.SetParameterValue("InvoiceNo", InvoiceNo);
            rptDoc.SetParameterValue("company_name", company_name);
            rptDoc.SetParameterValue("company_vat", company_vat_no);
            rptDoc.SetParameterValue("company_address", company_address);
            rptDoc.SetParameterValue("company_contact", company_contact_no);
            rptDoc.SetParameterValue("company_email", company_email);

            crystalReportViewer.ReportSource = rptDoc;

            if (IsDirectPrint)
            {
                rptDoc.PrintToPrinter(1, true, 0, 0);
            }
            //crystalReportViewer_sales_invoice.ReportSource = sales_invoice1;
            //crystalReportViewer_sales_invoice.RefreshReport();
        }
    }
}
