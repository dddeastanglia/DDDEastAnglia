using System;
using System.Collections.Generic;
using DDDEastAnglia.Areas.Admin.Models;
using DDDEastAnglia.Controllers;
using DDDEastAnglia.DataAccess;

namespace DDDEastAnglia.Tests.Sponsors.Query
{
    public class Context
    {
        protected IEnumerable<SponsorModel> SponsorList;
        private readonly AllPublicSponsors allPublicSponsors;
        private readonly ISponsorRepository sponsorRepository;

        public Context()
        {
            sponsorRepository = new InMemorySponsorRepository();
            allPublicSponsors = new AllPublicSponsors(sponsorRepository, new DefaultSponsorSorter());
        }

        protected void Given_premium_sponsor(string name, DateTime? paymentDate = null)
        {
            var sponsor = new SponsorBuilder()
                .PremiumSponsor(name)
                .WithPaymentDate(paymentDate)
                .Build();

            sponsorRepository.AddSponsor(sponsor);
        }

        protected void Given_gold_sponsor(string name, DateTime? paymentDate = null)
        {
            var sponsor = new SponsorBuilder()
                .GoldSponsor(name)
                .WithPaymentDate(paymentDate)
                .Build();

            sponsorRepository.AddSponsor(sponsor);
        }

        protected void Given_standard_sponsor(string name, DateTime? paymentDate = null)
        {
            var sponsor = new SponsorBuilder()
                .StandardSponsor(name)
                .WithPaymentDate(paymentDate)
                .Build();

            sponsorRepository.AddSponsor(sponsor);
        }

        protected void Given_unpaid_sponsor(string name)
        {
            var sponsor = new SponsorBuilder()
                .UnPaidSponsor(name)
                .Build();

            sponsorRepository.AddSponsor(sponsor);
        }

        protected void When_getting_sponsor_list()
        {
            SponsorList = allPublicSponsors.Get();
        }
    }
}
