using FluentMigrator;

namespace DDDEastAnglia.DatabaseMigrations.Migrations
{
    [Migration(20140705)]
    public class AddTrackInformationToConferencesTable : Migration
    {
        public override void Up()
        {
            Alter.Table("Conferences")
                    .AddColumn("NumberOfTimeSlots").AsInt32().NotNullable().WithDefaultValue(0)
                    .AddColumn("NumberOfTracks").AsInt32().NotNullable().WithDefaultValue(0);
        }

        public override void Down()
        {
            Delete.Column("NumberOfTimeSlots")
                    .Column("NumberOfTracks")
                    .FromTable("Conferences");
        }
    }
}
