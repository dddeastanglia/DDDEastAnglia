using System;

namespace DDDEastAnglia.Tests
{
    public class CannedResponseDateTimeOffsetProvider : IDateTimeOffsetProvider
    {
        private DateTimeOffset current = DateTimeOffset.MinValue;

        public void SetCurrentValue(DateTimeOffset dateTimeOffset)
        {
            this.current = dateTimeOffset;
        }

        public DateTimeOffset CurrentDateTime()
        {
            return current;
        }
    }
}
