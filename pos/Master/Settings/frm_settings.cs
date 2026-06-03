using POS.BLL;
using pos.UI;
using System;
using System.Windows.Forms;

namespace pos
{
    public partial class frm_settings : Form
    {
        private readonly SettingsBLL _settings = new SettingsBLL();

        public frm_settings()
        {
            InitializeComponent();
        }

        private void frm_settings_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            chkApplyShippingToItems.Checked = _settings.GetApplyShippingCostToPurchaseItems(false);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _settings.SetApplyShippingCostToPurchaseItems(chkApplyShippingToItems.Checked);
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
