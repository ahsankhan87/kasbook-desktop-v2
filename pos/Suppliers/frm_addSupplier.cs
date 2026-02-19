using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using pos.Suppliers.Supplier_Ledger_Report;
using POS.BLL;
using POS.Core;
using pos.UI;
using pos.UI.Busy;

namespace pos
{
    public partial class frm_addSupplier : Form
    {
        public static frm_addSupplier instance;
        public TextBox tb_id;
        public TextBox tb_first_name;
        public TextBox tb_last_name;
        public TextBox tb_address;
        public TextBox tb_vat_no;
        public TextBox tb_contact_no;
        public TextBox tb_email;
        public CheckBox vat_with_status;
        public Label tb_lbl_is_edit;
        private frm_suppliers mainForm;
        
        public frm_addSupplier(frm_suppliers mainForm): this()
        {
            this.mainForm = mainForm;
        }

        public frm_addSupplier()
        {
            InitializeComponent();
            instance = this;
            tb_id = txt_id;
            tb_first_name = txt_first_name;
            tb_last_name = txt_last_name;
            tb_address = txt_address;
            tb_vat_no = txt_vatno;
            tb_contact_no = txt_contact_no;
            tb_email = txt_email;
            tb_lbl_is_edit = lbl_edit_status;
            vat_with_status = chk_vat_status;
        }
        
        public void frm_addSupplier_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            StyleSupplierForm();

            txt_search.Focus();
            this.ActiveControl = txt_search;
            GetSupplierCode();
            get_accounts_dropdownlist();
        }

        private void StyleSupplierForm()
        {
            panel1.BackColor = AppTheme.PrimaryDark;
            panel1.ForeColor = AppTheme.TextOnPrimary;
            panel1.Padding = new Padding(8, 4, 8, 4);
            panel2.BackColor = SystemColors.Control;
            panel2.AutoScroll = true;
            panel2.AutoScrollMinSize = new Size(tableLayoutPanel1.PreferredSize.Width + 40, tableLayoutPanel1.PreferredSize.Height + 40);

            panel3.BackColor = SystemColors.Control;
            panelTransactionTop.BackColor = SystemColors.Control;
            panelTransactionBottom.BackColor = SystemColors.Control;

            lbl_customer_name.ForeColor = AppTheme.TextOnPrimary;
            label21.ForeColor = AppTheme.TextOnPrimary;

            foreach (Label lbl in panel1.Controls.OfType<Label>())
                lbl.ForeColor = AppTheme.TextOnPrimary;

            tabControl1.Font = AppTheme.FontTab;
            tabPage1.BackColor = SystemColors.Control;
            tabPage2.BackColor = SystemColors.Control;

            //tableLayoutPanel1.AutoSize = true;
            //tableLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            //tableLayoutPanel1.Dock = DockStyle.Top;

            groupBox1.Font = AppTheme.FontGroupBox;
            groupBox3.Font = AppTheme.FontGroupBox;
            groupBox2.Font = AppTheme.FontGroupBox;

            foreach (Button btn in new[] { btn_save, btn_update, btn_delete, btn_blank, btn_cancel, btn_refresh, btn_search, btn_payment, btn_trans_refresh, Btn_ledger_report, Btn_printPaymentReceipt })
            {
                if (btn == null) continue;
                btn.FlatStyle = FlatStyle.Flat;
                btn.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Regular);
                btn.Height = 34;
            }

