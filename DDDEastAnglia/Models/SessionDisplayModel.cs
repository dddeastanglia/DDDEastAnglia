
using System;
using System.Collections.Generic;

namespace DDDEastAnglia.Models
{
    public class SessionDisplayModel
    {
        public string SessionAbstract { get; set; }
        public int SessionId { get; set; }
        public string SessionTitle { get; set; }
        public string SpeakerName { get; set; }
        public string SpeakerUserName { get; set; }
    }

    public class SessionIndexModel
    {
        public bool IsOpenForSubmission
        {
            //HACK: Yes I know this is terrible but my first concern is to get something working
            get { return DateTime.Today >= new DateTime(2013, 4, 1) 
                         && DateTime.Today <= new DateTime(2013, 4, 28); }
        }

        public IEnumerable<SessionDisplayModel> Sessions { get; set; }
    }
}