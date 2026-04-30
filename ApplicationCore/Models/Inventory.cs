namespace ApplicationCore.Models
{
    public class Inventory
    {
        public Guid ProductGuid { get; set; }
        public int? CountToSubtract { get; set; } // for kafka to subtract
        public int? AbsoluteCount { get; set; } // for admin to update 
    }
}
