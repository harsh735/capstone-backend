namespace Authentication1.Models
{
    public class MutualFundsInvestments
    {
        public int MutualFundsID { get; set; }
        public int UserID { get; set; }
        public int InvestorInfoID { get; set; }
        public decimal Amount { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Type { get; set; }
        public DateTime Timeframe { get; set; }
        public string Risk { get; set; }
        public int ExpectedAmount { get; set; }
    }
}
