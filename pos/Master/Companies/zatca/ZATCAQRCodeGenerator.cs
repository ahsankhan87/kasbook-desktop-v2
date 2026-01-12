using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace pos.Master.Companies.zatca
{
    public class ZATCAQRCodeGenerator
    {
        public string GeneratePhase2QRCodeWithAllTags(
        DataRow invoice,
        string sellerName,
        string sellerVAT,
        decimal totalWithVat,
        decimal vatAmount,
        DateTime issueDateTime,
        string invoiceHash,
        string ecdsaSignature,
        string ecdsaPublicKeyThumbprint,  // Changed to thumbprint
        string zatcaCertificateThumbprint) // Changed to thumbprint)
        {
            // Phase 2 requires LOCAL KSA time format: YYYY-MM-DDTHH:MM:SS
            string timestamp = issueDateTime.ToString("yyyy-MM-ddTHH:mm:ss");

            // Create TLV encoded data
            List<byte> tlvData = new List<byte>();

            // Tag 1: Seller's Name
            AddTLVField(tlvData, 1, sellerName.Truncate(1000));

            // Tag 2: Seller's VAT Number
            string vatPadded = sellerVAT.PadLeft(15, '0');
            AddTLVField(tlvData, 2, vatPadded);

            // Tag 3: Invoice DateTime (yyyy-MM-ddTHH:mm:ss)
            AddTLVField(tlvData, 3, timestamp);

            // Tag 4: Invoice Total with VAT
            AddTLVField(tlvData, 4, totalWithVat.ToString("F2", CultureInfo.InvariantCulture));

            // Tag 5: VAT Amount
            AddTLVField(tlvData, 5, vatAmount.ToString("F2", CultureInfo.InvariantCulture));

            // Tag 6: Hash of XML invoice
            AddTLVField(tlvData, 6, invoiceHash);

            // Tag 7: ECDSA signature
            AddTLVField(tlvData, 7, ecdsaSignature);

            // Tag 8: SHA-256 thumbprint of ECDSA public key (Hex string, 64 chars)
            AddTLVField(tlvData, 8, ecdsaPublicKeyThumbprint);

            // Tag 9: For simplified invoices only - SHA-256 thumbprint of ZATCA's CA certificate
            if (!string.IsNullOrEmpty(zatcaCertificateThumbprint))
            {
                AddTLVField(tlvData, 9, zatcaCertificateThumbprint);
            }

            return Convert.ToBase64String(tlvData.ToArray());
        }
        // Generate SHA-256 thumbprint from certificate
        public static string GenerateCertificateThumbprint(string base64Certificate)
        {
            if (string.IsNullOrEmpty(base64Certificate))
                return string.Empty;

            try
            {
                // Remove any whitespace
                string cleanCert = Regex.Replace(base64Certificate, @"\s+", "");

                // Convert to bytes
                byte[] certBytes = Convert.FromBase64String(cleanCert);

                // Compute SHA-256 hash
                using (var sha256 = System.Security.Cryptography.SHA256.Create())
                {
                    byte[] hashBytes = sha256.ComputeHash(certBytes);

                    // Convert to hex string (lowercase, no separators)
                    return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating thumbprint: {ex.Message}");
                return string.Empty;
            }
        }

        public Dictionary<int, string> DecodeQRCode(string base64QRData)
        {
            var result = new Dictionary<int, string>();

            try
            {
                byte[] tlvBytes = Convert.FromBase64String(base64QRData);
                int index = 0;

                while (index < tlvBytes.Length)
                {
                    byte tag = tlvBytes[index++];
                    byte length = tlvBytes[index++];

                    if (index + length > tlvBytes.Length)
                        break;

                    string value = Encoding.UTF8.GetString(tlvBytes, index, length);
                    result[tag] = value;
                    index += length;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error decoding QR: {ex.Message}");
            }

            return result;
        }

        public void ValidateQRCodeStructure(string base64QRData)
        {
            var tags = DecodeQRCode(base64QRData);

            Console.WriteLine("=== QR Code Structure Validation ===");
            Console.WriteLine($"Total tags found: {tags.Count}");

            foreach (var tag in tags.OrderBy(t => t.Key))
            {
                Console.WriteLine($"Tag {tag.Key}: Length={tag.Value.Length}, Value={tag.Value}");
            }

            // Check required tags
            int[] requiredTags = { 1, 2, 3, 4, 5, 6, 7, 8 };
            foreach (int tag in requiredTags)
            {
                if (!tags.ContainsKey(tag))
                {
                    Console.WriteLine($"ERROR: Missing required Tag {tag}");
                }
            }

            // Validate Tag 3 timestamp
            if (tags.ContainsKey(3))
            {
                string timestamp = tags[3];
                bool isValid = Regex.IsMatch(timestamp, @"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}$");
                Console.WriteLine($"Tag 3 timestamp format valid: {isValid}");
            }
        }

        private void AddTLVField(List<byte> data, int tag, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException($"Value for Tag {tag} cannot be null or empty");
            }

            data.Add((byte)tag);
            byte[] valueBytes = Encoding.UTF8.GetBytes(value);

            if (valueBytes.Length > 255)
            {
                throw new ArgumentException($"Value for Tag {tag} is too long: {valueBytes.Length} bytes");
            }

            data.Add((byte)valueBytes.Length);
            data.AddRange(valueBytes);
        }

        public string GeneratePhase2QRCodeWithHash(DataRow invoice, string sellerName, string sellerVAT,
                                              decimal totalWithVat, decimal vatAmount,
                                              DateTime issueDateTime, string invoiceHash)
        {
            // Phase 2 requires LOCAL KSA time format: YYYY-MM-DDTHH:MM:SS
            string timestamp = issueDateTime.ToString("yyyy-MM-ddTHH:mm:ss");

            // Create TLV encoded data
            List<byte> tlvData = new List<byte>();

            // Tag 1: Seller's Name (max 1000 chars)
            AddTLVField(tlvData, 1, sellerName.Truncate(1000));

            // Tag 2: Seller's VAT Number (15 digits for KSA)
            string vatPadded = sellerVAT.PadLeft(15, '0');
            AddTLVField(tlvData, 2, vatPadded);

            // Tag 3: Invoice DateTime (MUST match XML IssueDate + IssueTime)
            AddTLVField(tlvData, 3, timestamp);

            // Tag 4: Invoice Total with VAT
            AddTLVField(tlvData, 4, totalWithVat.ToString("F2", CultureInfo.InvariantCulture));

            // Tag 5: VAT Amount
            AddTLVField(tlvData, 5, vatAmount.ToString("F2", CultureInfo.InvariantCulture));

            // Tag 6: Cryptographic Stamp (Hash) - REQUIRED for Phase 2
            AddTLVField(tlvData, 6, invoiceHash);

            return Convert.ToBase64String(tlvData.ToArray());
        }

        public string UpdateQRCodeWithHash(string base64QRData, string invoiceHash)
        {
            // Decode existing TLV
            byte[] tlvBytes = Convert.FromBase64String(base64QRData);
            List<byte> tlvData = new List<byte>(tlvBytes);

            // Add Tag 6: Cryptographic Stamp (Hash)
            AddTLVField(tlvData, 6, invoiceHash);

            return Convert.ToBase64String(tlvData.ToArray());
        }

       

        public bool ValidateQRCodeTimestamp(string base64QRData)
        {
            try
            {
                byte[] tlvBytes = Convert.FromBase64String(base64QRData);
                int index = 0;

                while (index < tlvBytes.Length)
                {
                    byte tag = tlvBytes[index++];
                    byte length = tlvBytes[index++];

                    if (tag == 3) // Timestamp tag
                    {
                        string timestamp = Encoding.UTF8.GetString(tlvBytes, index, length);

                        // Check format: YYYY-MM-DDTHH:MM:SS (no 'Z')
                        bool isValid = Regex.IsMatch(timestamp, @"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}$");

                        if (!isValid)
                        {
                            // Also check UTC format as fallback
                            isValid = Regex.IsMatch(timestamp, @"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}Z$");
                        }

                        return isValid;
                    }

                    index += length;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public string GetTimestampFromQRCode(string base64QRData)
        {
            try
            {
                byte[] tlvBytes = Convert.FromBase64String(base64QRData);
                int index = 0;

                while (index < tlvBytes.Length)
                {
                    byte tag = tlvBytes[index++];
                    byte length = tlvBytes[index++];

                    if (tag == 3) // Timestamp tag
                    {
                        return Encoding.UTF8.GetString(tlvBytes, index, length);
                    }

                    index += length;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
    }

    public static class StringExtensions
    {
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
    }
}
