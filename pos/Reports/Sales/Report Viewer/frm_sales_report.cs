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
    public partial class frm_sales_report : Form
    {
        DataTable _dt;
        bool _isPrint = false;

        public frm_sales_report(DataTable sales_detail, bool isPrint)
        {
            InitializeComponent();
            _dt = sales_detail;
            _isPrint = isPrint;
        }

        public frm_sales_report()
        {
            InitializeComponent();
        }

        private void frm_sales_report_Load(object sender, EventArgs e)
        {
            load_print();
        }

        public void load_print()
        {

            double total_amount = 0;
            double total_tax = 0;
            double total_discount = 0;
            double net_total = 0;
            string sale_date = "";

            foreach (DataRow dr in _dt.Rows)
            {
                total_amount += Convert.ToDouble(dr["total"]);
                total_tax += Convert.ToDouble(dr["vat"]);
                total_discount += Convert.ToDouble(dr["discount_value"]);
                sale_date = dr["sale_date"].ToString();
                //sale_date_1 = sale_date.Date.ToString("d");
            }
            net_total = total_amount - total_discount + total_tax;
            //string s_date = Convert.ToDateTime(sale_date).ToString("yyyy-MM-ddTHH:mm:ss");

            string appPath = Path.GetDirectoryName(Application.ExecutablePath);
            ReportDocument rptDoc = new ReportDocument();
            rptDoc.Load(appPath + @"\\reports\\sales_report.rpt");
            //rptDoc.Load("D:\\desktop app\\pos\\pos\\Reports\\Sales\\rpt\\sales_report.rpt");
            
            rptDoc.SetDataSource(_dt);

            rptDoc.SetParameterValue("subtotal", total_amount);
            rptDoc.SetParameterValue("total_discount", total_discount);
            rptDoc.SetParameterValue("total_vat", total_tax);
            rptDoc.SetParameterValue("net_total", net_total);
            crystalReportViewer_sales_report.ReportSource = rptDoc;

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

            //sales_invoice1.SetDataSource(_dt);
            rptDoc.SetParameterValue("company_name", company_name);
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
