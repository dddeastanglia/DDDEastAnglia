using System.Data.Entity.Migrations;
namespace DDDEastAnglia.DataAccess.EntityFramework.Migrations
{
    public partial class CalendarItemTypeEnumWorkAround : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CalendarItems", "EntryTypeString", c => c.String());
            Sql(UpdateTheEntryTypeSql);
        }
        
        public override void Down()
        {
            DropColumn("dbo.CalendarItems", "EntryTypeString");
        }

        private const string UpdateTheEntryTypeSql = @"
UPDATE dbo.CalendarItems 
SET EntryTypeString = 
    CASE 
        WHEN EntryType = 1 THEN 'SessionSubmission' 
        WHEN EntryType = 2 THEN 'Voting' 
        WHEN EntryType = 3 THEN 'AgendaPublished' 
        WHEN EntryType = 4 THEN 'Registration' 
        WHEN EntryType = 5 THEN 'Conference' 
        ELSE 'Unknown' 
    END";
    }
}
