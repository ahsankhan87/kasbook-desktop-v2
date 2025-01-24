using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pos
{
    class RedisCacheHelper
    {
        private static ConnectionMultiplexer _redis;
        private static IDatabase _cache;

        public static void InitializeRedis()
        {
            // Replace with your Redis server endpoint (use localhost if running locally)
            _redis = ConnectionMultiplexer.Connect("localhost:6379");
            _cache = _redis.GetDatabase();
        }

        public static void SetString(string key, string value)
        {
            _cache.StringSet(key, value);
        }

        public static string GetString(string key)
        {
            return _cache.StringGet(key);
        }
        public static IEnumerable<string> GetAllKeys(string pattern)
        {
            var server = _redis.GetServer(_redis.GetEndPoints()[0]);
            return server.Keys(pattern: pattern).Select(k => k.ToString());
        }

        public static void DeleteCache()
        {
            // Retrieve all keys that match "product_*"
            foreach (var key in GetAllKeys("product_*"))
            {
                // Delete the key from Redis
                _cache.KeyDelete(key);
            }

        }
        //public void AddProductToCache(ProductModal product)
        //{
        //    string key = $"product_{product.code}";
        //    string value = $"{product.name}|{product.unit_price}";
        //    RedisCacheHelper.SetString(key, value);

        //}

        //public void CacheProducts()
        //{
        //    try
        //    {
        //        List<ProductModal> products = ProductBLL.GetAllProducts();

        //        foreach (var product in products)
        //        {
        //            string key = $"product_{product.item_number}";
        //            string value = $"{product.name}|{product.unit_price}";

        //            RedisCacheHelper.SetString(key, value);
        //        }

        //        MessageBox.Show("Products cached successfully!");
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //        throw;
        //    }

        //}
    }
}
