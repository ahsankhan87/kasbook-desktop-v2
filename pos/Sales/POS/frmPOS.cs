// Designer/frmPOS.cs
using POS.BLL;
using POS.DLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Forms;

namespace pos
{
    public partial class frmPOS : Form
    {
        private ProductBLL productBLL;
        private SalesBLL salesBLL;
        private DataTable cartItems;
        private int currentBranchId = 1; // Default branch
        private int currentUserId = 1;   // Current user

        public frmPOS()
        {
            InitializeComponent();
            InitializeComponents();
            LoadPaymentMethods();
        }

        private void InitializeComponents()
        {
            productBLL = new ProductBLL();
            salesBLL = new SalesBLL();

            // Initialize cart
            InitializeCart();
            ConfigureDataGrids();

            // Event handlers
            txtSearch.TextChanged += TxtSearch_TextChanged;
            txtBarcode.KeyPress += TxtBarcode_KeyPress;
            dgvProducts.CellDoubleClick += DgvProducts_CellDoubleClick;
            dgvCart.CellEndEdit += DgvCart_CellEndEdit;
            btnCheckout.Click += BtnCheckout_Click;
            btnRemoveItem.Click += BtnRemoveItem_Click;
            btnClearCart.Click += BtnClearCart_Click;
        }

        private void InitializeCart()
        {
            cartItems = new DataTable();
            cartItems.Columns.Add("ProductCode", typeof(string));
            cartItems.Columns.Add("ProductName", typeof(string));
            cartItems.Columns.Add("Quantity", typeof(decimal));
            cartItems.Columns.Add("UnitPrice", typeof(decimal));
            cartItems.Columns.Add("DiscountValue", typeof(decimal));
            cartItems.Columns.Add("DiscountPercent", typeof(decimal));
            cartItems.Columns.Add("Total", typeof(decimal));
            cartItems.Columns.Add("CostPrice", typeof(decimal));
            cartItems.Columns.Add("TaxId", typeof(int));
            cartItems.Columns.Add("TaxRate", typeof(decimal));
            cartItems.Columns.Add("UnitId", typeof(int));
            cartItems.Columns.Add("PacketQty", typeof(decimal));
            cartItems.Columns.Add("ItemNumber", typeof(string));

            dgvCart.DataSource = cartItems;
        }

        //private void ConfigureDataGrids()
        //{
        //    // Configure Products Grid
        //    dgvProducts.AutoGenerateColumns = false;
        //    dgvProducts.Columns.Clear();

        //    dgvProducts.Columns.Add(new DataGridViewTextBoxColumn()
        //    {
        //        DataPropertyName = "code",
        //        HeaderText = "Code",
        //        Name = "colCode",
        //        Width = 80,
        //        ReadOnly = true
        //    });

        //    dgvProducts.Columns.Add(new DataGridViewTextBoxColumn()
        //    {
        //        DataPropertyName = "name",
        //        HeaderText = "Product Name",
        //        Name = "colName",
        //        Width = 200,
        //        ReadOnly = true
        //    });

        //    dgvProducts.Columns.Add(new DataGridViewTextBoxColumn()
        //    {
        //        DataPropertyName = "unit_price",
        //        HeaderText = "Price",
        //        Name = "colPrice",
        //        Width = 80,
        //        ReadOnly = true
        //    });

        //    dgvProducts.Columns.Add(new DataGridViewTextBoxColumn()
        //    {
        //        DataPropertyName = "qty",
        //        HeaderText = "Stock",
        //        Name = "colStock",
        //        Width = 60,
        //        ReadOnly = true
        //    });

        //    // Configure Cart Grid
        //    dgvCart.AutoGenerateColumns = false;
        //    dgvCart.Columns.Clear();

        //    dgvCart.Columns.Add(new DataGridViewTextBoxColumn()
        //    {
        //        DataPropertyName = "ProductName",
        //        HeaderText = "Product",
        //        Name = "colCartName",
        //        ReadOnly = true,
        //        Width = 150
        //    });

