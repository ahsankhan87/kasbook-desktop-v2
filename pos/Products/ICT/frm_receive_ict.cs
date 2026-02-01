using POS.BLL;
using POS.Core;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using pos.UI;
using pos.UI.Busy;

namespace pos.Products.ICT
{
    public partial class frm_receive_ict : Form
    {
        public frm_receive_ict()
        {
            InitializeComponent();
        }

        private void frm_receive_ict_Load(object sender, EventArgs e)
        {
            using (BusyScope.Show(this, UiMessages.T("Loading ICT requests...", "جاري تحميل طلبات النقل...")))
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
                grid_ict.DataSource = objSalesBLL.GetAll_ict_requests();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, captionEn: "Error", captionAr: "خطأ");
            }

        }

        private void btn_transfer_Click(object sender, EventArgs e)
        {
            transfer_qty();
        }

        private void transfer_qty()
        {
            using (BusyScope.Show(this, UiMessages.T("Processing receiving...", "جاري معالجة الاستلام...")))
            {
                try
                {
                    var confirm = UiMessages.ConfirmYesNo(
                        "Are you sure you want to receive quantity?",
                        "هل أنت متأكد من رغبتك في استلام الكمية؟",
                        captionEn: "Receiving Quantity",
                        captionAr: "استلام الكمية");

                    if (confirm != DialogResult.Yes)
                        return;

                    var objSalesBLL = new ICTBLL();
                    var ict_list = BuildSelectedTransferList();

                    if (ict_list.Count <= 0)
                    {
                        UiMessages.ShowWarning(
                            "Please select at least one row and enter a valid transfer quantity.",
                            "يرجى اختيار صف واحد على الأقل وإدخال كمية تحويل صحيحة.",
                            captionEn: "Receiving Quantity",
                            captionAr: "استلام الكمية");
                        return;
                    }

                    int sale_id = objSalesBLL.save_ict_transfer_qty(ict_list);

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
                            "Transaction was not saved.",
                            "لم يتم حفظ العملية.",
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

        private List<ICTModal> BuildSelectedTransferList()
        {
            var list = new List<ICTModal>();

            for (int i = 0; i < grid_ict.Rows.Count; i++)
            {
                var row = grid_ict.Rows[i];
                if (row == null || row.IsNewRow)
                    continue;

                bool selected = false;
                try
                {
                    if (row.Cells["chk"].Value != null)
                        selected = Convert.ToBoolean(row.Cells["chk"].Value);
                }
                catch
                {
                    selected = false;
                }

                if (!selected)
                    continue;

                // Require released_qty > 0 (business rule from original code)
                double releasedQty = 0;
                double.TryParse(Convert.ToString(row.Cells["released_qty"].Value), out releasedQty);
                if (releasedQty <= 0)
                    continue;

                double qty = 0;
                double.TryParse(Convert.ToString(row.Cells["qty"].Value), out qty);

                if (qty <= 0)
                    continue;

                list.Add(new ICTModal
                {
                    id = Convert.ToInt32(row.Cells["id"].Value),
                    quantity = qty,
                    item_code = Convert.ToString(row.Cells["item_code"].Value),
                    item_number = Convert.ToString(row.Cells["item_number"].Value),
                    destination_branch_id = Convert.ToInt16(Convert.ToString(row.Cells["destination_branch_id"].Value)),
                    source_branch_id = Convert.ToInt16(Convert.ToString(row.Cells["source_branch_id"].Value)),
                    status = true,
                    transfer_date = DateTime.Now,
                });
            }

            return list;
        }

        private List<ICTModal> BuildSelectedReleaseList()
        {
            var list = new List<ICTModal>();

            for (int i = 0; i < grid_ict.Rows.Count; i++)
            {
                var row = grid_ict.Rows[i];
                if (row == null || row.IsNewRow)
                    continue;

                bool selected = false;
                try
                {
                    if (row.Cells["chk"].Value != null)
                        selected = Convert.ToBoolean(row.Cells["chk"].Value);
                }
                catch
                {
                    selected = false;
                }

                if (!selected)
                    continue;

                double qty = 0;
                double.TryParse(Convert.ToString(row.Cells["qty_released"].Value), out qty);
                if (qty <= 0)
                    continue;

                list.Add(new ICTModal
                {
                    quantity = qty,
                    item_code = Convert.ToString(row.Cells["item_code"].Value),
                    item_number = Convert.ToString(row.Cells["item_number"].Value),
                    destination_branch_id = Convert.ToInt16(Convert.ToString(row.Cells["destination_branch_id"].Value)),
                    source_branch_id = Convert.ToInt16(Convert.ToString(row.Cells["source_branch_id"].Value)),
                    release_date = DateTime.Now,
                });
            }

            return list;
        }

        private void frm_receive_ict_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F3)
            {
                btn_transfer.PerformClick();
            }

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

                    List<ICTModal> ict_list = BuildSelectedReleaseList();
                    if (ict_list.Count <= 0)
                    {
                        UiMessages.ShowWarning(
                            "Please select at least one row and enter a valid release quantity.",
                            "يرجى اختيار صف واحد على الأقل وإدخال كمية اعتماد صحيحة.",
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
