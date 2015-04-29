using System;
using System.Collections.Generic;
using System.Linq;
using DDDEastAnglia.DataAccess.SimpleData.Models;

namespace DDDEastAnglia.DataAccess
{
    public static class SponsorExtensions
    {
        public static IEnumerable<Sponsor> OrderBySponsorSorter(this IEnumerable<Sponsor> sponsors)
        {
            return new DefaultSponsorSorter().Sort(sponsors);
        }
    }

    public sealed class DefaultSponsorSorter
    {
        public IEnumerable<Sponsor> Sort(IEnumerable<Sponsor> sponsors)
        {
            return sponsors.OrderByDescending(s => s.SponsorshipAmount)
                           .ThenBy(s => s.PaymentDate, new NullableDateComparer());
        }

        private class NullableDateComparer : IComparer<DateTime?>
        {
            public int Compare(DateTime? x, DateTime? y)
            {
                if (x.HasValue && y.HasValue)
                {
                    return DateTime.Compare(x.Value, y.Value);
                }

                if (!x.HasValue && y.HasValue)
                {
                    return 1;
                }

                if (x.HasValue)
                {
                    return -1;
                }

                return 0;
            }
        }
    }
}
