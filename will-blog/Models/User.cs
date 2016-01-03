using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace will_blog.Models
{
    public class User
    {
        private const int WorkFactor = 13;

        public virtual int Id { get; set; }
        public virtual string Username { get; set; }
        public virtual string Email { get; set; }
        public virtual string PasswordHash { get; set; }

        public virtual void SetPassword(string password)
        {
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
        }

        public virtual bool PasswordIsVerified(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
        }

        public static void CauseDelayWithFakeHash()
        {
            BCrypt.Net.BCrypt.HashPassword("fakePassword", WorkFactor);
        }
    }

    public class UserMap : ClassMapping<User>
    {
        public UserMap()
        {
            Table("users");
            Id(u => u.Id, x => x.Generator(Generators.Identity));
            Property(u => u.Username, x => x.NotNullable(true));
            Property(u => u.Email, x => x.NotNullable(true));
            Property(u => u.PasswordHash, x =>
            {
                x.Column("password_hash");
                x.NotNullable(true);
            });
        }
    }
}