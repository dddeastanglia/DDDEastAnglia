using System;
using System.ComponentModel;

namespace DDDEastAnglia.Areas.Admin.Models
{
    public class SponsorModel
    {
        public int SponsorId { get; set; }

        [DisplayName("Company Name")]
        public string Name { get; set; }

        [DisplayName("Website Url")]
        public string Url { get; set; }

        [DisplayName("Sponsorship Amount")]
        public int SponsorshipAmount { get; set; }

        [DisplayName("Sponsorship Recieved On")]
        public DateTime? PaymentDate { get; set; }
    }
}
