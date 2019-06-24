using FluentMigrator;

namespace DDDEastAnglia.DatabaseMigrations.Migrations
{
    [Migration(20190610)]
    public class RemoveExternalLogins : Migration
    {
        public override void Up()
        {
            Delete.Table("webpages_OAuthMembership");
        }

        public override void Down()
        {
            Create.Table("webpages_OAuthMembership")
                .WithColumn("Provider").AsString(30).NotNullable()
                .WithColumn("ProviderUserId").AsString(100).NotNullable()
                .WithColumn("UserId").AsInt32().NotNullable();

            Create.PrimaryKey()
                .OnTable("webpages_OAuthMembership")
                .Columns(new[] { "Provider", "ProviderUserId" });
        }
    }
}