        //    DataGridViewTextBoxColumn quantityColumn = new DataGridViewTextBoxColumn()
        //    {
        //        DataPropertyName = "Quantity",
        //        HeaderText = "Qty",
        //        Name = "colCartQty",
        //        Width = 60,
        //        ReadOnly = false
        //    };
        //    dgvCart.Columns.Add(quantityColumn);

        //    dgvCart.Columns.Add(new DataGridViewTextBoxColumn()
        //    {
        //        DataPropertyName = "UnitPrice",
        //        HeaderText = "Price",
        //        Name = "colCartPrice",
        //        ReadOnly = true,
        //        Width = 80
        //    });

        //    DataGridViewTextBoxColumn discountColumn = new DataGridViewTextBoxColumn()
        //    {
        //        DataPropertyName = "DiscountValue",
        //        HeaderText = "Discount",
        //        Name = "colCartDiscount",
        //        Width = 80,
        //        ReadOnly = false
        //    };
        //    dgvCart.Columns.Add(discountColumn);

        //    dgvCart.Columns.Add(new DataGridViewTextBoxColumn()
        //    {
        //        DataPropertyName = "Total",
        //        HeaderText = "Total",
        //        Name = "colCartTotal",
        //        ReadOnly = true,
        //        Width = 80
        //    });
        //}
        private void ConfigureDataGrids()
        {
            // Configure Products Grid
            dgvProducts.AutoGenerateColumns = false;
            dgvProducts.Columns.Clear();

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "code",
                HeaderText = "Code",
                Name = "colCode",
                Width = 80,
                ReadOnly = true
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "name",
                HeaderText = "Product Name",
                Name = "colName",
                Width = 200,
                ReadOnly = true
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "unit_price",
                HeaderText = "Price",
                Name = "colPrice",
                Width = 80,
                ReadOnly = true
            });

