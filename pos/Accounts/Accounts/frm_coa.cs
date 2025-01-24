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

namespace pos
{
    public partial class frm_coa : Form
    {
        public frm_coa()
        {
            InitializeComponent();
           
        }

        private void frm_coa_Load(object sender, EventArgs e)
        {
            Load_coa(UsersModal.fy_from_date, UsersModal.fy_to_date);
        }

        private void Load_coa(DateTime from_date, DateTime to_date)
        {
            try
            {
                grid_coa.DataSource = null;
                grid_coa.Rows.Clear();
                grid_coa.AutoGenerateColumns = false;

                AccountsBLL objAccountsBLL = new AccountsBLL();

                ///////////////////////Income start
                DataTable _level_2_ac_dt = new DataTable();
                DataTable _level_3_ac_dt = new DataTable();
                DataTable _amount_balance_dt = new DataTable();
                
                
                DataTable parent_ac_dt = objAccountsBLL.GetGroupAccountByParent(); //zero / empty select all account type
                foreach (DataRow dr in parent_ac_dt.Rows)
                {
                    string[] row00 = { dr["name"].ToString(), dr["name_2"].ToString(), "", "", "" };
                    grid_coa.Rows.Add(row00);
                    
                    _level_2_ac_dt = objAccountsBLL.GetGroupAccountByParent(int.Parse(dr["id"].ToString()));
                    
                    foreach (DataRow dr_2 in _level_2_ac_dt.Rows)
                    {
                        var account_name = "  " + dr_2["name"].ToString();
                        var account_name_2 = "  " + dr_2["name_2"].ToString();

                        string[] row0 = { account_name, account_name_2, "", "", "" };
                        grid_coa.Rows.Add(row0);

                        //////////// Level 3 acoutns
                        _level_3_ac_dt = objAccountsBLL.GetAccountByGroup(int.Parse(dr_2["id"].ToString()));
                        
                            foreach (DataRow dr_3 in _level_3_ac_dt.Rows)
                            {
                                var account_name_3 = "    " + dr_3["name"].ToString();
                                var account_name2_3 = "    " + dr_3["name_2"].ToString();
                                
                                double _dr_total = 0;
                                double _cr_total = 0;
                                double _balance_total = 0;

                                _amount_balance_dt = objAccountsBLL.AccountReport(from_date, to_date, int.Parse(dr_3["id"].ToString()));
                                foreach (DataRow dr_4 in _amount_balance_dt.Rows)
                                {
                                    _dr_total += double.Parse(dr_4["debit"].ToString());
                                    _cr_total += double.Parse(dr_4["credit"].ToString());
                                    _balance_total += Math.Abs(Convert.ToDouble(dr_4["balance"].ToString())); //remove negative sign

                                }

                                string[] row03 = { account_name_3, account_name2_3, "", "", _balance_total.ToString() };
                                grid_coa.Rows.Add(row03);

                                //_dr_total_income += Convert.ToDouble(dr_3["debit"].ToString());
                                //_cr_total_income += Convert.ToDouble(dr_3["credit"].ToString());

                            }

                            ///////////
                    }

                }

               
                //////////////// end here

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }

        private void ViewTotalInLastRow()
        {
            //grid_coa.Rows[grid_coa.Rows.Count - 1].Cells["invoice_no"].Style.BackColor = Color.LightGray;
            grid_coa.Rows[grid_coa.Rows.Count-1].Cells["account_name"].Style.BackColor = Color.LightGray;
            grid_coa.Rows[grid_coa.Rows.Count-1].Cells["debit"].Style.BackColor = Color.LightGray;
            grid_coa.Rows[grid_coa.Rows.Count - 1].Cells["credit"].Style.BackColor = Color.LightGray;
            grid_coa.Rows[grid_coa.Rows.Count - 1].Cells["balance"].Style.BackColor = Color.LightGray;
            //grid_coa.Rows[grid_coa.Rows.Count-1].Cells["description"].Style.BackColor = Color.LightGray;
        }

        private void frm_coa_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //when you enter in textbox it will goto next textbox, work like TAB key
                if (e.KeyData == Keys.Enter)
                {
                    SendKeys.Send("{TAB}");
                }

               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            DateTime from_date = txt_from_date.Value.Date;
            DateTime to_date = txt_to_date.Value.Date;
            
            Load_coa(from_date, to_date);
            //ViewTotalInLastRow();
            
        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            DGVPrinter printer = new DGVPrinter();
            printer.Title = "Chart of Accounts";
            printer.SubTitle = string.Format("{0} To {1}", txt_from_date.Value.Date.ToShortDateString(), txt_to_date.Value.Date.ToShortDateString());
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            //printer.PageSettings.Landscape = true;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = false;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = "khybersoft.com";
            printer.FooterSpacing = 15;
            printer.PrintPreviewDataGridView(grid_coa);
        }


    }
}
