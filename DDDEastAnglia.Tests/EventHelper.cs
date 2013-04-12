using System;
using DDDEastAnglia.DataModel;

namespace DDDEastAnglia.Tests
{
    public class EventHelper
    {
        public static Event BuildEvent(bool isOpenForSubmission, bool isOpenForVoting)
        {
            var submissionStartDate = isOpenForSubmission ? DateTime.Now.AddDays(-3) : new DateTime(2013, 1, 1);
            var submissionEndDate = isOpenForSubmission ? DateTime.Now.AddDays(3) : new DateTime(2013, 1, 31);
            var votingStartDate = isOpenForVoting ? DateTime.Now.AddDays(-3) : new DateTime(2013, 1, 1);
            var votingEndDate = isOpenForVoting ? DateTime.Now.AddDays(3) : new DateTime(2013, 1, 31);
            return new Event
            {
                Name = "DDD East Anglia 2013",
                ShortName = "DDDEA2013",
                Visible = true,
                PreConferenceAgenda = new PreConferenceAgenda(new[]
                        {
                            new PreConferenceAgendaItem{DateType = DateType.SubmissionStarts, Date = submissionStartDate},
                            new PreConferenceAgendaItem{DateType = DateType.SubmissionEnds, Date = submissionEndDate},
                            new PreConferenceAgendaItem{DateType = DateType.VotingStarts, Date = votingStartDate},
                            new PreConferenceAgendaItem{DateType = DateType.VotingEnds, Date = votingEndDate},
                            new PreConferenceAgendaItem{DateType = DateType.AgendaPublished, Date = new DateTime(2013, 5, 29)},
                            new PreConferenceAgendaItem{DateType = DateType.RegistrationOpens, Date = new DateTime(2013, 6, 1)},
                            new PreConferenceAgendaItem{DateType = DateType.EventStarts, Date = new DateTime(2013, 6, 29, 8, 30, 0)},
                            new PreConferenceAgendaItem{DateType = DateType.EventEnds, Date = new DateTime(2013, 6, 29, 18, 0, 0)},
                        })
            };
        } 
    }
}