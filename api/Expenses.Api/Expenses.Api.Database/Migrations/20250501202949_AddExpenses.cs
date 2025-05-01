using System.Data;
using FluentMigrator;

namespace Expenses.Api.Database.Migrations
{
    [Migration(20250501202949)]
    public class AddExpenses : Migration
    {
        public override void Up()
        {
            Create.Table("expenses")
                .WithColumn("id").AsGuid().PrimaryKey().NotNullable()
                .WithColumn("group_id").AsGuid().ForeignKey("groups", "id").OnDeleteOrUpdate(Rule.Cascade).NotNullable()
                .WithColumn("description").AsString().NotNullable()
                .WithColumn("currency").AsString().ForeignKey("currencies", "code").OnDeleteOrUpdate(Rule.None).NotNullable()
                .WithColumn("method").AsInt32().NotNullable()
                .WithColumn("created_by").AsGuid().ForeignKey("users", "id").OnDeleteOrUpdate(Rule.Cascade).NotNullable()
                .WithColumn("created_at").AsDateTimeOffset().NotNullable();
            
            Create.Table("expenses_credits")
                .WithColumn("expense_id").AsGuid().ForeignKey("expenses", "id").OnDeleteOrUpdate(Rule.Cascade).NotNullable().PrimaryKey()
                .WithColumn("group_member_id").AsGuid().ForeignKey("group_members", "id").OnDeleteOrUpdate(Rule.Cascade).NotNullable().PrimaryKey()
                .WithColumn("amount").AsDecimal().NotNullable();
            
            Create.Table("expenses_debts")
                .WithColumn("expense_id").AsGuid().ForeignKey("expenses", "id").OnDeleteOrUpdate(Rule.Cascade).NotNullable().PrimaryKey()
                .WithColumn("group_member_id").AsGuid().ForeignKey("group_members", "id").OnDeleteOrUpdate(Rule.Cascade).NotNullable().PrimaryKey()
                .WithColumn("amount").AsDecimal().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("expenses");
            Delete.Table("expenses_credits");
            Delete.Table("expenses_debts");
        }
    }
}
