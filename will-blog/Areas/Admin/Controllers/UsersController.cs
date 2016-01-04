using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NHibernate.Linq;
using will_blog.Areas.Admin.ViewModels;
using will_blog.Infrastructure;
using will_blog.Models;

namespace will_blog.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [SelectedTab("users")]
    public class UsersController : Controller
    {
        public ActionResult Index()
        {
            return View(new UsersIndex
            {
                Users = Database.Session.Query<User>().ToList()
            });
        }

        public ActionResult New()
        {
            return View(new UsersNew
            {
                Roles = Database.Session.Query<Role>().Select(role => new RoleCheckbox
                {
                    Id = role.Id,
                    IsChecked = false,
                    Name = role.Name
                }).ToList()
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult New(UsersNew form)
        {
            var user = new User();
            SyncRoles(form.Roles, user.Roles);

            if (Database.Session.Query<User>().Any(u => u.Username == form.Username))
                ModelState.AddModelError("Username", "Username must be unique.");

            if (!ModelState.IsValid)
                return View(form);

            user.Username = form.Username;
            user.Email = form.Email;
            user.SetPassword(form.Password);

            Database.Session.Save(user);

            return RedirectToAction("index");
        }

        public ActionResult Edit(int id)
        {
            var user = Database.Session.Load<User>(id);

            if (user == null)
                return HttpNotFound();

            return View(new UsersEdit
            {
                Username = user.Username,
                Email = user.Email,
                Roles = Database.Session.Query<Role>().Select(role => new RoleCheckbox
                {
                    Id = role.Id,
                    IsChecked = user.Roles.Contains(role),
                    Name = role.Name
                }).ToList()
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(int id, UsersEdit form)
        {
            var user = Database.Session.Load<User>(id);

            if (user == null)
                return HttpNotFound();

            SyncRoles(form.Roles, user.Roles);

            if (Database.Session.Query<User>().Any(u => u.Username == form.Username && u.Id != id))
                ModelState.AddModelError("Username", "Username must be unique.");

            if (!ModelState.IsValid)
                return View(form);

            user.Username = form.Username;
            user.Email = form.Email;
            Database.Session.Update(user);
            return RedirectToAction("index");
        }

        public ActionResult ResetPassword(int id)
        {
            var user = Database.Session.Load<User>(id);

            if (user == null)
                return HttpNotFound();

            return View(new UsersResetPassword
            {
                Username = user.Username
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ResetPassword(int id, UsersResetPassword form)
        {
            var user = Database.Session.Load<User>(id);

            if (user == null)
                return HttpNotFound();

            form.Username = user.Username;

            if (!ModelState.IsValid)
                return View(form);

            user.SetPassword(form.Password);
            Database.Session.Update(user);

            return RedirectToAction("index");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var user = Database.Session.Load<User>(id);

            if (user == null)
                return HttpNotFound();

            Database.Session.Delete(user);
            return RedirectToAction("index");
        }

        private void SyncRoles(IList<RoleCheckbox> checkboxes, ICollection<Role> userRoles)
        {
            var selectedRoles = FetchSelectedRoles(checkboxes);

            foreach (var roleToAdd in selectedRoles.Where(r => !userRoles.Contains(r)))
                userRoles.Add(roleToAdd);

            foreach (var roleToRemove in userRoles.Where(r => !selectedRoles.Contains(r)).ToList())
                userRoles.Remove(roleToRemove);
        }

        private static IReadOnlyCollection<Role> FetchSelectedRoles(IList<RoleCheckbox> checkboxes)
        {
            var selectedRoles = new List<Role>();

            foreach (var role in Database.Session.Query<Role>())
            {
                var checkbox = checkboxes.Single(c => c.Id == role.Id);
                checkbox.Name = role.Name;

                if (checkbox.IsChecked)
                    selectedRoles.Add(role);
            }
            return selectedRoles;
        }
    }
}