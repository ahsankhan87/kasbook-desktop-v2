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
    public partial class Frm_print_options : Form
    {
        private String _printOptions = string.Empty;

        public Frm_print_options()
        {
            InitializeComponent();
        }

        private void Frm_print_options_Load(object sender, EventArgs e)
        {
            this.ActiveControl = listBox1;
            listBox1.Focus();
            listBox1.SelectedIndex = 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // get the data from the control
            _printOptions = listBox1.SelectedIndex.ToString();

            // DialogResult.OK result
            DialogResult = System.Windows.Forms.DialogResult.OK;

            // close this dialog
            this.Close();
        }

        // public property
        public String PrintOptions
        {
            get
            {
                return _printOptions;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // close this dialog
            this.Close();
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            // get the data from the control
            _printOptions = listBox1.SelectedIndex.ToString();

            // DialogResult.OK result
            DialogResult = System.Windows.Forms.DialogResult.OK;

            // close this dialog
            this.Close();
        }
    }
}
