using System;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.Processors.SqlServer;

namespace DDDEastAnglia.DatabaseMigrations
{
    public class Migrator
    {
        private readonly string connectionString;

        public Migrator(string connectionString)
        {
            if (connectionString == null)
            {
                throw new ArgumentNullException("connectionString");
            }
            
            this.connectionString = connectionString;
        }

        public void MigrateToLatestSchema()
        {
            var todaysDate = DateTime.Today.ToString("yyyyMMdd");
            var todaysSchemaVersion = long.Parse(DateTime.Today.ToString("yyyyMMdd"));
            MigrateTo(todaysSchemaVersion);
        }

        private void MigrateTo(long targetVersion)
        {
            var options = new MigrationOptions { PreviewOnly = false,  Timeout = 60 };
            var announcer = new NullAnnouncer();
            var processor = new SqlServer2012ProcessorFactory().Create(connectionString, announcer, options);
            var migrationContext = new RunnerContext(announcer) { Namespace = "DDDEastAnglia.DatabaseMigrations.Migrations" };

            var runner = new MigrationRunner(GetType().Assembly, migrationContext, processor);
            runner.MigrateUp(targetVersion, true);
        }
    }
}
