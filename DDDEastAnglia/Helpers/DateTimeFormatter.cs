using System;

namespace DDDEastAnglia.Helpers
{
    public interface IDateTimeFormatter
    {
        string FormatTime(DateTimeOffset dateTime);
        string FormatDate(DateTimeOffset dateTime);
        string FormatStartDate(DateTimeOffset startDateTime);
        string FormatEndDate(DateTimeOffset? endDateTime);
    }

    public class DateTimeFormatter : IDateTimeFormatter
    {
        private const string DateOnlyPattern = "dddd d MMMM yyyy";
        private const string TimeOnlyPattern = "H:mm";
        private const string DateAndTimePattern = "dddd d MMMM yyyy, H:mm";

        public string FormatTime(DateTimeOffset dateTime)
        {
            return dateTime.ToString(TimeOnlyPattern);
        }

        public string FormatDate(DateTimeOffset dateTime)
        {
            return dateTime.ToString(DateOnlyPattern);
        }

        public string FormatStartDate(DateTimeOffset startDateTime)
        {
            var timeOfDay = startDateTime.TimeOfDay;
            return timeOfDay.Hours == 0
                        ? startDateTime.ToString(DateOnlyPattern)
                        : startDateTime.ToString(DateAndTimePattern);
        }

        public string FormatEndDate(DateTimeOffset? endDateTime)
        {
            if (endDateTime == null)
            {
                return string.Empty;
            }

            var endDateTimeValue = endDateTime.Value;
            var timeOfDay = endDateTimeValue.TimeOfDay;

            // if the end time is midnight, we need to subtract a little to push it back to the previous date
            if (timeOfDay.Hours == 0 && timeOfDay.Minutes == 0 && timeOfDay.Seconds == 0)
            {
                endDateTimeValue = endDateTimeValue - new TimeSpan(0, 0, 1);
            }

            return endDateTimeValue.ToString(DateOnlyPattern);
        }
    }
}