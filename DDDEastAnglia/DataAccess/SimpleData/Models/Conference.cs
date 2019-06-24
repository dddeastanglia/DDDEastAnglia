using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DDDEastAnglia.DataAccess.SimpleData.Models
{
    public class Conference
    {
        public int ConferenceId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string ShortName { get; set; }

        public int NumberOfTracks { get; set; }
        
        public int NumberOfTimeSlots { get; set; }

        public List<CalendarItem> CalendarItems { get; set; }

        public bool AnonymousSessions { get; set; }
    }
}