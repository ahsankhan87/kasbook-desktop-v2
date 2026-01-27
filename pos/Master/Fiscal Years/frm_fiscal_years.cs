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
using POS.Core;
using pos.UI;
using pos.UI.Busy;

namespace pos
{
    public partial class frm_fiscal_years : Form
    {

        public frm_fiscal_years()
        {
            InitializeComponent();
        }


        public void frm_fiscal_years_Load(object sender, EventArgs e)
        {
            load_fiscal_years_grid();
        }

        public void load_fiscal_years_grid()
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Loading fiscal years...", "جاري تحميل السنوات المالية...")))
                {
                    grid_fiscal_years.DataSource = null;

                    //bind data in data grid view  
                    GeneralBLL objBLL = new GeneralBLL();
                    grid_fiscal_years.AutoGenerateColumns = false;

                    String keyword = "*";
                    String table = "acc_fiscal_years";
                    grid_fiscal_years.DataSource = objBLL.GetRecord(keyword, table);
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }

        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            frm_addFiscalYear frm_addFiscalYear_obj = new frm_addFiscalYear(this);
            frm_addFiscalYear.instance.tb_lbl_is_edit.Text = "false";

            frm_addFiscalYear_obj.ShowDialog();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            if (grid_fiscal_years.CurrentRow == null)
            {
                UiMessages.ShowInfo(
                    "Please select a fiscal year record to update.",
                    "يرجى اختيار سنة مالية للتحديث.",
                    "Fiscal Years",
                    "السنوات المالية"
                );
                return;
            }

            string id = grid_fiscal_years.CurrentRow.Cells["id"].Value.ToString();
            string name = grid_fiscal_years.CurrentRow.Cells["name"].Value.ToString();
            string code = grid_fiscal_years.CurrentRow.Cells["code"].Value.ToString();
            string from_date = grid_fiscal_years.CurrentRow.Cells["from_date"].Value.ToString();
            string to_date = grid_fiscal_years.CurrentRow.Cells["to_date"].Value.ToString();

            frm_addFiscalYear frm_addFiscalYear_obj = new frm_addFiscalYear(this);
            frm_addFiscalYear.instance.tb_lbl_is_edit.Text = "true";

            frm_addFiscalYear.instance.tb_id.Text = id;
            frm_addFiscalYear.instance.tb_name.Text = name;
            frm_addFiscalYear.instance.tb_code.Text = code;
            frm_addFiscalYear.instance.tb_from_date.Text = from_date;
            frm_addFiscalYear.instance.tb_to_date.Text = to_date;

            frm_addFiscalYear.instance.Show();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (grid_fiscal_years.CurrentRow == null)
            {
                UiMessages.ShowInfo(
                    "Please select a fiscal year record to delete.",
                    "يرجى اختيار سنة مالية للحذف.",
                    "Fiscal Years",
                    "السنوات المالية"
                );
                return;
            }

            string idText = Convert.ToString(grid_fiscal_years.CurrentRow.Cells["id"].Value);
            int id;
            if (!int.TryParse(idText, out id) || id <= 0)
            {
                UiMessages.ShowInfo(
                    "The selected record is not valid.",
                    "السجل المحدد غير صالح.",
                    "Fiscal Years",
                    "السنوات المالية"
                );
                return;
            }

            var confirm = UiMessages.ConfirmYesNo(
                "Delete this fiscal year?",
                "هل تريد حذف هذه السنة المالية؟",
                captionEn: "Confirm Delete",
                captionAr: "تأكيد الحذف"
            );

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                using (BusyScope.Show(this, UiMessages.T("Deleting...", "جاري الحذف...")))
                {
                    FiscalYearBLL objBLL = new FiscalYearBLL();
                    objBLL.Delete(id);
                }

                UiMessages.ShowInfo(
                    "Record deleted successfully.",
                    "تم حذف السجل بنجاح.",
                    "Deleted",
                    "تم الحذف"
                );
                load_fiscal_years_grid();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            load_fiscal_years_grid();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Searching...", "جاري البحث...")))
                {
                    FiscalYearBLL objBLL = new FiscalYearBLL();
                    String condition = (txt_search.Text ?? string.Empty).Trim();
                    grid_fiscal_years.DataSource = objBLL.SearchRecord(condition);
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

        private void frm_fiscal_years_KeyDown(object sender, KeyEventArgs e)
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

        private void grid_fiscal_years_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string name = grid_fiscal_years.Columns[e.ColumnIndex].Name;
            if (name == "activate")
            {
                if (grid_fiscal_years.CurrentRow == null)
                {
                    UiMessages.ShowInfo(
                        "Please select a fiscal year to activate.",
                        "يرجى اختيار سنة مالية للتفعيل.",
                        "Fiscal Years",
                        "السنوات المالية"
                    );
                    return;
                }

                string idText = Convert.ToString(grid_fiscal_years.CurrentRow.Cells["id"].Value);
                int id;
                if (!int.TryParse(idText, out id) || id <= 0)
                {
                    UiMessages.ShowInfo(
                        "The selected record is not valid.",
                        "السجل المحدد غير صالح.",
                        "Fiscal Years",
                        "السنوات المالية"
                    );
                    return;
                }

                string from_date = Convert.ToString(grid_fiscal_years.CurrentRow.Cells["from_date"].Value);
                string to_date = Convert.ToString(grid_fiscal_years.CurrentRow.Cells["to_date"].Value);
                string desc = Convert.ToString(grid_fiscal_years.CurrentRow.Cells["name"].Value);

                var confirm = UiMessages.ConfirmYesNo(
                    "Activate this fiscal year?",
                    "هل تريد تفعيل هذه السنة المالية؟",
                    captionEn: "Confirm",
                    captionAr: "تأكيد"
                );

                if (confirm != DialogResult.Yes)
                    return;

                try
                {
                    using (BusyScope.Show(this, UiMessages.T("Activating...", "جاري التفعيل...")))
                    {
                        FiscalYearBLL objBLL = new FiscalYearBLL();
                        objBLL.SetAllStatusZero(id);
                        objBLL.UpdateStatus(id, true);

                        UsersModal.fiscal_year = desc;
                        UsersModal.fy_from_date = Convert.ToDateTime(from_date);
                        UsersModal.fy_to_date = Convert.ToDateTime(to_date);
                    }

                    UiMessages.ShowInfo(
                        "Fiscal year activated successfully.",
                        "تم تفعيل السنة المالية بنجاح.",
                        "Success",
                        "نجاح"
                    );
                    load_fiscal_years_grid();
                }
                catch (Exception ex)
                {
                    UiMessages.ShowError(ex.Message, ex.Message);
                }
            }
        }
    }
}
