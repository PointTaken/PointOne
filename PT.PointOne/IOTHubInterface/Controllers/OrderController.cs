using IOTHubInterface.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;
using System.Security;
using System.Configuration;

namespace IOTHubInterface.Controllers
{
    [RoutePrefix("Order")]
    public class OrderController : ApiController
    {
        public static List<Order> orders = new List<Order>();
        private bool Locked
        {
            get
            {
                return orders.Any(k => k.Status == OrderStatus.READY || k.Status == OrderStatus.POURING);
            }
        }

        [HttpPost]
        [Route("New")]
        public OrderStatusResponse New(NewOrderRequest request)
        {
            var x = Request.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(request.OrderId))
                return new OrderStatusResponse { Locked = Locked, RequestId = "", Status = OrderStatus.ERROR, Message = "Order ID missing" };

            // TODO: Create order entry in SP list 
            using (var ctx = new ClientContext("https://aspc1606.sharepoint.com/sites/PointOneArms/Management"))
            {
                ctx.Credentials = new SharePointOnlineCredentials("hs@aspc1606.onmicrosoft.com", GetPWD());
                ctx.Load(ctx.Web);
                ctx.ExecuteQuery(); 
               var list = ctx.Web.Lists.GetByTitle("Orders");
                ctx.Load(list);
                ctx.ExecuteQuery(); 

                var lic = new ListItemCreationInformation();
                
                var item = list.AddItem(lic);
                
                item["Title"] = "New order received";
                item.Update();
                list.Update();
                ctx.ExecuteQuery();

            }

        

        

        var RequestId = Guid.NewGuid();
        var order = new Order
        {
            OrderId = request.OrderId,
            RequestId = RequestId.ToString(),
            ProductId = "15",
            Created = DateTime.Now,
            Poured = null,
            Paid = true,
            Status = OrderStatus.QUEUED,
            TapStatus = TapStatus.Waiting
        };
        orders.Add(order);
            return new OrderStatusResponse { Locked = Locked, RequestId = RequestId.ToString(), Status = order.Status, Message = "" };
}

        private SecureString GetPWD()
        {
            var pwd = ConfigurationManager.AppSettings["Password"];
            var ss = new SecureString();
                foreach (var c in pwd)
                ss.AppendChar(c);

            return ss; 
        }

        [HttpPost]
        [Route("Pour")]
        public OrderStatusResponse Pour(OrderStatusRequest request)
        {
            try {
                if (DateTime.Now.Subtract(TapController.LastPing).TotalSeconds > 20)
                    return new OrderStatusResponse { Locked = Locked, Message = "Device not active.", RequestId = request.RequestID, Status = OrderStatus.ERROR };

                if (Locked)
                    return new OrderStatusResponse { Locked = Locked, Message = "", RequestId = request.RequestID, Status = OrderStatus.QUEUED }; 

                Guid RequestID;
                if(!Guid.TryParse(request.RequestID, out RequestID))                
                    return new OrderStatusResponse { Locked = Locked, RequestId = request.RequestID, Status = OrderStatus.ERROR, Message = "Invalid request ID" };
                
                var order = orders.Where(o => o.RequestId == RequestID.ToString() && o.Status == OrderStatus.QUEUED).FirstOrDefault();

                if (order == null)
                    return new OrderStatusResponse { Locked = Locked, RequestId = request.RequestID, Status = OrderStatus.ERROR, Message = "No orders for request id" };

                if (!order.Paid)
                    return new OrderStatusResponse { Locked = Locked, RequestId = request.RequestID, Status = OrderStatus.WAITING_FOR_PAYMENT, Message = "" };
                                
                order.TapStatus = TapStatus.Pour;
                order.Status = OrderStatus.READY;

                return new OrderStatusResponse { Locked = Locked, RequestId = request.RequestID, Status = order.Status , Message = "" };

            }
            catch(Exception ex)
            {
                return new OrderStatusResponse { Locked = true, RequestId = request.RequestID, Status = OrderStatus.ERROR, Message = "Exception occured, " + ex.Message };
            }
        }

        [HttpGet]
        [Route("Status/{requestID}")]
        public OrderStatusResponse Status(string requestID)
        {
            if (DateTime.Now.Subtract(TapController.LastPing).TotalSeconds > 20)
                return new OrderStatusResponse { Locked = Locked, Message = "Device not active.", RequestId = requestID, Status = OrderStatus.ERROR };

            var order = orders.Where(o => o.RequestId == requestID).FirstOrDefault();
            if (order == null)
                return new OrderStatusResponse { Locked = Locked, Message = "No order for request id", RequestId = requestID, Status = OrderStatus.ERROR };

            return new OrderStatusResponse { Locked = Locked, Message = "", RequestId = requestID, Status = order.Status, TapStatus = order.TapStatus };
        }
    }
}
