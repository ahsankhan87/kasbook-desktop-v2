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
    public partial class frm_locations : Form
    {

        public frm_locations()
        {
            InitializeComponent();
        }


        public void frm_locations_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            StyleForm();
            load_Locations_grid();
        }

        private void StyleForm()
        {
            AppTheme.ApplyListFormStyle(panel2, lbl_taxes_title, panel1, grid_locations, id);
        }

        public void load_Locations_grid()
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Loading locations...", "جاري تحميل المواقع...")))
                {
                    grid_locations.DataSource = null;

                    //bind data in data grid view  
                    GeneralBLL objBLL = new GeneralBLL();
                    grid_locations.AutoGenerateColumns = false;

                    String keyword = "*";
                    String table = "pos_Locations";
                    grid_locations.DataSource = objBLL.GetRecord(keyword, table);
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }

        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            frm_addLocation frm_addLocation_obj = new frm_addLocation(this);
            frm_addLocation.instance.tb_lbl_is_edit.Text = "false";

            frm_addLocation_obj.ShowDialog();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            if (grid_locations.CurrentRow == null)
            {
                UiMessages.ShowInfo(
                    "Please select a location record to update.",
                    "يرجى اختيار موقع للتحديث.",
                    "Locations",
                    "المواقع"
                );
                return;
            }

            string id = grid_locations.CurrentRow.Cells["id"].Value.ToString();
            string name = grid_locations.CurrentRow.Cells["name"].Value.ToString();
            string code = grid_locations.CurrentRow.Cells["code"].Value.ToString();
            
            frm_addLocation frm_addLocation_obj = new frm_addLocation(this);
            frm_addLocation.instance.tb_lbl_is_edit.Text = "true";

            frm_addLocation.instance.tb_id.Text = id;
            frm_addLocation.instance.tb_name.Text = name;
            frm_addLocation.instance.tb_code.Text = code;
            
            frm_addLocation.instance.Show();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (grid_locations.CurrentRow == null)
            {
                UiMessages.ShowInfo(
                    "Please select a location record to delete.",
                    "يرجى اختيار موقع للحذف.",
                    "Locations",
                    "المواقع"
                );
                return;
            }

            string idText = Convert.ToString(grid_locations.CurrentRow.Cells[0].Value);
            int id;
            if (!int.TryParse(idText, out id) || id <= 0)
            {
                UiMessages.ShowInfo(
                    "The selected record is not valid.",
                    "السجل المحدد غير صالح.",
                    "Locations",
                    "المواقع"
                );
                return;
            }

            var confirm = UiMessages.ConfirmYesNo(
                "Delete this location?",
                "هل تريد حذف هذا الموقع؟",
                captionEn: "Confirm Delete",
                captionAr: "تأكيد الحذف"
            );

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                using (BusyScope.Show(this, UiMessages.T("Deleting...", "جاري الحذف...")))
                {
                    LocationsBLL objBLL = new LocationsBLL();
                    objBLL.Delete(id);
                }

                UiMessages.ShowInfo(
                    "Record deleted successfully.",
                    "تم حذف السجل بنجاح.",
                    "Deleted",
                    "تم الحذف"
                );

                load_Locations_grid();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }

        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            load_Locations_grid();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Searching...", "جاري البحث...")))
                {
                    LocationsBLL objBLL = new LocationsBLL();
                    String condition = (txt_search.Text ?? string.Empty).Trim();
                    grid_locations.DataSource = objBLL.SearchRecord(condition);
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

        private void frm_locations_KeyDown(object sender, KeyEventArgs e)
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
