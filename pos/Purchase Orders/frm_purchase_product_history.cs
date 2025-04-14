using POS.BLL;
using POS.DLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos
{
    public partial class frm_purchase_product_history : Form
    {

        int _product_id;
        public bool _returnStatus = false;
        
        public frm_purchase_product_history(int product_id)
        {
            _product_id = product_id;
            InitializeComponent();
        }
        
        private void frm_purchase_product_history_Load(object sender, EventArgs e)
        {
            load_Products_grid();
           
        }

        public void load_Products_grid()
        {
            try
            {
                grid_search_products.DataSource = null;

                //bind data in data grid view  
                GeneralBLL objBLL = new GeneralBLL();
                grid_search_products.AutoGenerateColumns = false;

                String keyword = "I.id,P.name AS product_name,I.item_id,I.qty,I.unit_price,I.cost_price,I.invoice_no,I.description,trans_date, S.first_name AS supplier";
                String table = "pos_inventory I LEFT JOIN pos_products P ON P.id=I.item_id LEFT JOIN pos_suppliers S ON S.id=I.supplier_id WHERE I.item_id = " + _product_id + " AND I.description = 'Purchase' ORDER BY I.id DESC";
                grid_search_products.DataSource = objBLL.GetRecord(keyword, table);

                if(grid_search_products.Rows.Count < 0)
                {
                    _returnStatus = true;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }


        private void btn_ok_Click(object sender, EventArgs e)
        {

        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frm_purchase_product_history_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }



    }
}
