using System.Data;

namespace DDDEastAnglia.VotingData
{
    public interface IQueryResultObjectFactory<out T>
    {
        T Create(IDataReader reader);
    }
}