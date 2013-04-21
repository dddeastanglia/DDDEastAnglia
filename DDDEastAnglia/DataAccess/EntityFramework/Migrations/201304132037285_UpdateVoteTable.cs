using System.Data.Entity.Migrations;

namespace DDDEastAnglia.DataAccess.EntityFramework.Migrations
{
    public partial class UpdateVoteTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Votes", "Referrer", c => c.String());
            AlterColumn("dbo.Votes", "WebSessionId", c => c.String());
            DropColumn("dbo.Votes", "Referer");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Votes", "Referer", c => c.String());
            AlterColumn("dbo.Votes", "WebSessionId", c => c.Long(nullable: false));
            DropColumn("dbo.Votes", "Referrer");
        }
    }
}
