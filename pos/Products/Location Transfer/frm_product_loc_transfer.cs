using POS.BLL;
using POS.Core;
using POS.DLL;
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
using pos.UI;

namespace pos
{
    public partial class frm_product_loc_transfer : Form
    {

        public int inventory_acc_id = 0;
        public int item_variance_acc_id = 0;
        
        public frm_product_loc_transfer()
        {
            InitializeComponent();
            
        }
        
        private void frm_product_loc_transfer_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            StyleForm();
            GetMAXInvoiceNo();
            get_from_locations_dropdownlist();
            get_to_locations_dropdownlist();
        }

        private void StyleForm()
        {
            AppTheme.ApplyListFormStyleLightHeader(null, null, panel1, grid_search_products, id);
        }
        
        private void btn_update_Click(object sender, EventArgs e)
        {
            
            try
            {
                if (cmb_from_locations.SelectedValue.ToString() == cmb_to_locations.SelectedValue.ToString())
                {
                    MessageBox.Show("From and To Location shall not be same", "Inventory Location Transfer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Int32 qresult = 0;
                DialogResult result = MessageBox.Show("Are you sure you want to update", "Inventory Location Transfer", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    if (grid_search_products.Rows.Count > 0)
                    {
                        ProductModal info = new ProductModal();
                        ProductBLL productBLLObj = new ProductBLL();
                        
                        for (int i = 0; i < grid_search_products.Rows.Count; i++)
                        {
                            if (grid_search_products.Rows[i].Cells["id"].Value != null && grid_search_products.Rows[i].Cells["transfer_qty"].Value != "")
                            {

                                info.invoice_no = txt_ref_no.Text;
                                info.code = grid_search_products.Rows[i].Cells["code"].Value.ToString();
                                info.qty = (grid_search_products.Rows[i].Cells["transfer_qty"].Value != "" ? double.Parse(grid_search_products.Rows[i].Cells["transfer_qty"].Value.ToString()) : 0);
                                info.location_code = cmb_to_locations.SelectedValue.ToString();
                                info.from_location_code = cmb_from_locations.SelectedValue.ToString();
                                info.description = "Product Location Transfer";

                                qresult = Convert.ToInt32(productBLLObj.UpdateProductLocationTransfer(info));

                               
                            }
                            
                        }
                        
                        if (qresult > 0)
                        {
                            MessageBox.Show("Transfer processed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            GetMAXInvoiceNo();
                            grid_search_products.DataSource = null;
                            grid_search_products.Refresh();
                            txt_search.Focus();
                        }
                        else
                        {
                            MessageBox.Show("Record not saved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("no record found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void GetMAXInvoiceNo()
        {
            ProductBLL objBLL = new ProductBLL();
            txt_ref_no.Text = objBLL.GetMaxLocationTransferInvoiceNo();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                if(txt_search.Text != string.Empty)
                {
                    bool by_code = rb_by_code.Checked;
                    bool by_name = rb_by_name.Checked;
                    
                    //ProductBLL objBLL = new ProductBLL();
                    //grid_search_products.AutoGenerateColumns = false;

                    String condition = txt_search.Text.Trim();
                    //grid_search_products.DataSource = objBLL.SearchRecord(condition, by_code, by_name);
                    
                    GeneralBLL objBLL = new GeneralBLL();
                    grid_search_products.AutoGenerateColumns = false;

                    String keyword = "P.id, P.code,P.name,P.qty,P.qty as transfer_qty";
                    String table = "pos_products_location_view P";
                    
                    if (by_code)
                    {
                        table += " WHERE (P.code LIKE '%" + condition + "%' OR replace(code,'-','') LIKE '%" + condition + "%')";
                        
                    }
                    else if (by_name)
                    {
                        table += " WHERE P.name LIKE '%" + condition + "%'";
                        
                    }
                    table += " AND P.loc_code = '" + cmb_from_locations.SelectedValue.ToString() + "'";
                    grid_search_products.DataSource = objBLL.GetRecord(keyword, table);
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

        private void frm_product_loc_transfer_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //when you enter in textbox it will goto next textbox, work like TAB key
                //if (e.KeyData == Keys.Enter)
                //{
                //    SendKeys.Send("{TAB}");
                //}

                if (e.KeyCode == Keys.F3)
                {
                    btn_update.PerformClick();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        public void get_from_locations_dropdownlist()
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "code,name";
            string table = "pos_locations";

            DataTable locations = generalBLL_obj.GetRecord(keyword, table);
            //DataRow emptyRow = locations.NewRow();
            //emptyRow[0] = 0;              // Set Column Value
            //emptyRow[1] = "All";              // Set Column Value
            //locations.Rows.InsertAt(emptyRow, 0);

            
            cmb_from_locations.DisplayMember = "name";
            cmb_from_locations.ValueMember = "code";
            cmb_from_locations.DataSource = locations;

        }


        public void get_to_locations_dropdownlist()
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "code,name";
            string table = "pos_locations";

            DataTable locations = generalBLL_obj.GetRecord(keyword, table);
            //DataRow emptyRow = locations.NewRow();
            //emptyRow[0] = 0;              // Set Column Value
            //emptyRow[1] = "All";              // Set Column Value
            //locations.Rows.InsertAt(emptyRow, 0);

            cmb_to_locations.DisplayMember = "name";
            cmb_to_locations.ValueMember = "code";
            cmb_to_locations.DataSource = locations;

        }

        private void cmb_from_locations_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(cmb_from_locations.SelectedValue != null)
                {
                    grid_search_products.DataSource = null;
                    //bind data in data grid view  
                    GeneralBLL objBLL = new GeneralBLL();
                    grid_search_products.AutoGenerateColumns = false;

                    //String keyword = "P.id,P.code,P.name,(select sum(PS.qty) from  pos_product_stocks PS where PS.id=P.id AND PS.loc_code = '" + cmb_from_locations.SelectedValue.ToString() + "') as qty";
                    String keyword = "P.id, P.code,P.name,P.qty,P.qty as transfer_qty";
                    String table = "pos_products_location_view P where P.loc_code = '" + cmb_from_locations.SelectedValue.ToString() + "' ";
                    //String table = "pos_products AS P";
                    grid_search_products.DataSource = objBLL.GetRecord(keyword, table);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
            
        }

    }
}
