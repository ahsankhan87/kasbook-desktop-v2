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
    public partial class frm_product_sales_detail : Form
    {
        private frm_sales main_sale_frm;

        public string _product_id;

        public frm_product_sales_detail(frm_sales main_sale_frm, string product_id)
        {
            this.main_sale_frm = main_sale_frm;
            _product_id = product_id;
           
            InitializeComponent();
        }

        public frm_product_sales_detail()
        {
            InitializeComponent();
        }

        private void frm_product_sales_detail_Load(object sender, EventArgs e)
        {
            
            get_product_detail(_product_id);
           
        }

        private void get_product_detail(string product_code)
        {
            try
            {
                ProductBLL productsBLL_obj = new ProductBLL();
                DataTable product_dt = new DataTable();

                if (product_code != string.Empty)
                {
                    product_dt = productsBLL_obj.SearchRecordByProductCode(product_code);
                }
                if (product_dt.Rows.Count > 0)
                {
                    foreach (DataRow myProductView in product_dt.Rows)
                    {

                        int id = Convert.ToInt32(myProductView["id"]);
                        string code = myProductView["code"].ToString();
                        string name = myProductView["name"].ToString();
                        string qty = (myProductView["purchase_demand_qty"].ToString() == string.Empty || (decimal)myProductView["purchase_demand_qty"] == 0 ? "1" : myProductView["purchase_demand_qty"].ToString());
                        double cost_price = Convert.ToDouble(myProductView["cost_price"]);
                        double unit_price = Convert.ToDouble(myProductView["unit_price"]);
                        
                        txt_shop_qty.Text = qty.ToString();
                        txt_order_qty.Text = "";
                        txt_company_qty.Text = qty.ToString();
                        
                    }


                }
                else
                {
                    MessageBox.Show("Record not found", "Products", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } 
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }
        
        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
