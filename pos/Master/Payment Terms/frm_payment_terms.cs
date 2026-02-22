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
    public partial class frm_payment_terms : Form
    {

        public frm_payment_terms()
        {
            InitializeComponent();
        }


        public void frm_payment_terms_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            StyleForm();
            load_payment_terms_grid();
        }

        private void StyleForm()
        {
            AppTheme.ApplyListFormStyle(panel2, lbl_taxes_title, panel1, grid_payment_terms, id);
        }

        public void load_payment_terms_grid()
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Loading payment terms...", "جاري تحميل شروط الدفع...")))
                {
                    grid_payment_terms.DataSource = null;

                    //bind data in data grid view  
                    GeneralBLL objBLL = new GeneralBLL();
                    grid_payment_terms.AutoGenerateColumns = false;

                    String keyword = "id,code,description,date_created";
                    String table = "pos_payment_terms";
                    grid_payment_terms.DataSource = objBLL.GetRecord(keyword, table);
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }

        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            frm_addPaymentTerm frm_addPaymentTerm_obj = new frm_addPaymentTerm(this);
            frm_addPaymentTerm.instance.tb_lbl_is_edit.Text = "false";

            frm_addPaymentTerm_obj.ShowDialog();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            if (grid_payment_terms.CurrentRow == null)
            {
                UiMessages.ShowInfo(
                    "Please select a payment term to update.",
                    "يرجى اختيار شرط دفع للتحديث.",
                    "Payment Terms",
                    "شروط الدفع"
                );
                return;
            }

            if(grid_payment_terms.Rows.Count > 0)
            {
                string id = grid_payment_terms.CurrentRow.Cells[0].Value.ToString();
                string code = grid_payment_terms.CurrentRow.Cells[1].Value.ToString();
                string desc = grid_payment_terms.CurrentRow.Cells[2].Value.ToString();

                frm_addPaymentTerm frm_addPaymentTerm_obj = new frm_addPaymentTerm(this);
                frm_addPaymentTerm.instance.tb_lbl_is_edit.Text = "true";

                frm_addPaymentTerm.instance.tb_id.Text = id;
                frm_addPaymentTerm.instance.tb_code.Text = code;
                frm_addPaymentTerm.instance.tb_desc.Text = desc;

                frm_addPaymentTerm.instance.Show();
            }
            
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (grid_payment_terms.CurrentRow == null)
            {
                UiMessages.ShowInfo(
                    "Please select a payment term to delete.",
                    "يرجى اختيار شرط دفع للحذف.",
                    "Payment Terms",
                    "شروط الدفع"
                );
                return;
            }

            string idText = Convert.ToString(grid_payment_terms.CurrentRow.Cells[0].Value);
            int id;
            if (!int.TryParse(idText, out id) || id <= 0)
            {
                UiMessages.ShowInfo(
                    "The selected record is not valid.",
                    "السجل المحدد غير صالح.",
                    "Payment Terms",
                    "شروط الدفع"
                );
                return;
            }

            var confirm = UiMessages.ConfirmYesNo(
                "Delete this payment term?",
                "هل تريد حذف شرط الدفع هذا؟",
                captionEn: "Confirm Delete",
                captionAr: "تأكيد الحذف"
            );

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                using (BusyScope.Show(this, UiMessages.T("Deleting...", "جاري الحذف...")))
                {
                    PaymentTermsBLL objBLL = new PaymentTermsBLL();
                    objBLL.Delete(id);
                }

                UiMessages.ShowInfo(
                    "Record deleted successfully.",
                    "تم حذف السجل بنجاح.",
                    "Deleted",
                    "تم الحذف"
                );

                load_payment_terms_grid();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            load_payment_terms_grid();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Searching...", "جاري البحث...")))
                {
                    PaymentTermsBLL objBLL = new PaymentTermsBLL();
                    String condition = (txt_search.Text ?? string.Empty).Trim();
                    grid_payment_terms.DataSource = objBLL.SearchRecord(condition);
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

        private void frm_payment_terms_KeyDown(object sender, KeyEventArgs e)
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
