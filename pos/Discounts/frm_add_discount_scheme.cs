using POS.BLL;
using POS.Core;
using pos.UI;
using System;
using System.Data;
using System.Windows.Forms;

namespace pos.Discounts
{
    public partial class frm_add_discount_scheme : Form
    {
        private readonly frm_discount_schemes _parent;
        private readonly int _editId;
        private readonly bool _isEdit;

        public frm_add_discount_scheme(frm_discount_schemes parent, int editId = 0)
        {
            InitializeComponent();
            _parent = parent;
            _editId = editId;
            _isEdit = editId > 0;
        }

        private void frm_add_discount_scheme_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            cmb_calc_type.Items.Clear();
            cmb_calc_type.Items.Add("PERCENT");
            cmb_calc_type.Items.Add("AMOUNT");
            cmb_calc_type.SelectedIndex = 0;

            if (_isEdit)
            {
                lbl_header_title.Text = "Edit Discount Scheme";
                btn_save.Text = "Update";
                LoadForEdit();
            }
            else
            {
                lbl_header_title.Text = "New Discount Scheme";
                btn_save.Text = "Save";
                chk_is_active.Checked = true;
            }
        }

        private void LoadForEdit()
        {
            try
            {
                var bll = new DiscountSchemesBLL();
                DataTable dt = bll.GetById(_editId);
                if (dt.Rows.Count == 0) return;

                DataRow r = dt.Rows[0];
                txt_id.Text = r["id"].ToString();
                txt_name.Text = r["name"].ToString();
                txt_name_ar.Text = r["name_ar"].ToString();
                txt_value.Text = r["value"].ToString();
                chk_is_active.Checked = Convert.ToBoolean(r["is_active"]);
                cmb_calc_type.SelectedItem = r["calc_type"].ToString();

                if (r["start_date"] != DBNull.Value)
                    dtp_start.Value = Convert.ToDateTime(r["start_date"]);
                if (r["end_date"] != DBNull.Value)
                    dtp_end.Value = Convert.ToDateTime(r["end_date"]);

                chk_no_end.Checked = (r["end_date"] == DBNull.Value);
                chk_no_start.Checked = (r["start_date"] == DBNull.Value);
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (!Validate_()) return;

            try
            {
                var info = new DiscountSchemeModal
                {
                    id = _isEdit ? _editId : 0,
                    name = txt_name.Text.Trim(),
                    name_ar = txt_name_ar.Text.Trim(),
                    calc_type = cmb_calc_type.SelectedItem.ToString(),
                    value = double.Parse(txt_value.Text.Trim()),
                    is_active = chk_is_active.Checked,
                    start_date = chk_no_start.Checked ? (DateTime?)null : dtp_start.Value.Date,
                    end_date = chk_no_end.Checked ? (DateTime?)null : dtp_end.Value.Date,
                    branch_id = UsersModal.logged_in_branch_id,
                    company_id = UsersModal.loggedIncompanyID,
                    created_by = UsersModal.logged_in_userid
                };

                var bll = new DiscountSchemesBLL();
                if (_isEdit)
                    bll.Update(info);
                else
                    bll.Insert(info);

                UiMessages.ShowInfo(
                    _isEdit ? "Scheme updated successfully." : "Scheme saved successfully.",
                    _isEdit ? "تم تحديث الخطة بنجاح." : "تم حفظ الخطة بنجاح.",
                    "Discount Schemes", "خطط الخصم");

                _parent?.RefreshGrid();
                Close();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private bool Validate_()
        {
            if (string.IsNullOrWhiteSpace(txt_name.Text))
            {
                UiMessages.ShowInfo("Name is required.", "الاسم مطلوب.", "Validation", "التحقق");
                txt_name.Focus();
                return false;
            }

            double val;
            if (!double.TryParse(txt_value.Text.Trim(), out val) || val < 0)
            {
                UiMessages.ShowInfo("Discount value must be a valid positive number.", "قيمة الخصم يجب أن تكون رقماً موجباً.", "Validation", "التحقق");
                txt_value.Focus();
                return false;
            }

            if (cmb_calc_type.SelectedItem?.ToString() == "PERCENT" && val > 100)
            {
                UiMessages.ShowInfo("Percent discount cannot exceed 100%.", "نسبة الخصم لا يمكن أن تتجاوز 100%.", "Validation", "التحقق");
                txt_value.Focus();
                return false;
            }

            return true;
        }

        private void btn_cancel_Click(object sender, EventArgs e) => Close();

        private void chk_no_start_CheckedChanged(object sender, EventArgs e)
            => dtp_start.Enabled = !chk_no_start.Checked;

        private void chk_no_end_CheckedChanged(object sender, EventArgs e)
            => dtp_end.Enabled = !chk_no_end.Checked;
    }
}
