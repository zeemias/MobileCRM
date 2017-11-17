using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MobileCRM
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            var httpException = exception as HttpException;
            if (httpException != null)
            {
                if (httpException.GetHttpCode() == 404)
                {
                    ShowErrorPage("Error404.cshtml", exception);
                    throw new Exception("Ошибка 404");
                    return;
                }
            }
            throw new Exception("Ошибка попалась");
            //ShowErrorPage("Error.cshtml", exception);
        }

        private void ShowErrorPage(string v, Exception exception)
        {
            
        }
    }
}
