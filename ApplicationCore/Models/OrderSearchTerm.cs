using System.Text.Json.Serialization;

namespace ApplicationCore.Models
{
    public class OrderSearchTerm
    {
        //Guid? orderGuid, string? Username, DateOnly? orderDate, OrderStatus? status, string? TrackingId, Guid? ProductGuid
        [JsonPropertyName("order guid")]
        public Guid? OrderGuid { get; set; }
        [JsonPropertyName("username")]
        public string? Username { get; set; }
        [JsonPropertyName("order date")]
        public DateOnly? OrderDate { get; set; }
        [JsonPropertyName("order status")]
        public OrderStatus? Status { get; set; }
        [JsonPropertyName("tracking id")]
        public string? TrackingId { get; set; }
        [JsonPropertyName("email address")]
        public string? EmailAddress { get; set; }
        [JsonPropertyName("product guid")]
        public Guid? ProductGuid { get; set; }
        [JsonPropertyName("total amount")]
        public decimal? TotalAmount { get; set; }
        [JsonPropertyName("sort by")]
        public string? SortBy { get; set; }
        [JsonPropertyName("sort order")]
        public string? SortOrder { get; set; }
        public int PageSize { get; set; } = 10; // default value
        public int PageNumber { get; set; } = 1; // default value

    }
}
