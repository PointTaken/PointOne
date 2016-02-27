using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PT.PointOne.WebAPI
{
    public class Beer
    {
        public string Title { get; set; }
        public string Colour { get; set; }
        public double Bitterness { get; set; }
        public double Freshness { get; set; }
        public double Alcohol { get; set; }
        public double Out { get; set; }
        public string Brewery { get; set; }
        public string Country { get; set; }

        public int ID { get; set;  }
        public string ImageURL
        {
            get; set;
        }

        public string Score { get; set; }
    }
}