using System;

namespace DDDEastAnglia.Areas.Admin.Models
{
    public class SponsorModel
    {
        private DateTime? paymentDate;
        public int SponsorId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int SponsorshipAmount { get; set; }
        public DateTime? PaymentDate { get { return paymentDate; } set { paymentDate = value; } }
    }
}
