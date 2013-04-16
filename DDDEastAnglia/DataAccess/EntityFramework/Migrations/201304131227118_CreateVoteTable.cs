using System.Data.Entity.Migrations;

namespace DDDEastAnglia.DataAccess.EntityFramework.Migrations
{
    public partial class CreateVoteTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Votes",
                columnBuilder => new
                    {
                        VoteId = columnBuilder.Int(nullable: false, identity: true),
                        Event = columnBuilder.String(nullable: false),
                        SessionId = columnBuilder.Int(nullable: false),
                        CookieId = columnBuilder.Guid(nullable: false),
                        IsVote = columnBuilder.Boolean(nullable: false),
                        TimeRecorded = columnBuilder.DateTime(nullable: false),
                        UserId = columnBuilder.Int(nullable: false),
                        IPAddress = columnBuilder.String(),
                        WebSessionId = columnBuilder.Long(nullable: false),
                        UserAgent = columnBuilder.String(),
                        Referer = columnBuilder.String(),
                        ScreenResolution = columnBuilder.String(),
                    })
                .PrimaryKey(t => t.VoteId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Votes");
        }
    }
}
