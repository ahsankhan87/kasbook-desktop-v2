using com.sun.rowset.@internal;
using POS.BLL;
using POS.Core;
using System;
using System.Data;
using System.Windows.Forms;

namespace pos
{
    public partial class frm_product_adjustment : Form
    {

        public int inventory_acc_id = 0;
        public int item_variance_acc_id = 0;
        
        public frm_product_adjustment()
        {
            InitializeComponent();
            
        }
        
        private void frm_product_adjustment_Load(object sender, EventArgs e)
        {
            GetMAXInvoiceNo();
            Get_AccountID_From_Company();
        }
        
        private void btn_update_Click(object sender, EventArgs e)
        {
            
            try
            {
                string qresult = "";
                DialogResult result = MessageBox.Show("Are you sure you want to update", "Adjustment", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    if (grid_search_products.Rows.Count > 0)
                    {
                        ProductModal info = new ProductModal();
                        ProductBLL productBLLObj = new ProductBLL();
                        string invoice_no = productBLLObj.GetMaxAdjustmentInvoiceNo();

                        for (int i = 0; i < grid_search_products.Rows.Count; i++)
                        {
                            if (grid_search_products.Rows[i].Cells["id"].Value != null)
                            {
                                double adjustment_qty = (grid_search_products.Rows[i].Cells["adjustment_qty"].Value != null ? double.Parse(grid_search_products.Rows[i].Cells["adjustment_qty"].Value.ToString()) : 0);
                                double qty = (grid_search_products.Rows[i].Cells["qty"].Value != null ? double.Parse(grid_search_products.Rows[i].Cells["qty"].Value.ToString()) : 0);
                                double avg_cost = (grid_search_products.Rows[i].Cells["avg_cost"].Value != null ? double.Parse(grid_search_products.Rows[i].Cells["avg_cost"].Value.ToString()) : 0); 
                                double unit_price = (grid_search_products.Rows[i].Cells["unit_price"].Value != null ? double.Parse(grid_search_products.Rows[i].Cells["unit_price"].Value.ToString()) : 0); 
                                
                                info.invoice_no = invoice_no;
                                info.item_number = grid_search_products.Rows[i].Cells["item_number"].Value.ToString();
                                info.code = grid_search_products.Rows[i].Cells["code"].Value.ToString();
                                info.id = int.Parse(grid_search_products.Rows[i].Cells["id"].Value.ToString()); 
                                info.cost_price = avg_cost;
                                info.unit_price = unit_price;
                                info.location_code = grid_search_products.Rows[i].Cells["location_code"].Value.ToString();
                                info.qty = qty;
                                info.adjustment_qty = adjustment_qty;

                                qresult = productBLLObj.UpdateQtyAdjustment(info);

                                //Account Entry
                                double net_qty = (adjustment_qty - qty);
                                double total = avg_cost * net_qty;
                                
                                if (net_qty > 0)
                                {
                                    if(total > 0)
                                    {
                                        //Product Adjustment JOURNAL ENTRY (credit)
                                        Insert_Journal_entry(invoice_no, item_variance_acc_id, 0, total, txt_date.Value.Date, "Product Adjustment", 0, 0, 0);
                                        /////////////

                                        ///Inventory JOURNAL ENTRY (debit)
                                        Insert_Journal_entry(invoice_no, inventory_acc_id, total, 0, txt_date.Value.Date, "Product Adjustment", 0, 0, 0);
                                    }
                                    
                                }
                                else
                                {
                                    if (Math.Abs(total) > 0)
                                    {
                                        //Product Adjustment JOURNAL ENTRY (Debit)
                                        Insert_Journal_entry(invoice_no, item_variance_acc_id, Math.Abs(total), 0, txt_date.Value.Date, "Product Adjustment", 0, 0, 0);

                                        ///Inventory JOURNAL ENTRY (credit)
                                        Insert_Journal_entry(invoice_no, inventory_acc_id, 0, Math.Abs(total), txt_date.Value.Date, "Product Adjustment", 0, 0, 0);

                                    }
                                }
                                
                            }
                            
                        }
                        
                        if (qresult.Length > 0)
                        {
                            MessageBox.Show("Record updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            GetMAXInvoiceNo();
                            grid_search_products.DataSource = null;
                            grid_search_products.Rows.Clear();
                            grid_search_products.Refresh();
                            txt_search.Focus();
                        }
                        else
                        {
                            MessageBox.Show("Record not saved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("no record found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void GetMAXInvoiceNo()
        {
            ProductBLL objBLL = new ProductBLL();
            txt_ref_no.Text =  objBLL.GetMaxAdjustmentInvoiceNo();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_search.Text != "")
                {
                    bool by_code = rb_by_code.Checked;
                    bool by_name = rb_by_name.Checked;

                    frm_searchProducts search_product_obj = new frm_searchProducts(null, null, null, txt_search.Text, "", "", 0, false, false, null,null,this);
                    search_product_obj.ShowDialog();

                }

                //if(txt_search.Text != string.Empty)
                //{
                //    bool by_code = rb_by_code.Checked;
                //    bool by_name = rb_by_name.Checked;
                    
                //    ProductBLL objBLL = new ProductBLL();
                //    grid_search_products.AutoGenerateColumns = false;

                //    String condition = txt_search.Text.Trim();
                //    grid_search_products.DataSource = objBLL.SearchProductByLocation(condition, by_code, by_name);

                //}
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void Load_product_to_grid(string product_code = "")
        {
            try
            {
                ProductBLL productsBLL_obj = new ProductBLL();
                DataTable product_dt = new DataTable();

                if (!string.IsNullOrWhiteSpace(product_code))
                {
                    product_dt = productsBLL_obj.SearchRecordByProductNumber(product_code);
                }

                if (product_dt.Rows.Count > 0)
                {
                    foreach (DataRow myProductView in product_dt.Rows)
                    {
                        int id = Convert.ToInt32(myProductView["id"]);
                        string code = Convert.ToString(myProductView["code"]);
                        string category = Convert.ToString(myProductView["category"]);
                        string name = Convert.ToString(myProductView["name"]);
                        string name_ar = Convert.ToString(myProductView["name_ar"]);
                        string location_code = Convert.ToString(myProductView["location_code"]);
                        decimal qty = Math.Round(Convert.ToDecimal(myProductView["qty"]), 2);
                        decimal avg_cost = Math.Round(Convert.ToDecimal(myProductView["avg_cost"]), 2);
                        decimal unit_price = Math.Round(Convert.ToDecimal(myProductView["unit_price"]), 2);
                        string description = Convert.ToString(myProductView["description"]);
                        string item_type = Convert.ToString(myProductView["item_type"]);
                        string btn_delete = "Del";
                        string item_number = Convert.ToString(myProductView["item_number"]);

                        // Show qty dialog per product; default to current qty
                        decimal enteredQty = qty;
                        using (var qtyDlg = new pos.Products.Adjustment.frm_adjust_qty(qty))
                        {
                            if (qtyDlg.ShowDialog(this) == DialogResult.OK)
                            {
                                enteredQty = qtyDlg.EnteredQty; // this is a decimal
                            }
                            else
                            {
                                // If cancelled, keep default (current qty)
                                enteredQty = qty;
                            }
                        }

                        string[] row0 =
                        {
                            id.ToString(), code, category, name, name_ar, location_code,
                            qty.ToString("N2"),
                            enteredQty.ToString("N2"), // adjustment_qty (from dialog)
                            avg_cost.ToString("N2"), unit_price.ToString("N2"),
                            btn_delete, description, item_type, item_number
                        };

                        grid_search_products.Rows.Add(row0);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void Get_AccountID_From_Company()
        {
            GeneralBLL objBLL = new GeneralBLL();

            String keyword = "TOP 1 *";
            String table = "pos_companies";
            DataTable companies_dt = objBLL.GetRecord(keyword, table);
            foreach (DataRow dr in companies_dt.Rows)
            {
                item_variance_acc_id = (int)dr["item_variance_acc_id"];
                inventory_acc_id = (int)dr["inventory_acc_id"];
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frm_product_adjustment_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //when you enter in textbox it will goto next textbox, work like TAB key
                //if (e.KeyData == Keys.Enter)
                //{
                //    SendKeys.Send("{TAB}");
                //}

                if (e.KeyCode == Keys.F3)
                {
                    btn_update.PerformClick();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txt_search_KeyDown(object sender, KeyEventArgs e)
        {
            if (txt_search.Text != "" && e.KeyData == Keys.Enter)
            {

                btn_search.PerformClick();
            }
        }

        private void grid_search_products_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string name = grid_search_products.Columns[e.ColumnIndex].Name;
                if (name == "btn_delete")
                {
                    grid_search_products.Rows.RemoveAt(e.RowIndex);

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Products", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }

        private void txt_ref_no_KeyPress(object sender, KeyPressEventArgs e)
        {
            // When press enter search product adjustment
            if (txt_ref_no.Text != "" && e.KeyChar == (char)Keys.Enter)
            {
                // validation
                if (txt_ref_no.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Please enter valid reference no.", "Adjustment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // select adjustment from table and fill grid
                ProductBLL _productBll = new ProductBLL();
                DataTable dt = _productBll.GetProductAdjustmentsByInvoiceNo(txt_ref_no.Text);

                if (dt.Rows.Count > 0)
                {
                    grid_search_products.Rows.Clear();
                    foreach (DataRow myProductView in dt.Rows)
                    {
                        int id = Convert.ToInt32(myProductView["id"]);
                        string code = Convert.ToString(myProductView["item_code"]);
                        string category = Convert.ToString(myProductView["category_code"]);
                        string name = Convert.ToString(myProductView["name"]);
                        string name_ar = Convert.ToString(myProductView["name_ar"]);
                        string location_code = Convert.ToString(myProductView["location_code"]);
                        decimal qty = Math.Round(Convert.ToDecimal(myProductView["qty"]), 2);
                        decimal adjustment_qty = Math.Round(Convert.ToDecimal(myProductView["adjustment_qty"]), 2);
                        decimal avg_cost = Math.Round(Convert.ToDecimal(myProductView["cost_price"]), 2);
                        decimal unit_price = Math.Round(Convert.ToDecimal(myProductView["unit_price"]), 2);
                        string description = Convert.ToString(myProductView["description"]);
                        string item_type = Convert.ToString(myProductView["item_type"]);
                        string btn_delete = "Del";
                        string item_number = Convert.ToString(myProductView["item_number"]);
                        string[] row0 =
                        {
                            id.ToString(), code, category, name, name_ar, location_code,
                            qty.ToString("N2"),
                            adjustment_qty.ToString("N2"), // adjustment_qty
                            avg_cost.ToString("N2"), unit_price.ToString("N2"),
                            btn_delete, description, item_type, item_number
                        };
                        grid_search_products.Rows.Add(row0);
                    }
                }
                else
                {
                    MessageBox.Show("No record found.", "Adjustment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void Btn_clear_Click(object sender, EventArgs e)
        {
            // Clear all fields
            txt_ref_no.Clear();
            grid_search_products.DataSource = null;
            grid_search_products.Rows.Clear();
            grid_search_products.Refresh();

        }
    }
}
