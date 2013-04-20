using System.Data.Entity.Migrations;

namespace DDDEastAnglia.DataAccess.EntityFramework.Migrations
{
    public partial class ConferenceCalendar : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CalendarItems",
                columnBuilder => new
                    {
                        CalendarItemId = columnBuilder.Int(nullable: false, identity: true),
                        ConferenceId = columnBuilder.Int(nullable: false),
                        IsPublic = columnBuilder.Boolean(nullable: false, defaultValue: false),
                        Authorised = columnBuilder.Boolean(nullable: false, defaultValue: false),
                        Description = columnBuilder.String(nullable: false),
                        StartDate = columnBuilder.DateTimeOffset(nullable: false),
                        EndDate = columnBuilder.DateTimeOffset(nullable: true),
                        EntryType = columnBuilder.Int(nullable: false)
                    })
                .PrimaryKey(t => t.CalendarItemId)
                .ForeignKey("dbo.Conferences", t => t.ConferenceId, cascadeDelete: true)
                .Index(t => t.ConferenceId);
            
            AlterColumn("dbo.Conferences", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Conferences", "ShortName", c => c.String(nullable: false));
            AddForeignKey("dbo.Sessions", "ConferenceId", "dbo.Conferences", "ConferenceId", cascadeDelete: true);
            CreateIndex("dbo.Sessions", "ConferenceId");

            Sql("INSERT INTO dbo.CalendarItems (ConferenceId, IsPublic, Authorised, Description, StartDate, EndDate, EntryType)" +
                "VALUES " +
                "(1, 1, 1, 'Session submission', '2013-04-01 00:00:00.0000000 +01:00', '2013-04-29 00:00:00.0000000 +01:00', 1)" +
                ",(1, 1, 1, 'Voting', '2013-05-01 00:00:00.0000000 +01:00', '2013-05-25 00:00:00.0000000 +01:00', 2)" +
                ",(1, 1, 1, 'Agenda announced', '2013-05-01 00:00:00.0000000 +01:00', null, 3)" +
                ",(1, 1, 1, 'Registration', '2013-06-01 00:00:00.0000000 +01:00', '2013-06-29 08:30:00.0000000 +01:00', 4)" +
                ",(1, 1, 1, 'Conference', '2013-06-29 08:30:00.0000000 + 01:00', '2013-06-29 18:30:00.0000000 + 01:00', 5)");
        }
        
        public override void Down()
        {
            DropIndex("dbo.CalendarItems", new[] { "ConferenceId" });
            DropIndex("dbo.Sessions", new[] { "ConferenceId" });
            DropForeignKey("dbo.CalendarItems", "ConferenceId", "dbo.Conferences");
            DropForeignKey("dbo.Sessions", "ConferenceId", "dbo.Conferences");
            AlterColumn("dbo.Conferences", "ShortName", c => c.String());
            AlterColumn("dbo.Conferences", "Name", c => c.String());
            DropTable("dbo.CalendarItems");
        }
    }
}
