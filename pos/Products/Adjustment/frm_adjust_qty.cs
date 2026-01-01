using System;
using System.Windows.Forms;

namespace pos.Products.Adjustment
{
    public partial class frm_adjust_qty : Form
    {
        public decimal EnteredQty { get; private set; }

        public frm_adjust_qty(decimal defaultQty = 0m)
        {
            InitializeComponent();
            txtQty.Text = defaultQty.ToString("N2");
            txtQty.Focus();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtQty.Text, out var val))
            {
                MessageBox.Show("Please enter a valid quantity.", "Adjustment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
                return;
            }
            EnteredQty = Math.Round(val, 2);
        }

        private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow digits, control keys, and one decimal point
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
                return;
            }
            if (e.KeyChar == '.' && (txtQty.Text.Contains(".") || txtQty.Text.Length == 0))
            {
                e.Handled = true;
            }
        }
    }
}