using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using System.Web.Helpers;
using System.Threading;
using System.Security.Principal;
using shoppingCart.BusinessLogic;

namespace _2Hours_Ver2
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            AntiForgeryConfig.SuppressIdentityHeuristicChecks = true;

        }

        protected void Session_Start()
        {
            Session session = new Session();
            var id = session.SessionID;

            // remove expired sessions
            session.RemoveExpired(id);
            // check for duplicates
            session.ValidateSession(id);
            // add session to visit tbl
            session.CastSession(id);
        }

        protected void Session_End()
        {
            var elapsed = this.Session.SessionID;
            Session session = new Session();
            // remove all products from productvisit tbl
            session.RemoveItems(elapsed);

        }

        void Application_PostAuthenticateRequest()
        {
            if (User.Identity.IsAuthenticated)
            {
                var name = User.Identity.Name; // Get current user name.

                mergedEntities db = new mergedEntities();
                var user = db.AspNetUsers.Where(u => u.UserName == name).FirstOrDefault();
                IQueryable<string> roleQuery = from u in db.AspNetUsers
                                               from r in u.AspNetRoles
                                               where u.UserName == Context.User.Identity.Name
                                               select r.Name;
                string[] roles = roleQuery.ToArray();

                HttpContext.Current.User = Thread.CurrentPrincipal =
                                           new GenericPrincipal(User.Identity, roles);
            }
        }
    }



}