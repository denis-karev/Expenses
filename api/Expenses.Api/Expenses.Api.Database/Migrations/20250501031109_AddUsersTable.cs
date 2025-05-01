using FluentMigrator;

namespace Expenses.Api.Database.Migrations
{
    [Migration(20250501031109)]
    public class AddUsersTable : Migration
    {
        public override void Up()
        {
            Create.Table("users")
                .WithColumn("id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("email").AsString().Unique().NotNullable()
                .WithColumn("name").AsString().NotNullable()
                .WithColumn("encrypted_google_token").AsString().Nullable();
        }

        public override void Down()
        {
            Delete.Table("users");
        }
    }
}
