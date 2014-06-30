namespace DDDEastAnglia.Domain
{
    public interface IConference
    {
        int Id{get;}
        string Name{get;}
        string ShortName{get;}
        bool CanSubmit();
        bool CanVote();
        bool CanPublishAgenda();
        bool CanRegister();
        bool CanShowSessions();
        bool CanShowSpeakers();
    }
}
