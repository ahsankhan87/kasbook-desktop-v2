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
using POS.Core;
using pos.Master.Companies.zatca;

namespace pos
{
    public partial class frm_sales_return : Form
    {
        SalesBLL objSalesBLL = new SalesBLL();
        DataTable sales_dt;
        string _invoice_no = "";
        
        public int cash_account_id = 0;
        public int sales_account_id = 0;
        public int receivable_account_id = 0;
        public int tax_account_id = 0;
        public int sales_discount_acc_id = 0;
        public int inventory_acc_id = 0;
        public int purchases_acc_id = 0;

        public double employee_commission_percent = 0;
        public double user_commission_percent = 0;
        
        public frm_sales_return()
        {
            InitializeComponent();
            //_invoice_no = invoice_no;
        }
        public frm_sales_return(string invoiceNo)
        {
            InitializeComponent();
            _invoice_no = invoiceNo;
            txt_invoice_no.Text = _invoice_no;
        }

        public void frm_sales_return_Load(object sender, EventArgs e)
        {
            //load_sales_return_grid(sale_id);
            Get_AccountID_From_Company();
            LoadReturnReasonsDDL();
            autoCompleteInvoice();
            txt_invoice_no.Focus();
            Get_user_total_commission();
            LoadSalesReturnGrid();

        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            LoadSalesReturnGrid();
        }
        public void LoadSalesReturnGrid()
        {
            try
            {
                if(!string.IsNullOrEmpty(txt_invoice_no.Text))
                {
                    grid_sales_return.DataSource = null;
                    grid_sales_return.AutoGenerateColumns = false;
                    grid_sales_return.DataSource = objSalesBLL.GetReturnSaleItems(txt_invoice_no.Text);
                    sales_dt = load_sales_return_grid(txt_invoice_no.Text);
                    MarkFullyReturnedRows();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        private void MarkFullyReturnedRows()
        {
            foreach (DataGridViewRow row in grid_sales_return.Rows)
            {
                decimal avail = Convert.ToDecimal(row.Cells["ReturnableQty"].Value);
                if (avail <= 0)
                {
                    row.ReadOnly = true; // whole row
                    row.Cells["ReturnQty"].Value = 0m;
                }
            }
            ApplyRowStyles();
        }

        private void ApplyRowStyles()
        {
            foreach (DataGridViewRow row in grid_sales_return.Rows)
            {
                decimal avail = Convert.ToDecimal(row.Cells["ReturnableQty"].Value);
                if (avail <= 0)
                {
                    row.DefaultCellStyle.BackColor = Color.LightGray;
                    row.DefaultCellStyle.ForeColor = Color.DarkGray;
                }
            }
        }

        public bool item_checked = false;
        private void btn_return_Click(object sender, EventArgs e)
        {
            ////Checking item is selected or not
            for (int i = 0; i < grid_sales_return.Rows.Count; i++)
            {
                if (grid_sales_return.Rows[i].Cells["id"].Value != null)
                {
                    if (Convert.ToBoolean(grid_sales_return.Rows[i].Cells["chk"].Value))
                    {
                        item_checked = true;
                    }
                }
            }
            if (!item_checked)
            {
                MessageBox.Show("Please select product", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            /////////

            DialogResult result = MessageBox.Show("Are you sure you want to return", "Sale Return Transaction", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                if (grid_sales_return.Rows.Count > 0)
                {
                    string returnReasonCode = ((KeyValuePair<string, string>)cmbReturnReason.SelectedItem).Key;
                    string returnReason = ((KeyValuePair<string, string>)cmbReturnReason.SelectedItem).Value;

                    string prev_invoice_no = txt_invoice_no.Text.Trim();
                    DateTime prev_invoice_date = Convert.ToDateTime(sales_dt.Rows[0]["sale_date"].ToString()).Date;
                    string new_invoice_no = GetMAXInvoiceNo();
                    string invoice_subtype_code = sales_dt.Rows[0]["invoice_subtype_code"].ToString();
                    
                    if (string.IsNullOrEmpty(new_invoice_no))
                    {
                        MessageBox.Show("No invoice number found, please check your settings", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    double total_tax = 0;
                    double total_amount = 0;
                    double total_cost_amount = 0;
                    double total_discount = 0;
                    double sub_total = 0;

                    int employee_id = 0;
                    string description = "Sale Return Prev Inv #:"+ prev_invoice_no+" Prev Date: "+prev_invoice_date.Date;
                    
                    DateTime sale_date = DateTime.Now;

                    //GET VALUES FROM LOADED GRID
                    for (int i = 0; i < grid_sales_return.RowCount; i++)
                    {
                        if (Convert.ToBoolean(grid_sales_return.Rows[i].Cells["chk"].Value))
                        {
                            double tax_rate = (grid_sales_return.Rows[i].Cells["tax_rate"].Value.ToString() == "" ? 0 : double.Parse(grid_sales_return.Rows[i].Cells["tax_rate"].Value.ToString()));
                            sub_total = (Convert.ToDouble(grid_sales_return.Rows[i].Cells["ReturnQty"].Value) * Convert.ToDouble(grid_sales_return.Rows[i].Cells["unit_price"].Value)- Convert.ToDouble(grid_sales_return.Rows[i].Cells["discount_value"].Value));
                            double tax = (sub_total * tax_rate / 100);

                            total_tax += tax;

                            //total_tax += Convert.ToInt32(grid_sales_return.Rows[i].Cells["vat"].Value) / Convert.ToInt32(grid_sales_return.Rows[i].Cells["quantity_sold"].Value) * Convert.ToInt32(grid_sales_return.Rows[i].Cells["ReturnQty"].Value);
                            total_amount += sub_total+ Convert.ToDouble(grid_sales_return.Rows[i].Cells["discount_value"].Value);
                            total_discount += Convert.ToDouble(grid_sales_return.Rows[i].Cells["discount_value"].Value);
                            total_cost_amount += Convert.ToDouble(grid_sales_return.Rows[i].Cells["cost_price"].Value.ToString()) * Convert.ToDouble(grid_sales_return.Rows[i].Cells["ReturnQty"].Value.ToString());

                        } 
                    }

                    List<SalesModalHeader> sales_model_header = new List<SalesModalHeader> { };
                    List<SalesModal> sales_model_detail = new List<SalesModal> { };

                    //GET ALREADY SAVED SALES 
                    foreach (DataRow sales_dr in sales_dt.Rows)
                    {
                        
                        employee_id = (sales_dr["employee_id"].ToString() == string.Empty ? 0 : int.Parse(sales_dr["employee_id"].ToString()));
                        int customer_id = (sales_dr["customer_id"].ToString() == string.Empty ? 0 : int.Parse(sales_dr["customer_id"].ToString()));
                        string sale_type = sales_dr["sale_type"].ToString();

                        /////Added sales header into the List
                        sales_model_header.Add(new SalesModalHeader
                        {
                            customer_id = customer_id,
                            employee_id = employee_id,
                            invoice_no = new_invoice_no,
                            invoice_subtype = invoice_subtype_code,
                            total_amount = total_amount,
                            total_tax = total_tax,
                            total_discount = total_discount,
                            //total_discount_percent = (string.IsNullOrEmpty(txt_total_disc_percent.Text) ? 0 : Convert.ToDouble(txt_total_disc_percent.Text)),
                            sale_type = sale_type,
                            sale_date = sale_date,
                            sale_time = sale_date,
                            description = description,
                            //payment_terms_id = payment_terms_id,
                            //payment_method_id = payment_method_id,
                            account = "Return",
                            //is_return = false,
                            old_invoice_no = prev_invoice_no,
                            previousInvoiceDate = prev_invoice_date,
                            returnReasonCode = returnReasonCode,
                            returnReason = returnReason,

                            total_cost_amount = total_cost_amount,
                            cash_account_id = cash_account_id,
                            receivable_account_id = receivable_account_id,
                            tax_account_id = tax_account_id,
                            sales_discount_acc_id = sales_discount_acc_id,
                            inventory_acc_id = inventory_acc_id,
                            purchases_acc_id = purchases_acc_id,
                            sales_account_id = sales_account_id,

                        });
                        //////
                    }
                    
                    for (int i = 0; i < grid_sales_return.Rows.Count; i++)
                    {
                        if (grid_sales_return.Rows[i].Cells["id"].Value != null)
                        {
                            if (Convert.ToBoolean(grid_sales_return.Rows[i].Cells["chk"].Value))
                            {
                                ///// Added sales detail in to List
                                sales_model_detail.Add(new SalesModal
                                {
                                    invoice_no = new_invoice_no,
                                    item_number= grid_sales_return.Rows[i].Cells["item_number"].Value.ToString(),
                                    code = grid_sales_return.Rows[i].Cells["item_code"].Value.ToString(),
                                    name = grid_sales_return.Rows[i].Cells["product_name"].Value.ToString(),
                                    quantity_sold = double.Parse(grid_sales_return.Rows[i].Cells["ReturnQty"].Value.ToString()),
                                    packet_qty = double.Parse(grid_sales_return.Rows[i].Cells["packet_qty"].Value.ToString()),
                                    unit_price = double.Parse(grid_sales_return.Rows[i].Cells["unit_price"].Value.ToString()),
                                    discount = double.Parse(grid_sales_return.Rows[i].Cells["discount_value"].Value.ToString()),
                                    //discount_percent = double.Parse(grid_sales.Rows[i].Cells["discount_percent"].Value.ToString()),
                                    cost_price = Convert.ToDouble(grid_sales_return.Rows[i].Cells["cost_price"].Value.ToString()),// its avg cost actually ,
                                    //item_type = grid_sales.Rows[i].Cells["item_type"].Value.ToString(),
                                    location_code = grid_sales_return.Rows[i].Cells["loc_code"].Value.ToString(),
                                    tax_id = Convert.ToInt16(grid_sales_return.Rows[i].Cells["tax_id"].Value.ToString()),
                                    tax_rate = Convert.ToDouble(grid_sales_return.Rows[i].Cells["tax_rate"].Value.ToString()),
                                    sale_date = sale_date,
                                });
                                //////////////
                            }
                        }

                    }

                    int sale_id = objSalesBLL.InsertReturnSales(sales_model_header, sales_model_detail);

                    //Employee commission entry
                    if (employee_id != 0)
                    {
                        employee_commission_percent = Get_emp_total_commission(employee_id);
                        double emp_commission_amount = employee_commission_percent * total_amount / 100;
                        Insert_emp_commission(new_invoice_no, 0, emp_commission_amount, 0, sale_date, description, employee_id);
                    }
                    /////

                    //User commission entry
                    if (user_commission_percent > 0)
                    {
                        double user_commission_amount = user_commission_percent * total_amount / 100;
                        Insert_user_commission(new_invoice_no, 0, user_commission_amount, 0, sale_date, description);
                    }
                    /////

                    if (sale_id > 0)
                    {
                        //sign the credit note invoice to ZATCA
                        if (UsersModal.useZatcaEInvoice == true)
                        {
                            DataRow activeZatcaCredential = ZatcaInvoiceGenerator.GetActiveZatcaCSID();
                            if (activeZatcaCredential == null)
                            {
                                MessageBox.Show("No active ZATCA CSID/credentials found. Please configure them first.");
                            }

                            // Retrieve PCSID credentials from the database using the credentialId
                            DataRow PCSID_dataRow = ZatcaInvoiceGenerator.GetZatcaCredentialByParentID(Convert.ToInt32(activeZatcaCredential["id"]));
                            if (PCSID_dataRow == null)
                            {
                                //MessageBox.Show("No Production CSID credentials found for the selected ZATCA CSID.");

                                //Sign Invoice with CSID instead of Production CSID
                                ZatcaHelper.SignCreditNoteToZatcaAsync(new_invoice_no, prev_invoice_no, prev_invoice_date);
                            }
                            else
                            {
                                //If PCSID exist then sign it 
                                ZatcaHelper.PCSID_SignCreditNoteToZatcaAsync(new_invoice_no, prev_invoice_no, prev_invoice_date);
                            }

                        }
                        //////
                        ///
                        
                        MessageBox.Show("Return transaction Saved", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        grid_sales_return.DataSource = null;
                        grid_sales_return.Rows.Clear();
                        grid_sales_return.Refresh();
                        txt_invoice_no.Focus();
                    }
                    else
                    {
                        MessageBox.Show("Record not saved", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Please search for invoice", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private int Insert_Journal_entry(string invoice_no, int account_id, double debit, double credit, DateTime date,
            string description, int customer_id, int supplier_id, int entry_id)
        {
            int journal_id = 0;
            JournalsModal JournalsModal_obj = new JournalsModal();
            JournalsBLL JournalsObj = new JournalsBLL();

            JournalsModal_obj.invoice_no = invoice_no;
            JournalsModal_obj.entry_date = date;
            JournalsModal_obj.debit = debit;
            JournalsModal_obj.credit = credit;
            JournalsModal_obj.account_id = account_id;
            JournalsModal_obj.description = description;
            JournalsModal_obj.customer_id = customer_id;
            JournalsModal_obj.supplier_id = supplier_id;
            JournalsModal_obj.entry_id = entry_id;

            journal_id = JournalsObj.Insert(JournalsModal_obj);
            return journal_id;
        }

        private int Insert_emp_commission(string invoice_no, int account_id, double debit, double credit, DateTime date,
            string description, int employee_id = 0)
        {
            int journal_id = 0;
            JournalsModal JournalsModal_obj = new JournalsModal();
            EmployeeBLL emp_Obj = new EmployeeBLL();

            JournalsModal_obj.invoice_no = invoice_no;
            JournalsModal_obj.entry_date = date;
            JournalsModal_obj.debit = debit;
            JournalsModal_obj.credit = credit;
            JournalsModal_obj.account_id = account_id;
            JournalsModal_obj.description = description;
            JournalsModal_obj.employee_id = employee_id;

            journal_id = emp_Obj.InsertEmpCommission(JournalsModal_obj);
            return journal_id;
        }

        private int Insert_user_commission(string invoice_no, int account_id, double debit, double credit, DateTime date,
            string description)
        {
            int journal_id = 0;
            JournalsModal JournalsModal_obj = new JournalsModal();
            UsersBLL emp_Obj = new UsersBLL();

            JournalsModal_obj.invoice_no = invoice_no;
            JournalsModal_obj.entry_date = date;
            JournalsModal_obj.debit = debit;
            JournalsModal_obj.credit = credit;
            JournalsModal_obj.account_id = account_id;
            JournalsModal_obj.description = description;

            journal_id = emp_Obj.InsertUserCommission(JournalsModal_obj);
            return journal_id;
        }
        
        private void Get_AccountID_From_Company()
        {
            GeneralBLL objBLL = new GeneralBLL();

            String keyword = "TOP 1 *";
            String table = "pos_companies";
            DataTable companies_dt = objBLL.GetRecord(keyword, table);
            foreach (DataRow dr in companies_dt.Rows)
            {
                cash_account_id = (int)dr["cash_acc_id"];
                sales_account_id = (int)dr["sales_acc_id"];
                receivable_account_id = (int)dr["receivable_acc_id"];
                tax_account_id = (int)dr["tax_acc_id"];
                sales_discount_acc_id = (int)dr["sales_discount_acc_id"];
                inventory_acc_id = (int)dr["inventory_acc_id"];
                purchases_acc_id = (int)dr["purchases_acc_id"];
            }
        }

        public DataTable load_sales_items_return_grid(string invoice_no)
        {
              
            //bind data in data grid view  
            DataTable dt = objSalesBLL.GetReturnSaleItems(invoice_no);
            return dt;
            
        }

        public DataTable load_sales_return_grid(string invoice_no)
        {

            //bind data in data grid view  
            DataTable dt = objSalesBLL.GetReturnSales(invoice_no);
            return dt;

        }

        public string GetMAXInvoiceNo()
        {
            SalesBLL salesBLL_obj = new SalesBLL();
            return salesBLL_obj.GetMaxSalesReturnInvoiceNo();
        }

        public void autoCompleteInvoice()
        {
            try
            {
                txt_invoice_no.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                txt_invoice_no.AutoCompleteSource = AutoCompleteSource.CustomSource;
                AutoCompleteStringCollection coll = new AutoCompleteStringCollection();

                GeneralBLL invoicesBLL_obj = new GeneralBLL();
                string keyword = "TOP 1000 invoice_no ";
                string table = "pos_sales WHERE account = 'Sale' AND branch_id=" + UsersModal.logged_in_branch_id + " ORDER BY id desc";
                DataTable dt = invoicesBLL_obj.GetRecord(keyword, table);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        coll.Add(dr["invoice_no"].ToString());

                    }

                }

                txt_invoice_no.AutoCompleteCustomSource = coll;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        
        private void frm_sales_return_KeyDown(object sender, KeyEventArgs e)
        {
            //when you enter in textbox it will goto next textbox, work like TAB key
            if (e.KeyData == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
            if (e.KeyCode == Keys.Escape)
            {
                txt_close.PerformClick();
            }

            if (e.KeyCode == Keys.F3)
            {
                btn_return.PerformClick();
            }
        }

        private void txt_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void Get_user_total_commission()
        {
            UsersBLL obj = new UsersBLL();
            DataTable users = obj.GetUser(UsersModal.logged_in_userid);

            foreach (DataRow dr in users.Rows)
            {
                user_commission_percent = double.Parse(dr["commission_percent"].ToString());
            }

        }

        private double Get_emp_total_commission(int employee_id)
        {
            EmployeeBLL obj = new EmployeeBLL();
            DataTable employees = obj.SearchRecordByID(employee_id);
            double commission_percent = 0;
            
            foreach (DataRow dr in employees.Rows)
            {
                commission_percent = double.Parse(dr["commission_percent"].ToString());
            }
            return commission_percent;
        }

        private void grid_sales_return_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(tb_KeyPress);

            if (grid_sales_return.CurrentCell.ColumnIndex == 7 || grid_sales_return.CurrentCell.ColumnIndex == 8) //qty, unit price and discount Column will accept only numeric
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(tb_KeyPress);
                }
            }
        }
        void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = sender as TextBox;

            // Allow only digits, control keys (Backspace, Delete), and one decimal point
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
                return;
            }

            // Prevent multiple decimal points
            if (e.KeyChar == '.' && tb.Text.Contains("."))
            {
                e.Handled = true;
                return;
            }

            // Prevent entering "." as the first character
            if (e.KeyChar == '.' && tb.Text.Length == 0)
            {
                e.Handled = true;
                return;
            }
        }
        private void LoadReturnReasonsDDL()
        {
            // Example reasons; replace with your actual codes and reasons
            var reasons = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("01", "Goods returned"),
                new KeyValuePair<string, string>("02", "Invoice correction"),
                new KeyValuePair<string, string>("03", "Service not provided"),
                new KeyValuePair<string, string>("04", "Duplicate invoice"),
                new KeyValuePair<string, string>("05", "Incorrect amount"),
                new KeyValuePair<string, string>("06", "Cancellation of order"),
                new KeyValuePair<string, string>("07", "Price adjustment"),
                new KeyValuePair<string, string>("08", "Damaged goods"),
                new KeyValuePair<string, string>("09", "Incorrect tax calculation"),
                new KeyValuePair<string, string>("10", "Other")
            };

            cmbReturnReason.DataSource = new BindingSource(reasons, null);
            cmbReturnReason.DisplayMember = "Value";
            cmbReturnReason.ValueMember = "Key";

            cmbReturnReason.SelectedIndex = 0;  // Set default selection
        }

        private void grid_sales_return_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (grid_sales_return.Columns[e.ColumnIndex].Name == "ReturnQty")
            {
                var row = grid_sales_return.Rows[e.RowIndex];
                if (row.ReadOnly) return;

                decimal avail = Convert.ToDecimal(row.Cells["ReturnableQty"].Value);
                if (string.IsNullOrWhiteSpace(e.FormattedValue?.ToString()))
                {
                    row.Cells["ReturnQty"].Value = 0m;
                    return;
                }
                if (!decimal.TryParse(e.FormattedValue.ToString(), out var entered) || entered < 0)
                {
                    e.Cancel = true;
                    MessageBox.Show("Invalid quantity.", "Return", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (entered > avail)
                {
                    e.Cancel = true;
                    MessageBox.Show($"Cannot return more than available ({avail}).", "Return", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}
