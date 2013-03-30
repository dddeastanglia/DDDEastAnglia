using System.Data.Entity;
using DDDEastAnglia.DataAccess;

namespace DDDEastAnglia.Models
{
    public class Initialiser : DropCreateDatabaseIfModelChanges<DDDEAContext>
    {

    }
}