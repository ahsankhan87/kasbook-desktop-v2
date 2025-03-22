using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;
using POS.BLL;

namespace pos
{
    class Subscription
    {
        private const string EncryptionKey = "abcdefghijklmnopqrstuvwxyz123456"; // Replace with your own encryption key
        private const string HMACKey = "YourHMACKey"; // Replace with a secure HMAC key
                                                      // Validate a renewal key and extend subscription if valid
        public bool VerifySubscriptionKey(int userId, string renewalKey, out DateTime expirationDate, string systemID)
        {
            expirationDate = DateTime.MinValue;

            string[] parts = renewalKey.Split('_');
            if (parts.Length != 2)
            {
                return false; // Invalid format
            }

            string encryptedData = parts[0];
            string hmac = parts[1];

            if (!VerifyHMAC(encryptedData, hmac, HMACKey))
            {
                return false; // HMAC verification failed
            }

            string decryptedData = DecryptData(encryptedData, EncryptionKey);
            string[] decryptedParts = decryptedData.Split('_');
            if (decryptedParts.Length != 3)
            {
                return false; // Invalid decrypted data
            }

            if (!int.TryParse(decryptedParts[0], out int parsedUserId) || parsedUserId != userId)
            {
                return false; // User ID mismatch
            }

            if (!DateTime.TryParse(decryptedParts[1], out expirationDate))
            {
                return false; // Invalid expiration date
            }

            if (expirationDate < DateTime.Now)
            {
                CompaniesBLL company_obj = new CompaniesBLL();
                company_obj.updateAppLock(userId, true);
                return false;
            }

            ///bypass the system id by commenting the follwing code
            //if (decryptedParts[2].ToString() != systemID)
            //{
            //    MessageBox.Show("This computer system is not registered, please contact vendor", "Computer Registration", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return false; // Check Computer System ID mismatch
            //}
            // Optionally, perform additional checks on the newExpirationDate, e.g., ensure it's in the future

            return true; // Renewal key is valid
        }

        public string GenerateRenewalKey(int userId, DateTime expirationDate, string systemID)
        {
            string data = $"{userId}_{expirationDate:yyyy-MM-dd}_{systemID}";
            string encryptedData = EncryptData(data, EncryptionKey);
            string hmac = ComputeHMAC(encryptedData, HMACKey);
            return $"{encryptedData}_{hmac}";
        }
        // Encrypt data using AES encryption
        private string EncryptData(string data, string key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = new byte[16]; // Use a fixed IV for simplicity

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                byte[] encryptedBytes;
                using (var ms = new System.IO.MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (var sw = new System.IO.StreamWriter(cs))
                        {
                            sw.Write(data);
                        }
                    }
                    encryptedBytes = ms.ToArray();
                }

                return Convert.ToBase64String(encryptedBytes);
            }
        }

        public bool RenewSubscription(int userId, string renewalKey, out DateTime newExpirationDate)
        {
            newExpirationDate = DateTime.MinValue;

            string[] parts = renewalKey.Split('_');
            if (parts.Length != 2)
            {
                return false; // Invalid format
            }

            string encryptedData = parts[0];
            string hmac = parts[1];

            if (!VerifyHMAC(encryptedData, hmac, HMACKey))
            {
                return false; // HMAC verification failed
            }

            string decryptedData = DecryptData(encryptedData, EncryptionKey);
            string[] decryptedParts = decryptedData.Split('_');
            if (decryptedParts.Length != 2)
            {
                return false; // Invalid decrypted data
            }

            if (!int.TryParse(decryptedParts[0], out int parsedUserId) || parsedUserId != userId)
            {
                return false; // User ID mismatch
            }

            if (!DateTime.TryParse(decryptedParts[1], out newExpirationDate))
            {
                return false; // Invalid expiration date
            }

            if (newExpirationDate < DateTime.Now)
            {
                return false;
            }
            // Optionally, perform additional checks on the newExpirationDate, e.g., ensure it's in the future

            return true; // Renewal key is valid
        }

        // Decrypt data using AES encryption
        private string DecryptData(string encryptedData, string key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = new byte[16]; // Use a fixed IV for simplicity

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                byte[] encryptedBytes = Convert.FromBase64String(encryptedData);
                string decryptedData;
                using (var ms = new System.IO.MemoryStream(encryptedBytes))
                {
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (var sr = new System.IO.StreamReader(cs))
                        {
                            decryptedData = sr.ReadToEnd();
                        }
                    }
                }

                return decryptedData;
            }
        }

        // Compute HMAC using SHA256 hash function
        private string ComputeHMAC(string data, string key)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                return Convert.ToBase64String(hash);
            }
        }

        // Verify HMAC
        private bool VerifyHMAC(string data, string hmac, string key)
        {
            string computedHMAC = ComputeHMAC(data, key);
            return computedHMAC == hmac;
        }
        //////////////
        ///

    }
}
