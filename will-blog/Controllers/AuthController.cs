using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using NHibernate.Linq;
using will_blog.Models;
using will_blog.ViewModel;

namespace will_blog.Controllers
{
    public class AuthController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToRoute("home");
        }

        [HttpPost]
        public ActionResult Login(AuthLogin form, string returnUrl)
        {
            var user = Database.Session.Query<User>().FirstOrDefault(u => u.Username == form.Username);

            // makes it very hard for an attacker to determine if a user is in the database through 
            // simply attempting to login and looking at login POST delay
            if (user == null)
                Models.User.CauseDelayWithFakeHash();

            if (user == null || !user.PasswordIsVerified(form.Password))
                ModelState.AddModelError("Username", "Username or password is incorrect.");

            if (!ModelState.IsValid)
                return View(form);

            FormsAuthentication.SetAuthCookie(form.Username, true);

            if (!string.IsNullOrWhiteSpace(returnUrl))
                return Redirect(returnUrl);

            return RedirectToRoute("home");
        }
    }
}