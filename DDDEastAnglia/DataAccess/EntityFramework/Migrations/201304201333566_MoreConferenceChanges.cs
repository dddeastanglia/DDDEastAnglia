using System.Data.Entity.Migrations;

namespace DDDEastAnglia.DataAccess.EntityFramework.Migrations
{
    public partial class MoreConferenceChanges : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CalendarItems", "StartDate", c => c.DateTimeOffset(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CalendarItems", "StartDate", c => c.DateTimeOffset());
        }
    }
}
