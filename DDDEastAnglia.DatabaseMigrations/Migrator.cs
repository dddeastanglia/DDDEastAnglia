using FluentMigrator;
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
            this.connectionString = connectionString;
        }

        public void MigrateTo(long targetVersion)
        {
            var factory = new SqlServer2012ProcessorFactory();
            var announcer = new ConsoleAnnouncer
                                    {
                                        ShowElapsedTime = true,
                                        ShowSql = true
                                    };
            var options = new MigrationOptions
                                    {
                                        PreviewOnly = false, 
                                        Timeout = 60
                                    };
            var processor = factory.Create(connectionString, announcer, options);
            var migrationContext = new RunnerContext(announcer) { Namespace = "DDDEastAnglia.DatabaseMigrations.Migrations" };

            var runner = new MigrationRunner(GetType().Assembly, migrationContext, processor);
            runner.MigrateUp(targetVersion, true);
        }
    }

    public class MigrationOptions : IMigrationProcessorOptions
    {
        public bool PreviewOnly{get;set;}
        public int Timeout{get;set;}
        public string ProviderSwitches{get;set;}
    }
}
