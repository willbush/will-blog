using System.Web.Mvc;
using will_blog.ViewModel;

namespace will_blog.Controllers
{
    public class AuthController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(AuthLogin form)
        {
            if (!ModelState.IsValid)
                return View(form);

            return Content("Form is valid");
        }
    }
}