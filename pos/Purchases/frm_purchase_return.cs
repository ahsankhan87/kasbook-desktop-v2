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

namespace pos
{
    public partial class frm_purchase_return : Form
    {
        PurchasesBLL objPurchaseBLL = new PurchasesBLL();
        DataTable Purchases_dt;
        //string _invoice_no = "";

        public int cash_account_id = 0;
        //public int sales_account_id = 0;
        public int payable_account_id = 0;
        public int tax_account_id = 0;
        public int purchases_discount_acc_id = 0;
        public int inventory_acc_id = 0;
        public int purchases_acc_id = 0;

        public frm_purchase_return()
        {
            InitializeComponent();
            Get_AccountID_From_Company();
            
            //_invoice_no = invoice_no;
        }


        public void frm_purchase_return_Load(object sender, EventArgs e)
        {
            //load_purchase_return_grid(Purchase_id);
            LoadReturnReasonsDDL();
            autoCompleteInvoice();
            
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
             {
                grid_purchase_return.DataSource = null;
                 //grid_purchase_return.Rows.Clear();
                grid_purchase_return.Refresh();
                grid_purchase_return.AutoGenerateColumns = false;
                grid_purchase_return.DataSource = objPurchaseBLL.GetReturnPurchaseItems(txt_invoice_no.Text);
                Purchases_dt = load_purchase_return_grid(txt_invoice_no.Text);

                MarkFullyReturnedRows();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        private void MarkFullyReturnedRows()
        {
            foreach (DataGridViewRow row in grid_purchase_return.Rows)
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
            foreach (DataGridViewRow row in grid_purchase_return.Rows)
            {
                decimal avail = Convert.ToDecimal(row.Cells["ReturnableQty"].Value);
                if (avail <= 0)
                {
                    row.DefaultCellStyle.BackColor = Color.LightGray;
                    row.DefaultCellStyle.ForeColor = Color.DarkGray;
                }
            }
        }

        private void btn_return_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to return", "Purchase Return Transaction", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                if (grid_purchase_return.Rows.Count > 0)
                {
                    string returnReason = ((KeyValuePair<string, string>)cmbReturnReason.SelectedItem).Value;
                    string prev_invoice_no = txt_invoice_no.Text.Trim();

                    string new_invoice_no = GetMAXInvoiceNo();

                    decimal total_tax = 0;
                    decimal total_amount = 0;
                    decimal total_discount = 0;
                    decimal sub_total = 0;

                    //GET VALUES FROM LOADED GRID
                    for (int i = 0; i < grid_purchase_return.RowCount; i++)
                    {
                        if (Convert.ToBoolean(grid_purchase_return.Rows[i].Cells["chk"].Value))
                        {
                            decimal tax_rate = (grid_purchase_return.Rows[i].Cells["tax_rate"].Value.ToString() == "" ? 0 : decimal.Parse(grid_purchase_return.Rows[i].Cells["tax_rate"].Value.ToString()));
                            sub_total = (Convert.ToDecimal(grid_purchase_return.Rows[i].Cells["ReturnQty"].Value) * Convert.ToDecimal(grid_purchase_return.Rows[i].Cells["cost_price"].Value) - Convert.ToDecimal(grid_purchase_return.Rows[i].Cells["discount_value"].Value));

                            // decimal tax = (Convert.ToDecimal(grid_purchase_return.Rows[i].Cells["cost_price"].Value) * tax_rate / 100);
                            decimal tax = (sub_total * tax_rate / 100);

                            total_tax += tax;
                            //total_tax += tax * Convert.ToDecimal(grid_purchase_return.Rows[i].Cells["ReturnQty"].Value);
                            total_amount += sub_total + Convert.ToDecimal(grid_purchase_return.Rows[i].Cells["discount_value"].Value);
                            total_discount += Convert.ToDecimal(grid_purchase_return.Rows[i].Cells["discount_value"].Value);

                        } 
                    }

                    List<PurchaseModalHeader> purchase_model_header = new List<PurchaseModalHeader> { };
                    List<PurchasesModal> purchase_model_detail = new List<PurchasesModal> { };

                    //GET ALREADY SAVED PurchaseS 
                    foreach (DataRow Purchases_dr in Purchases_dt.Rows)
                    {
                        /////Added sales header into the List
                        purchase_model_header.Add(new PurchaseModalHeader
                        {
                            supplier_id = (Purchases_dr["supplier_id"].ToString() == string.Empty ? 0 : int.Parse(Purchases_dr["supplier_id"].ToString())),
                            employee_id = (Purchases_dr["employee_id"].ToString() == string.Empty ? 0 : int.Parse(Purchases_dr["employee_id"].ToString())),
                            invoice_no = new_invoice_no,
                            //supplier_invoice_no = txt_supplier_invoice.Text,
                            total_amount = total_amount,
                            total_tax = Math.Round(total_tax, 6),
                            total_discount = total_discount,
                            //total_discount_percent = (string.IsNullOrEmpty(txt_total_disc_percent.Text) ? 0 : Convert.ToDouble(txt_total_disc_percent.Text)),
                            purchase_type = Purchases_dr["purchase_type"].ToString(),
                            purchase_date = DateTime.Now,
                            purchase_time = DateTime.Now,
                            description = "Purchase Return Inv #:" + prev_invoice_no,
                            //shipping_cost = (string.IsNullOrEmpty(txt_shipping_cost.Text) ? 0 : Convert.ToDecimal(txt_shipping_cost.Text)),
                            account = "Return",

                            old_invoice_no = prev_invoice_no,
                            returnReason = returnReason,


                            cash_account_id = cash_account_id,
                            payable_account_id = payable_account_id,
                            tax_account_id = tax_account_id,
                            purchases_discount_acc_id = purchases_discount_acc_id,
                            inventory_acc_id = inventory_acc_id,
                            purchases_acc_id = purchases_acc_id,

                        });
                        //////

                    }
                    
                    for (int i = 0; i < grid_purchase_return.Rows.Count; i++)
                    {
                        if (grid_purchase_return.Rows[i].Cells["id"].Value != null)
                        {
                            if (Convert.ToBoolean(grid_purchase_return.Rows[i].Cells["chk"].Value))
                            {
                                ///// Added sales detail in to List
                                purchase_model_detail.Add(new PurchasesModal
                                {
                                    invoice_no = new_invoice_no,
                                    item_number = grid_purchase_return.Rows[i].Cells["item_number"].Value.ToString(),
                                    code = grid_purchase_return.Rows[i].Cells["item_code"].Value.ToString(),
                                    //name = grid_purchases.Rows[i].Cells["name"].Value.ToString(),
                                    quantity = decimal.Parse(grid_purchase_return.Rows[i].Cells["ReturnQty"].Value.ToString()),
                                    packet_qty = decimal.Parse(grid_purchase_return.Rows[i].Cells["packet_qty"].Value.ToString()),
                                    cost_price = Convert.ToDecimal(grid_purchase_return.Rows[i].Cells["cost_price"].Value.ToString()),// its avg cost actually ,
                                    unit_price = decimal.Parse(grid_purchase_return.Rows[i].Cells["unit_price"].Value.ToString()),
                                    discount = decimal.Parse(grid_purchase_return.Rows[i].Cells["discount_value"].Value.ToString()),
                                    tax_id = Convert.ToInt16(grid_purchase_return.Rows[i].Cells["tax_id"].Value.ToString()),
                                    tax_rate = Convert.ToDecimal(grid_purchase_return.Rows[i].Cells["tax_rate"].Value.ToString()),
                                    purchase_date = DateTime.Now,
                                    location_code = grid_purchase_return.Rows[i].Cells["loc_code"].Value.ToString()
                                });
                                //////////////
                            }
                        }

                    }

                    int Purchase_id = objPurchaseBLL.InsertReturnPurchase(purchase_model_header, purchase_model_detail);

                    if (Purchase_id > 0)
                    {
                        MessageBox.Show("Return transaction Saved", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        grid_purchase_return.DataSource = null;
                        grid_purchase_return.Rows.Clear();
                        grid_purchase_return.Refresh();
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

        private int Insert_Journal_entry(string invoice_no, int account_id, decimal debit, decimal credit, DateTime date,
            string description, int customer_id, int supplier_id, int entry_id)
        {
            int journal_id = 0;
            JournalsModal JournalsModal_obj = new JournalsModal();
            JournalsBLL JournalsObj = new JournalsBLL();

            JournalsModal_obj.invoice_no = invoice_no;
            JournalsModal_obj.entry_date = date;
            JournalsModal_obj.debit = Convert.ToDouble(debit);
            JournalsModal_obj.credit = Convert.ToDouble(credit);
            JournalsModal_obj.account_id = account_id;
            JournalsModal_obj.description = description;
            JournalsModal_obj.customer_id = customer_id;
            JournalsModal_obj.supplier_id = supplier_id;
            JournalsModal_obj.entry_id = entry_id;

            journal_id = JournalsObj.Insert(JournalsModal_obj);
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
                payable_account_id = (int)dr["payable_acc_id"];
                tax_account_id = (int)dr["tax_acc_id"];
                purchases_discount_acc_id = (int)dr["purchases_discount_acc_id"];
                inventory_acc_id = (int)dr["inventory_acc_id"];
                purchases_acc_id = (int)dr["purchases_acc_id"];
            }
        }

        public DataTable load_Purchases_items_return_grid(string invoice_no)
        {
              
            //bind data in data grid view  
            DataTable dt = objPurchaseBLL.GetReturnPurchaseItems(invoice_no);
            return dt;
            
        }

        public DataTable load_purchase_return_grid(string invoice_no)
        {

            //bind data in data grid view  
            DataTable dt = objPurchaseBLL.GetReturnPurchase(invoice_no);
            return dt;

        }

        public string GetMAXInvoiceNo()
        {
            PurchasesBLL PurchasesBLL_obj = new PurchasesBLL();
            return PurchasesBLL_obj.GetMaxReturnInvoiceNo(); //GetMaxInvoiceNo();
        }

        public void autoCompleteInvoice()
        {
            try
            {
                txt_invoice_no.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                txt_invoice_no.AutoCompleteSource = AutoCompleteSource.CustomSource;
                AutoCompleteStringCollection coll = new AutoCompleteStringCollection();

                GeneralBLL invoicesBLL_obj = new GeneralBLL();
                string keyword = "TOP 500 invoice_no ";
                string table = "pos_purchases WHERE account <> 'return'  AND branch_id="+ UsersModal.logged_in_branch_id + " ORDER BY id desc";
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
        
        private void frm_purchase_return_KeyDown(object sender, KeyEventArgs e)
        {
            //when you enter in textbox it will goto next textbox, work like TAB key
            if (e.KeyData == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
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

        private void grid_purchase_return_DataError(object sender, DataGridViewDataErrorEventArgs anError)
        {
            MessageBox.Show("Error happened " + anError.Context.ToString(),"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (anError.Context == DataGridViewDataErrorContexts.Commit)
            {
                MessageBox.Show("Commit error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (anError.Context == DataGridViewDataErrorContexts.CurrentCellChange)
            {
                MessageBox.Show("Cell change", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (anError.Context == DataGridViewDataErrorContexts.Parsing)
            {
                MessageBox.Show("parsing error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (anError.Context == DataGridViewDataErrorContexts.LeaveControl)
            {
                MessageBox.Show("leave control error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if ((anError.Exception) is ConstraintException)
            {
                DataGridView view = (DataGridView)sender;
                view.Rows[anError.RowIndex].ErrorText = "an error";
                view.Rows[anError.RowIndex].Cells[anError.ColumnIndex].ErrorText = "an error";

                anError.ThrowException = false;
            }
        }

        private void grid_purchase_return_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (grid_purchase_return.Columns[e.ColumnIndex].Name == "ReturnQty")
            {
                var row = grid_purchase_return.Rows[e.RowIndex];
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
                    MessageBox.Show("Invalid quantity.");
                    return;
                }
                if (entered > avail)
                {
                    e.Cancel = true;
                    MessageBox.Show($"Cannot return more than available ({avail}).","Return",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                }
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

    }
}
