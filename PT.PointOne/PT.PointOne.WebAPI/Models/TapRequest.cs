using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PT.PointOne.WebAPI.Models
{
    
    public class TapRequest
    {
        public string RequestId { get; set; }
        public TapStatus Status { get; set; }
    }
}
