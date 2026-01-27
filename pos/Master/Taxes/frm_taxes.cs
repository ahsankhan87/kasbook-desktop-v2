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
    public partial class frm_taxes : Form
    {

        public frm_taxes()
        {
            InitializeComponent();
        }


        public void frm_taxes_Load(object sender, EventArgs e)
        {
            load_Taxes_grid();
        }

        public void load_Taxes_grid()
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Loading taxes...", "جاري تحميل الضرائب...")))
                {
                    grid_taxes.DataSource = null;

                    //bind data in data grid view  
                    GeneralBLL objBLL = new GeneralBLL();
                    grid_taxes.AutoGenerateColumns = false;

                    String keyword = "id,title,rate,date_created";
                    String table = "pos_taxes";
                    grid_taxes.DataSource = objBLL.GetRecord(keyword, table);
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }

        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            frm_addTax frm_addTax_obj = new frm_addTax(this);
            frm_addTax.instance.tb_lbl_is_edit.Text = "false";

            frm_addTax_obj.ShowDialog();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            if (grid_taxes.CurrentRow == null)
            {
                UiMessages.ShowInfo(
                    "Please select a tax record to update.",
                    "يرجى اختيار ضريبة للتحديث.",
                    "Taxes",
                    "الضرائب"
                );
                return;
            }

            if (grid_taxes.Rows.Count > 0)
            {
                string id = grid_taxes.CurrentRow.Cells["id"].Value.ToString();
                string title = grid_taxes.CurrentRow.Cells["title"].Value.ToString();
                string rate = grid_taxes.CurrentRow.Cells["rate"].Value.ToString();

                frm_addTax frm_addTax_obj = new frm_addTax(this);
                frm_addTax.instance.tb_lbl_is_edit.Text = "true";

                frm_addTax.instance.tb_id.Text = id;
                frm_addTax.instance.tb_title.Text = title;
                frm_addTax.instance.tb_rate.Text = rate;

                frm_addTax.instance.Show();
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (grid_taxes.CurrentRow == null)
            {
                UiMessages.ShowInfo(
                    "Please select a tax record to delete.",
                    "يرجى اختيار ضريبة للحذف.",
                    "Taxes",
                    "الضرائب"
                );
                return;
            }

            string idText = Convert.ToString(grid_taxes.CurrentRow.Cells[0].Value);
            int id;
            if (!int.TryParse(idText, out id) || id <= 0)
            {
                UiMessages.ShowInfo(
                    "The selected record is not valid.",
                    "السجل المحدد غير صالح.",
                    "Taxes",
                    "الضرائب"
                );
                return;
            }

            var confirm = UiMessages.ConfirmYesNo(
                "Delete this tax?",
                "هل تريد حذف هذه الضريبة؟",
                captionEn: "Confirm Delete",
                captionAr: "تأكيد الحذف"
            );

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                using (BusyScope.Show(this, UiMessages.T("Deleting...", "جاري الحذف...")))
                {
                    TaxBLL objBLL = new TaxBLL();
                    objBLL.Delete(id);
                }

                UiMessages.ShowInfo(
                    "Record deleted successfully.",
                    "تم حذف السجل بنجاح.",
                    "Deleted",
                    "تم الحذف"
                );
                load_Taxes_grid();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }

        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            load_Taxes_grid();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Searching...", "جاري البحث...")))
                {
                    TaxBLL objBLL = new TaxBLL();
                    String condition = (txt_search.Text ?? string.Empty).Trim();
                    grid_taxes.DataSource = objBLL.SearchRecord(condition);
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

        private void frm_taxes_KeyDown(object sender, KeyEventArgs e)
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
