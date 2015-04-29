using System;
using DDDEastAnglia.DataAccess.SimpleData.Models;

namespace DDDEastAnglia.Tests
{
    internal class SponsorBuilder
    {
        private readonly byte[] logo;
        private string name = "Sponsor";
        private DateTime? paymentDate = DateTime.UtcNow;
        private bool showPublicly = true;
        private int sponsorId;
        private int sponsorshipAmount = 1000;
        private string url = "http://Sponsor.com";
        
        public SponsorBuilder UnPaidSponsor(string name)
        {
            showPublicly = false;
            sponsorshipAmount = 0;
            this.name = name;
            return this;
        }

        public SponsorBuilder PremiumSponsor(string name)
        {
            showPublicly = true;
            sponsorshipAmount = 2000;
            this.name = name;
            return this;
        }

        public SponsorBuilder GoldSponsor(string name)
        {
            this.name = name;
            sponsorshipAmount = 1000;
            return this;
        }

        public SponsorBuilder StandardSponsor(string name)
        {
            this.name = name;
            sponsorshipAmount = 500;
            return this;
        }

        public SponsorBuilder WithPaymentDate(DateTime? paymentDate)
        {
            this.paymentDate = paymentDate;
            return this;
        }

        public Sponsor Build()
        {
            return new Sponsor
            {
                Logo = logo,
                Name = name,
                PaymentDate = paymentDate,
                ShowPublicly = showPublicly,
                SponsorId = sponsorId,
                SponsorshipAmount = sponsorshipAmount,
                Url = url
            };
        }
    }
}