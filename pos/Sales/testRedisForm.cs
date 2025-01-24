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

namespace pos.Sales
{
    public partial class testRedisForm : Form
    {
        public testRedisForm()
        {
            InitializeComponent();
        }

        private void testRedisForm_Load(object sender, EventArgs e)
        {
            //LoadProductsFromCache();
        }


        private void BindProductsToDataGridView(List<ProductModal> products)
        {
            if (products == null || products.Count == 0)
            {
                MessageBox.Show("No products found.");
                dataGridViewProducts.DataSource = null;
                return;
            }
            dataGridViewProducts.AutoGenerateColumns = false;
            dataGridViewProducts.DataSource = products;
            CustomizeDataGridView();
        }

        private void CustomizeDataGridView()
        {
            dataGridViewProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewProducts.Columns["Price"].DefaultCellStyle.Format = "C2";
            dataGridViewProducts.Columns["ProductID"].Width = 50;
            dataGridViewProducts.Columns["ProductName"].Width = 200;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text;

            // Search products using the full-text search method
            //SearchProducts(Convert.ToInt32(searchTerm));
            SearchProductsInCache(searchTerm);
            //List<ProductModal> products = SearchProducts(Convert.ToInt32(searchTerm));

            // Bind the results to the DataGridView
            //BindProductsToDataGridView(products);
        }
        public List<ProductModal> SearchProductsInCache(string searchTerm)
        {
            List<ProductModal> products = new List<ProductModal>();

            foreach (var key in RedisCacheHelper.GetAllKeys("product_*"))
            {
                string cachedProduct = RedisCacheHelper.GetString(key);

                if (!string.IsNullOrEmpty(cachedProduct))
                {
                    string[] productDetails = cachedProduct.Split('|');
                    string productName = productDetails[0];
                    double price = double.Parse(productDetails[1]);
                    //string address = productDetails[3];  // Assuming you store the address in the cache

                    if (key.Contains(searchTerm) ||
                        productName.Contains(searchTerm))
                        //|| address.Contains(searchTerm))
                    {
                        products.Add(new ProductModal
                        {
                            id = int.Parse(key.Replace("product_", "")),
                            name= productName,
                            unit_price = price,
                            //name = address
                        });
                    }
                }
            }

            return products;
        }

        public void SearchProducts(int productId)
        {
            string cacheKey = $"product_{productId}";
            
            // Retrieve cached data
            if (!string.IsNullOrEmpty(cacheKey))
            {
                // Product found in cache
                string cachedProduct = RedisCacheHelper.GetString(cacheKey);

                MessageBox.Show($"Retrieved from cache: {cachedProduct}");

            }


        }

        public List<ProductModal> GetAllCachedProducts()
        {
            List<ProductModal> products = new List<ProductModal>();

            // Assuming the cache keys are in the format "product_{ProductID}"
            foreach (var key in RedisCacheHelper.GetAllKeys("product_*"))
            {
                string cachedProduct = RedisCacheHelper.GetString(key);

                if (!string.IsNullOrEmpty(cachedProduct))
                {
                    string[] productDetails = cachedProduct.Split('|');
                    products.Add(new ProductModal
                    {
                        code = key.Replace("product_", ""), // Extract ProductID from the key
                        name = productDetails[0],
                        unit_price = double.Parse(productDetails[1])
                    });
                }
            }

            return products;
        }
        private void LoadProductsFromCache()
        {
            // Get all cached products
            List<ProductModal> products = GetAllCachedProducts();

            // Bind the products to the DataGridView
            BindProductsToDataGridView(products);
        }
        
    }
}
