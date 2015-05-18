namespace DDDEastAnglia.Controllers
{
    public interface IViewModelQuery<out TResult>
    {
        TResult Get();
    }
}