            // Add stock column
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "current_stock",
                HeaderText = "Stock",
                Name = "colStock",
                Width = 60,
                ReadOnly = true
            });

            // Configure Cart Grid - Add stock validation
            dgvCart.AutoGenerateColumns = false;
            dgvCart.Columns.Clear();

            dgvCart.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "ProductName",
                HeaderText = "Product",
                Name = "colCartName",
                ReadOnly = true,
                Width = 150
            });

            DataGridViewTextBoxColumn quantityColumn = new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Quantity",
                HeaderText = "Qty",
                Name = "colCartQty",
                Width = 60,
                ReadOnly = false
            };
            dgvCart.Columns.Add(quantityColumn);

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "unit_price",
                HeaderText = "Price",
                Name = "colPrice",
                Width = 80,
                ReadOnly = true
            });

            // Add stock column to products grid
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "current_stock",
                HeaderText = "Stock",
                Name = "colStock",
                Width = 60,
                ReadOnly = true
            });
        }
        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch.Text.Length > 2)
            {
                try
                {
                    DataTable products = productBLL.SearchProducts(txtSearch.Text, currentBranchId);
                    dgvProducts.DataSource = products;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error searching products: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (string.IsNullOrEmpty(txtSearch.Text))
            {
                dgvProducts.DataSource = null;
            }
        }

        private void TxtBarcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                string barcode = txtBarcode.Text.Trim();
                if (!string.IsNullOrEmpty(barcode))
                {
                    try
                    {
                        DataTable product = productBLL.GetProductByBarcode(barcode, currentBranchId);
                        if (product.Rows.Count > 0)
                        {
                            AddProductToCart(product.Rows[0]);
                            txtBarcode.Clear();
                        }
                        else
                        {
                            MessageBox.Show("Product not found!", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error scanning barcode: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                e.Handled = true;
            }
        }

        private void DgvProducts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvProducts.DataSource != null)
            {
                try
                {
                    DataRowView row = (DataRowView)dgvProducts.Rows[e.RowIndex].DataBoundItem;
                    AddProductToCart(row.Row);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error adding product to cart: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void AddProductToCart(DataRow productRow)
        {
            string productCode = productRow["code"].ToString();
            string productName = productRow["name"].ToString();
            decimal unitPrice = Convert.ToDecimal(productRow["unit_price"]);
            decimal costPrice = productRow["cost_price"] != DBNull.Value ?
                Convert.ToDecimal(productRow["cost_price"]) : 0m;
            decimal currentStock = productRow["current_stock"] != DBNull.Value ?
                Convert.ToDecimal(productRow["current_stock"]) : 0m;

            // Check stock availability
            if (currentStock <= 0)
            {
                MessageBox.Show($"Product '{productName}' is out of stock!", "Stock Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check if product already in cart
            DataRow[] existingRows = cartItems.Select($"ProductCode = '{productCode.Replace("'", "''")}'");
            if (existingRows.Length > 0)
            {
                decimal newQuantity = Convert.ToDecimal(existingRows[0]["Quantity"]) + 1;

                // Validate stock for increased quantity
                if (newQuantity > currentStock)
                {
                    MessageBox.Show($"Cannot add more. Only {currentStock} units available for '{productName}'",
                        "Stock Limit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                existingRows[0]["Quantity"] = newQuantity;
                existingRows[0]["Total"] = newQuantity * unitPrice - Convert.ToDecimal(existingRows[0]["DiscountValue"]);
            }
            else
            {
                DataRow newRow = cartItems.NewRow();
                newRow["ProductCode"] = productCode;
                newRow["ProductName"] = productName;
                newRow["Quantity"] = 1m;
                newRow["UnitPrice"] = unitPrice;
                newRow["DiscountValue"] = 0m;
                newRow["DiscountPercent"] = 0m;
                newRow["Total"] = unitPrice;
                newRow["CostPrice"] = costPrice;
                newRow["TaxId"] = productRow["tax_id"] ?? DBNull.Value;
                newRow["TaxRate"] = 0m;
                newRow["UnitId"] = productRow["unit_id"] ?? DBNull.Value;
                newRow["PacketQty"] = productRow["packet_qty"] ?? 1m;
                newRow["ItemNumber"] = productRow["item_number"] ?? DBNull.Value;

                cartItems.Rows.Add(newRow);
            }

            UpdateTotals();
            dgvCart.Refresh();
        }

        //private void AddProductToCart(DataRow productRow)
        //{
        //    string productCode = productRow["code"].ToString();
        //    string productName = productRow["name"].ToString();
        //    decimal unitPrice = Convert.ToDecimal(productRow["unit_price"]);
        //    decimal costPrice = productRow["cost_price"] != DBNull.Value ?
        //        Convert.ToDecimal(productRow["cost_price"]) : 0m;

        //    // Check if product already in cart
        //    DataRow[] existingRows = cartItems.Select($"ProductCode = '{productCode.Replace("'", "''")}'");
        //    if (existingRows.Length > 0)
        //    {
        //        existingRows[0]["Quantity"] = Convert.ToDecimal(existingRows[0]["Quantity"]) + 1;
        //        existingRows[0]["Total"] = Convert.ToDecimal(existingRows[0]["Quantity"]) * unitPrice - Convert.ToDecimal(existingRows[0]["DiscountValue"]);
        //    }
        //    else
        //    {
        //        DataRow newRow = cartItems.NewRow();
        //        newRow["ProductCode"] = productCode;
        //        newRow["ProductName"] = productName;
        //        newRow["Quantity"] = 1m;
        //        newRow["UnitPrice"] = unitPrice;
        //        newRow["DiscountValue"] = 0m;
        //        newRow["DiscountPercent"] = 0m;
        //        newRow["Total"] = unitPrice;
        //        newRow["CostPrice"] = costPrice;
        //        newRow["TaxId"] = productRow["tax_id"] ?? DBNull.Value;
        //        newRow["TaxRate"] = 0m; // Calculate based on tax_id if needed
        //        newRow["UnitId"] = productRow["unit_id"] ?? DBNull.Value;
        //        newRow["PacketQty"] = productRow["packet_qty"] ?? 1m;
        //        newRow["ItemNumber"] = productRow["item_number"] ?? DBNull.Value;

        //        cartItems.Rows.Add(newRow);
        //    }

        //    UpdateTotals();
        //    dgvCart.Refresh();
        //}

        private void DgvCart_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    DataGridViewRow row = dgvCart.Rows[e.RowIndex];
                    decimal quantity = Convert.ToDecimal(row.Cells["colCartQty"].Value);
                    decimal unitPrice = Convert.ToDecimal(row.Cells["colCartPrice"].Value);
                    decimal discount = Convert.ToDecimal(row.Cells["colCartDiscount"].Value);

                    // Update the total
                    row.Cells["colCartTotal"].Value = (quantity * unitPrice) - discount;

                    // Update the underlying DataTable
                    DataRowView dataRowView = (DataRowView)row.DataBoundItem;
                    dataRowView["Quantity"] = quantity;
                    dataRowView["DiscountValue"] = discount;
                    dataRowView["Total"] = (quantity * unitPrice) - discount;

                    UpdateTotals();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating quantity: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void UpdateTotals()
        {
            decimal subtotal = 0;
            foreach (DataRow row in cartItems.Rows)
            {
                subtotal += Convert.ToDecimal(row["Total"]);
            }

            decimal tax = salesBLL.CalculateTax(cartItems);
            decimal total = subtotal + tax;

            lblSubtotal.Text = $"Subtotal: {subtotal:C}";
            lblTax.Text = $"Tax: {tax:C}";
            lblTotal.Text = $"Total: {total:C}";
        }

        private void BtnRemoveItem_Click(object sender, EventArgs e)
        {
            if (dgvCart.CurrentRow != null)
            {
                try
                {
                    cartItems.Rows.RemoveAt(dgvCart.CurrentRow.Index);
                    UpdateTotals();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error removing item: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select an item to remove.", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnClearCart_Click(object sender, EventArgs e)
        {
            if (cartItems.Rows.Count > 0)
            {
                if (MessageBox.Show("Clear all items from cart?", "Confirm Clear Cart",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cartItems.Rows.Clear();
                    UpdateTotals();
                }
            }
            else
            {
                MessageBox.Show("Cart is already empty.", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnCheckout_Click(object sender, EventArgs e)
        {
            if (cartItems.Rows.Count == 0)
            {
                MessageBox.Show("Cart is empty! Please add items before checkout.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbPaymentMethod.SelectedItem == null)
            {
                MessageBox.Show("Please select a payment method!", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                decimal totalAmount = salesBLL.CalculateTotal(cartItems);
                decimal taxAmount = salesBLL.CalculateTax(cartItems);
                decimal discount = CalculateTotalDiscount();

                int paymentMethodId = ((KeyValuePair<int, string>)cmbPaymentMethod.SelectedItem).Key;
                string customerName = txtCustomer.Text.Trim();

                // Confirm checkout
                DialogResult result = MessageBox.Show(
                    $"Total Amount: {totalAmount + taxAmount:C}\n" +
                    $"Proceed with checkout?",
                    "Confirm Checkout",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    int saleId = salesBLL.ProcessSale(cartItems, totalAmount, taxAmount, discount,
                        0, currentUserId, currentBranchId, paymentMethodId, customerName);

                    MessageBox.Show($"Sale completed successfully!\nSale ID: {saleId}", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Reset form
                    cartItems.Rows.Clear();
                    txtCustomer.Clear();
                    UpdateTotals();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing sale: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private decimal CalculateTotalDiscount()
        {
            decimal totalDiscount = 0;
            foreach (DataRow row in cartItems.Rows)
            {
                totalDiscount += Convert.ToDecimal(row["DiscountValue"]);
            }
            return totalDiscount;
        }

        private void LoadPaymentMethods()
        {
            try
            {
                // This should come from database, hardcoded for example
                cmbPaymentMethod.DisplayMember = "Value";
                cmbPaymentMethod.ValueMember = "Key";

                cmbPaymentMethod.Items.Add(new KeyValuePair<int, string>(1, "Cash"));
                cmbPaymentMethod.Items.Add(new KeyValuePair<int, string>(2, "Credit Card"));
                cmbPaymentMethod.Items.Add(new KeyValuePair<int, string>(3, "Debit Card"));
                cmbPaymentMethod.Items.Add(new KeyValuePair<int, string>(4, "Mobile Payment"));

                cmbPaymentMethod.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading payment methods: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    // Helper class for KeyValuePair display in ComboBox
    public class KeyValuePair<TKey, TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }

        public KeyValuePair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}