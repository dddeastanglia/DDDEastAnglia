namespace DDDEastAnglia.Domain
{
    public interface IConference
    {
        int Id{get;}
        string Name{get;}
        string ShortName{get;}

        int NumberOfTimeSlots{get;}
        int NumberOfTracks{get;}
        int TotalNumberOfSessions{get;}

        bool CanSubmit();
        bool CanVote();
        bool CanPublishAgenda();
        bool AgendaBeingPrepared();
        bool CanRegister();
        bool CanShowSessions();
        bool CanShowSpeakers();
        bool IsPreview();
        bool IsClosed();
        bool AnonymousSessions();
    }
}
