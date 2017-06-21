using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DDDEastAnglia.VotingData.Queries;

namespace DDDEastAnglia.VotingData
{
    public class QueryRunner
    {
        private readonly string connectionString;

        public QueryRunner(ConnectionStringProvider connectionStringProvider)
        {
            if (connectionStringProvider == null)
            {
                throw new ArgumentNullException("connectionStringProvider");
            }

            connectionString = connectionStringProvider.GetConnectionString();
        }

        public IList<T> RunQuery<T>(IQuery<T> query)
        {
            var items = new List<T>();
            var objectFactory = query.ObjectFactory;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = query.Sql;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = objectFactory.Create(reader);
                        items.Add(item);
                    }
                }
            }

            return items;
        }
    }
}