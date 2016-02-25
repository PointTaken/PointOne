using System.Web;
using System.Web.Mvc;

namespace PT.PointOne.H2H.CRMWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
