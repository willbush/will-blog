using System.Web.Mvc;

namespace will_blog.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class PostsController : Controller
    {
        public ActionResult Index()
        {
            return Content("Admin Posts!!");
        }
    }
}