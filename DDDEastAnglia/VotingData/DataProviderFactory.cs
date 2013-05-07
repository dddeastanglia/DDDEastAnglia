using System.Configuration;

namespace DDDEastAnglia.VotingData
{
    // This class can be removed once Ninject is present.
    public static class DataProviderFactory
    {
        public static DataProvider Create()
        {
            var connectionStringSettings = ConfigurationManager.ConnectionStrings["DDDEastAnglia"];
            var queryRunner = new QueryRunner(connectionStringSettings.ConnectionString);
            return new DataProvider(queryRunner);
        }
    }
}