using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApplicationCore.Models
{
    public class Order
    {
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public Guid? guid { get; set; }
        [JsonIgnore]
        public DateOnly? CreatedDate { get; set; }
        [Required]
        public int  Count { get; set; }
        [Required]
        public decimal TotalAmount { get; set; }
        [Required]
        public Guid ProductGuid { get; set; }
        [Required]
        public string ProductName { get; set; }
        public OrderStatus Status { get; set; }
        public string? TrackingId { get; set; }
        [Required]
        public Guid UserGuid { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string EmailAddress { get; set; }
    }
}
