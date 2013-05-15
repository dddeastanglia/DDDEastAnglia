namespace DDDEastAnglia.VotingData.Queries
{
    public interface IQuery<T>
    {
        string Sql{get;}
        IQueryResultObjectFactory<T> ObjectFactory{get;}
    }
}