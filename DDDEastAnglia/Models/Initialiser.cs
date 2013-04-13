using System.Data.Entity;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.DataAccess.EntityFramework.Migrations;

namespace DDDEastAnglia.Models
{
    internal class Initialiser : MigrateDatabaseToLatestVersion<DDDEAContext, Configuration>
    {

    }
}