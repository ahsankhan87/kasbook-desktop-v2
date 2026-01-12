using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pos.Master.Companies.zatca
{
    public static class ZatcaPhase2QrGenerator
    {
        // NOTE:
        // ZATCA TLV is binary:
        // - Tag is a single byte.
        // - Length is binary (not ASCII) and can be 1+ bytes (BER style) when needed.
        // - Value bytes are raw bytes (UTF8 for textual tags, raw bytes for cryptographic/base64 inputs).

        private static void AddTlvHeader(List<byte> buffer, byte tag, int length)
        {
            buffer.Add(tag);

            // Short form
            if (length < 0x80)
            {
                buffer.Add((byte)length);
                return;
            }

            // Long form (support up to 4 bytes)
            var lenBytes = new List<byte>();
            int tmp = length;
            while (tmp > 0)
            {
                lenBytes.Insert(0, (byte)(tmp & 0xFF));
                tmp >>= 8;
            }

            if (lenBytes.Count > 4)
                throw new Exception("TLV length too large.");

            buffer.Add((byte)(0x80 | lenBytes.Count));
            buffer.AddRange(lenBytes);
        }

        private static void AddTlvUtf8(List<byte> buffer, byte tag, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new Exception($"TLV Tag {tag} empty.");

            byte[] valueBytes = Encoding.UTF8.GetBytes(value);
            AddTlvHeader(buffer, tag, valueBytes.Length);
            buffer.AddRange(valueBytes);
        }

        private static void AddTlvBase64Bytes(List<byte> buffer, byte tag, string base64Value)
        {
            if (string.IsNullOrWhiteSpace(base64Value))
                throw new Exception($"TLV Tag {tag} empty.");

            // IMPORTANT:
            // Tags 6-9 are cryptographic fields provided as base64 strings.
            // Their TLV value MUST be the decoded bytes, not UTF8 bytes of the base64 text.
            byte[] valueBytes = Convert.FromBase64String(base64Value);
            AddTlvHeader(buffer, tag, valueBytes.Length);
            buffer.AddRange(valueBytes);
        }

        public static byte[] BuildZatcaQrTlv(
            string sellerName,
            string vatNumber,
            string issueDateTime,
            decimal totalWithVat,
            decimal vatAmount,
            string invoiceHashBase64,
            string ecdsaSignatureBase64,
            string ecdsaPublicKeyBase64,
            string certificateSignatureBase64)
        {
            var buffer = new List<byte>();

            AddTlvUtf8(buffer, 1, sellerName);
            AddTlvUtf8(buffer, 2, vatNumber);
            AddTlvUtf8(buffer, 3, issueDateTime);
            AddTlvUtf8(buffer, 4, totalWithVat.ToString("0.00", CultureInfo.InvariantCulture));
            AddTlvUtf8(buffer, 5, vatAmount.ToString("0.00", CultureInfo.InvariantCulture));

            // Phase 2 tags: value is raw bytes (base64-decoded)
            AddTlvBase64Bytes(buffer, 6, invoiceHashBase64);
            AddTlvBase64Bytes(buffer, 7, ecdsaSignatureBase64);
            AddTlvBase64Bytes(buffer, 8, ecdsaPublicKeyBase64);
            AddTlvBase64Bytes(buffer, 9, certificateSignatureBase64);

            return buffer.ToArray();
        }

        public static string GenerateQrBase64(
            string sellerName,
            string vatNumber,
            string issueDate,        // YYYY-MM-DD
            string issueTime,        // HH:mm:ss
            decimal invoiceTotal,
            decimal vatTotal,
            string invoiceHashBase64,
            string ecdsaSignatureBase64,
            string ecdsaPublicKeyBase64,
            string certificateSignatureBase64)
        {
            // KSA-25 compliant timestamp
            string timestamp = $"{issueDate}T{issueTime}";

            byte[] tlvBytes = BuildZatcaQrTlv(
                sellerName,
                vatNumber,
                timestamp,
                invoiceTotal,
                vatTotal,
                invoiceHashBase64,
                ecdsaSignatureBase64,
                ecdsaPublicKeyBase64,
                certificateSignatureBase64);

            return Convert.ToBase64String(tlvBytes);
        }

        // Decoder below remains as a debug helper for the simple short-form length case.
        // It will not correctly decode long-form lengths.
        public static List<TlvItem> DecodeZatcaQr(string qrBase64)
        {
            byte[] bytes = Convert.FromBase64String(qrBase64);
            List<TlvItem> tlvs = new List<TlvItem>();

            int index = 0;

            while (index < bytes.Length)
            {
                byte tag = bytes[index++];
                int length = bytes[index++];

                byte[] valueBytes = bytes.Skip(index).Take(length).ToArray();
                index += length;

                string value = Encoding.UTF8.GetString(valueBytes);

                tlvs.Add(new TlvItem
                {
                    Tag = tag,
                    Length = length,
                    Value = value
                });
            }

            return tlvs;
        }

        public static void ValidateZatcaQr(List<TlvItem> tlvs)
        {
            // Required tags
            for (int i = 1; i <= 9; i++)
            {
                if (!tlvs.Any(t => t.Tag == i))
                    throw new Exception($"Missing TLV tag {i}");
            }

            // Timestamp validation (KSA-25)
            string timestamp = tlvs.First(t => t.Tag == 3).Value;

            if (!DateTime.TryParseExact(
                timestamp,
                "yyyy-MM-dd'T'HH:mm:ss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out _))
            {
                throw new Exception("Invalid QR timestamp format (KSA-25).");
            }

            // Length safety (only valid for short-form decoder)
            foreach (var tlv in tlvs)
            {
                if (tlv.Length != Encoding.UTF8.GetByteCount(tlv.Value))
                    throw new Exception($"TLV length mismatch for tag {tlv.Tag}");
            }
        }

    }

    public class TlvItem
    {
        public byte Tag { get; set; }
        public int Length { get; set; }
        public string Value { get; set; }
    }
}
