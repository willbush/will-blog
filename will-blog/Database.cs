using System.Web;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using will_blog.Models;

namespace will_blog
{
    public static class Database
    {
        private const string SessionKey = "will_blog.Database.SessionKey";
        private static ISessionFactory _sessionFactory;
        public static ISession Session => (ISession) HttpContext.Current.Items[SessionKey];

        public static void Configure()
        {
            var config = new Configuration();
            config.Configure();

            var mapper = new ModelMapper();
            mapper.AddMapping<UserMap>();
            mapper.AddMapping<RoleMap>();

            config.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());

            _sessionFactory = config.BuildSessionFactory();
        }

        public static void OpenSession()
        {
            HttpContext.Current.Items[SessionKey] = _sessionFactory.OpenSession();
        }

        public static void CloseSession()
        {
            var session = HttpContext.Current.Items[SessionKey] as ISession;
            session?.Close();
            HttpContext.Current.Items.Remove(SessionKey);
        }
    }
}