using System.Collections.Generic;
using System.Web.Http;

namespace PT.PointOne.WebAPI.Controllers
{
    [RoutePrefix("Stock")]
    public class StockController : ApiController
    {

        [HttpGet]
        [Route("All")]         
        public List<Stock> All()
        {
            return Pub.StockList;
        }
    }
}