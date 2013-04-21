using System.Data.Entity;
using DDDEastAnglia.DataAccess.EntityFramework.Migrations;

namespace DDDEastAnglia.DataAccess.EntityFramework
{
    internal class Initialiser : MigrateDatabaseToLatestVersion<DDDEAContext, Configuration>
    {

    }
}