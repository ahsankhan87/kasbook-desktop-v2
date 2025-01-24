using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using POS.BLL;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;

namespace pos
{
    public partial class frm_all_sales : Form
    {
        public SalesBLL objSalesBLL = new SalesBLL();

        public frm_all_sales()
        {
            InitializeComponent();
        }

        public void frm_all_sales_Load(object sender, EventArgs e)
        {
            load_all_sales_grid();

        }

        public void load_all_sales_grid()
        {
            try
            {
                grid_all_sales.DataSource = null;

                //bind data in data grid view  
                grid_all_sales.AutoGenerateColumns = false;

                //String keyword = "id,name,date_created";
                // String table = "pos_all_sales";
                grid_all_sales.DataSource = objSalesBLL.GetAllSales();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            load_all_sales_grid();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {

                String condition = txt_search.Text;
                if (!string.IsNullOrEmpty(condition))
                {
                    grid_all_sales.DataSource = objSalesBLL.SearchRecord(condition);
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void txt_search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) // Enter
            {
                btn_search.PerformClick();
            }
        }

        private void grid_all_sales_KeyDown(object sender, KeyEventArgs e)
        {
            if (grid_all_sales.RowCount > 0 && e.KeyCode == Keys.Enter)
            {
                var sale_id = grid_all_sales.CurrentRow.Cells["id"].Value.ToString(); // retreive the current row
                var invoice_no = grid_all_sales.CurrentRow.Cells["invoice_no"].Value.ToString();

                load_sales_items_detail(Convert.ToInt32(sale_id), invoice_no);
            }
        }

        private void grid_all_sales_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var sale_id = grid_all_sales.CurrentRow.Cells["id"].Value.ToString(); // retreive the current row
            var invoice_no = grid_all_sales.CurrentRow.Cells["invoice_no"].Value.ToString();

            load_sales_items_detail(Convert.ToInt32(sale_id), invoice_no);
        }

        private void load_sales_items_detail(int sale_id, string invoice_no)
        {
            frm_sales_detail frm_sales_detail_obj = new frm_sales_detail(sale_id, invoice_no);
            //frm_sales_detail_obj.load_sales_detail_grid();
            frm_sales_detail_obj.ShowDialog();
        }

        private void frm_all_sales_KeyDown(object sender, KeyEventArgs e)
        {

        }

        public DataTable load_sales_receipt()
        {
            //DataTable dt = new DataTable();
            if (grid_all_sales.Rows.Count > 0)
            {
                var invoice_no = grid_all_sales.CurrentRow.Cells["invoice_no"].Value.ToString();
                //bind data in data grid view  
                return objSalesBLL.SaleReceipt(invoice_no);
            }
            return null;

        }

        private void btn_print_Click(object sender, EventArgs e)
        {

            using (frm_sales_receipt obj = new frm_sales_receipt(load_sales_receipt()))
            {
                obj.ShowDialog();
            }

        }

        private void btn_print_invoice_Click(object sender, EventArgs e)
        {
            if (grid_all_sales.Rows.Count > 0)
            {
                DialogResult result1 = MessageBox.Show("Print invoice with product code?", "Print Invoice", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

                bool isPrintInvoiceWithCode = false;
                if (result1 == DialogResult.Yes)
                {
                    isPrintInvoiceWithCode = true;
                }

                string sale_time = grid_all_sales.CurrentRow.Cells["sale_time"].Value.ToString();
                string invoice_no = grid_all_sales.CurrentRow.Cells["invoice_no"].Value.ToString();
                double netTotal = Convert.ToDouble(grid_all_sales.CurrentRow.Cells["total"].Value.ToString());
                double totalTax = Convert.ToDouble(grid_all_sales.CurrentRow.Cells["total_tax"].Value.ToString());
                string Zetca_qrcode = grid_all_sales.CurrentRow.Cells["Zetca_qrcode"].Value.ToString();

                //pos.Reports.Sales.New.SaleInvoiceReport saleInvoiceReport = new Reports.Sales.New.SaleInvoiceReport();
                //saleInvoiceReport.LoadReport(invoice_no, sale_time, netTotal,totalTax,false, isPrintInvoiceWithCode, Zetca_qrcode);
                //saleInvoiceReport.Show();

                using (frm_sales_invoice obj = new frm_sales_invoice(load_sales_receipt(), false, isPrintInvoiceWithCode))
                {
                    obj.ShowDialog();
                }
            }
        }

        private void grid_all_sales_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string name = grid_all_sales.Columns[e.ColumnIndex].Name;
                if (name == "detail")
                {
                    var sale_id = grid_all_sales.CurrentRow.Cells["id"].Value.ToString(); // retreive the current row
                    var invoice_no = grid_all_sales.CurrentRow.Cells["invoice_no"].Value.ToString();

                    load_sales_items_detail(Convert.ToInt32(sale_id), invoice_no);

                }
                if (name == "btn_delete")
                {
                    DialogResult result = MessageBox.Show("Are you sure you want to delete", "Sale Transaction", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        var invoice_no = grid_all_sales.CurrentRow.Cells["invoice_no"].Value.ToString();

                        int qresult = objSalesBLL.DeleteSales(invoice_no);
                        if (qresult > 0)
                        {
                            MessageBox.Show(invoice_no + " deleted successfully", "Delete Sales", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            load_all_sales_grid();
                        }
                        else
                        {
                            MessageBox.Show(invoice_no + " not deleted, please try again", "Delete Sales", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(grid_all_sales.CurrentRow.Cells["invoice_no"].Value.ToString());

        }

        private void BtnCustomerNameChange_Click(object sender, EventArgs e)
        {
            try
            {
                if (grid_all_sales.Rows.Count > 0)
                {
                    string invoiceNo = grid_all_sales.CurrentRow.Cells["invoice_no"].Value.ToString();
                    pos.Customers.CustomerNameChange customerNameChange = new Customers.CustomerNameChange(invoiceNo);
                    customerNameChange.ShowDialog();
                    load_all_sales_grid();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            

        }

        private void BtnUBLInvoice_Click(object sender, EventArgs e)
        {
            string invoiceNo = grid_all_sales.CurrentRow.Cells["invoice_no"].Value.ToString();
            Sales.EInvoice eInvoice = new Sales.EInvoice();
            string xmlContent = eInvoice.CreateUBLInvoice(invoiceNo);
            
            SaveXmlToFile(xmlContent, invoiceNo);
        }
        private void SaveXmlToFile(string xmlContent, string invoiceNo)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "XML Files (*.xml)|*.xml";
                saveFileDialog.Title = "Save UBL XML File";
                saveFileDialog.FileName = invoiceNo+".xml";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Save the XML content to the selected file
                    File.WriteAllText(saveFileDialog.FileName, xmlContent);
                    System.Windows.MessageBox.Show("XML file saved successfully.");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Sales.ZatcaInvoiceApp zatcaInvoiceApp = new Sales.ZatcaInvoiceApp();
                zatcaInvoiceApp.Show();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
