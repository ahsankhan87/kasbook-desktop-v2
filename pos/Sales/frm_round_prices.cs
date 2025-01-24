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
    public partial class frm_round_prices : Form
    {
        private frm_sales main_sale_frm;

        public double _sub_total_amount;
        public double _total_amount;
        
        public frm_round_prices(frm_sales main_sale_frm,double sub_total, double total_amount)
        {
            this.main_sale_frm = main_sale_frm;
            _sub_total_amount = sub_total;
            _total_amount = total_amount;
          

            InitializeComponent();
        }

        public frm_round_prices()
        {
            InitializeComponent();
        }

        private void frm_round_prices_Load(object sender, EventArgs e)
        {
            //this.ActiveControl = txt_new_total_amount;

            txt_subtotal.Text = _sub_total_amount.ToString();
            txt_total_amount.Text = (_total_amount).ToString();
            
            txt_new_total_amount.Text = _total_amount.ToString();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            double new_amount = Convert.ToDouble(txt_new_total_amount.Text);
            double old_total_amount = Convert.ToDouble(txt_total_amount.Text);
            double old_sub_total_amount = Convert.ToDouble(txt_subtotal.Text);
            //double percent_of_total_amount = new_amount * 100 / old_total_amount;//Get Percentage of total amount
            //double diff_amount = new_amount*percent_of_total_amount/100; //get new diff amount 
            main_sale_frm.round_total_amount(new_amount, old_total_amount, old_sub_total_amount);
            this.Close();
        }

        private void txt_new_total_amount_KeyUp(object sender, KeyEventArgs e)
        {
           
        }
    }
}
