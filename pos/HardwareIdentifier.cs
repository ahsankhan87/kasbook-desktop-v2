using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace pos
{
    class HardwareIdentifier
    {
        public string GetUniqueHardwareId()
        {
            string hardwareId = string.Empty;

            try
            {
                // Retrieve system's hardware components
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
                ManagementObjectCollection collection = searcher.Get();

                foreach (ManagementObject obj in collection)
                {
                    hardwareId += obj["SerialNumber"].ToString(); // Append serial number of the motherboard
                }

                searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
                collection = searcher.Get();

                foreach (ManagementObject obj in collection)
                {
                    hardwareId += obj["ProcessorId"].ToString(); // Append processor ID
                }

                // You can include other hardware components (e.g., HDD serial number, BIOS version) for more robust identification

                // Hash the hardware identifier to improve security
                hardwareId = ComputeSHA256Hash(hardwareId);
            }
            catch (Exception ex)
            {
                // Handle exception
                return ex.Message;
            }

            return hardwareId;
        }

        private string ComputeSHA256Hash(string input)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
