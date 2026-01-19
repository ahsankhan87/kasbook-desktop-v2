using DGVPrinterHelper;
using POS.BLL;
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
    public partial class frm_warehouse_report : Form
    {
               
        public frm_warehouse_report()
        {
            InitializeComponent();
            
        }

        private void warehouse_report_Load(object sender, EventArgs e)
        {
            cmb_item_type.SelectedIndex = 0;
            get_brands_dropdownlist();
            get_categories_dropdownlist();
            get_locations_dropdownlist();
            get_units_dropdownlist();
            
        }

        public void load_products()
        {
            //txt_product_code.Text = _product_name;
        }

        
        private async void btn_search_Click(object sender, EventArgs e)
        {
            using (pos.UI.Busy.BusyScope.Show(this, "Loading warehouse report..."))
            {
                try
                {
                    string[] arr_brands = new string[lb_brands.SelectedItems.Count];
                    string[] arr_categories = new string[lb_category.SelectedItems.Count];
                    string[] arr_locations = new string[lb_locations.SelectedItems.Count];

                    int i = 0;
                    foreach (var item in lb_brands.SelectedItems)
                    {
                        arr_brands[i] = ((DataRowView)item)["code"].ToString();
                        i++;
                    }

                    int j = 0;
                    foreach (var item in lb_category.SelectedItems)
                    {
                        arr_categories[j] = ((DataRowView)item)["code"].ToString();
                        j++;
                    }

                    int k = 0;
                    foreach (var item in lb_locations.SelectedItems)
                    {
                        arr_locations[k] = ((DataRowView)item)["code"].ToString();
                        k++;
                    }

                    int unit_id = Convert.ToInt16(cmb_units.SelectedValue);
                    string item_type = cmb_item_type.SelectedItem.ToString();
                    bool qty_onhand = chk_qty_on_hand.Checked;

                    grid_sales_report.AutoGenerateColumns = false;

                    // Run DB/report work off the UI thread
                    DataTable accounts_dt = await Task.Run(() =>
                    {
                        WarehouseReportBLL sale_report_obj = new WarehouseReportBLL();
                        return sale_report_obj.WarehouseReport(arr_categories, arr_brands, arr_locations, unit_id, item_type, qty_onhand);
                    });

                    double _quantity_sold_total = 0;
                    double _unit_price_total = 0;
                    double _cost_price_total = 0;
                    double _total = 0;

                    foreach (DataRow dr in accounts_dt.Rows)
                    {
                        _quantity_sold_total += (dr["qty"].ToString() != "" ? Convert.ToDouble(dr["qty"].ToString()) : 0);
                        _cost_price_total += (dr["cost_price"].ToString() != "" ? Convert.ToDouble(dr["cost_price"].ToString()) : 0);
                        _unit_price_total += (dr["unit_price"].ToString() != "" ? Convert.ToDouble(dr["unit_price"].ToString()) : 0);
                        _total += Convert.ToDouble(dr["total_cost"].ToString());
                    }

                    DataRow newRow = accounts_dt.NewRow();
                    newRow[5] = "Total";
                    newRow[2] = _quantity_sold_total;
                    newRow[4] = _unit_price_total;
                    newRow[3] = _cost_price_total;
                    newRow[11] = _total;
                    accounts_dt.Rows.InsertAt(newRow, accounts_dt.Rows.Count);

                    grid_sales_report.DataSource = accounts_dt;
                    CustomizeDataGridView();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
            }
        }
        private void CustomizeDataGridView()
        {
            // Get the last row in the DataGridView
            DataGridViewRow lastRow = grid_sales_report.Rows[grid_sales_report.Rows.Count - 1];

            // Loop through all cells in the row
            foreach (DataGridViewCell cell in lastRow.Cells)
            {
                DataGridViewCellStyle style = new DataGridViewCellStyle(cell.Style);

                // Set the font to bold
                style.Font = new Font(grid_sales_report.Font, FontStyle.Bold);

                // Set the background color
                style.BackColor = Color.LightGray;

                // Apply the style to the current cell
                cell.Style = style;
            }

        }

        private void btn_search_products_Click(object sender, EventArgs e)
        {
            frm_searchProducts obj = new frm_searchProducts();
            obj.ShowDialog();
        }

        
        private void frm_warehouse_report_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //when you enter in textbox it will goto next textbox, work like TAB key
                if (e.KeyData == Keys.Enter)
                {
                    SendKeys.Send("{TAB}");
                }

                if (e.KeyCode == Keys.F3)
                {
                    btn_search.PerformClick();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        public void get_brands_dropdownlist()
        {
            BrandsBLL brandsBLL = new BrandsBLL();
            DataTable brands = brandsBLL.GetAll();

            //DataRow emptyRow = brands.NewRow();
            //emptyRow[0] = 0;              // Set Column Value
            //emptyRow[1] = "All";              // Set Column Value
            //brands.Rows.InsertAt(emptyRow, 0);

            lb_brands.DataSource = brands;
            lb_brands.DisplayMember = "name";
            lb_brands.ValueMember = "code";
            
        }

        public void get_locations_dropdownlist()
        {
            LocationsBLL locationsBLL = new LocationsBLL();

            DataTable locations = locationsBLL.GetAll();
            //DataRow emptyRow = locations.NewRow();
            //emptyRow[0] = 0;              // Set Column Value
            //emptyRow[1] = "All";              // Set Column Value
            //locations.Rows.InsertAt(emptyRow, 0);

            lb_locations.DisplayMember = "name";
            lb_locations.ValueMember = "code";
            lb_locations.DataSource = locations;
            
        }

        public void get_units_dropdownlist()
        {
            UnitsBLL unitsBLL = new UnitsBLL();

            DataTable units = unitsBLL.GetAll(); 
            DataRow emptyRow = units.NewRow();
            emptyRow[0] = 0;              // Set Column Value
            emptyRow[3] = "All";              // Set Column Value

            units.Rows.InsertAt(emptyRow, 0);

            cmb_units.DataSource = units;
            cmb_units.DisplayMember = "name";
            cmb_units.ValueMember = "id";
            
        }

        public void get_categories_dropdownlist()
        {
            CategoriesBLL categoriesBLL = new CategoriesBLL();

            DataTable categories = categoriesBLL.GetAll();
            //DataRow emptyRow = categories.NewRow();
            //emptyRow[0] = 0;              // Set Column Value
            //emptyRow[1] = "All";              // Set Column Value
            //categories.Rows.InsertAt(emptyRow, 0);

            lb_category.DisplayMember = "name";
            lb_category.ValueMember = "code";
            lb_category.DataSource = categories;
            
        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            DGVPrinter printer = new DGVPrinter();
            printer.Title = "Warehouse Report";
            printer.SubTitle = string.Format("Date: {0}", DateTime.Now.Date);
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = "kasbook app";
            printer.FooterSpacing = 15;
            printer.PrintPreviewDataGridView(grid_sales_report);
            //printer.PrintDataGridView(grid_sales_report);
        }


      

    }
}
