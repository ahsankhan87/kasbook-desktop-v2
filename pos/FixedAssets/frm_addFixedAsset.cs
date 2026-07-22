using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using POS.BLL;
using POS.BLL.FixedAssets;
using POS.Core;
using pos.UI;
using pos.UI.Busy;

namespace pos.FixedAssets
{
    public partial class frm_addFixedAsset : Form
    {
        private FixedAssetBLL _assetBLL;
        private List<CategoryModel> _categories;
        private List<LocationModel> _locations;

        // Edit mode
        private bool _isEditMode = false;
        private int _editAssetId = 0;

        public int NewAssetId { get; set; }

        public frm_addFixedAsset()
        {
            InitializeComponent();
            _assetBLL = new FixedAssetBLL();
            _categories = new List<CategoryModel>();
            _locations = new List<LocationModel>();
            this.DialogResult = DialogResult.Cancel;
            this.StartPosition = FormStartPosition.CenterParent;
        }

        /// <summary>
        /// Edit-mode constructor. Locks cost/category/dep-method fields as they are
        /// accounting-controlled after creation; only name, location and notes are editable.
        /// </summary>
        public frm_addFixedAsset(FixedAssetModel asset) : this()
        {
            if (asset == null) throw new ArgumentNullException("asset");
            _isEditMode = true;
            _editAssetId = asset.AssetId;
            Tag = asset; // stored for use after Load
        }

        private void frm_addFixedAsset_Load(object sender, EventArgs e)
        {
            try
            {
                AppTheme.Apply(this);
                LoadCategories();
                LoadLocations();

                if (_isEditMode && Tag is FixedAssetModel asset)
                {
                    this.Text = "Edit Asset";
                    PopulateForEdit(asset);
                }
                else
                {
                    SetDefaults();
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, "Error", "خطأ");
            }
        }

        private void PopulateForEdit(FixedAssetModel asset)
        {
            // Editable fields
            txtAssetName.Text = asset.AssetName;
            txtNotes.Text = asset.AssetDescription ?? "";

            // Locate matching location in the list (offset by 1 due to "(None)" at index 0)
            int locIdx = _locations.FindIndex(l => l.LocationId == asset.LocationId);
            ddlLocation.SelectedIndex = locIdx >= 0 ? locIdx + 1 : 0;

            // Read-only fields – show for context but disable editing
            txtAssetCode.Text = asset.AssetCode;
            txtAssetCode.ReadOnly = true;

            int catIdx = _categories.FindIndex(c => c.CategoryId == asset.CategoryId);
            if (catIdx >= 0) ddlCategory.SelectedIndex = catIdx;
            ddlCategory.Enabled = false;

            dtPurchaseDate.Value = asset.PurchaseDate;
            dtPurchaseDate.Enabled = false;

            txtCost.Text = asset.Cost.ToString("N2");
            txtCost.ReadOnly = true;

            txtSalvageValue.Text = asset.ResidualValue.ToString("N2");
            txtSalvageValue.ReadOnly = true;

            txtUsefulLifeMonths.Text = asset.UsefulLifeMonths.ToString();
            txtUsefulLifeMonths.ReadOnly = true;

            int depIdx = ddlDeprecationMethod.Items.IndexOf(asset.DepMethod ?? "STRAIGHT_LINE");
            if (depIdx >= 0) ddlDeprecationMethod.SelectedIndex = depIdx;
            ddlDeprecationMethod.Enabled = false;

            txtSerialNumber.Text = asset.SerialNumber ?? "";
            txtReplacementCost.Text = asset.ReplacementCost.HasValue
                ? asset.ReplacementCost.Value.ToString("N2") : "";
        }

        private void LoadCategories()
        {
            try
            {
                _categories = _assetBLL.GetCategories(activeOnly: true);
                ddlCategory.Items.Clear();

                foreach (var cat in _categories)
                {
                    ddlCategory.Items.Add(cat.CategoryName);
                }

                if (ddlCategory.Items.Count > 0)
                    ddlCategory.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                UiMessages.ShowError("Failed to load categories: " + ex.Message, "فشل تحميل الفئات", "Error", "خطأ");
            }
        }

        private void LoadLocations()
        {
            try
            {
                _locations = _assetBLL.GetLocations(activeOnly: true);
                ddlLocation.Items.Clear();
                ddlLocation.Items.Add("(None)");

                foreach (var loc in _locations)
                {
                    ddlLocation.Items.Add(loc.LocationName);
                }

                if (ddlLocation.Items.Count > 0)
                    ddlLocation.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                UiMessages.ShowError("Failed to load locations: " + ex.Message, "فشل تحميل المواقع", "Error", "خطأ");
            }
        }

        private void SetDefaults()
        {
            dtPurchaseDate.Value = DateTime.Today;
            ddlDeprecationMethod.Items.Clear();
            ddlDeprecationMethod.Items.AddRange(new object[] { "STRAIGHT_LINE", "REDUCING_BALANCE", "UNITS_OF_PRODUCTION" });
            ddlDeprecationMethod.SelectedIndex = 0;

            txtUsefulLifeMonths.Text = "60";
            txtCost.Text = "0.00";
            txtSalvageValue.Text = "0.00";
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInputs())
                    return;

