using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IOTHubInterface.Models
{
    public class Beer
    {
        public string Title { get; set; }
        public string Colour { get; set; }
        public double Bitterness { get; set; }
        public double Freshness { get; set;  }
        public double Alcohol { get; set;  }

        public double Out { get; set; }
        public int Brewery { get; set; }
        public string Country { get; set;  }

    }
}