using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOTHubInterface.Models
{
    public enum OrderStatus { WAITING_FOR_PAYMENT, QUEUED, READY, POURING,  ERROR, COMPLETE  }
    public class OrderStatusResponse
    {
        public OrderStatus Status;
        public TapStatus TapStatus; 
        public string RequestId;
        public bool Locked;
        public string Message; 
    }
}
