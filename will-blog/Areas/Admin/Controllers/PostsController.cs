using System;
using System.Linq;
using System.Web.Mvc;
using NHibernate.Linq;
using will_blog.Areas.Admin.ViewModels;
using will_blog.Infrastructure;
using will_blog.Models;

namespace will_blog.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [SelectedTab("posts")]
    public class PostsController : Controller
    {
        private const int PostsPerPage = 5;

        public ActionResult Index(int page = 1)
        {
            var totalPostCount = Database.Session.Query<Post>().Count();
            var currentPostPage = Database.Session.Query<Post>()
                .OrderByDescending(c => c.CreatedAt)
                .Skip((page - 1) * PostsPerPage)
                .Take(PostsPerPage)
                .ToList();

            return View(new PostsIndex
            {
                Posts = new PagedData<Post>(currentPostPage, totalPostCount, page, PostsPerPage)
            });
        }

        public ActionResult New()
        {
            return View("Form", new PostsForm {IsNew = true});
        }

        public ActionResult Edit(int id)
        {
            var post = Database.Session.Load<Post>(id);
            if (post == null)
                return HttpNotFound();

            return View("Form", new PostsForm
            {
                IsNew = false,
                Content = post.Content,
                PostId = id,
                Slug = post.Slug,
                Title = post.Title
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Form(PostsForm form)
        {
            form.IsNew = form.PostId == null;

            if (!ModelState.IsValid)
                return View(form);

            Post post;

            if (form.IsNew)
            {
                post = new Post
                {
                    CreatedAt = DateTime.UtcNow,
                    User = Auth.CurrentUser
                };
            }
            else
            {
                post = Database.Session.Load<Post>(form.PostId);

                if (post == null)
                    return HttpNotFound();

                post.UpdatedAt = DateTime.UtcNow;
            }
            post.Title = form.Title;
            post.Slug = form.Slug;
            post.Content = form.Content;

            Database.Session.SaveOrUpdate(post);
            return RedirectToAction("Index");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Trash(int id)
        {
            return ModifyPost(id, delegate(Post post)
            {
                post.DeletedAt = DateTime.UtcNow;
                Database.Session.Update(post);
                return RedirectToAction("Index");
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            return ModifyPost(id, delegate(Post post)
            {
                Database.Session.Delete(post);
                return RedirectToAction("Index");
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Restore(int id)
        {
            return ModifyPost(id, delegate(Post post)
            {
                post.DeletedAt = null;
                Database.Session.Update(post);
                return RedirectToAction("Index");
            });
        }

        private ActionResult ModifyPost(int id, Func<Post, ActionResult> modify)
        {
            var post = Database.Session.Load<Post>(id);
            return post == null ? HttpNotFound() : modify(post);
        }
    }
}