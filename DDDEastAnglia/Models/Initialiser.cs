using System.Data.Entity;
using DDDEastAnglia.DataAccess;

namespace DDDEastAnglia.Models
{
    public class Initialiser : CreateDatabaseIfNotExists<DDDEAContext>
    {

    }
}