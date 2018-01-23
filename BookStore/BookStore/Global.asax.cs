using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.Entity;
using BookStore.DAL;

namespace BookStore
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DbInitializer();
        }

        private void DbInitializer()
        {
            var bookContext = new BookContext();
            Database.SetInitializer<BookContext>(new BookInitializer());
            //Database.SetInitializer(new BookInitializer());
            //bookContext.Database.Initialize(true);
        }
    }
}
