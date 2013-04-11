namespace DDDEastAnglia.DataAccess
{
    public interface IRepository<TEntity, in TIdentifier>
    {
        TEntity Get(TIdentifier identifier);
        void Update(TEntity entity);
        void Save(TEntity entity);
        void Delete(TEntity entity);
        void Delete(TIdentifier identifier);
        bool Exists(TIdentifier identifier);
    }
}