using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApplicationCore.Models
{
    public class Product 
    {
        [JsonIgnore]
        public Guid Guid { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Sku { get; set; }
        [JsonIgnore]
        public string Currency { get; set; }
        [Required]
        public decimal Amount { get; set; }
    }
}
