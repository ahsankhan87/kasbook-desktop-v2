using System;

namespace POS.Core
{
    /// <summary>
    /// Represents a single row in the pos_settings table.
    /// </summary>
    public class AccountingSettingModel
    {
        public string Key          { get; set; }
        public string Value        { get; set; }

        /// <summary>STRING | INT | DECIMAL | BOOL | DATE | ACCOUNT_ID</summary>
        public string SettingType  { get; set; }

        public string Description  { get; set; }
        public bool   IsEncrypted  { get; set; }
        public string Category     { get; set; }
        public bool   IsRequired   { get; set; }
        public int?   ModifiedBy   { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
