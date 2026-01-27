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
    public partial class frm_payment_method : Form
    {

        public frm_payment_method()
        {
            InitializeComponent();
        }


        public void frm_payment_method_Load(object sender, EventArgs e)
        {
            load_payment_method_grid();
        }

        public void load_payment_method_grid()
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Loading payment methods...", "جاري تحميل طرق الدفع...")))
                {
                    grid_payment_method.DataSource = null;

                    //bind data in data grid view  
                    GeneralBLL objBLL = new GeneralBLL();
                    grid_payment_method.AutoGenerateColumns = false;

                    String keyword = "id,code,description,date_created";
                    String table = "pos_payment_method";
                    grid_payment_method.DataSource = objBLL.GetRecord(keyword, table);
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }

        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            frm_addPaymentMethod frm_addPaymentMethod_obj = new frm_addPaymentMethod(this);
            frm_addPaymentMethod.instance.tb_lbl_is_edit.Text = "false";

            frm_addPaymentMethod_obj.ShowDialog();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            if (grid_payment_method.CurrentRow == null)
            {
                UiMessages.ShowInfo(
                    "Please select a payment method to update.",
                    "يرجى اختيار طريقة دفع للتحديث.",
                    "Payment Methods",
                    "طرق الدفع"
                );
                return;
            }

            string id = grid_payment_method.CurrentRow.Cells[0].Value.ToString();
            string code = grid_payment_method.CurrentRow.Cells[1].Value.ToString();
            string desc = grid_payment_method.CurrentRow.Cells[2].Value.ToString();

            frm_addPaymentMethod frm_addPaymentMethod_obj = new frm_addPaymentMethod(this);
            frm_addPaymentMethod.instance.tb_lbl_is_edit.Text = "true";

            frm_addPaymentMethod.instance.tb_id.Text = id;
            frm_addPaymentMethod.instance.tb_code.Text = code;
            frm_addPaymentMethod.instance.tb_desc.Text = desc;

            frm_addPaymentMethod.instance.Show();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (grid_payment_method.CurrentRow == null)
            {
                UiMessages.ShowInfo(
                    "Please select a payment method to delete.",
                    "يرجى اختيار طريقة دفع للحذف.",
                    "Payment Methods",
                    "طرق الدفع"
                );
                return;
            }

            string idText = Convert.ToString(grid_payment_method.CurrentRow.Cells[0].Value);
            int id;
            if (!int.TryParse(idText, out id) || id <= 0)
            {
                UiMessages.ShowInfo(
                    "The selected record is not valid.",
                    "السجل المحدد غير صالح.",
                    "Payment Methods",
                    "طرق الدفع"
                );
                return;
            }

            var confirm = UiMessages.ConfirmYesNo(
                "Delete this payment method?",
                "هل تريد حذف طريقة الدفع هذه؟",
                captionEn: "Confirm Delete",
                captionAr: "تأكيد الحذف"
            );

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                using (BusyScope.Show(this, UiMessages.T("Deleting...", "جاري الحذف...")))
                {
                    PaymentMethodBLL objBLL = new PaymentMethodBLL();
                    objBLL.Delete(id);
                }

                UiMessages.ShowInfo(
                    "Record deleted successfully.",
                    "تم حذف السجل بنجاح.",
                    "Deleted",
                    "تم الحذف"
                );

                load_payment_method_grid();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            load_payment_method_grid();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Searching...", "جاري البحث...")))
                {
                    PaymentMethodBLL objBLL = new PaymentMethodBLL();
                    String condition = (txt_search.Text ?? string.Empty).Trim();
                    grid_payment_method.DataSource = objBLL.SearchRecord(condition);
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

        private void frm_payment_method_KeyDown(object sender, KeyEventArgs e)
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
