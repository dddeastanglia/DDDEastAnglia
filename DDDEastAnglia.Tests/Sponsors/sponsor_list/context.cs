using System;
using System.Collections.Generic;
using DDDEastAnglia.Controllers;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Tests.Sponsors.sponsor_list
{
    public class context
    {
        protected ISponsorRepository sponsorRepository;
        protected IEnumerable<SponsorModel> sponsor_list;
        private SponsorModelQuery sponsorModelQuery;

        public context()
        {
            sponsorRepository = new InMemorySponsorRepository();
            sponsorModelQuery = new SponsorModelQuery(sponsorRepository);
        }

        protected void Given_sponsor(string name, int amount = 0, bool showPublically = true, DateTime? paymentDate = null)
        {
            var sponsorBuilder = new SponsorBuilder()
                .WithName(name)
                .WithShowPublicly(showPublically)
                .WithSponsorshipAmount(amount)
                .WithPaymentDate(paymentDate);
            
            sponsorRepository.AddSponsor(sponsorBuilder.Build());
        }

        protected void Given_unpaid_sponsor(string name)
        {
            Given_sponsor(name, 0, false);
        }

        protected void When_getting_sponsor_list()
        {
            sponsor_list = sponsorModelQuery.Get();
        }
    }
}
