using System;
using System.Collections.Generic;
using System.Linq;
using DDDEastAnglia.DataAccess.DataModel;

namespace DDDEastAnglia.DataAccess
{
    public class EventRepository
    {
        public IEnumerable<Event> GetAll()
        {
            yield return DDDEA2013;
        }

        public Event Get(string shortName)
        {
            return
                GetAll()
                    .SingleOrDefault(evt => evt.ShortName.Equals(shortName, StringComparison.InvariantCultureIgnoreCase));
        }

        private static Event DDDEA2013
        {
            get
            {
                return new Event
                    {
                        Name = "DDD East Anglia 2013",
                        ShortName = "DDDEA2013",
                        Visible = true,
                        PreConferenceAgenda = new PreConferenceAgenda(new[]
                            {
                                new PreConferenceAgendaItem{DateType = DateType.SubmissionStarts, Date = new DateTime(2013, 4, 1)},
                                new PreConferenceAgendaItem{DateType = DateType.SubmissionEnds, Date = new DateTime(2013, 4, 28, 23, 59, 59)},
                                new PreConferenceAgendaItem{DateType = DateType.VotingStarts, Date = new DateTime(2013, 5, 1)},
                                new PreConferenceAgendaItem{DateType = DateType.VotingEnds, Date = new DateTime(2013, 5, 24, 23, 59, 59)},
                                new PreConferenceAgendaItem{DateType = DateType.AgendaPublished, Date = new DateTime(2013, 5, 29)},
                                new PreConferenceAgendaItem{DateType = DateType.RegistrationOpens, Date = new DateTime(2013, 6, 1)},
                                new PreConferenceAgendaItem{DateType = DateType.EventStarts, Date = new DateTime(2013, 6, 29, 8, 30, 0)},
                                new PreConferenceAgendaItem{DateType = DateType.EventEnds, Date = new DateTime(2013, 6, 29, 18, 0, 0)},
                            })
                    };
            }
        }
    }
}