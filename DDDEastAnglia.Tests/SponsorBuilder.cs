using System;
using DDDEastAnglia.DataAccess.SimpleData.Models;

namespace DDDEastAnglia.Tests
{
    internal class SponsorBuilder
    {
        private byte[] logo;
        private string name;
        private DateTime? paymentDate;
        private bool showPublicly = true;
        private int sponsorId;
        private int sponsorshipAmount;
        private string url;

        public Sponsor Build()
        {
            return new Sponsor
            {
                Logo = this.logo,
                Name = this.name,
                PaymentDate = this.paymentDate,
                ShowPublicly = this.showPublicly,
                SponsorId = this.sponsorId,
                SponsorshipAmount = this.sponsorshipAmount,
                Url = this.url
            };
        }

        public SponsorBuilder WithName(string name)
        {
            this.name = name;
            return this;
        }

        public SponsorBuilder WithSponsorshipAmount(int sponsorshipAmount)
        {
            this.sponsorshipAmount = sponsorshipAmount;
            return this;
        }

        public SponsorBuilder WithShowPublicly(bool showPublicly)
        {
            this.showPublicly = showPublicly;
            return this;
        }

        public SponsorBuilder WithPaymentDate(DateTime? paymentDate)
        {
            this.paymentDate = paymentDate;
            return this;
        }
    }
}