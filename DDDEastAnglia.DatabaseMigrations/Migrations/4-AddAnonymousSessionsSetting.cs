using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;

namespace DDDEastAnglia.DatabaseMigrations.Migrations
{
    [Migration(20190523)]
    public class AddAnonymousSessionsSetting : ForwardOnlyMigration
    {
        public override void Up()
        {
            Alter.Table("Conferences")
                .AddColumn("AnonymousSessions").AsBoolean().Nullable();
        }
    }
}
