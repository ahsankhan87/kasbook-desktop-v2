using POS.BLL;
using POS.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using pos.UI;
using pos.UI.Busy;

namespace pos.Products.ICT
{
    public partial class frm_ict : Form
    {
        public frm_ict()
        {
            InitializeComponent();
        }

        private void frm_ict_Load(object sender, EventArgs e)
        {
            using (BusyScope.Show(this, UiMessages.T("Loading ICT transfers...", "جاري تحميل تحويلات الفروع...")))
            {
                load_all_ict_grid();
            }
        }

        public void load_all_ict_grid()
        {
            try
            {
                ICTBLL objSalesBLL = new ICTBLL();
                grid_ict.DataSource = null;

                grid_ict.AutoGenerateColumns = false;
                grid_ict.DataSource = objSalesBLL.GetAll_ict_transfer_transactions();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, captionEn: "Error", captionAr: "خطأ");
            }
        }

        private void btn_transfer_Click(object sender, EventArgs e)
        {
            // If there is a transfer action later, route it here.
            // Keeping method for designer wiring.
            transfer_qty();
        }

        private void transfer_qty()
        {
            using (BusyScope.Show(this, UiMessages.T("Processing transfer...", "جاري تنفيذ التحويل...")))
            {
                try
                {
                    DialogResult result = UiMessages.ConfirmYesNo(
                        "Are you sure you want to transfer quantity?",
                        "هل أنت متأكد أنك تريد تحويل الكمية؟",
                        captionEn: "Transfer Quantity",
                        captionAr: "تحويل الكمية");

                    if (result != DialogResult.Yes)
                        return;

                    List<ICTModal> ict_list = BuildSelectedIctList(useReleaseDate: false);
                    if (ict_list.Count == 0)
                    {
                        UiMessages.ShowWarning(
                            "Please select at least one row and enter a valid quantity.",
                            "يرجى اختيار صف واحد على الأقل وإدخال كمية صحيحة.",
                            captionEn: "Transfer Quantity",
                            captionAr: "تحويل الكمية");
                        return;
                    }

                    // NOTE: existing code had this disabled.
                    // int sale_id = objSalesBLL.save_ict_transfer(ict_list);
                    int sale_id = 0;

                    if (sale_id > 0)
                    {
                        UiMessages.ShowInfo(
                            "ICT quantity transferred successfully.",
                            "تم تحويل كمية ICT بنجاح.",
                            captionEn: "Success",
                            captionAr: "نجاح");
                        load_all_ict_grid();
                    }
                    else
                    {
                        UiMessages.ShowWarning(
                            "Transfer was not saved.",
                            "لم يتم حفظ التحويل.",
                            captionEn: "Transfer Quantity",
                            captionAr: "تحويل الكمية");
                    }
                }
                catch (Exception ex)
                {
                    UiMessages.ShowError(ex.Message, ex.Message, captionEn: "Error", captionAr: "خطأ");
                }
            }
        }

        private List<ICTModal> BuildSelectedIctList(bool useReleaseDate)
        {
            var list = new List<ICTModal>();

            for (int i = 0; i < grid_ict.Rows.Count; i++)
            {
                var row = grid_ict.Rows[i];
                if (row == null || row.IsNewRow)
                    continue;

                var chkCell = row.Cells["chk"];
                if (chkCell == null || chkCell.Value == null)
                    continue;

                bool selected;
                try { selected = Convert.ToBoolean(chkCell.Value); }
                catch { selected = false; }

                if (!selected)
                    continue;

                double qty = 0;
                var qtyCell = row.Cells["qty_released"];
                if (qtyCell != null && qtyCell.Value != null)
                {
                    double.TryParse(Convert.ToString(qtyCell.Value), out qty);
                }

                // Require positive qty
                if (qty <= 0)
                    continue;

                list.Add(new ICTModal
                {
                    quantity = qty,
                    item_code = Convert.ToString(row.Cells["item_code"].Value),
                    item_number = Convert.ToString(row.Cells["item_number"].Value),
                    destination_branch_id = Convert.ToInt16(Convert.ToString(row.Cells["destination_branch_id"].Value)),
                    source_branch_id = Convert.ToInt16(Convert.ToString(row.Cells["source_branch_id"].Value)),
                    transfer_date = useReleaseDate ? default(DateTime) : DateTime.Now,
                    release_date = useReleaseDate ? DateTime.Now : default(DateTime)
                });
            }

            return list;
        }

        private void frm_ict_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                btn_refresh.PerformClick();
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            using (BusyScope.Show(this, UiMessages.T("Refreshing...", "جاري التحديث...")))
            {
                load_all_ict_grid();
            }
        }

        private void btn_release_qty_Click(object sender, EventArgs e)
        {
            using (BusyScope.Show(this, UiMessages.T("Releasing quantity...", "جاري اعتماد الكمية...")))
            {
                try
                {
                    DialogResult result = UiMessages.ConfirmYesNo(
                        "Are you sure you want to release quantity?",
                        "هل أنت متأكد أنك تريد اعتماد الكمية؟",
                        captionEn: "Release Quantity",
                        captionAr: "اعتماد الكمية");

                    if (result != DialogResult.Yes)
                        return;

                    ICTBLL objSalesBLL = new ICTBLL();

                    List<ICTModal> ict_list = BuildSelectedIctList(useReleaseDate: true);
                    if (ict_list.Count == 0)
                    {
                        UiMessages.ShowWarning(
                            "Please select at least one row and enter a valid quantity.",
                            "يرجى اختيار صف واحد على الأقل وإدخال كمية صحيحة.",
                            captionEn: "Release Quantity",
                            captionAr: "اعتماد الكمية");
                        return;
                    }

                    int sale_id = objSalesBLL.save_ict_release_qty(ict_list);

                    if (sale_id > 0)
                    {
                        UiMessages.ShowInfo(
                            "ICT quantity released successfully.",
                            "تم اعتماد كمية ICT بنجاح.",
                            captionEn: "Success",
                            captionAr: "نجاح");
                        load_all_ict_grid();
                    }
                    else
                    {
                        UiMessages.ShowWarning(
                            "Release was not saved.",
                            "لم يتم حفظ الاعتماد.",
                            captionEn: "Release Quantity",
                            captionAr: "اعتماد الكمية");
                    }
                }
                catch (Exception ex)
                {
                    UiMessages.ShowError(ex.Message, ex.Message, captionEn: "Error", captionAr: "خطأ");
                }
            }
        }
    }
}
