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

            routes.MapRoute(
                name: "TagForRealThisTime",
                url: "tag/{idAndSlug}",
                defaults: new {controller = "Posts", action = "Tag"},
                namespaces: namespaces);

            routes.MapRoute(
                name: "Tag",
                url: "tag/{id}-{slug}",
                defaults: new {controller = "Posts", action = "Tag"},
                namespaces: namespaces);

            routes.MapRoute(
                name: "PostForRealThisTime",
                url: "post/{idAndSlug}",
                defaults: new {controller = "Posts", action = "Show"},
                namespaces: namespaces);

            routes.MapRoute(
                name: "Post",
                url: "post/{id}-{slug}",
                defaults: new {controller = "Posts", action = "Show"},
                namespaces: namespaces);

            routes.MapRoute(
                name: "Home",
                url: "",
                defaults: new {controller = "Posts", action = "Index"},
                namespaces: namespaces);

            routes.MapRoute(
                name: "Login",
                url: "login",
                defaults: new {controller = "Auth", action = "Login"},
                namespaces: namespaces);

            routes.MapRoute(
                name: "Logout",
                url: "logout",
                defaults: new {controller = "Auth", action = "Logout"},
                namespaces: namespaces);
        }
    }
}