namespace ApplicationCore.Models
{
    public enum OrderStatus
    {
        Created = 0,
        Paid = 1,
        InProcess = 2,
        InTransit = 3,
        Delivered = 4,
        RefundInitiated = 5,
        RefundInHouse = 6,
        RefundProcessed = 7,
        RefundSuccessfull = 8
    }
}
