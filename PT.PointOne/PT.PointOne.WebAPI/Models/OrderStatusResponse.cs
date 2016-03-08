namespace PT.PointOne.WebAPI.Models
{
    public enum OrderStatus {
        WAITING_FOR_PAYMENT,
        QUEUED,
        READY,
        POURING,
        ERROR,
        COMPLETE
    }

    public class OrderStatusResponse
    {
        public OrderStatus Status
        { get; set; }
        public TapStatus TapStatus { get; set; } 
        public string RequestId { get; set; }
        public bool Locked { get; set; }
        public string Message { get; set; }
    }
}
