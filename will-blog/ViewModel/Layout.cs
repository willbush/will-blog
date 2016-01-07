using System.Collections.Generic;

namespace will_blog.ViewModel
{
    public class SidebarTag
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Slug { get; private set; }
        public int PostCount { get; private set; }

        public SidebarTag(int id, string name, string slug, int postCount)
        {
            Id = id;
            Name = name;
            Slug = slug;
            PostCount = postCount;
        }
    }

    public class LayoutSidebar
    {
        public string Username { get; set; }
        public bool IsLoggedIn { get; set; }
        public bool IsAdmin { get; set; }
        public IEnumerable<SidebarTag> Tags { get; set; }
    }
}