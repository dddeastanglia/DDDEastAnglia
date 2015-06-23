using System;
using DDDEastAnglia.DataAccess.SimpleData.Models;

namespace DDDEastAnglia.Tests
{
    internal class SponsorBuilder
    {
        private string name = "Sponsor";
        private DateTime? paymentDate;
        private bool showPublicly = true;
        private int sponsorshipAmount;

        public SponsorBuilder UnPaidSponsor(string name)
        {
            showPublicly = false;
            sponsorshipAmount = 0;
            this.name = name;
            return this;
        }

        public SponsorBuilder PremiumSponsor(string name)
        {
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

        private static int nextSponsorId = 1;

        public Sponsor Build()
        {
            return new Sponsor
            {
                Logo = new byte[] {1, 2, 3, 4},
                Name = name,
                PaymentDate = paymentDate,
                ShowPublicly = showPublicly,
                SponsorId = nextSponsorId++,
                SponsorshipAmount = sponsorshipAmount,
                Url = "http://www.sponsor.com"
            };
        }
    }
}