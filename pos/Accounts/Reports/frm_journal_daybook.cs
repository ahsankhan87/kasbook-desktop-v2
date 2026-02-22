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
    public partial class frm_journal_daybook : Form
    {
        public frm_journal_daybook()
        {
            InitializeComponent();
           
        }

        private void frm_journal_daybook_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            StyleForm();
            DateTime date = txt_entry_date.Value.Date;
            Load_journal_daybook(date);
            
        }

        private void StyleForm()
        {
            AppTheme.ApplyListFormStyleLightHeader(panel1, null, panel2, grid_journal_daybook);
        }

        private void Load_journal_daybook(DateTime date)
        {
            try
            {
                grid_journal_daybook.DataSource = null;
                grid_journal_daybook.AutoGenerateColumns = false;

                GeneralBLL objBLL = new GeneralBLL();

                String keyword = "id,account_name,invoice_no,debit,credit,description";
                String table = "acc_entries WHERE entry_date = '" + date.ToString("yyyy-MM-dd") + "' AND branch_id="+ UsersModal.logged_in_branch_id + "";

                DataTable accounts_dt = new DataTable();
                accounts_dt = objBLL.GetRecord(keyword, table);

                double _dr_total = 0;
                double _cr_total = 0;

                foreach(DataRow dr in accounts_dt.Rows)
                {
                    _dr_total += Convert.ToDouble(dr["debit"].ToString());
                    _cr_total += Convert.ToDouble(dr["credit"].ToString());
                }

                DataRow newRow = accounts_dt.NewRow();
                newRow[1] = "Total";
                newRow[3] = _dr_total;
                newRow[4] = _cr_total;
                accounts_dt.Rows.InsertAt(newRow, accounts_dt.Rows.Count);

                grid_journal_daybook.DataSource = accounts_dt;
                CustomizeDataGridView();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }

        private void txt_entry_date_ValueChanged(object sender, EventArgs e)
        {
            DateTime date = txt_entry_date.Value.Date;
            Load_journal_daybook(date);
            
        }

        private void CustomizeDataGridView()
        {
            // Get the last row in the DataGridView
            DataGridViewRow lastRow = grid_journal_daybook.Rows[grid_journal_daybook.Rows.Count - 1];

            // Loop through all cells in the row
            foreach (DataGridViewCell cell in lastRow.Cells)
            {
                DataGridViewCellStyle style = new DataGridViewCellStyle(cell.Style);

                // Set the font to bold
                style.Font = new Font(grid_journal_daybook.Font, FontStyle.Bold);

                // Set the background color
                style.BackColor = Color.LightGray;

                // Apply the style to the current cell
                cell.Style = style;
            }

        }

        private void frm_journal_daybook_KeyDown(object sender, KeyEventArgs e)
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

        private void btn_print_Click(object sender, EventArgs e)
        {
            DGVPrinter printer = new DGVPrinter();
            printer.Title = "Group Report";
            printer.SubTitle = string.Format("Date: {0}", txt_entry_date.Value.Date.ToShortDateString());
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = false;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = "kasbook app";
            printer.PorportionalColumns = false; printer.FooterSpacing = 15;
            printer.PrintPreviewDataGridView(grid_journal_daybook);
            //printer.PrintDataGridView(grid_sales_report);
        }

       

    }
}
