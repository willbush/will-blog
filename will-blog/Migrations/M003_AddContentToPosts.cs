using FluentMigrator;

namespace will_blog.Migrations
{
    [Migration(3)]
    public class M003_AddContentToPosts : Migration
    {
        public override void Up()
        {
            Create.Column("content").OnTable("posts").AsCustom("TEXT");
        }

        public override void Down()
        {
            Delete.Column("content").FromTable("posts");
        }
    }
}