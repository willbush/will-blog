using System.Web.Mvc;
using System.Web.Routing;
using will_blog.Controllers;

namespace will_blog
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            string[] namespaces = {typeof (PostsController).Namespace};

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Home", "", new {controller = "Posts", action = "Index"}, namespaces);
            routes.MapRoute("Login", "login", new {controller = "Auth", action = "Login"}, namespaces);
        }
    }
}