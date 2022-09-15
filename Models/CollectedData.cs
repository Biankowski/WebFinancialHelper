
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebFinancialHelper.Models
{
    // This is the Database table model
    public class CollectedData
    {
        [Key]
        public int Id { get; set; }
        public string  ResponsibleUsername { get; set; }
        public string PlaceOfPurchase { get; set; }
        public decimal Price { get; set; }
        public string PurchaseDate { get; set; }
        public string PurchaseTime { get; set; }
        public string UploadDate { get; set; }
    }
}