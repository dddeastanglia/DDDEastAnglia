using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace DDDEastAnglia.Areas.Admin.Models
{
    public class SponsorModel
    {
        public int SponsorId { get; set; }

        [Required]
        [DisplayName("Company name")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Website url")]
        public string Url { get; set; }

        [DisplayName("Sponsorship amount")]
        public int SponsorshipAmount { get; set; }

        [DisplayName("Sponsorship recieved")]
        public DateTime? PaymentDate { get; set; }

        [DisplayName("Show publicly?")]
        public bool ShowPublicly { get; set; }

        [Required]
        [DisplayName("Logo")]
        public HttpPostedFileBase Logo { get; set; }
    }
}
