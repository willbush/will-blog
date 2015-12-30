using System.Web.Mvc;

namespace will_blog.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        public ActionResult Index()
        {
            return Content("Users!");
        }
    }
}