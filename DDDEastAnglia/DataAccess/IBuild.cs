using DDDEastAnglia.Helpers;

namespace DDDEastAnglia.DataAccess
{
    public interface IBuild<T>
    {
        T Get(int id);
    }
}