using System;
using System.Collections.Generic;
using System.Linq;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.DataAccess
{
    public interface ISponsorSorter
    {
        IEnumerable<Sponsor> Sort(IEnumerable<Sponsor> sponsors);
    }

    public class SponsorSorter : ISponsorSorter
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
