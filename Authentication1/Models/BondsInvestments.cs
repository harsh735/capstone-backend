using System.ComponentModel.DataAnnotations;

public class BondsInvestments
{

    [Key]
    public int? BondsID { get; set; }
    public int? UserID { get; set; }
    public int? InvestorInfoID { get; set; }
    public decimal? Amount { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public string? Type { get; set; }
    public DateTime? MaturityDate { get; set; }
    public string? Risk { get; set; }
    public string? Series { get; set; }
    public string? BondName { get; set; }
    public string? Credit { get; set; }
    public float? CouponValue { get; set; }
    public float? ExpectedReturn { get; set; }
}
