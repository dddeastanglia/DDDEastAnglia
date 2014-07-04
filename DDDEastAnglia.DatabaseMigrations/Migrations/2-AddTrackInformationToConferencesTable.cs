using FluentMigrator;

namespace DDDEastAnglia.DatabaseMigrations.Migrations
{
    [Migration(20140704)]
    public class AddTrackInformationToConferencesTable : Migration
    {
        public override void Up()
        {
            Alter.Table("Conferences")
                    .AddColumn("NumberOfTimeSlots").AsInt32().NotNullable().WithDefaultValue(0)
                    .AddColumn("NumberOfTracks").AsInt32().NotNullable().WithDefaultValue(0);

            Execute.Sql("UPDATE [dbo].[Conferences] SET [NumberOfTimeSlots] = 5, [NumberOfTracks] = 5");
        }

        public override void Down()
        {
            Delete.Column("NumberOfTimeSlots")
                    .Column("NumberOfTracks")
                    .FromTable("Conferences");
        }
    }
}
