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

namespace pos
{
    public partial class frm_purchases_orders_detail : Form
    {

        public frm_purchases_orders_detail()
        {
            InitializeComponent();
        }


        public void frm_purchases_orders_detail_Load(object sender, EventArgs e)
        {
            //load_purchases_orders_detail_grid(sale_id);
        }

        public void load_purchases_orders_detail_grid(int sale_id)
        {
            try
            {
                grid_purchases_orders_detail.DataSource = null;

                //bind data in data grid view  
                Purchases_orderBLL objPurchases_orderBLL = new Purchases_orderBLL();
                grid_purchases_orders_detail.AutoGenerateColumns = false;

                //String keyword = "id,name,date_created";
               // String table = "pos_purchases_orders_detail";
                grid_purchases_orders_detail.DataSource = objPurchases_orderBLL.GetAllPurchases_orderItems(sale_id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }

        private void txt_search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) // Enter
            {
                txt_close.PerformClick();
            }
        }

        private void frm_purchases_orders_detail_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

        private void txt_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
