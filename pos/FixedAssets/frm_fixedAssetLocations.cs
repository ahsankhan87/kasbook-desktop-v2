using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using POS.BLL.FixedAssets;
using POS.Core;
using pos.UI;
using pos.UI.Busy;

namespace pos.FixedAssets
{
    public partial class frm_fixedAssetLocations : Form
    {
        private FixedAssetBLL _assetBLL;
        private List<LocationModel> _locations;

        public frm_fixedAssetLocations()
        {
            InitializeComponent();
            _assetBLL = new FixedAssetBLL();
            _locations = new List<LocationModel>();
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void frm_fixedAssetLocations_Load(object sender, EventArgs e)
        {
            try
            {
                AppTheme.Apply(this);
                LoadLocations();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, "Error", "خطأ");
            }
        }

        private void LoadLocations()
        {
            try
            {
                using (BusyScope.Show(this, "Loading locations..."))
                {
                    _locations = _assetBLL.GetLocations(activeOnly: false);
                    RefreshGrid();
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, "Error", "خطأ");
            }
        }

        private void RefreshGrid()
        {
            dgvLocations.Rows.Clear();

            foreach (var location in _locations)
            {
                int rowIdx = dgvLocations.Rows.Add(
                    location.LocationId,
                    location.LocationCode,
                    location.LocationName,
                    location.LocationType,
                    location.ParentLocationId.HasValue ? location.ParentLocationId.ToString() : "-",
                    location.IsActive ? "Active" : "Inactive"
                );

                var row = dgvLocations.Rows[rowIdx];
                if (!location.IsActive)
                    row.DefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string locationCode = PromptInput("Enter Location Code:", "Code");
                if (string.IsNullOrEmpty(locationCode))
                    return;

                string locationName = PromptInput("Enter Location Name:", "Name");
                if (string.IsNullOrEmpty(locationName))
                    return;

                using (BusyScope.Show(this, "Creating location..."))
                {
                    int newId = _assetBLL.InsertLocation(
                        locationCode: locationCode,
                        locationName: locationName,
                        locationType: "LOCATION",
                        parentLocationId: null
                    );

                    UiMessages.ShowInfo($"Location created with ID: {newId}", "تم إنشاء الموقع بنجاح", "Success", "نجاح");
                    LoadLocations();
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, "Error", "خطأ");
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvLocations.SelectedRows.Count == 0)
            {
                UiMessages.ShowError("Please select a location to edit", "يرجى تحديد موقع للتعديل", "Selection Required", "مطلوب الاختيار");
                return;
            }

            try
            {
                DataGridViewRow row = dgvLocations.SelectedRows[0];
                int locationId = (int)row.Cells[0].Value;
                string locationCode = row.Cells[1].Value?.ToString() ?? "";
                string locationName = row.Cells[2].Value?.ToString() ?? "";

                string newName = PromptInput("Edit Location Name:", "Name", locationName);
                if (string.IsNullOrEmpty(newName))
                    return;

                using (BusyScope.Show(this, "Updating location..."))
                {
                    _assetBLL.UpdateLocation(
                        locationId: locationId,
                        locationName: newName,
                        locationType: "LOCATION",
                        parentLocationId: null,
                        isActive: true
                    );

                    UiMessages.ShowInfo("Location updated successfully", "تم تحديث الموقع بنجاح", "Success", "نجاح");
                    LoadLocations();
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, "Error", "خطأ");
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvLocations.SelectedRows.Count == 0)
            {
                UiMessages.ShowError("Please select a location to delete", "يرجى تحديد موقع للحذف", "Selection Required", "مطلوب الاختيار");
                return;
            }

            try
            {
                DataGridViewRow row = dgvLocations.SelectedRows[0];
                int locationId = (int)row.Cells[0].Value;
                string locationName = row.Cells[2].Value?.ToString() ?? "";

                if (UiMessages.ConfirmYesNo($"Delete location '{locationName}'?", $"حذف الموقع '{locationName}'؟", "Confirm", "تأكيد") != DialogResult.Yes)
                    return;

                using (BusyScope.Show(this, "Deleting location..."))
                {
                    _assetBLL.DeleteLocation(locationId);
                    UiMessages.ShowInfo("Location deleted successfully", "تم حذف الموقع بنجاح", "Success", "نجاح");
                    LoadLocations();
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, "Error", "خطأ");
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private string PromptInput(string prompt, string title, string defaultValue = "")
        {
            using (var form = new Form())
            using (var label = new Label())
            using (var textBox = new TextBox())
            using (var buttonOk = new Button())
            using (var buttonCancel = new Button())
            {
                form.Text = title;
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.StartPosition = FormStartPosition.CenterParent;
                form.ClientSize = new Size(360, 140);
                form.MinimizeBox = false;
                form.MaximizeBox = false;
                form.ShowInTaskbar = false;
                form.AcceptButton = buttonOk;
                form.CancelButton = buttonCancel;

                label.Text = prompt;
                label.AutoSize = true;
                label.Left = 12;
                label.Top = 12;

                textBox.Left = 12;
                textBox.Top = 40;
                textBox.Width = 330;
                textBox.Text = defaultValue;

                buttonOk.Text = "OK";
                buttonOk.DialogResult = DialogResult.OK;
                buttonOk.Left = 182;
                buttonOk.Top = 75;
                buttonOk.Width = 75;

                buttonCancel.Text = "Cancel";
                buttonCancel.DialogResult = DialogResult.Cancel;
                buttonCancel.Left = 267;
                buttonCancel.Top = 75;
                buttonCancel.Width = 75;

                form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });

                return form.ShowDialog(this) == DialogResult.OK ? textBox.Text.Trim() : string.Empty;
            }
        }
    }
}