            StyleLedgerGrid(grid_supplier_transactions);
        }

        private static void StyleLedgerGrid(DataGridView grid)
        {
            if (grid == null) return;

            typeof(DataGridView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.SetProperty,
                null, grid, new object[] { true });

            grid.BackgroundColor = SystemColors.AppWorkspace;
            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.GridColor = SystemColors.ControlLight;
            grid.RowHeadersVisible = false;
            grid.EnableHeadersVisualStyles = false;

            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grid.ColumnHeadersHeight = 34;
            grid.RowTemplate.Height = 30;

            grid.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = SystemColors.Control,
                ForeColor = SystemColors.ControlText,
                Font = AppTheme.FontGridHeader,
                SelectionBackColor = SystemColors.Control,
                SelectionForeColor = SystemColors.ControlText,
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(6, 4, 6, 4)
            };

            grid.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = SystemColors.Window,
                ForeColor = AppTheme.TextPrimary,
                Font = AppTheme.FontGrid,
                SelectionBackColor = SystemColors.Highlight,
                SelectionForeColor = SystemColors.HighlightText,
                Padding = new Padding(6, 2, 6, 2)
            };

            grid.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = AppTheme.GridAltRow,
                ForeColor = AppTheme.TextPrimary,
                Font = AppTheme.FontGrid,
                SelectionBackColor = SystemColors.Highlight,
                SelectionForeColor = SystemColors.HighlightText
            };
        }
        private void GetSupplierCode()
        {
            try
            {
                SupplierBLL objBLL = new SupplierBLL();
                string newCode = objBLL.getNextSupplierCode();
                txt_supplier_code.Text = newCode;
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    "Failed to generate supplier code. " + ex.Message,
                    "تعذر إنشاء رمز المورد. " + ex.Message,
                    "Error",
                    "خطأ"
                );
            }
        }
        public void get_accounts_dropdownlist()
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "id,name";
            string table = "acc_accounts";

            DataTable accounts = generalBLL_obj.GetRecord(keyword, table);
            DataRow emptyRow = accounts.NewRow();
            emptyRow[0] = 0;              // Set Column Value
            emptyRow[1] = "Please Select";              // Set Column Value
            accounts.Rows.InsertAt(emptyRow, 0);

            cmb_GL_account_code.DisplayMember = "name";
            cmb_GL_account_code.ValueMember = "id";
            cmb_GL_account_code.DataSource = accounts;

            cmb_GL_account_code.SelectedValue = "6"; // 6 is the default Ac payable Account id in acc_accounts table

        }

        public DataTable get_GL_accounts_dt()
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "id,name";
            string table = "acc_accounts";

            DataTable dt = generalBLL_obj.GetRecord(keyword, table);
            DataRow emptyRow = dt.NewRow();
            emptyRow[0] = 0;              // Set Column Value
            emptyRow[1] = "Select Account";              // Set Column Value
            dt.Rows.InsertAt(emptyRow, 0);
            return dt;
        }

        
        private void btn_save_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_first_name.Text))
            {
                UiMessages.ShowInfo(
                    "Supplier name is required name.",
                    "اسم المورد مطلوب (الاسم الأول واسم العائلة).",
                    "Validation",
                    "التحقق"
                );
                return;
            }
            var supplierBLL = new SupplierBLL();
            if(supplierBLL.IsSupplierCodeExists(txt_supplier_code.Text.Trim()))
            {
                UiMessages.ShowWarning(
                    "Supplier code already exists.",
                    "رمز المورد موجود بالفعل.",
                    "Validation",
                    "تحقق"
                );
                txt_supplier_code.Focus();
                return;
            }

            var confirm = UiMessages.ConfirmYesNo(
                "Save this supplier?",
                "هل تريد حفظ هذا المورد؟",
                captionEn: "Confirm",
                captionAr: "تأكيد"
            );
            if (confirm != DialogResult.Yes) return;

            try
            {
                SupplierModal info = new SupplierModal();
                info.first_name = txt_first_name.Text;
                info.last_name = txt_last_name.Text;
                info.email = txt_email.Text;
                info.vat_no = txt_vatno.Text;
                info.address = txt_address.Text;
                info.contact_no = txt_contact_no.Text;
                info.vat_with_status = chk_vat_status.Checked;
                info.StreetName = txt_StreetName.Text.Trim();
                info.CityName = txt_cityName.Text.Trim();
                info.BuildingNumber = txt_buildingNumber.Text.Trim();
                info.CitySubdivisionName = txt_citySubdivisionName.Text.Trim();
                info.PostalCode = txt_postalCode.Text.Trim();
                info.CountryName = txt_countryName.Text.Trim();
                info.GLAccountID = int.Parse(cmb_GL_account_code.SelectedValue.ToString());
                info.supplier_code = txt_supplier_code.Text.Trim();

                SupplierBLL objBLL = new SupplierBLL();
                int result = objBLL.Insert(info);
                if (result > 0)
                {
                    UiMessages.ShowInfo(
                        "Supplier has been created successfully.",
                        "تم إنشاء المورد بنجاح.",
                        "Success",
                        "نجاح"
                    );
                    clear_all();

                    if (mainForm != null)
                        mainForm.load_Suppliers_grid();
                }
                else
                {
                    UiMessages.ShowError(
                        "Supplier could not be saved. Please try again.",
                        "تعذر حفظ المورد. يرجى المحاولة مرة أخرى.",
                        "Error",
                        "خطأ"
                    );
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            //this.Dispose(); 
            this.Close();
        }

        private void frm_addSupplier_KeyDown(object sender, KeyEventArgs e)
        {
            //when you enter in textbox it will goto next textbox, work like TAB key
            if (e.KeyData == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
            if (e.KeyData == Keys.F3)
            {
                btn_save.PerformClick();
            }
            if (e.KeyData == Keys.F4)
            {
                btn_update.PerformClick();
            }
            if (e.KeyData == Keys.F5)
            {
                btn_refresh.PerformClick();
            }
            if( e.KeyData == Keys.F9)
            {
                txt_search.Focus();
            }
            
        }

        private void btn_blank_Click(object sender, EventArgs e)
        {
            clear_all();
        }
        
        private void clear_all()
        {
            txt_id.Text = "";
            txt_first_name.Text = "";
            txt_last_name.Text = "";
            txt_address.Text = "";
            txt_vatno.Text = "";
            txt_contact_no.Text = "";
            txt_email.Text = "";
            chk_vat_status.Checked = false;
            lbl_customer_name.Text = "";
            grid_supplier_transactions.DataSource = null;

            txt_StreetName.Text = "";
            txt_cityName.Text = "";
            txt_buildingNumber.Text = "";
            txt_citySubdivisionName.Text = "";
            txt_postalCode.Text = "";
            txt_countryName.Text = "SA";
            GetSupplierCode();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txt_id.Text))
            {
                UiMessages.ShowInfo(
                    "Please select a supplier record to update.",
                    "يرجى اختيار سجل مورد للتحديث.",
                    "Not Found",
                    "غير موجود"
                );
                return;
            }

            if (string.IsNullOrWhiteSpace(txt_first_name.Text))
            {
                UiMessages.ShowInfo(
                    "Supplier name is required name.",
                    "اسم المورد مطلوب.",
                    "Validation",
                    "التحقق"
                );
                return;
            }
            var supplierBLL = new SupplierBLL();
            if(supplierBLL.IsSupplierCodeExists(txt_supplier_code.Text.Trim(), int.Parse(txt_id.Text)))
            {
                UiMessages.ShowWarning(
                    "Supplier code already exists.",
                    "رمز المورد موجود بالفعل.",
                    "Validation",
                    "تحقق"
                );
                txt_supplier_code.Focus();
                return;
            }

            var confirm = UiMessages.ConfirmYesNo(
                "Update this supplier?",
                "هل تريد تحديث هذا المورد؟",
                captionEn: "Confirm",
                captionAr: "تأكيد"
            );
            if (confirm != DialogResult.Yes) return;

            try
            {
                SupplierModal info = new SupplierModal();
                info.first_name = txt_first_name.Text;
                info.last_name = txt_last_name.Text;
                info.email = txt_email.Text;
                info.vat_no = txt_vatno.Text;
                info.address = txt_address.Text;
                info.contact_no = txt_contact_no.Text;
                info.vat_with_status = chk_vat_status.Checked;
                info.StreetName = txt_StreetName.Text.Trim();
                info.CityName = txt_cityName.Text.Trim();
                info.BuildingNumber = txt_buildingNumber.Text.Trim();
                info.CitySubdivisionName = txt_citySubdivisionName.Text.Trim();
                info.PostalCode = txt_postalCode.Text.Trim();
                info.CountryName = txt_countryName.Text.Trim();
                info.GLAccountID = int.Parse(cmb_GL_account_code.SelectedValue.ToString());
                info.id = int.Parse(txt_id.Text);
                info.supplier_code = txt_supplier_code.Text.Trim();

                SupplierBLL objBLL = new SupplierBLL();
                int result = objBLL.Update(info);
                if (result > 0)
                {
                    UiMessages.ShowInfo(
                        "Supplier has been updated successfully.",
                        "تم تحديث المورد بنجاح.",
                        "Success",
                        "نجاح"
                    );
                    clear_all();

                    if (mainForm != null)
                        mainForm.load_Suppliers_grid();
                }
                else
                {
                    UiMessages.ShowError(
                        "Supplier could not be updated. Please try again.",
                        "تعذر تحديث المورد. يرجى المحاولة مرة أخرى.",
                        "Error",
                        "خطأ"
                    );
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            string id = txt_id.Text;

            if (string.IsNullOrWhiteSpace(id))
            {
                UiMessages.ShowInfo(
                    "Please select a supplier record to delete.",
                    "يرجى اختيار سجل مورد للحذف.",
                    "Delete",
                    "حذف"
                );
                return;
            }

            var confirm = UiMessages.ConfirmYesNo(
                "Delete this supplier? This action cannot be undone.",
                "هل تريد حذف هذا المورد؟ لا يمكن التراجع عن هذا الإجراء.",
                captionEn: "Confirm Delete",
                captionAr: "تأكيد الحذف"
            );
            if (confirm != DialogResult.Yes) return;

            try
            {
                SupplierBLL objBLL = new SupplierBLL();
                objBLL.Delete(int.Parse(id));

                UiMessages.ShowInfo(
                    "Supplier has been deleted successfully.",
                    "تم حذف المورد بنجاح.",
                    "Deleted",
                    "تم الحذف"
                );
                clear_all();

                if (mainForm != null)
                    mainForm.load_Suppliers_grid();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    "Failed to delete supplier. " + ex.Message,
                    "تعذر حذف المورد. " + ex.Message,
                    "Error",
                    "خطأ"
                );
            }
        }

        public void load_transactions_grid(int supplier_id)
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Loading supplier transactions...", "جاري تحميل حركات المورد...")))
                {
                    grid_supplier_transactions.DataSource = null;

                    //bind data in data grid view  
                    GeneralBLL objBLL = new GeneralBLL();
                    grid_supplier_transactions.AutoGenerateColumns = false;

                    String keyword = "id,invoice_no,debit,credit,(credit-debit) AS balance,description,entry_date,account_id,account_name";
                    String table = "pos_suppliers_payments WHERE supplier_id = " + supplier_id + "";

                    DataTable dt = new DataTable();
                    dt = objBLL.GetRecord(keyword, table);

                    double _dr_total = 0;
                    double _cr_total = 0;

                    foreach (DataRow dr in dt.Rows)
                    {
                        _dr_total += Convert.ToDouble(dr["debit"].ToString());
                        _cr_total += Convert.ToDouble(dr["credit"].ToString());

                    }

                    DataRow newRow = dt.NewRow();
                    newRow[8] = "Total";
                    newRow[2] = _dr_total;
                    newRow[3] = _cr_total;
                    newRow[4] = (_cr_total - _dr_total);
                    dt.Rows.InsertAt(newRow, dt.Rows.Count);

                    grid_supplier_transactions.DataSource = dt;
                    CustomizeDataGridView();
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    "Failed to load supplier transactions. " + ex.Message,
                    "تعذر تحميل حركات المورد. " + ex.Message,
                    "Error",
                    "خطأ"
                );
                throw;
            }

        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            string supplier_id = txt_id.Text;

            if (!string.IsNullOrWhiteSpace(supplier_id))
            {
                using (BusyScope.Show(this, UiMessages.T("Loading supplier details...", "جاري تحميل بيانات المورد...")))
                {
                    load_detail(int.Parse(supplier_id));
                }

            }
            else
            {
                UiMessages.ShowInfo(
                    "Please select a supplier first.",
                    "يرجى اختيار مورد أولاً.",
                    "Supplier",
                    "المورد"
                );
            }
        }

        private void btn_trans_refresh_Click(object sender, EventArgs e)
        {
            string supplier_id = txt_id.Text;
            if (!string.IsNullOrWhiteSpace(supplier_id))
            {
                load_transactions_grid(int.Parse(supplier_id));
            }
            else
            {
                UiMessages.ShowInfo(
                    "Please select a supplier first.",
                    "يرجى اختيار مورد أولاً.",
                    "Supplier",
                    "المورد"
                );
            }
        }

        private void btn_payment_Click(object sender, EventArgs e)
        {
            string supplier_id = txt_id.Text;
            string supplier_name = lbl_customer_name.Text;
            if (!string.IsNullOrWhiteSpace(supplier_id))
            {
                frm_supplier_payment obj = new frm_supplier_payment(this, int.Parse(supplier_id), supplier_name);
                obj.ShowDialog();
                CustomizeDataGridView();
            }
            else
            {
                UiMessages.ShowInfo(
                    "Please select a supplier first.",
                    "يرجى اختيار مورد أولاً.",
                    "Supplier",
                    "المورد"
                );
            }
        }

        private void Btn_printPaymentReceipt_Click(object sender, EventArgs e)
        {
            try
            {
                if (grid_supplier_transactions.SelectedRows.Count == 0)
                {
                    UiMessages.ShowInfo(
                        "Please select a payment record to print.",
                        "يرجى اختيار سجل دفعة للطباعة.",
                        "Receipt",
                        "الإيصال"
                    );
                    return;
                }

                string payment_id = grid_supplier_transactions.SelectedRows[0].Cells["id"].Value.ToString();
                if (string.IsNullOrEmpty(payment_id))
                {
                    UiMessages.ShowError(
                        "The selected payment record is not valid.",
                        "سجل الدفعة المحدد غير صالح.",
                        "Error",
                        "خطأ"
                    );
                    return;
                }

                Frm_SupplierPaymentReceipt reportForm = new Frm_SupplierPaymentReceipt(payment_id);
                reportForm.ShowDialog();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void CustomizeDataGridView()
        {
            // Get the last row in the DataGridView
            DataGridViewRow lastRow = grid_supplier_transactions.Rows[grid_supplier_transactions.Rows.Count - 1];

            // Loop through all cells in the row
            foreach (DataGridViewCell cell in lastRow.Cells)
            {
                DataGridViewCellStyle style = new DataGridViewCellStyle(cell.Style);

                // Set the font to bold
                style.Font = new Font(grid_supplier_transactions.Font, FontStyle.Bold);

                // Set the background color
                style.BackColor = Color.LightGray;

                // Apply the style to the current cell
                cell.Style = style;
            }

        }

        private void Btn_ledger_report_Click(object sender, EventArgs e)
        {
            string supplier_id = txt_id.Text;
            if (!string.IsNullOrWhiteSpace(supplier_id))
            {
                using (BusyScope.Show(this, UiMessages.T("Opening ledger report...", "جاري فتح تقرير كشف الحساب...")))
                {
                    pos.Suppliers.Supplier_Ledger_Report.FrmSupplierLedgerReport obj = new Suppliers.Supplier_Ledger_Report.FrmSupplierLedgerReport(supplier_id);
                    obj.ShowDialog();
                }
            }
            else
            {
                UiMessages.ShowInfo(
                    "Please select a supplier to view the ledger report.",
                    "يرجى اختيار مورد لعرض تقرير كشف الحساب.",
                    "Supplier",
                    "المورد"
                );
            }
        }

        public void load_detail(int supplier_id)
        {
            SupplierBLL objBLL = new SupplierBLL();
            DataTable dt = objBLL.SearchRecordBySupplierID(supplier_id);
            foreach (DataRow myProductView in dt.Rows)
            {
                txt_id.Text = myProductView["id"].ToString();
                txt_first_name.Text = myProductView["first_name"].ToString();
                txt_last_name.Text = myProductView["last_name"].ToString();
                txt_address.Text = myProductView["address"].ToString();
                txt_vatno.Text = myProductView["vat_no"].ToString();
                txt_contact_no.Text = myProductView["contact_no"].ToString();
                txt_email.Text = myProductView["email"].ToString();
                chk_vat_status.Checked = bool.TryParse(Convert.ToString(myProductView["vat_status"]), out var vat) && vat;
                txt_StreetName.Text = myProductView["StreetName"].ToString();
                txt_cityName.Text = myProductView["CityName"].ToString();
                txt_buildingNumber.Text = myProductView["BuildingNumber"].ToString();
                txt_citySubdivisionName.Text = myProductView["CitySubdivisionName"].ToString();
                txt_postalCode.Text = myProductView["PostalCode"].ToString();
                txt_countryName.Text = myProductView["CountryName"].ToString();
                cmb_GL_account_code.SelectedValue = (myProductView["GLAccountID"].ToString() == "" ? 0 : Convert.ToInt32(myProductView["GLAccountID"].ToString()));
                txt_supplier_code.Text = myProductView["supplier_code"].ToString();
            }
            lbl_customer_name.Visible = true;
            lbl_customer_name.Text = txt_first_name.Text + ' ' + txt_last_name.Text;
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            frm_search_suppliers search_obj = new frm_search_suppliers(this, txt_search.Text);
            search_obj.ShowDialog();
        }

    }
}
