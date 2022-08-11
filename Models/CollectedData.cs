using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebFinancialHelper.Models
{
    public class CollectedData
    {
        [Key]
        public int Id { get; set; }
        public DateTime Day { get; set; } = DateTime.Now;
        public decimal Value { get; set; }
        public string? Place { get; set; }
        
    }
}
