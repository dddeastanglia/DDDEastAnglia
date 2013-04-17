using System.Data.Entity.Migrations;

namespace DDDEastAnglia.DataAccess.EntityFramework.Migrations
{
    public partial class LinkSessionToConference : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Conferences",
                columnBuilder => new
                    {
                        ConferenceId = columnBuilder.Int(nullable: false, identity: true),
                        Name = columnBuilder.String(),
                        ShortName = columnBuilder.String(),
                    })
                .PrimaryKey(t => t.ConferenceId);
            
            Sql("INSERT INTO dbo.Conferences (Name, ShortName) VALUES ('DDD East Anglia 2013', 'DDDEA2013')");

            AddColumn("dbo.Sessions", "ConferenceId", c => c.Int(nullable: false, defaultValue: 1));
            AddForeignKey("dbo.Sessions", "ConferenceId", "dbo.Conferences", "ConferenceId", name: "FK_Session_Conferences");
            DropColumn("dbo.Votes", "Event");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Votes", "Event", c => c.String(nullable: false, defaultValue: "DDDEA2013"));
            DropForeignKey("dbo.Session", "FK_Session_Conferences");
            DropColumn("dbo.Sessions", "ConferenceId");
            DropTable("dbo.Conferences");
        }


    }
}
