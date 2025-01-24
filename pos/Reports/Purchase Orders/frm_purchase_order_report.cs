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

namespace pos
{
    public partial class frm_purchase_order_report : Form
    {
        DataTable _dt;
        bool _isPrint = false;

        public frm_purchase_order_report(DataTable sales_detail, bool isPrint)
        {
            InitializeComponent();
            _dt = sales_detail;
            _isPrint = isPrint;
        }

        private void frm_purchase_order_report_Load(object sender, EventArgs e)
        {
            load_print();
        }

        public void load_print()
        {
            
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

           
            string appPath = Path.GetDirectoryName(Application.ExecutablePath);
            ReportDocument rptDoc = new ReportDocument();
            rptDoc.Load(appPath + @"\\reports\\purchase_order.rpt");
           // rptDoc.Load("D:\\desktop app\\pos\\pos\\Reports\\Purchase Orders\\purchase_order.rpt");
            rptDoc.SetDataSource(_dt);

            rptDoc.SetParameterValue("company_name", company_name);
            //rptDoc.SetParameterValue("company_vat", company_vat_no);
            //rptDoc.SetParameterValue("company_address", company_address);
            //rptDoc.SetParameterValue("company_contact", company_contact_no);

           crystalReportViewer.ReportSource = rptDoc;

            //sales_invoice1.SetDataSource(_dt);
            //sales_invoice1.SetParameterValue("company_name",company_name);
            //sales_invoice1.SetParameterValue("company_vat",company_vat_no);
            //sales_invoice1.SetParameterValue("company_address",company_address);
            //sales_invoice1.SetParameterValue("company_contact",company_contact_no);

            //sales_invoice1.SetParameterValue("subtotal", total_amount);
            //sales_invoice1.SetParameterValue("total_discount", total_discount);
            //sales_invoice1.SetParameterValue("total_vat", total_tax);
            //sales_invoice1.SetParameterValue("net_total", net_total);

            if (_isPrint)
            {
                rptDoc.PrintToPrinter(1, true, 0, 0);
            }
            //crystalReportViewer_sales_invoice.ReportSource = sales_invoice1;
            //crystalReportViewer_sales_invoice.RefreshReport();
        }
    }
}
