using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DDDEastAnglia.Domain;
using DDDEastAnglia.Domain.Calendar;

namespace DDDEastAnglia.DataAccess.EntityFramework.Models
{
    public class Conference
    {
        public int ConferenceId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string ShortName { get; set; }

        public List<CalendarItem> CalendarItems { get; set; }
    }

    public class CalendarItem
    {
        public int CalendarItemId { get; set; }
        public int ConferenceId { get; set; }
        public bool IsPublic { get; set; }
        public bool Authorised { get; set; }     
        public string Description { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public string EntryTypeString { get; set; }
        public CalendarEntryType EntryType
        {
            get
            {
                CalendarEntryType result;
                if (Enum.TryParse(EntryTypeString, true, out result))
                {
                    return result;
                }
                return CalendarEntryType.Unknown;
            }
            set { EntryTypeString = value.ToString(); }
        }
    }

    
}