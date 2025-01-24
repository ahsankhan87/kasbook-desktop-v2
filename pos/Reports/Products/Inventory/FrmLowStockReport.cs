using CrystalDecisions.CrystalReports.Engine;
using POS.BLL;
using POS.Core;
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

namespace pos.Reports.Products.Inventory
{
    public partial class FrmLowStockReport : Form
    {
        public FrmLowStockReport()
        {
            InitializeComponent();
        }

        private void Btn_generate_Click(object sender, EventArgs e)
        {
            /// Assuming you retrieve logged -in user details
            int branchId = UsersModal.logged_in_branch_id;
            int userId = 1;

            string selectedCategory = null;
            string selectedBrand = null;
            string selectedLocation = null;

            // Load the report
            LoadLowStockReport(branchId, userId, selectedCategory, selectedBrand, selectedLocation);
        }
        public void LoadLowStockReport(int branchId, int userId, string category = null, string brand = null, string location = null)
        {
            // Create a new dataset
            //_ = new DataSet();
            WarehouseReportBLL sale_report_obj = new WarehouseReportBLL();
            DataSet ds = sale_report_obj.InventoryReport(branchId, userId, category, brand, location,2);

            // Load Crystal Report
            // Create an instance of your report
            string appPath = Path.GetDirectoryName(Application.ExecutablePath);
            ReportDocument reportDoc = new ReportDocument();
            reportDoc.Load(appPath + @"\\reports\\Accounts\\Inventory\\LowStockReport.rpt");

            reportDoc.SetDataSource(ds.Tables["StockReport"]);

            CompaniesBLL company_obj = new CompaniesBLL();
            DataTable company_dt = company_obj.GetCompany();
            string company_name = "";

            foreach (DataRow dr_company in company_dt.Rows)
            {
                company_name = dr_company["name"].ToString();
            }

            // Set the parameter value
            reportDoc.SetParameterValue("CompanyName", company_name);

            // Set report to viewer
            crystalReportViewer1.ReportSource = reportDoc;
            crystalReportViewer1.Refresh();
        }

    }
}
