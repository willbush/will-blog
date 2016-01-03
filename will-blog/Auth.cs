using System.Linq;
using System.Web;
using NHibernate.Linq;
using will_blog.Models;

namespace will_blog
{
    public static class Auth
    {
        private const string UserKey = "will-blog.Auth.UserKey";

        public static User GetCurrentUser()
        {
            var userIdentity = HttpContext.Current.User.Identity;

            if (!userIdentity.IsAuthenticated)
                return null;

            var user = HttpContext.Current.Items[UserKey] as User;
            if (user != null) return user;

            user = Database.Session.Query<User>().FirstOrDefault(u => u.Username == userIdentity.Name);
            if (user == null)
                return null;

            HttpContext.Current.Items[UserKey] = user;
            return user;
        }
    }
}