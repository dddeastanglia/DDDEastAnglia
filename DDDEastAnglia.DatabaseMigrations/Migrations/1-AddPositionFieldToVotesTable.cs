using FluentMigrator;

namespace DDDEastAnglia.DatabaseMigrations.Migrations
{
    [Migration(20140619)]
    public class AddPositionFieldToVotesTable : Migration
    {
        public override void Up()
        {
            Alter.Table("Votes")
                    .AddColumn("PositionInList").AsInt32().NotNullable();
        }

        public override void Down()
        {
            Delete.Column("PositionInList")
                    .FromTable("Votes");
        }
    }
}
