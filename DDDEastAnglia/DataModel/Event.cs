using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace DDDEastAnglia.DataModel
{
    public class Event
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public bool Visible { get; set; }
        public PreConferenceAgenda PreConferenceAgenda { get; set; }

        public bool CanSubmit()
        {
            return PreConferenceAgenda.CanSubmit();
        }

        public bool CanVote()
        {
            bool overrideAgenda;
            if (!bool.TryParse(ConfigurationManager.AppSettings["DeveloperOverrideVoting"], out overrideAgenda))
            {
                overrideAgenda = false;
            }
            return overrideAgenda || PreConferenceAgenda.CanVote();
        }
    }

    public class PreConferenceAgenda : IEnumerable<PreConferenceAgendaItem>
    {
        private readonly Dictionary<DateType, PreConferenceAgendaItem> _items;

        public PreConferenceAgenda(IEnumerable<PreConferenceAgendaItem> items)
        {
            _items = items.ToDictionary(item => item.DateType);
        }
        
        public PreConferenceAgendaItem this[DateType key]
        {
            get { return _items[key]; }
        }

        public bool CanSubmit()
        {
            return CanSubmit(DateTime.Now);
        }

        public bool CanSubmit(DateTime date)
        {
            PreConferenceAgendaItem submissionStart;
            PreConferenceAgendaItem submissionEnd;
            if (_items.TryGetValue(DateType.SubmissionStarts, out submissionStart) 
                && _items.TryGetValue(DateType.SubmissionEnds, out submissionEnd))
            {
                return submissionStart.Date <= date
                       && date <= submissionEnd.Date;
            }
            return false;
        }

        public bool CanVote()
        {
            return CanVote(DateTime.Now);
        }

        public bool CanVote(DateTime date)
        {
            PreConferenceAgendaItem votingStart;
            PreConferenceAgendaItem votingEnd;
            if (_items.TryGetValue(DateType.VotingStarts, out votingStart)
                && _items.TryGetValue(DateType.VotingEnds, out votingEnd))
            {
                return votingStart.Date <= date
                       && date <= votingEnd.Date;
            }
            return false;
        }

        public IEnumerator<PreConferenceAgendaItem> GetEnumerator()
        {
            return _items.Values.OrderBy(item => item.Date).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class PreConferenceAgendaItem
    {
        public DateType DateType { get; set; }
        public DateTime Date { get; set; }
    }

    public enum DateType
    {
        SubmissionStarts,
        SubmissionEnds,
        VotingStarts,
        VotingEnds,
        AgendaPublished,
        RegistrationOpens,
        EventStarts,
        EventEnds
    }
}