using System.Web.Optimization;

namespace will_blog
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/styles")
                .Include("~/content/styles/bootstrap.css")
                .Include("~/content/styles/Site.css"));

            bundles.Add(new StyleBundle("~/admin/styles")
                .Include("~/content/styles/bootstrap.css")
                .Include("~/content/styles/Admin.css"));

            bundles.Add(BundleScripts("~/admin/scripts"));
            bundles.Add(BundleScripts("~/scripts"));
        }

        private static Bundle BundleScripts(string path)
        {
            return new StyleBundle(path)
                .Include("~/scripts/jquery-2.1.4.js")
                .Include("~/scripts/jquery.validate.js")
                .Include("~/scripts/jquery.validate.unobtrusive.js")
                .Include("~/scripts/bootstrap.js");
        }
    }
}