                if (_isEditMode)
                {
                    using (BusyScope.Show(this, "Updating asset..."))
                    {
                        int? locationId = GetSelectedLocationId();
                        string notes = string.IsNullOrWhiteSpace(txtNotes.Text) ? null : txtNotes.Text.Trim();

                        _assetBLL.UpdateAssetDetails(
                            assetId: _editAssetId,
                            assetName: txtAssetName.Text.Trim(),
                            locationId: locationId,
                            notes: notes,
                            isActive: true
                        );
                    }

                    UiMessages.ShowInfo("Asset updated successfully.", "تم تحديث الأصل بنجاح", "Success", "نجاح");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    using (BusyScope.Show(this, "Creating asset..."))
                    {
                        int categoryId = GetSelectedCategoryId();
                        int? locationId = GetSelectedLocationId();

                        NewAssetId = _assetBLL.InsertAsset(
                            assetCode: txtAssetCode.Text.Trim(),
                            assetName: txtAssetName.Text.Trim(),
                            categoryId: categoryId,
                            locationId: locationId,
                            serialNumber: string.IsNullOrWhiteSpace(txtSerialNumber.Text) ? null : txtSerialNumber.Text.Trim(),
                            purchaseDate: dtPurchaseDate.Value,
                            cost: decimal.Parse(txtCost.Text),
                            depMethod: ddlDeprecationMethod.SelectedItem.ToString(),
                            usefulLifeMonths: int.Parse(txtUsefulLifeMonths.Text),
                            salvageValue: decimal.Parse(txtSalvageValue.Text),
                            replacementCost: string.IsNullOrWhiteSpace(txtReplacementCost.Text) ? (decimal?)null : decimal.Parse(txtReplacementCost.Text),
                            notes: string.IsNullOrWhiteSpace(txtNotes.Text) ? null : txtNotes.Text.Trim(),
                            createdBy: UsersModal.logged_in_userid
                        );
                    }

                    UiMessages.ShowInfo("Asset created successfully with ID: " + NewAssetId, "تم إنشاء الأصل بنجاح", "Success", "نجاح");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (ArgumentException ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, "Validation Error", "خطأ في التحقق");
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, "Error", "خطأ");
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtAssetName.Text))
            {
                UiMessages.ShowError("Asset name is required", "اسم الأصل مطلوب", "Validation Error", "خطأ في التحقق");
                txtAssetName.Focus();
                return false;
            }

            // The fields below are only relevant when creating a new asset
            if (!_isEditMode)
            {
                if (string.IsNullOrWhiteSpace(txtAssetCode.Text))
                {
                    UiMessages.ShowError("Asset code is required", "رمز الأصل مطلوب", "Validation Error", "خطأ في التحقق");
                    txtAssetCode.Focus();
                    return false;
                }

                if (ddlCategory.SelectedIndex < 0)
                {
                    UiMessages.ShowError("Please select a category", "يرجى تحديد فئة", "Validation Error", "خطأ في التحقق");
                    ddlCategory.Focus();
                    return false;
                }

                if (!decimal.TryParse(txtCost.Text, out decimal cost) || cost < 0)
                {
                    UiMessages.ShowError("Cost must be a valid positive number", "التكلفة يجب أن تكون رقما موجبا صحيحا", "Validation Error", "خطأ في التحقق");
                    txtCost.Focus();
                    return false;
                }

                if (!int.TryParse(txtUsefulLifeMonths.Text, out int months) || months <= 0)
                {
                    UiMessages.ShowError("Useful life must be greater than 0", "يجب أن تكون فترة الاستخدام المفيدة أكثر من 0", "Validation Error", "خطأ في التحقق");
                    txtUsefulLifeMonths.Focus();
                    return false;
                }

                if (!decimal.TryParse(txtSalvageValue.Text, out decimal salvage) || salvage < 0)
                {
                    UiMessages.ShowError("Salvage value must be a valid positive number", "قيمة الخردة يجب أن تكون رقما موجبا صحيحا", "Validation Error", "خطأ في التحقق");
                    txtSalvageValue.Focus();
                    return false;
                }

                if (!string.IsNullOrWhiteSpace(txtReplacementCost.Text) && !decimal.TryParse(txtReplacementCost.Text, out decimal replacement))
                {
                    UiMessages.ShowError("Replacement cost must be a valid number", "تكلفة الاستبدال يجب أن تكون رقما صحيحا", "Validation Error", "خطأ في التحقق");
                    txtReplacementCost.Focus();
                    return false;
                }
            }

            return true;
        }

        private int GetSelectedCategoryId()
        {
            if (ddlCategory.SelectedIndex >= 0 && ddlCategory.SelectedIndex < _categories.Count)
                return _categories[ddlCategory.SelectedIndex].CategoryId;

            throw new InvalidOperationException("Invalid category selected");
        }

        private int? GetSelectedLocationId()
        {
            if (ddlLocation.SelectedIndex <= 0 || ddlLocation.SelectedItem.ToString() == "(None)")
                return null;

            if (ddlLocation.SelectedIndex - 1 >= 0 && ddlLocation.SelectedIndex - 1 < _locations.Count)
                return _locations[ddlLocation.SelectedIndex - 1].LocationId;

            return null;
        }

        private void TxtCost_Leave(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtCost.Text, out decimal cost))
                txtCost.Text = cost.ToString("N2");
        }

        private void TxtSalvageValue_Leave(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtSalvageValue.Text, out decimal salvage))
                txtSalvageValue.Text = salvage.ToString("N2");
        }

        private void TxtReplacementCost_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtReplacementCost.Text) && decimal.TryParse(txtReplacementCost.Text, out decimal replacement))
                txtReplacementCost.Text = replacement.ToString("N2");
        }
    }
}
