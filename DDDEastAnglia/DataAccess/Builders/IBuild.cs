namespace DDDEastAnglia.DataAccess.Builders
{
    public interface IBuild<in TFrom, out TTo>
    {
        TTo Build(TFrom item);
    }
}