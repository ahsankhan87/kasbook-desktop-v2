namespace POS.Core
{
    public class CurrencyModal
    {
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string symbol { get; set; }
        public decimal exchange_rate { get; set; }
        public bool is_active { get; set; }
        public int branch_id { get; set; }
        public int user_id { get; set; }
        public string date_created { get; set; }
        public string date_updated { get; set; }
    }
}
