using System.Data.Entity.Migrations;

namespace DDDEastAnglia.DataAccess.EntityFramework.Migrations
{
    public partial class CreateVoteTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Votes",
                c => new
                    {
                        VoteId = c.Int(nullable: false, identity: true),
                        Event = c.String(nullable: false),
                        SessionId = c.Int(nullable: false),
                        CookieId = c.Guid(nullable: false),
                        IsVote = c.Boolean(nullable: false),
                        TimeRecorded = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                        IPAddress = c.String(),
                        WebSessionId = c.Long(nullable: false),
                        UserAgent = c.String(),
                        Referer = c.String(),
                        ScreenResolution = c.String(),
                    })
                .PrimaryKey(t => t.VoteId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Votes");
        }
    }
}
