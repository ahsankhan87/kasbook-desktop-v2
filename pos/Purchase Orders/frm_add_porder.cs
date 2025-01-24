using POS.BLL;
using POS.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos
{
    public partial class frm_add_porder : Form
    {
        private frm_sales main_sale_frm;

        public string _product_code;
        public string _product_name;
        public int _product_id;
        public string _category_code;
        public double _cost_price;
        public double _unit_price;
        
        DataTable PO_dt = new DataTable();
        Purchases_orderModal Purchases_orderModal_obj = new Purchases_orderModal();
        Purchases_orderBLL purchases_orderObj = new Purchases_orderBLL();

        public frm_add_porder(frm_sales main_sale_frm, int product_id, string product_code, string product_name, string category_code, double cost_price, double unit_price)
        {
            this.main_sale_frm = main_sale_frm;
            _product_id = product_id;
            _product_code = product_code;
            _product_name = product_name;
            _category_code = category_code;
            _cost_price= cost_price;
            _unit_price = unit_price;
            
            InitializeComponent();
        }

        public frm_add_porder()
        {
            InitializeComponent();
        }

        private void frm_add_porder_Load(object sender, EventArgs e)
        {
            this.ActiveControl = txt_order_qty;
            txt_order_qty.Focus();
            txt_product_id.Text = _product_id.ToString();
            txt_product_name.Text = _product_name.ToString();
            txt_product_code.Text = _product_code.ToString();

            GetMAXInvoiceNo();

        }

        public void GetMAXInvoiceNo()
        {
            Purchases_orderBLL Purchases_orderBLL_obj = new Purchases_orderBLL();
            PO_dt = Purchases_orderBLL_obj.GetPOrder_bycategory_code(_category_code);

            if (PO_dt.Rows.Count > 0) //IF PO already exist using category id then get inv no 
            {
                foreach (DataRow dr in PO_dt.Rows)
                {
                    txt_invoice_no.Text = dr["invoice_no"].ToString();
                }
            }
            else { // Create new inv no for PO

                txt_invoice_no.Text = Purchases_orderBLL_obj.GetMaxInvoiceNo();
            }
            
            
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 purchase_id = 0;
                DialogResult result = MessageBox.Show("Are you sure you want to order", "Purchase Order Transaction", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    if (PO_dt.Rows.Count > 0) //IF PO already exist using category id then get inv no 
                    {
                        foreach (DataRow dr in PO_dt.Rows)
                        {
                            purchase_id = int.Parse(dr["id"].ToString());
                            Purchases_orderModal_obj.invoice_no = txt_invoice_no.Text;
                    
                        }
                    }
                    else { // Create new inv no for PO
                        purchase_id = this.Insert_new_purchase_order();
                    }

                    Purchases_orderModal_obj.purchase_id = purchase_id;
                    Purchases_orderModal_obj.code = _product_code;
                    //Purchases_orderModal_obj.name = grid_purchases_order.Rows[i].Cells["name"].Value.ToString();
                    Purchases_orderModal_obj.quantity = double.Parse(txt_order_qty.Value.ToString());
                    Purchases_orderModal_obj.cost_price = _cost_price;
                    Purchases_orderModal_obj.unit_price = _unit_price;
                    Purchases_orderModal_obj.discount = 0;
                    Purchases_orderModal_obj.tax_id = 0;
                    double tax_rate = 0;
                    Purchases_orderModal_obj.tax_rate = tax_rate;

                    purchases_orderObj.InsertPurchases_orderItems(Purchases_orderModal_obj);
                }
                if (purchase_id > 0)
                {
                    MessageBox.Show("Purchase Order Saved", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    MessageBox.Show("Record not saved", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.Close();
        }
        
        private int Insert_new_purchase_order()
        {
            DateTime purchase_date = txt_po_date.Value.Date;
            int supplier_id = 0;
            int employee_id = 0;

            Purchases_orderModal_obj.employee_id = employee_id;
            Purchases_orderModal_obj.supplier_id = supplier_id;
            Purchases_orderModal_obj.invoice_no = txt_invoice_no.Text;
            //Purchases_orderModal_obj.total_amount = total_amount;
            //Purchases_orderModal_obj.purchase_type = cmb_purchase_type.SelectedValue.ToString();
            //Purchases_orderModal_obj.total_discount = total_discount;
            //Purchases_orderModal_obj.total_tax = total_tax;
            //Purchases_orderModal_obj.description = txt_description.Text;
            Purchases_orderModal_obj.category_code = _category_code;
            Purchases_orderModal_obj.purchase_date = purchase_date.ToString("yyyy-MM-dd");
            Purchases_orderModal_obj.delivery_date = purchase_date.ToString("yyyy-MM-dd");
            Purchases_orderModal_obj.account = "Purchase Order";

            //set the date from datetimepicker and set time to te current time
            DateTime now = DateTime.Now;
            txt_po_date.Value = new DateTime(txt_po_date.Value.Year, txt_po_date.Value.Month, txt_po_date.Value.Day, now.Hour, now.Minute, now.Second);
            /////////////////////
            Purchases_orderModal_obj.purchase_time = txt_po_date.Value;

            Int32 purchase_id = purchases_orderObj.InsertPurchases_order(Purchases_orderModal_obj);
            
            return purchase_id;
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
