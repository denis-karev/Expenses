using System.Data;
using FluentMigrator;

namespace ConsoleApplication1.Migrations
{
    [Migration(20250501164846)]
    public class AddGroups : Migration
    {
        public override void Up()
        {
            Create.Table("currencies")
                .WithColumn("code").AsString().PrimaryKey().NotNullable()
                .WithColumn("name").AsString().NotNullable();

            Create.Table("groups")
                .WithColumn("id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("name").AsString().NotNullable()
                .WithColumn("currency").AsString().ForeignKey("currencies", "code").OnDeleteOrUpdate(Rule.None).NotNullable()
                .WithColumn("created_at").AsDateTimeOffset().NotNullable();
            
            Create.Table("group_members")
                .WithColumn("id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("group_id").AsGuid().ForeignKey("groups", "id").OnDeleteOrUpdate(Rule.Cascade).NotNullable()
                .WithColumn("user_id").AsGuid().ForeignKey("users", "id").OnDeleteOrUpdate(Rule.Cascade).Nullable()
                .WithColumn("name").AsString().Nullable()
                .WithColumn("joined_at").AsDateTimeOffset().NotNullable();
            
            Create.Index("ix_group_member_group_id_user_id")
                .OnTable("group_members")
                .InSchema("public")
                .OnColumn("group_id").Ascending()
                .OnColumn("user_id").Ascending()
                .WithOptions();
        }

        public override void Down()
        {
            Delete.Table("group_members");
            Delete.Table("groups");
            Delete.Table("currencies");
        }
    }
}
