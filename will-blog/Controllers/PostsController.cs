using System.Web.Mvc;

namespace will_blog.Controllers
{
    public class PostsController : Controller
    {
        public ActionResult Index()
        {
            return Content("Hello, World");
        }
    }
}