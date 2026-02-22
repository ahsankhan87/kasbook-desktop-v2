using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using POS.BLL;
using pos.UI;
using pos.UI.Busy;

namespace pos
{
    public partial class frm_branches : Form
    {

        public frm_branches()
        {
            InitializeComponent();
        }


        public void frm_branches_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            StyleForm();
            load_branches_grid();
        }

        private void StyleForm()
        {
            AppTheme.ApplyListFormStyle(panel2, lbl_taxes_title, panel1, grid_branches, id);
        }

        public void load_branches_grid()
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Loading branches...", "جاري تحميل الفروع...")))
                {
                    grid_branches.DataSource = null;

                    //bind data in data grid view  
                    GeneralBLL objBLL = new GeneralBLL();
                    grid_branches.AutoGenerateColumns = false;

                    String keyword = "id,name,description, date_created";
                    String table = "pos_branches";
                    grid_branches.DataSource = objBLL.GetRecord(keyword, table);
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }

        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            frm_addBranch frm_addBranch_obj = new frm_addBranch(this);
            frm_addBranch.instance.tb_lbl_is_edit.Text = "false";

            frm_addBranch_obj.ShowDialog();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            if (grid_branches.CurrentRow == null)
            {
                UiMessages.ShowInfo(
                    "Please select a branch record to update.",
                    "يرجى اختيار سجل فرع للتحديث.",
                    "Branches",
                    "الفروع"
                );
                return;
            }

            string id = grid_branches.CurrentRow.Cells[0].Value.ToString();
            string title = grid_branches.CurrentRow.Cells[1].Value.ToString();

            frm_addBranch frm_addBranch_obj = new frm_addBranch(this);
            frm_addBranch.instance.tb_lbl_is_edit.Text = "true";

            frm_addBranch.instance.tb_id.Text = id;
            frm_addBranch.instance.tb_name.Text = title;

            frm_addBranch.instance.Show();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (grid_branches.CurrentRow == null)
            {
                UiMessages.ShowInfo(
                    "Please select a branch record to delete.",
                    "يرجى اختيار سجل فرع للحذف.",
                    "Branches",
                    "الفروع"
                );
                return;
            }

            string id = grid_branches.CurrentRow.Cells[0].Value.ToString();
            int branchId;
            if (!int.TryParse(id, out branchId) || branchId <= 0)
            {
                UiMessages.ShowInfo(
                    "The selected branch record is not valid.",
                    "سجل الفرع المحدد غير صالح.",
                    "Branches",
                    "الفروع"
                );
                return;
            }

            var confirm = UiMessages.ConfirmYesNo(
                "Delete this branch?",
                "هل تريد حذف هذا الفرع؟",
                captionEn: "Confirm Delete",
                captionAr: "تأكيد الحذف"
            );

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                using (BusyScope.Show(this, UiMessages.T("Deleting...", "جاري الحذف...")))
                {
                    BranchesBLL objBLL = new BranchesBLL();
                    objBLL.Delete(branchId);
                }

                UiMessages.ShowInfo(
                    "Branch has been deleted successfully.",
                    "تم حذف الفرع بنجاح.",
                    "Deleted",
                    "تم الحذف"
                );

                load_branches_grid();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }

        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            var confirm = UiMessages.ConfirmYesNo(
                "Refresh the branch list?",
                "هل تريد تحديث قائمة الفروع؟",
                captionEn: "Confirm Refresh",
                captionAr: "تأكيد التحديث"
            );

            if (confirm != DialogResult.Yes)
                return;

            load_branches_grid();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Searching...", "جاري البحث...")))
                {
                    BranchesBLL objBLL = new BranchesBLL();
                    String condition = (txt_search.Text ?? string.Empty).Trim();
                    grid_branches.DataSource = objBLL.SearchRecord(condition);
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }

        }

        private void txt_search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) // Enter
            {
                btn_search.PerformClick();
            }
        }

        private void frm_branches_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.N)
            {
                btn_new.PerformClick();

            }

            if (e.Control == true && e.KeyCode == Keys.U)
            {
                btn_update.PerformClick();
            }

            if (e.Control == true && e.KeyCode == Keys.D)
            {
                btn_delete.PerformClick();

            }
        }
    }
}
