namespace PT.PointOne.WebAPI.Models
{
    public class NewOrderRequest
    {
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public string Price { get; set; }
        public string Product { get; set; }
    }
}
