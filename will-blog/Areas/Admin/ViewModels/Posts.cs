using will_blog.Infrastructure;
using will_blog.Models;

namespace will_blog.Areas.Admin.ViewModels
{
    public class PostsIndex
    {
        public PagedData<Post> Posts { get; set; }
    }
}