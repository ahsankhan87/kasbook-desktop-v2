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
    public partial class frm_fixedAssetCategories : Form
    {
        private FixedAssetBLL _assetBLL;
        private List<CategoryModel> _categories;

        public frm_fixedAssetCategories()
        {
            InitializeComponent();
            _assetBLL = new FixedAssetBLL();
            _categories = new List<CategoryModel>();
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void frm_fixedAssetCategories_Load(object sender, EventArgs e)
        {
            try
            {
                AppTheme.Apply(this);
                LoadCategories();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, "Error", "خطأ");
            }
        }

        private void LoadCategories()
        {
            try
            {
                using (BusyScope.Show(this, "Loading categories..."))
                {
                    _categories = _assetBLL.GetCategories(activeOnly: false);
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
            dgvCategories.Rows.Clear();

            foreach (var category in _categories)
            {
                int rowIdx = dgvCategories.Rows.Add(
                    category.CategoryId,
                    category.CategoryCode,
                    category.CategoryName,
                    category.DepreciationMethod,
                    category.UsefulLifeMonths,
                    category.AnnualDepreciationRate.ToString("N2"),
                    category.IsActive ? "Active" : "Inactive"
                );

                var row = dgvCategories.Rows[rowIdx];
                if (!category.IsActive)
                    row.DefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string categoryCode = PromptInput("Enter Category Code:", "Code");
                if (string.IsNullOrEmpty(categoryCode))
                    return;

                string categoryName = PromptInput("Enter Category Name:", "Name");
                if (string.IsNullOrEmpty(categoryName))
                    return;

                using (BusyScope.Show(this, "Creating category..."))
                {
                    int newId = _assetBLL.InsertCategory(
                        categoryCode: categoryCode,
                        categoryName: categoryName,
                        deprecationMethod: "STRAIGHT_LINE",
                        usefulLifeMonths: 60,
                        annualDepreciationRate: null
                    );

                    UiMessages.ShowInfo($"Category created with ID: {newId}", "تم إنشاء الفئة بنجاح", "Success", "نجاح");
                    LoadCategories();
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, "Error", "خطأ");
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvCategories.SelectedRows.Count == 0)
            {
                UiMessages.ShowError("Please select a category to edit", "يرجى تحديد فئة للتعديل", "Selection Required", "مطلوب الاختيار");
                return;
            }

            try
            {
                DataGridViewRow row = dgvCategories.SelectedRows[0];
                int categoryId = (int)row.Cells[0].Value;
                string categoryCode = row.Cells[1].Value?.ToString() ?? "";
                string categoryName = row.Cells[2].Value?.ToString() ?? "";

                string newName = PromptInput("Edit Category Name:", "Name", categoryName);
                if (string.IsNullOrEmpty(newName))
                    return;

                using (BusyScope.Show(this, "Updating category..."))
                {
                    _assetBLL.UpdateCategory(
                        categoryId: categoryId,
                        categoryName: newName,
                        deprecationMethod: "STRAIGHT_LINE",
                        usefulLifeMonths: 60,
                        annualDepreciationRate: null,
                        isActive: true
                    );

                    UiMessages.ShowInfo("Category updated successfully", "تم تحديث الفئة بنجاح", "Success", "نجاح");
                    LoadCategories();
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, "Error", "خطأ");
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvCategories.SelectedRows.Count == 0)
            {
                UiMessages.ShowError("Please select a category to delete", "يرجى تحديد فئة للحذف", "Selection Required", "مطلوب الاختيار");
                return;
            }

            try
            {
                DataGridViewRow row = dgvCategories.SelectedRows[0];
                int categoryId = (int)row.Cells[0].Value;
                string categoryName = row.Cells[2].Value?.ToString() ?? "";

                if (UiMessages.ConfirmYesNo($"Delete category '{categoryName}'?", $"حذف الفئة '{categoryName}'؟", "Confirm", "تأكيد") != DialogResult.Yes)
                    return;

                using (BusyScope.Show(this, "Deleting category..."))
                {
                    _assetBLL.DeleteCategory(categoryId);
                    UiMessages.ShowInfo("Category deleted successfully", "تم حذف الفئة بنجاح", "Success", "نجاح");
                    LoadCategories();
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
