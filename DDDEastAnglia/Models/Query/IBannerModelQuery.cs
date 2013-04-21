namespace DDDEastAnglia.Models.Query
{
    public interface IBannerModelQuery
    {
        BannerModel Get(string conferenceShortName);
    }
}