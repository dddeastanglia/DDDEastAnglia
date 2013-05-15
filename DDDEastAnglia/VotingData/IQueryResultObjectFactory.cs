using System.Data;

namespace DDDEastAnglia.VotingData
{
    public interface IQueryResultObjectFactory<T>
    {
        T Create(IDataReader reader);
    }
}