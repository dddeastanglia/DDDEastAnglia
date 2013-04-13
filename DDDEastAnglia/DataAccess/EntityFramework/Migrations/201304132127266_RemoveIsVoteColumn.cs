using System.Data.Entity.Migrations;

namespace DDDEastAnglia.DataAccess.EntityFramework.Migrations
{
    public partial class RemoveIsVoteColumn : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Votes", "IsVote");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Votes", "IsVote", c => c.Boolean(nullable: false));
        }
    }
}
