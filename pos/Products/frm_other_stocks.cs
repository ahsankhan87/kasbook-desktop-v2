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

namespace pos
{
    public partial class frm_other_stocks : Form
    {
        public string _item_number;
        public string _product_id;
        public string _product_name;

        public frm_other_stocks(string product_id, string item_number, string product_name)
        {
            _product_id = product_id;
            _item_number = item_number;
            _product_name = product_name;
            InitializeComponent();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frm_other_stocks_Load(object sender, EventArgs e)
        {
            if (_item_number.Length != 0)
            {
                load_product_detail(_product_id, _item_number, _product_name);
            }
        }

        public void load_product_detail(string product_id, string item_number, string product_name)
        {
            ProductBLL objBLL = new ProductBLL();
            
            DataTable dt = objBLL.Get_otherStock(product_id, item_number);
            foreach (DataRow myProductView in dt.Rows)
            {
                lbl_product_name.Text = myProductView["item_code"].ToString() + " "+ product_name;
                string compnay_name = myProductView["branch_name"].ToString();
                string qty = myProductView["qty"].ToString();

                string[] row0 = {compnay_name, qty};

                grid_other_stock.Rows.Add(row0);
            }

            
        }

    }
}
