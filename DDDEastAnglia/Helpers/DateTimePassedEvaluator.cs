using System;

namespace DDDEastAnglia.Helpers
{
    public interface IDateTimePassedEvaluator
    {
        bool HasDatePassed(DateTimeOffset dateTime);
    }

    public class DateTimePassedEvaluator : IDateTimePassedEvaluator
    {
        private readonly IDateTimeOffsetProvider dateTimeOffsetProvider;

        public DateTimePassedEvaluator(IDateTimeOffsetProvider dateTimeOffsetProvider)
        {
            if (dateTimeOffsetProvider == null)
            {
                throw new ArgumentNullException(nameof(dateTimeOffsetProvider));
            }

            this.dateTimeOffsetProvider = dateTimeOffsetProvider;
        }

        public bool HasDatePassed(DateTimeOffset dateTime)
        {
            return dateTimeOffsetProvider.CurrentDateTime() >= dateTime;
        }
    }
}
