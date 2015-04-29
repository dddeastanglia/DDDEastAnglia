using System;
using DDDEastAnglia.DataAccess.SimpleData.Models;

namespace DDDEastAnglia.Tests
{
    internal class SponsorBuilder
    {
        private readonly byte[] _logo;
        private string _name = "Sponsor";
        private DateTime? _paymentDate = DateTime.UtcNow;
        private bool _showPublicly = true;
        private int _sponsorId;
        private int _sponsorshipAmount = 1000;
        private string _url = "http://Sponsor.com";

        public SponsorBuilder()
        {

        }
        
        public SponsorBuilder UnPaidSponsor(string name)
        {
            _showPublicly = false;
            _sponsorshipAmount = 0;
            _name = name;
            return this;
        }

        public SponsorBuilder PremiumSponsor(string name)
        {
            _showPublicly = true;
            _sponsorshipAmount = 2000;
            _name = name;
            return this;
        }

        public SponsorBuilder GoldSponsor(string name)
        {
            _name = name;
            _sponsorshipAmount = 1000;
            return this;
        }

        public SponsorBuilder StandardSponsor(string name)
        {
            _name = name;
            _sponsorshipAmount = 500;
            return this;
        }

        public SponsorBuilder WithPaymentDate(DateTime? paymentDate)
        {
            this._paymentDate = paymentDate;
            return this;
        }

        public Sponsor Build()
        {
            return new Sponsor
            {
                Logo = _logo,
                Name = _name,
                PaymentDate = _paymentDate,
                ShowPublicly = _showPublicly,
                SponsorId = _sponsorId,
                SponsorshipAmount = _sponsorshipAmount,
                Url = _url
            };
        }
    }
}