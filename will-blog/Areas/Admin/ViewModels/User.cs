using System.Collections.Generic;
using will_blog.Models;

namespace will_blog.Areas.Admin.ViewModels
{
    public class UsersIndex
    {
        public IEnumerable<User> Users { get; set; }
    }
}