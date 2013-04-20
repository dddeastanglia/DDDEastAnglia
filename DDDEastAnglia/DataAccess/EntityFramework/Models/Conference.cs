using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
}