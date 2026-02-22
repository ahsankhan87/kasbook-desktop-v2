using DGVPrinterHelper;
using POS.BLL;
using POS.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using pos.UI;

namespace pos
{
    public partial class frm_group_report : Form
    {
        public frm_group_report()
        {
            InitializeComponent();
           
        }

        private void frm_group_report_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            StyleForm();
            get_groups_dropdownlist();
        }

        private void StyleForm()
        {
            AppTheme.ApplyListFormStyleLightHeader(panel1, null, panel2, grid_group_report);
        }

        private void CustomizeDataGridView()
        {
            // Get the last row in the DataGridView
            DataGridViewRow lastRow = grid_group_report.Rows[grid_group_report.Rows.Count - 1];

            // Loop through all cells in the row
            foreach (DataGridViewCell cell in lastRow.Cells)
            {
                DataGridViewCellStyle style = new DataGridViewCellStyle(cell.Style);

                // Set the font to bold
                style.Font = new Font(grid_group_report.Font, FontStyle.Bold);

                // Set the background color
                style.BackColor = Color.LightGray;

                // Apply the style to the current cell
                cell.Style = style;
            }

        }
        private void frm_group_report_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //when you enter in textbox it will goto next textbox, work like TAB key
                if (e.KeyData == Keys.Enter)
                {
                    SendKeys.Send("{TAB}");
                }

                if (e.Control && e.KeyCode == Keys.P)
                {
                    btn_print.PerformClick();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void get_groups_dropdownlist()
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "id,name";
            string table = "acc_groups";

            DataTable taxes = generalBLL_obj.GetRecord(keyword, table);
            DataRow emptyRow = taxes.NewRow();
            emptyRow[0] = Convert.ToInt32("0");              // Set Column Value
            taxes.Rows.InsertAt(emptyRow, 0);

            cmb_groups.DataSource = taxes;

            cmb_groups.DisplayMember = "name";
            cmb_groups.ValueMember = "id";
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            DateTime from_date = txt_from_date.Value.Date;
            DateTime to_date = txt_to_date.Value.Date;
            int group_id = (int)cmb_groups.SelectedValue;

            Load_group_report(from_date, to_date, group_id);
            
        }

        private void Load_group_report(DateTime from_date, DateTime to_date, int group_id)
        {
            try
            {
                grid_group_report.DataSource = null;
                grid_group_report.AutoGenerateColumns = false;

                AccountsBLL objgroupBLL = new AccountsBLL();

                DataTable groups_dt = new DataTable();
                groups_dt = objgroupBLL.GroupReport(from_date, to_date, group_id);

                double _dr_total = 0;
                double _cr_total = 0;

                foreach (DataRow dr in groups_dt.Rows)
                {
                    _dr_total += Convert.ToDouble(dr["debit"].ToString());
                    _cr_total += Convert.ToDouble(dr["credit"].ToString());
                }

                DataRow newRow = groups_dt.NewRow();
                newRow[0] = "Total";
                newRow[1] = _dr_total;
                newRow[2] = _cr_total;
                groups_dt.Rows.InsertAt(newRow, groups_dt.Rows.Count);

                grid_group_report.DataSource = groups_dt;

                CustomizeDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            DGVPrinter printer = new DGVPrinter();
            printer.Title = "Group Report";
            printer.SubTitle = string.Format("{0} To {1}", txt_from_date.Value.Date.ToShortDateString(), txt_to_date.Value.Date.ToShortDateString());
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = false;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = "kasbook app";
            printer.PorportionalColumns = false; printer.FooterSpacing = 15;
            printer.PrintPreviewDataGridView(grid_group_report);
            //printer.PrintDataGridView(grid_sales_report);
        }


    }
}
