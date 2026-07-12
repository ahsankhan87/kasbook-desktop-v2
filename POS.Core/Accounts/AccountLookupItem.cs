namespace POS.Core
{
    public class AccountLookupItem
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string DisplayText { get; set; }
        public string AccountType { get; set; }
        public string NormalBalance { get; set; }
        public bool IsBank { get; set; }
        public bool IsCash { get; set; }
        public bool IsActive { get; set; }
    }
}