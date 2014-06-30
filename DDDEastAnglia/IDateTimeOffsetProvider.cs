using System;

namespace DDDEastAnglia
{
    public interface IDateTimeOffsetProvider
    {
        DateTimeOffset CurrentDateTime();
    }
}
