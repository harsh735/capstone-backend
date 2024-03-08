using System.ComponentModel.DataAnnotations;

namespace Authentication1.Models
{
    public class UserBankDetails
    {
        [Key]
        public int UserID { get; set; }
        public string PanNumber { get; set; }
        public byte[]? PanFile { get; set; }
        public string BankAccNumber { get; set; }
        public string AccHolderName { get; set; }
        public string BankName { get; set; }
        public string IFSCCode { get; set; }
    }
}
