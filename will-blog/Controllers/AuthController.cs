using System.Web.Mvc;

namespace will_blog.Controllers
{
    public class AuthController : Controller
    {
        public ActionResult Login()
        {
            return Content("Login!");
        }
    }
}