using System.Web.Mvc;
using will_blog.Infrastructure;

namespace will_blog.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [SelectedTab("users")]
    public class UsersController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}