using DDDEastAnglia.DataModel;

namespace DDDEastAnglia.DataAccess
{
    public interface IVoteRepository
    {
        void Save(Vote vote);
    }
}