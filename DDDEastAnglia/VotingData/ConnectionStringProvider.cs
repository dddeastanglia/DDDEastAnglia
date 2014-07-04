using System.Configuration;

namespace DDDEastAnglia.VotingData
{
    public class ConnectionStringProvider
    {
        public string GetConnectionString()
        {
            var connectionStringSettings = ConfigurationManager.ConnectionStrings["DDDEastAnglia"];
            return connectionStringSettings.ConnectionString;
        }
    }
}
