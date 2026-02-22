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
    public partial class frm_units : Form
    {

        public frm_units()
        {
            InitializeComponent();
        }


        public void frm_units_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            StyleForm();
            load_units_grid();
        }

        private void StyleForm()
        {
            AppTheme.ApplyListFormStyle(panel2, lbl_taxes_title, panel1, grid_units, id);
        }

        public void load_units_grid()
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Loading units...", "جاري تحميل الوحدات...")))
                {
                    grid_units.DataSource = null;

                    //bind data in data grid view  
                    GeneralBLL objBLL = new GeneralBLL();
                    grid_units.AutoGenerateColumns = false;

                    String keyword = "id,name,date_created";
                    String table = "pos_units";
                    grid_units.DataSource = objBLL.GetRecord(keyword, table);
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }

        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            frm_addUnit frm_addUnit_obj = new frm_addUnit(this);
            frm_addUnit.instance.tb_lbl_is_edit.Text = "false";

            frm_addUnit_obj.ShowDialog();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            if (grid_units.CurrentRow == null)
            {
                UiMessages.ShowInfo(
                    "Please select a unit to update.",
                    "يرجى اختيار وحدة للتحديث.",
                    "Units",
                    "الوحدات"
                );
                return;
            }

            string id = grid_units.CurrentRow.Cells[0].Value.ToString();
            string title = grid_units.CurrentRow.Cells[1].Value.ToString();
            string rate = grid_units.CurrentRow.Cells[2].Value.ToString();

            frm_addUnit frm_addUnit_obj = new frm_addUnit(this);
            frm_addUnit.instance.tb_lbl_is_edit.Text = "true";

            frm_addUnit.instance.tb_id.Text = id;
            frm_addUnit.instance.tb_name.Text = title;

            frm_addUnit.instance.Show();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (grid_units.CurrentRow == null)
            {
                UiMessages.ShowInfo(
                    "Please select a unit to delete.",
                    "يرجى اختيار وحدة للحذف.",
                    "Units",
                    "الوحدات"
                );
                return;
            }

            string idText = Convert.ToString(grid_units.CurrentRow.Cells[0].Value);
            int id;
            if (!int.TryParse(idText, out id) || id <= 0)
            {
                UiMessages.ShowInfo(
                    "The selected record is not valid.",
                    "السجل المحدد غير صالح.",
                    "Units",
                    "الوحدات"
                );
                return;
            }

            var confirm = UiMessages.ConfirmYesNo(
                "Delete this unit?",
                "هل تريد حذف هذه الوحدة؟",
                captionEn: "Confirm Delete",
                captionAr: "تأكيد الحذف"
            );

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                using (BusyScope.Show(this, UiMessages.T("Deleting...", "جاري الحذف...")))
                {
                    UnitsBLL objBLL = new UnitsBLL();
                    objBLL.Delete(id);
                }

                UiMessages.ShowInfo(
                    "Record deleted successfully.",
                    "تم حذف السجل بنجاح.",
                    "Deleted",
                    "تم الحذف"
                );

                load_units_grid();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            load_units_grid();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Searching...", "جاري البحث...")))
                {
                    UnitsBLL objBLL = new UnitsBLL();
                    String condition = (txt_search.Text ?? string.Empty).Trim();
                    grid_units.DataSource = objBLL.SearchRecord(condition);
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

        private void frm_units_KeyDown(object sender, KeyEventArgs e)
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
