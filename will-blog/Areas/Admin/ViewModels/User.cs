using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using will_blog.Models;

namespace will_blog.Areas.Admin.ViewModels
{
    public class RoleCheckbox
    {
        public int Id { get; set; }
        public bool IsChecked { get; set; }
        public string Name { get; set; }
    }

    public class UsersIndex
    {
        public IEnumerable<User> Users { get; set; }
    }

    public class UsersNew
    {
        public IList<RoleCheckbox> Roles { get; set; }

        [Required, MaxLength(128)]
        public string Username { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, MaxLength(256), DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }

    public class UsersEdit
    {
        public IList<RoleCheckbox> Roles { get; set; }

        [Required, MaxLength(128)]
        public string Username { get; set; }

        [Required, MaxLength(256), DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }

    public class UsersResetPassword
    {
        public string Username { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}