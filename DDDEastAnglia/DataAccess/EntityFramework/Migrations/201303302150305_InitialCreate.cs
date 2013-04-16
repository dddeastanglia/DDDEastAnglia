using System.Data.Entity.Migrations;

namespace DDDEastAnglia.DataAccess.EntityFramework.Migrations
{
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserProfile",
                columnBuilder => new
                    {
                        UserId = columnBuilder.Int(nullable: false, identity: true),
                        UserName = columnBuilder.String(),
                        Name = columnBuilder.String(nullable: false),
                        EmailAddress = columnBuilder.String(nullable: false),
                        WebsiteUrl = columnBuilder.String(),
                        TwitterHandle = columnBuilder.String(),
                        Bio = columnBuilder.String(),
                        MobilePhone = columnBuilder.String(),
                        NewSpeaker = columnBuilder.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Sessions",
                columnBuilder => new
                    {
                        SessionId = columnBuilder.Int(nullable: false, identity: true),
                        Title = columnBuilder.String(nullable: false),
                        Abstract = columnBuilder.String(nullable: false),
                        SpeakerUserName = columnBuilder.String(),
                        Votes = columnBuilder.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SessionId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Sessions");
            DropTable("dbo.UserProfile");
        }
    }
}
