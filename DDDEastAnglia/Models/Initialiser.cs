using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DDDEastAnglia.Models
{
    public class Initialiser : DropCreateDatabaseIfModelChanges<Context>
    {

    }
}