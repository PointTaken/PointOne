using IOTHubInterface.Models;
using PT.PointOne.WebAPI.Models;
using System;
using System.Linq;
using System.Web.Http;

namespace PT.PointOne.WebAPI.Controllers
{
    public class TapController : ApiController
    {
        public static DateTime LastPing;

        [HttpGet]
        [Route("Tap/Pour")]
        public DeviceStatusResponse Pour()
        {
            LastPing = DateTime.Now;
            var order = OrderController.orders.OrderByDescending(o => o.Created).FirstOrDefault(o => o.Status == OrderStatus.READY);
            if (order == null)
                return new DeviceStatusResponse { RequestId = null, TapStatus = TapStatus.Waiting };

            return new DeviceStatusResponse { RequestId = order.RequestId, TapStatus = TapStatus.Pour };
        }

        [HttpPost]
        [Route("Tap/Pouring")]
        public DeviceStatusResponse Pouring(TapRequest request)
        {
            LastPing = DateTime.Now;
            var order = OrderController.orders.FirstOrDefault(o => o.RequestId == request.RequestId);
            if (order == null) // Todo: Handle what happens if this is null.. it shouldn't be. 
                return new DeviceStatusResponse { RequestId = request.RequestId, TapStatus = TapStatus.Error, Message = "NOORDER" };

            order.Status = OrderStatus.POURING;
            order.TapStatus = TapStatus.Pouring;

            return new DeviceStatusResponse { RequestId = request.RequestId, TapStatus = order.TapStatus };
        }

        [HttpPost]
        [Route("Tap/Poured")]
        public DeviceStatusResponse Poured(TapRequest request)
        {
            LastPing = DateTime.Now;
            var order = OrderController.orders.FirstOrDefault(o => o.RequestId == request.RequestId);
            if (order == null) // Todo: Handle what happens if this is null.. it shouldn't be. 
                return new DeviceStatusResponse { RequestId = request.RequestId, TapStatus = TapStatus.Error, Message = "NOORDER" };

            order.Status = OrderStatus.COMPLETE;
            order.TapStatus = TapStatus.Poured;
            order.Poured = DateTime.Now;

            // TODO: Update order in sharepoint list. 
            SharePointOnline.UpdateOrder(OrderController.orders.Where(k => k.RequestId == request.RequestId).FirstOrDefault());

            return new DeviceStatusResponse { RequestId = request.RequestId, TapStatus = order.TapStatus };
        }
    }
}
