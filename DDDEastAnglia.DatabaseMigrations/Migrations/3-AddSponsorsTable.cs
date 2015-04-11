using FluentMigrator;

namespace DDDEastAnglia.DatabaseMigrations.Migrations
{
    [Migration(20150411)]
    public class AddSponsorsTable : Migration
    {
        public override void Up()
        {
            Create.Table("Sponsors")
                .WithColumn("SponsorId").AsInt32().NotNullable().Identity().PrimaryKey()
                .WithColumn("Name").AsString(50).NotNullable()
                .WithColumn("Url").AsString(255).NotNullable()
                .WithColumn("SponsorshipAmount").AsInt32().Nullable()
                .WithColumn("Logo").AsBinary(int.MaxValue).NotNullable()
                .WithColumn("PaymentDate").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.Table("Sponsors");
        }
    }
}