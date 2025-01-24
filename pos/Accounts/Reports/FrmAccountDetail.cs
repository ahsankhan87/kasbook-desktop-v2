using POS.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos.Accounts.Reports
{
    public partial class FrmAccountDetail : Form
    {
        int _accountID;
        DateTime _from_date;
        DateTime _to_date;

        public FrmAccountDetail(int accountID, DateTime from_date, DateTime to_date)
        {
            InitializeComponent();
            _accountID = accountID;
            //_invoice_no = invoice_no;
            _from_date = from_date;
            _to_date = to_date;
        }

        private void FrmAccountDetail_Load(object sender, EventArgs e)
        {
            
            Load_account_report(_from_date, _to_date, _accountID);
        }

        private void Load_account_report(DateTime from_date, DateTime to_date, int account_id)
        {
            try
            {
                grid_account_report.DataSource = null;
                grid_account_report.AutoGenerateColumns = false;

                AccountsBLL objAccountBLL = new AccountsBLL();

                DataTable accounts_dt = new DataTable();
                accounts_dt = objAccountBLL.AccountReport(from_date, to_date, account_id);

                double _dr_total = 0;
                double _cr_total = 0;
                //double _balance_total = 0;

                foreach (DataRow dr in accounts_dt.Rows)
                {
                    _dr_total += Convert.ToDouble(dr["debit"].ToString());
                    _cr_total += Convert.ToDouble(dr["credit"].ToString());
                }

                DataRow newRow = accounts_dt.NewRow();
                newRow[1] = "Total";
                newRow[3] = _dr_total;
                newRow[4] = _cr_total;
                newRow[5] = (_dr_total - _cr_total);
                accounts_dt.Rows.InsertAt(newRow, accounts_dt.Rows.Count);

                grid_account_report.DataSource = accounts_dt;

                CustomizeDataGridView();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }
        private void CustomizeDataGridView()
        {
            // Get the last row in the DataGridView
            DataGridViewRow lastRow = grid_account_report.Rows[grid_account_report.Rows.Count - 1];

            // Loop through all cells in the row
            foreach (DataGridViewCell cell in lastRow.Cells)
            {
                DataGridViewCellStyle style = new DataGridViewCellStyle(cell.Style);

                // Set the font to bold
                style.Font = new Font(grid_account_report.Font, FontStyle.Bold);

                // Set the background color
                style.BackColor = Color.LightGray;

                // Apply the style to the current cell
                cell.Style = style;
            }

        }

        private void grid_account_report_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var invoice_no = grid_account_report.CurrentRow.Cells["invoice_no"].Value.ToString();
            string invoice_chr = invoice_no.Substring(0, 1);

            if (invoice_chr.ToUpper() == "S")// for invoice check
            {
                LoadSalesInvoiceDetail(invoice_no);
            }else if(invoice_chr.ToUpper() == "P")
            {
                LoadPurchaseDetail(invoice_no);
            }
            
        }
        private void LoadSalesInvoiceDetail(string invoiceNo)
        {
            try
            {
                double _total_qty = 0;
                double _total_cost = 0;
                double _total_vat = 0;
                double _total_discount = 0;
                double _grand_total = 0;

                grid_sales_detail.DataSource = null;
                grid_sales_detail.Rows.Clear();
                grid_sales_detail.AutoGenerateColumns = false;
                SalesBLL objSalesBLL = new SalesBLL();
                DataTable dt = objSalesBLL.GetAllSalesItems(invoiceNo);

                foreach (DataRow dr in dt.Rows)
                {

                    string[] row00 = {
                        dr["id"].ToString(),
                        dr["invoice_no"].ToString(),
                        dr["item_code"].ToString(),
                        dr["item_name"].ToString(),
                        dr["loc_code"].ToString(),
                        Math.Round(Convert.ToDouble(dr["quantity_sold"]),2).ToString(),
                        Math.Round(Convert.ToDouble(dr["unit_price"]),2).ToString(),
                        Math.Round(Convert.ToDouble(dr["discount_value"]),2).ToString(),
                        Math.Round(Convert.ToDouble(dr["vat"]),2).ToString(),
                        Math.Round((Convert.ToDouble(dr["quantity_sold"])*Convert.ToDouble(dr["unit_price"])),2).ToString()

                    };
                    _total_qty += Convert.ToDouble(dr["quantity_sold"].ToString());
                    _total_cost += Convert.ToDouble(dr["unit_price"].ToString());
                    _total_discount += Convert.ToDouble(dr["discount_value"].ToString());
                    _total_vat += Convert.ToDouble(dr["vat"].ToString());
                    _grand_total += Convert.ToDouble(dr["quantity_sold"]) * Convert.ToDouble(dr["unit_price"]);

                    grid_sales_detail.Rows.Add(row00);

                }
                string[] row12 = { "", "", "", "", "Total", _total_qty.ToString(), _total_cost.ToString(), _total_discount.ToString(), _total_vat.ToString(), _grand_total.ToString() };
                grid_sales_detail.Rows.Add(row12);

                //CustomizeSalesDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        public void LoadPurchaseDetail(string invoice_no)
        {
            try
            {
                double _total_qty = 0;
                double _total_cost = 0;
                double _total_vat = 0;
                double _total_discount = 0;
                double _grand_total = 0;

                grid_sales_detail.Rows.Clear();

                //bind data in data grid view  
                PurchasesBLL objpurchasesBLL = new PurchasesBLL();
                grid_sales_detail.AutoGenerateColumns = false;

                //String keyword = "id,name,date_created";
                // String table = "pos_purchases_detail";
                DataTable dt = objpurchasesBLL.GetAllPurchasesItems(invoice_no);

                foreach (DataRow dr in dt.Rows)
                {

                    string[] row00 = {
                        dr["id"].ToString(),
                        dr["invoice_no"].ToString(),
                        dr["item_code"].ToString(),
                        dr["product_name"].ToString(),
                        dr["loc_code"].ToString(),
                        Math.Round(Convert.ToDouble(dr["quantity"]),2).ToString(),
                        Math.Round(Convert.ToDouble(dr["cost_price"]),2).ToString(),
                        Math.Round(Convert.ToDouble(dr["discount_value"]),2).ToString(),
                        Math.Round(Convert.ToDouble(dr["vat"]),2).ToString(),
                        Math.Round((Convert.ToDouble(dr["cost_price"])*Convert.ToDouble(dr["quantity"])),2).ToString()
                    };

                    _total_qty += Convert.ToDouble(dr["quantity"].ToString());
                    _total_cost += Convert.ToDouble(dr["cost_price"].ToString());
                    _total_discount += Convert.ToDouble(dr["discount_value"].ToString());
                    _total_vat += Convert.ToDouble(dr["vat"].ToString());
                    _grand_total += Convert.ToDouble(dr["cost_price"]) * Convert.ToDouble(dr["quantity"]);

                    grid_sales_detail.Rows.Add(row00);

                }
                string[] row12 = { "", "", "", "", "Total", _total_qty.ToString(), _total_cost.ToString(), _total_discount.ToString(), _total_vat.ToString(), _grand_total.ToString() };
                grid_sales_detail.Rows.Add(row12);
                //CustomizeDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }
        private void CustomizeSalesDataGridView()
        {
            // Get the last row in the DataGridView
            DataGridViewRow lastRow = grid_sales_detail.Rows[grid_account_report.Rows.Count - 1];

            // Loop through all cells in the row
            foreach (DataGridViewCell cell in lastRow.Cells)
            {
                DataGridViewCellStyle style = new DataGridViewCellStyle(cell.Style);

                // Set the font to bold
                style.Font = new Font(grid_sales_detail.Font, FontStyle.Bold);

                // Set the background color
                style.BackColor = Color.LightGray;

                // Apply the style to the current cell
                cell.Style = style;
            }

        }
    }
}
