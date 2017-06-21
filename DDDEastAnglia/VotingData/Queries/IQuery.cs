namespace DDDEastAnglia.VotingData.Queries
{
    public interface IQuery<out T>
    {
        string Sql{get;}
        IQueryResultObjectFactory<T> ObjectFactory{get;}
    }
}