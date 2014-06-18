using System;

namespace DDDEastAnglia
{
    public class LocalDateTimeOffsetProvider : IDateTimeOffsetProvider
    {
        public DateTimeOffset CurrentDateTime()
        {
            return DateTimeOffset.Now;
        }
    }
}
