using POS.BLL;
using System;
using System.Windows.Forms;

namespace pos
{
    public partial class frm_small_sale_settings : Form
    {
        private readonly SettingsBLL _settings = new SettingsBLL();

        public frm_small_sale_settings()
        {
            InitializeComponent();
        }

        private void frm_small_sale_settings_Load(object sender, EventArgs e)
        {
            nudThreshold.Value = Convert.ToDecimal(_settings.GetSmallSaleThreshold());
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _settings.SetSmallSaleThreshold(Convert.ToDouble(nudThreshold.Value));
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
