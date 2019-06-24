using FluentMigrator;

namespace DDDEastAnglia.DatabaseMigrations.Migrations
{
    [Migration(20190523)]
    public class AddAnonymousSessionsSetting : Migration
    {
        public override void Up()
        {
            Alter.Table("Conferences")
                .AddColumn("AnonymousSessions").AsBoolean().NotNullable().SetExistingRowsTo(false);
        }

        public override void Down()
        {
            Delete.Column("AnonymousSessions")
                .FromTable("Conferences");
        }
    }
}
