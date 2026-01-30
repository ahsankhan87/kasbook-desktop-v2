using POS.BLL;
using POS.Core;
using System;
using System.Data;
using System.Windows.Forms;
using pos.UI;
using pos.UI.Busy;

namespace pos.Products.ICT
{
    public partial class frm_destination_branch : Form
    {
        public int _branch_id;

        public frm_destination_branch()
        {
            InitializeComponent();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frm_destination_branch_Load(object sender, EventArgs e)
        {
            using (BusyScope.Show(this, UiMessages.T("Loading branches...", "جاري تحميل الفروع...")))
            {
                get_branches_DDL();
            }
        }

        public void get_branches_DDL()
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "id,name";
            string table = "pos_branches WHERE id <> " + UsersModal.logged_in_branch_id;

            DataTable branches_DDL = generalBLL_obj.GetRecord(keyword, table);

            cmb_branches.DisplayMember = "name";
            cmb_branches.ValueMember = "id";
            cmb_branches.DataSource = branches_DDL;

            if (branches_DDL == null || branches_DDL.Rows.Count == 0)
            {
                UiMessages.ShowWarning(
                    "No destination branches available.",
                    "لا توجد فروع متاحة للاختيار.",
                    captionEn: "Branches",
                    captionAr: "الفروع");

                btn_ok.Enabled = false;
                return;
            }

            btn_ok.Enabled = true;
            if (cmb_branches.Items.Count > 0)
                cmb_branches.SelectedIndex = 0;
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmb_branches.SelectedValue == null)
                {
                    UiMessages.ShowWarning(
                        "Please select a destination branch.",
                        "يرجى اختيار فرع الوجهة.",
                        captionEn: "Branches",
                        captionAr: "الفروع");
                    cmb_branches.Focus();
                    return;
                }

                int id;
                if (!int.TryParse(Convert.ToString(cmb_branches.SelectedValue), out id) || id <= 0)
                {
                    UiMessages.ShowWarning(
                        "Invalid destination branch selected.",
                        "تم اختيار فرع غير صالح.",
                        captionEn: "Branches",
                        captionAr: "الفروع");
                    cmb_branches.Focus();
                    return;
                }

                _branch_id = id;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, captionEn: "Error", captionAr: "خطأ");
            }
        }
    }
}
