using System.Configuration;
using DDDEastAnglia.DataAccess.SimpleData;

namespace DDDEastAnglia.VotingData
{
    // TODO: This class can be removed once Ninject is present.
    public static class DataProviderFactory
    {
        public static IDataProvider Create()
        {
            var connectionStringSettings = ConfigurationManager.ConnectionStrings["DDDEastAnglia"];
            var queryRunner = new QueryRunner(connectionStringSettings.ConnectionString);
            return new DataProvider(queryRunner, new VoteRepository(), new CalendarItemRepository());
        }
    }
}
