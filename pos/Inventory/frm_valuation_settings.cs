using POS.BLL.Inventory;
using POS.Core;
using POS.Core.Inventory;
using pos.UI;
using System;
using System.Data;
using System.Windows.Forms;

namespace pos.Inventory
{
    public partial class frm_valuation_settings : Form
    {
        private readonly InventoryValuationBLL _bll = new InventoryValuationBLL();
        private InventoryValuationSettings _settings;

        public frm_valuation_settings()
        {
            InitializeComponent();
        }

        private void frm_valuation_settings_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            LoadAccounts();
            LoadSettings();
        }

        private void LoadAccounts()
        {
            try
            {
                // COGS — expense accounts
                DataTable cogsAccounts = _bll.GetExpenseAccounts();
                cmbCogsAccount.DataSource    = cogsAccounts;
                cmbCogsAccount.DisplayMember = "account_name";
                cmbCogsAccount.ValueMember   = "id";
                cmbCogsAccount.SelectedIndex = -1;

                // Inventory — asset accounts
                DataTable invAccounts = _bll.GetAssetAccounts();
                cmbInventoryAccount.DataSource    = invAccounts;
                cmbInventoryAccount.DisplayMember = "account_name";
                cmbInventoryAccount.ValueMember   = "id";
                cmbInventoryAccount.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    "Failed to load accounts: " + ex.Message,
                    "فشل تحميل الحسابات: " + ex.Message);
            }
        }

        private void LoadSettings()
        {
            try
            {
                _settings = _bll.GetSettings(UsersModal.logged_in_branch_id);

                // Method
                switch (_settings.ValuationMethod)
                {
                    case "FIFO":     rbFifo.Checked     = true; break;
                    case "STANDARD": rbStandard.Checked = true; break;
                    default:         rbWac.Checked      = true; break;
                }

                // Components
                rbWithLanded.Checked   = _settings.CostComponents == "WITH_LANDED";
                rbPurchaseOnly.Checked = _settings.CostComponents != "WITH_LANDED";

                // Include filter
                switch (_settings.IncludeFilter)
                {
                    case "ALL":           rbAll.Checked         = true; break;
                    case "EXCLUDE_ZERO":  rbExcludeZero.Checked = true; break;
                    default:              rbActiveOnly.Checked  = true; break;
                }

                // Accounts
                if (_settings.CogsAccountId.HasValue)
                    cmbCogsAccount.SelectedValue = _settings.CogsAccountId.Value;

                if (_settings.InventoryAccountId.HasValue)
                    cmbInventoryAccount.SelectedValue = _settings.InventoryAccountId.Value;

                chkPostPerProduct.Checked = _settings.PostPerProduct;
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    "Failed to load settings: " + ex.Message,
                    "فشل تحميل الإعدادات: " + ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var settings = new InventoryValuationSettings
                {
                    BranchId         = UsersModal.logged_in_branch_id,
                    ValuationMethod  = rbFifo.Checked ? "FIFO" : rbStandard.Checked ? "STANDARD" : "WAC",
                    CostComponents   = rbWithLanded.Checked ? "WITH_LANDED" : "PURCHASE_ONLY",
                    IncludeFilter    = rbAll.Checked ? "ALL" : rbExcludeZero.Checked ? "EXCLUDE_ZERO" : "ACTIVE_ONLY",
                    CogsAccountId    = cmbCogsAccount.SelectedValue != null
                                       ? (int?)Convert.ToInt32(cmbCogsAccount.SelectedValue) : null,
                    InventoryAccountId = cmbInventoryAccount.SelectedValue != null
                                         ? (int?)Convert.ToInt32(cmbInventoryAccount.SelectedValue) : null,
                    PostPerProduct   = chkPostPerProduct.Checked
                };

                _bll.SaveSettings(settings);

                UiMessages.ShowInfo(
                    "Valuation settings saved successfully.",
                    "تم حفظ إعدادات التقييم بنجاح.");

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
