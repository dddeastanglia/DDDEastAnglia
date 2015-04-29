using System;
using System.Collections.Generic;
using DDDEastAnglia.Areas.Admin.Models;
using DDDEastAnglia.Controllers;
using DDDEastAnglia.DataAccess;

namespace DDDEastAnglia.Tests.Sponsors.sponsor_list
{
    public class context
    {
        protected IEnumerable<SponsorModel> sponsor_list;
        private readonly SponsorModelQuery _sponsorModelQuery;
        private readonly ISponsorRepository _sponsorRepository;

        public context()
        {
            _sponsorRepository = new InMemorySponsorRepository();
            _sponsorModelQuery = new SponsorModelQuery(_sponsorRepository);
        }

        protected void Given_premium_sponsor(string name, DateTime? paymentDate = null)
        {
            var sponsor = new SponsorBuilder()
                .PremiumSponsor(name)
                .WithPaymentDate(paymentDate)
                .Build();

            _sponsorRepository.AddSponsor(sponsor);
        }

        protected void Given_gold_sponsor(string name, DateTime? paymentDate = null)
        {
            var sponsor = new SponsorBuilder()
                .GoldSponsor(name)
                .WithPaymentDate(paymentDate)
                .Build();

            _sponsorRepository.AddSponsor(sponsor);
        }

        protected void Given_standard_sponsor(string name, DateTime? paymentDate = null)
        {
            var sponsor = new SponsorBuilder()
                .StandardSponsor(name)
                .WithPaymentDate(paymentDate)
                .Build();

            _sponsorRepository.AddSponsor(sponsor);
        }

        protected void Given_unpaid_sponsor(string name)
        {
            var sponsor = new SponsorBuilder()
                .UnPaidSponsor(name)
                .Build();

            _sponsorRepository.AddSponsor(sponsor);
        }

        protected void When_getting_sponsor_list()
        {
            sponsor_list = _sponsorModelQuery.Get();
        }
    }
}
