using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOTHubInterface.Models
{    
    public class DeviceStatusResponse
    {           
        public TapStatus TapStatus;                 
        public string RequestId;        
        public string Message; 
    }
}
