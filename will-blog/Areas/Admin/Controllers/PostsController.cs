using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NHibernate.Linq;
using will_blog.Areas.Admin.ViewModels;
using will_blog.Infrastructure;
using will_blog.Infrastructure.Extensions;
using will_blog.Models;

namespace will_blog.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [SelectedTab("posts")]
    public class PostsController : Controller
    {
        private const int PostsPerPage = 10;

        public ActionResult Index(int page = 1)
        {
            var totalPostCount = Database.Session.Query<Post>().Count();

            var baseQuery = Database.Session.Query<Post>().OrderByDescending(c => c.CreatedAt);

            var postIds = baseQuery
                .Skip((page - 1) * PostsPerPage)
                .Take(PostsPerPage)
                .Select(p => p.Id)
                .ToArray();

            var currentPostPage = baseQuery
                .Where(p => postIds.Contains(p.Id))
                .FetchMany(f => f.Tags)
                .Fetch(f => f.User)
                .ToList();

            return View(new PostsIndex
            {
                Posts = new PagedData<Post>(currentPostPage, totalPostCount, page, PostsPerPage)
            });
        }

        public ActionResult New()
        {
            return View("Form", new PostsForm
            {
                IsNew = true,
                Tags = Database.Session.Query<Tag>().Select(tag => new TagCheckbox
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    IsChecked = false
                }).ToList()
            });
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
                Title = post.Title,
                Tags = Database.Session.Query<Tag>().Select(tag => new TagCheckbox
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    IsChecked = post.Tags.Contains(tag)
                }).ToList()
            });
        }

        // validate input set to false to allow ckeditor to work.
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Form(PostsForm form)
        {
            form.IsNew = form.PostId == null;

            if (!ModelState.IsValid)
                return View(form);

            var selectedTags = ReconsileTags(form.Tags).ToList();

            Post post;

            if (form.IsNew)
            {
                post = new Post
                {
                    CreatedAt = DateTime.UtcNow,
                    User = Auth.CurrentUser
                };
                foreach (var tag in selectedTags)
                    post.Tags.Add(tag);
            }
            else
            {
                post = Database.Session.Load<Post>(form.PostId);

                if (post == null)
                    return HttpNotFound();

                post.UpdatedAt = DateTime.UtcNow;

                foreach (var tagToAdd in selectedTags.Where(t => !post.Tags.Contains(t)))
                    post.Tags.Add(tagToAdd);
                foreach (var tagToRemove in post.Tags.Where(t => !selectedTags.Contains(t)).ToList())
                    post.Tags.Remove(tagToRemove);
            }
            post.Title = form.Title;
            post.Slug = form.Slug;
            post.Content = form.Content;

            Database.Session.SaveOrUpdate(post);
            return RedirectToAction("Index");
        }

        private static IEnumerable<Tag> ReconsileTags(IEnumerable<TagCheckbox> tags)
        {
            foreach (var tag in tags.Where(t => t.IsChecked))
            {
                if (tag.Id != null)
                {
                    yield return Database.Session.Load<Tag>(tag.Id);
                    continue;
                }

                var existingTag = Database.Session.Query<Tag>().FirstOrDefault(t => t.Name == tag.Name);
                if (existingTag != null)
                {
                    yield return existingTag;
                    continue;
                }
                var newTag = new Tag
                {
                    Name = tag.Name,
                    Slug = tag.Name.Slugify()
                };
                Database.Session.Save(newTag);
                yield return newTag;
            }
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