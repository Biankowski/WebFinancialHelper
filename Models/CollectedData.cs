
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebFinancialHelper.Models
{
    public class CollectedData
    {
        [Key]
        public int Id { get; set; }
        public string? PlaceOfPurchase { get; set; }
<<<<<<< HEAD
        public decimal? Price { get; set; }
        public string? PurchaseDate { get; set; }
        public string? PurchaseTime { get; set; }
=======
        public decimal? Price { get; set; } 
        public DateTime PurchaseDate { get; set; }
        public string? PurchaseTime { get; set; } 
>>>>>>> 9a06fdb68958f87989c17946dd79a52162d72441
        public string? UploadDate { get; set; }
    }
}