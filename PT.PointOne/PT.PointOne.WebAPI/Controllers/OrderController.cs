using PT.PointOne.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using System.Web.Http.Cors;
using PT.PointOne.OfficeAppWeb;

namespace PT.PointOne.WebAPI.Controllers
{
    [RoutePrefix("Order")]
    public class OrderController : ApiController
    {
        public static List<Order> orders = new List<Order>();
        public static double OrderCount
        {
            get
            {
                return orders.Count;
            }
        }
        private bool Locked
        {
            get
            {
                return orders.Any(k => k.Status == OrderStatus.READY || k.Status == OrderStatus.POURING);
            }
        }

        [HttpPost]
        [Route("New")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public OrderStatusResponse New(NewOrderRequest request)
        {
            var x = Request.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(request.OrderId))
                return new OrderStatusResponse { Locked = Locked, RequestId = "", Status = OrderStatus.ERROR, Message = "Order ID missing" };

            var rnd = new Random();
            var productId = rnd.Next(32) + 2; // IDs 2 to 34

            var RequestId = Guid.NewGuid();
            var order = new Order
            {
                OrderId = request.OrderId,
                RequestId = RequestId.ToString(),
                ProductId = productId.ToString(),
                Created = DateTime.Now,
                Poured = null,
                Paid = true,
                Price = double.Parse(request.Price),
                UserId = request.UserId,
                Status = OrderStatus.QUEUED,
                TapStatus = TapStatus.Waiting
            };
            SharePointOnline.AddNewOrder(order);          
            orders.Add(order);
            DistributR.Distribute(BarStatus());
            return new OrderStatusResponse { Locked = Locked, RequestId = RequestId.ToString(), Status = order.Status, Message = "" };
        }

        [HttpPost]
        [Route("Pour")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public OrderStatusResponse Pour(OrderStatusRequest request)
        {
            try
            {
                if (DateTime.Now.Subtract(TapController.LastPing).TotalSeconds > 20)
                    return new OrderStatusResponse { Locked = Locked, Message = "Device not active.", RequestId = request.RequestID, Status = OrderStatus.ERROR };

                if (Locked)
                    return new OrderStatusResponse { Locked = Locked, Message = "", RequestId = request.RequestID, Status = OrderStatus.QUEUED };

                Guid RequestID;
                if (!Guid.TryParse(request.RequestID, out RequestID))
                    return new OrderStatusResponse { Locked = Locked, RequestId = request.RequestID, Status = OrderStatus.ERROR, Message = "Invalid request ID" };
                
                var order = orders.Where(o => o.RequestId == RequestID.ToString() && o.Status == OrderStatus.QUEUED).FirstOrDefault();

                if (order == null)
                    return new OrderStatusResponse { Locked = Locked, RequestId = request.RequestID, Status = OrderStatus.ERROR, Message = "No orders for request id" };

                if (!order.Paid)
                    return new OrderStatusResponse { Locked = Locked, RequestId = request.RequestID, Status = OrderStatus.WAITING_FOR_PAYMENT, Message = "" };

                order.TapStatus = TapStatus.Pour;
                order.Status = OrderStatus.READY;

                return new OrderStatusResponse { Locked = Locked, RequestId = request.RequestID, Status = order.Status, Message = "" };

            }
            catch (Exception ex)
            {
                return new OrderStatusResponse { Locked = true, RequestId = request.RequestID, Status = OrderStatus.ERROR, Message = "Exception occured, " + ex.Message };
            }
        }

        [HttpGet]
        [Route("Status/{requestID}")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public OrderStatusResponse Status(string requestID)
        {
            if (DateTime.Now.Subtract(TapController.LastPing).TotalSeconds > 20)
                return new OrderStatusResponse { Locked = Locked, Message = "Device not active.", RequestId = requestID, Status = OrderStatus.ERROR };

            var order = orders.Where(o => o.RequestId == requestID).FirstOrDefault();
            if (order == null)
                return new OrderStatusResponse { Locked = Locked, Message = "No order for request id", RequestId = requestID, Status = OrderStatus.ERROR };

            return new OrderStatusResponse { Locked = Locked, Message = "", RequestId = requestID, Status = order.Status, TapStatus = order.TapStatus };
        }

        [HttpGet]
        [Route("Test")]
        public void Test()
        {
          //  DistributR.Distribute("Testing12123");
        }

        public class BarStatusResponse
        {
            public string SoldTonight
            {
                get; set; }
            public string SoldLastNight { get; set; }
            public string PatronsActive {
                get; set; }
            public string MoneyEarnt { get; set; }
            public string MoneySpent { get; set; } 
        }
       [HttpGet]
       [Route("BarStatus")]
       public BarStatusResponse BarStatus()
        {
            var resp = new BarStatusResponse();
            resp.SoldTonight = orders.Count.ToString();
            resp.SoldLastNight = 49.ToString(); 
            resp.PatronsActive = new Random().Next(10, 140).ToString();
            // TODO: linq it up
            double x = 0;
            foreach(var order in orders)
            {
                x += order.Price;                 
            }
            resp.MoneyEarnt = x.ToString(); 
            resp.MoneySpent = (orders.Count * 19).ToString();
            return resp;
        }
      
    }
}
