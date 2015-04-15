using System;

namespace DDDEastAnglia.DataAccess.SimpleData.Models
{
    public class Sponsor
    {
        public int SponsorId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int SponsorshipAmount { get; set; }
        public byte[] Logo { get; set; }
        public DateTime? PaymentDate { get; set; }
        public bool ShowPublicly { get; set; }
    }
}
