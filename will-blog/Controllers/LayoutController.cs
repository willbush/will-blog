using System.Linq;
using System.Web.Mvc;
using NHibernate.Linq;
using will_blog.Models;
using will_blog.ViewModel;

namespace will_blog.Controllers
{
    public class LayoutController : Controller
    {
        [ChildActionOnly]
        public ActionResult Sidebar()
        {
            return View(new LayoutSidebar
            {
                IsLoggedIn = Auth.CurrentUser != null,
                Username = Auth.CurrentUser != null ? Auth.CurrentUser.Username : "",
                IsAdmin = User.IsInRole("admin"),
                Tags = Database.Session.Query<Tag>().Select(tag => new
                {
                    tag.Id,
                    tag.Name,
                    tag.Slug,
                    PostCount = tag.Posts.Count
                }).Where(t => t.PostCount > 0)
                    .OrderByDescending(p => p.PostCount)
                    .Select(tag => new SidebarTag(tag.Id, tag.Name, tag.Slug, tag.PostCount))
                    .ToList()
            });
        }
    }
}