using FluentMigrator;

namespace DDDEastAnglia.DatabaseMigrations.Migrations
{
    [Migration(20190620)]
    public class AddSessionDuration : Migration
    {
        public override void Up()
        {
            Alter.Table("Sessions")
                .AddColumn("DurationInMinutes").AsInt32().NotNullable().SetExistingRowsTo(60);
        }

        public override void Down()
        {
            Delete.Column("DurationInMinutes")
                .FromTable("Sessions");
        }
    }
}