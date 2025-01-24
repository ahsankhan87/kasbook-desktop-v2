﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POS.BLL;
using POS.Core;

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
            txt_search.Focus();
            this.ActiveControl = txt_search;
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
            
            if (txt_first_name.Text != string.Empty && txt_last_name.Text != string.Empty)
            {
                SupplierModal info = new SupplierModal();
                info.first_name = txt_first_name.Text;
                info.last_name = txt_last_name.Text;
                info.email = txt_email.Text;
                info.vat_no = txt_vatno.Text;
                info.address = txt_address.Text;
                info.contact_no = txt_contact_no.Text;
                info.vat_with_status = chk_vat_status.Checked;
                    
                SupplierBLL objBLL = new SupplierBLL();
                    
                
                int result = objBLL.Insert(info);
                if (result > 0)
                {
                    MessageBox.Show("Record updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear_all();

                }
                else
                {
                    MessageBox.Show("Record not saved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                    
                if(mainForm != null)
                {
                    mainForm.load_Suppliers_grid();

                }

                
            }
            else
            {
                MessageBox.Show("Please enter value in field", "Invalid Data", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
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
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txt_id.Text))
            {
                MessageBox.Show("Record not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (txt_first_name.Text != string.Empty && txt_last_name.Text != string.Empty)
            {
                SupplierModal info = new SupplierModal();
                info.first_name = txt_first_name.Text;
                info.last_name = txt_last_name.Text;
                info.email = txt_email.Text;
                info.vat_no = txt_vatno.Text;
                info.address = txt_address.Text;
                info.contact_no = txt_contact_no.Text;
                info.vat_with_status = chk_vat_status.Checked;

                SupplierBLL objBLL = new SupplierBLL();

                info.id = int.Parse(txt_id.Text);

                int result = objBLL.Update(info);
                if (result > 0)
                {
                    MessageBox.Show("Record updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear_all();

                }
                else
                {
                    MessageBox.Show("Record not saved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
                if (mainForm != null)
                {
                    mainForm.load_Suppliers_grid();

                }

            }
            else
            {
                MessageBox.Show("Please enter value in field", "Invalid Data", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
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
                chk_vat_status.Checked = bool.Parse(myProductView["vat_status"].ToString());
                
            }
            lbl_customer_name.Visible = true;
            lbl_customer_name.Text = txt_first_name.Text + ' ' + txt_last_name.Text;
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            frm_search_suppliers search_obj = new frm_search_suppliers(this, txt_search.Text);
            search_obj.ShowDialog();
        }

        public void load_transactions_grid(int supplier_id)
        {
            try
            {
                grid_supplier_transactions.DataSource = null;

                //bind data in data grid view  
                GeneralBLL objBLL = new GeneralBLL();
                grid_supplier_transactions.AutoGenerateColumns = false;

                String keyword = "id,invoice_no,debit,credit,(debit-credit) AS balance,description,entry_date,account_id,account_name";
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
                newRow[4] = (_dr_total - _cr_total);
                dt.Rows.InsertAt(newRow, dt.Rows.Count);

                grid_supplier_transactions.DataSource = dt;
                CustomizeDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            string id = txt_id.Text;
           
                if (!string.IsNullOrWhiteSpace(id))
                {
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show("Are you sure you want to delete", "Delete Record", buttons, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        SupplierBLL objBLL = new SupplierBLL();

                    try
                    {
                        int deleteResult = objBLL.Delete(int.Parse(id));
                        //if (deleteResult > 0)
                        //{
                            MessageBox.Show("Record deleted successfully.", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            clear_all();
                        //}
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                    else
                    {
                        MessageBox.Show("Please select a record to delete.", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    }
                }
            
        }
        private void btn_refresh_Click(object sender, EventArgs e)
        {
            string supplier_id = txt_id.Text;

            if (supplier_id != "")
            {
                load_detail(int.Parse(supplier_id));
                
            }

        }

        private void btn_trans_refresh_Click(object sender, EventArgs e)
        {
            string supplier_id = txt_id.Text;
            if (supplier_id != "")
            {
                load_transactions_grid(int.Parse(supplier_id));
                
            }
        }

        private void btn_payment_Click(object sender, EventArgs e)
        {
            string supplier_id = txt_id.Text;
            if (supplier_id != "")
            {
                frm_supplier_payment obj = new frm_supplier_payment(this, int.Parse(supplier_id));
                obj.ShowDialog();
                CustomizeDataGridView();
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


    }
}
