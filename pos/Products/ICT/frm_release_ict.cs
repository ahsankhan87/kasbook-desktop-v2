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

namespace pos.Products.ICT
{
    public partial class frm_release_ict : Form
    {
        public frm_release_ict()
        {
            InitializeComponent();
        }

        private void frm_release_ict_Load(object sender, EventArgs e)
        {
            load_all_ict_grid();
        }

        public void load_all_ict_grid()
        {
            try
            {
                ICTBLL objSalesBLL = new ICTBLL();
                grid_ict.DataSource = null;

                //bind data in data grid view  
                grid_ict.AutoGenerateColumns = false;

                //String keyword = "id,name,date_created";
                // String table = "pos_all_sales";
                grid_ict.DataSource = objSalesBLL.GetAll_ict_releases();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }

        private void btn_transfer_Click(object sender, EventArgs e)
        {
            
        }
        private void transfer_qty()
        {
            try
            {
                DialogResult result = MessageBox.Show("Are you sure you want to transfer quantity", "Transfer Quantity", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    ICTModal modal = new ICTModal();
                    SalesBLL objSalesBLL = new SalesBLL();

                    List<ICTModal> ict_list = new List<ICTModal> { };

                    for (int i = 0; i < grid_ict.Rows.Count; i++)
                    {
                        if (grid_ict.Rows[i].Cells["chk"].Value != null)
                        {
                            if (Convert.ToBoolean(grid_ict.Rows[i].Cells["chk"].Value))
                            {
                                double qty = 0;
                                if (grid_ict.Rows[i].Cells["qty_released"].Value != null)
                                {
                                    if (!string.IsNullOrEmpty(grid_ict.Rows[i].Cells["qty_released"].Value.ToString()))
                                    {
                                        qty = double.Parse(grid_ict.Rows[i].Cells["qty_released"].Value.ToString());
                                    }
                                }
                                ///// Added sales detail in to List
                                ict_list.Add(new ICTModal
                                {

                                    quantity = qty,
                                    item_code = grid_ict.Rows[i].Cells["item_code"].Value.ToString(),
                                    item_number = grid_ict.Rows[i].Cells["item_number"].Value.ToString(),

                                    destination_branch_id = Convert.ToInt16(grid_ict.Rows[i].Cells["destination_branch_id"].Value.ToString()),
                                    source_branch_id = Convert.ToInt16(grid_ict.Rows[i].Cells["source_branch_id"].Value.ToString()),
                                    //status = Convert.ToBoolean(grid_ict.Rows[i].Cells["chk"].Value),
                                    transfer_date = DateTime.Now,
                                });
                                //////////////
                            }
                        }

                    }

                    int sale_id = 0; // objSalesBLL.save_ict_transfer(ict_list);

                    if (sale_id > 0)
                    {
                        MessageBox.Show("ICT quantity transferred successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        load_all_ict_grid();
                    }
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }

        private void frm_release_ict_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F3)
            {
                btn_release_qty.PerformClick();
            }
            
            if (e.KeyCode == Keys.F5)
            {
                btn_refresh.PerformClick();
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            load_all_ict_grid();
        }

        private void btn_release_qty_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Are you sure you want to transfer quantity", "Transfer Quantity", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    ICTBLL objSalesBLL = new ICTBLL();

                    List<ICTModal> ict_list = new List<ICTModal> { };

                    for (int i = 0; i < grid_ict.Rows.Count; i++)
                    {
                        if (grid_ict.Rows[i].Cells["chk"].Value != null)
                        {
                            
                                if (Convert.ToBoolean(grid_ict.Rows[i].Cells["chk"].Value) && Convert.ToDouble(grid_ict.Rows[i].Cells["qty_released"].Value.ToString()) > 0)
                                {
                                    double qty = 0;
                                    if (grid_ict.Rows[i].Cells["qty_released"].Value != null)
                                    {
                                        if (!string.IsNullOrEmpty(grid_ict.Rows[i].Cells["qty_released"].Value.ToString()))
                                        {
                                            qty = double.Parse(grid_ict.Rows[i].Cells["qty_released"].Value.ToString());
                                        }
                                    }

                                    if (qty > 0)// transfer qty should be greater than zero
                                    {
                                        ///// Added sales detail in to List
                                        ict_list.Add(new ICTModal
                                        {
                                            id = int.Parse(grid_ict.Rows[i].Cells["id"].Value.ToString()),
                                            quantity = qty,
                                            item_code = grid_ict.Rows[i].Cells["item_code"].Value.ToString(),
                                            item_number = grid_ict.Rows[i].Cells["item_number"].Value.ToString(),
                                            destination_branch_id = Convert.ToInt16(grid_ict.Rows[i].Cells["destination_branch_id"].Value.ToString()),
                                            source_branch_id = Convert.ToInt16(grid_ict.Rows[i].Cells["source_branch_id"].Value.ToString()),
                                            //status = Convert.ToBoolean(grid_ict.Rows[i].Cells["chk"].Value),
                                            release_date = DateTime.Now,
                                        });
                                        //////////////
                                    }

                                }
                            
                        }

                    }

                    if (ict_list.Count > 0)
                    {
                        int sale_id = objSalesBLL.save_ict_release_qty(ict_list);

                        if (sale_id > 0)
                        {
                            MessageBox.Show("ICT quantity released successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            load_all_ict_grid();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Transaction not saved", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
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
