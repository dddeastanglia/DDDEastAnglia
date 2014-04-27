using System;

namespace DDDEastAnglia.DatabaseMigrations
{
    public class Program
    {
        internal static void Main()
        {
            string connectionString = @"Data Source=.;Initial Catalog=DDDEastAnglia-migration;Integrated Security=True";
            var migrator = new Migrator(connectionString);
            migrator.MigrateTo(20140101);

            Console.WriteLine("done");
            Console.ReadLine();
        }
    }
}