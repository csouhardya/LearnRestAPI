namespace ApplicationCore.Models
{
    public class Product 
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
    }
}
