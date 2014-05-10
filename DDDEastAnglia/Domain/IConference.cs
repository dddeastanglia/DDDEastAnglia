namespace DDDEastAnglia.Domain
{
    public interface IConference
    {
        int Id{get;}
        bool CanSubmit();
        bool CanVote();
        bool CanPublishAgenda();
        bool CanRegister();
        bool CanShowSessions();
    }
}