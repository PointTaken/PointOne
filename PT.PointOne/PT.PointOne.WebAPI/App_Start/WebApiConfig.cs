using System.Web.Http;

namespace PT.PointOne.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.EnableCors();
            // Web API routes
            config.MapHttpAttributeRoutes();
        }
    }
}
