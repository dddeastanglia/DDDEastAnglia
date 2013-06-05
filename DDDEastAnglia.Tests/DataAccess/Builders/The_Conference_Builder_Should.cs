using System;
using System.Collections.Generic;
using DDDEastAnglia.DataAccess.EntityFramework.Builders;
using DDDEastAnglia.DataAccess.EntityFramework.Builders.Calendar;
using DDDEastAnglia.DataAccess.EntityFramework.Models;
using DDDEastAnglia.Domain.Calendar;
using NUnit.Framework;
using Conference = DDDEastAnglia.Domain.Conference;

namespace DDDEastAnglia.Tests.DataAccess.Builders
{
    [TestFixture]
    public class The_Conference_Builder_Should
    {
        [Test]
        public void Build_A_Completely_Closed_Conference_If_No_CalendarItems_Are_Supplied()
        {
            Given_A_DataModel_With_No_CalendarItems();
            When_I_Build_The_Domain_Model();
            Then_The_Conference_Is_Closed_For_Submission();
            And_The_Conference_Is_Closed_For_Voting();
            And_The_Conference_Is_Closed_For_Publishing_The_Agenda();
            And_The_Conference_Is_Closed_For_Registration();
        }

        [Test]
        public void Build_A_Conference_Open_For_Submission_If_The_CalendarItem_Is_Supplied()
        {
            Given_A_DataModel_With_An_Open_Submission_Calendar_Item();
            When_I_Build_The_Domain_Model();
            Then_The_Conference_Is_Open_For_Submission();
            And_The_Conference_Is_Closed_For_Voting();
            And_The_Conference_Is_Closed_For_Publishing_The_Agenda();
            And_The_Conference_Is_Closed_For_Registration();
        }

        [Test]
        public void Build_A_Conference_Open_For_Voting_If_The_CalendarItem_Is_Supplied()
        {
            Given_A_DataModel_With_An_Open_Voting_Calendar_Item();
            When_I_Build_The_Domain_Model();
            Then_The_Conference_Is_Closed_For_Submission();
            And_The_Conference_Is_Open_For_Voting();
            And_The_Conference_Is_Closed_For_Publishing_The_Agenda();
            And_The_Conference_Is_Closed_For_Registration();
        }

        [Test]
        public void Build_A_Conference_Open_For_Publishng_The_Agenda_If_The_CalendarItem_Is_Supplied()
        {
            Given_A_DataModel_With_An_Open_Publishing_The_Agenda_Calendar_Item();
            When_I_Build_The_Domain_Model();
            Then_The_Conference_Is_Closed_For_Submission();
            And_The_Conference_Is_Closed_For_Voting();
            And_The_Conference_Is_Open_For_Publishing_The_Agenda();
            And_The_Conference_Is_Closed_For_Registration();
        }

        [Test]
        public void Build_A_Conference_Open_For_Publishng_The_Agenda_And_For_Registration_If_The_CalendarItem_Is_Supplied()
        {
            Given_A_DataModel_With_An_Open_Publishing_The_Agenda_Calendar_Item_And_An_Open_Registration_Calendar_Item();
            When_I_Build_The_Domain_Model();
            Then_The_Conference_Is_Closed_For_Submission();
            And_The_Conference_Is_Closed_For_Voting();
            And_The_Conference_Is_Open_For_Publishing_The_Agenda();
            And_The_Conference_Is_Open_For_Registration();
        }

        private void Given_A_DataModel_With_No_CalendarItems()
        {
            _source = new DDDEastAnglia.DataAccess.EntityFramework.Models.Conference
                {
                    ConferenceId = 1,
                    Name = "",
                    ShortName = ""
                };
        }

        private void Given_A_DataModel_With_An_Open_Submission_Calendar_Item()
        {
            _source = new DDDEastAnglia.DataAccess.EntityFramework.Models.Conference
                {
                    ConferenceId = 1,
                    Name = "",
                    ShortName = "",
                    CalendarItems = new List<CalendarItem>
                        {
                            new CalendarItem
                                {
                                    EntryType = CalendarEntryType.SessionSubmission,
                                    Authorised = true,
                                    StartDate = DateTimeOffset.Now.AddDays(-1),
                                    EndDate = DateTimeOffset.Now.AddDays(1)
                                }
                        }
                };
        }

        private void Given_A_DataModel_With_An_Open_Voting_Calendar_Item()
        {
            _source = new DDDEastAnglia.DataAccess.EntityFramework.Models.Conference
            {
                ConferenceId = 1,
                Name = "",
                ShortName = "",
                CalendarItems = new List<CalendarItem>
                        {
                            new CalendarItem
                                {
                                    EntryType = CalendarEntryType.SessionSubmission,
                                    Authorised = true,
                                    StartDate = DateTimeOffset.Now.AddDays(-2),
                                    EndDate = DateTimeOffset.Now.AddDays(-1)
                                },
                            new CalendarItem
                                {
                                    EntryType = CalendarEntryType.Voting,
                                    Authorised = true,
                                    StartDate = DateTimeOffset.Now.AddDays(-1),
                                    EndDate = DateTimeOffset.Now.AddDays(1)
                                }
                        }
            };
        }

        public void Given_A_DataModel_With_An_Open_Publishing_The_Agenda_Calendar_Item()
        {
            _source = new DDDEastAnglia.DataAccess.EntityFramework.Models.Conference
            {
                ConferenceId = 1,
                Name = "",
                ShortName = "",
                CalendarItems = new List<CalendarItem>
                        {
                            new CalendarItem
                                {
                                    EntryType = CalendarEntryType.SessionSubmission,
                                    Authorised = true,
                                    StartDate = DateTimeOffset.Now.AddDays(-2),
                                    EndDate = DateTimeOffset.Now.AddDays(-1)
                                },
                            new CalendarItem
                                {
                                    EntryType = CalendarEntryType.Voting,
                                    Authorised = true,
                                    StartDate = DateTimeOffset.Now.AddDays(-2),
                                    EndDate = DateTimeOffset.Now.AddDays(-1)
                                },
                            new CalendarItem
                                {
                                    EntryType = CalendarEntryType.AgendaPublished,
                                    Authorised = true,
                                    StartDate = DateTimeOffset.Now.AddDays(-2),
                                    EndDate = null
                                }
                        }
            };
        }

        private void Given_A_DataModel_With_An_Open_Publishing_The_Agenda_Calendar_Item_And_An_Open_Registration_Calendar_Item()
        {
            _source = new DDDEastAnglia.DataAccess.EntityFramework.Models.Conference
            {
                ConferenceId = 1,
                Name = "",
                ShortName = "",
                CalendarItems = new List<CalendarItem>
                        {
                            new CalendarItem
                                {
                                    EntryType = CalendarEntryType.SessionSubmission,
                                    Authorised = true,
                                    StartDate = DateTimeOffset.Now.AddDays(-2),
                                    EndDate = DateTimeOffset.Now.AddDays(-1)
                                },
                            new CalendarItem
                                {
                                    EntryType = CalendarEntryType.Voting,
                                    Authorised = true,
                                    StartDate = DateTimeOffset.Now.AddDays(-2),
                                    EndDate = DateTimeOffset.Now.AddDays(-1)
                                },
                            new CalendarItem
                                {
                                    EntryType = CalendarEntryType.AgendaPublished,
                                    Authorised = true,
                                    StartDate = DateTimeOffset.Now.AddDays(-2),
                                    EndDate = null
                                },
                            new CalendarItem
                                {
                                    EntryType = CalendarEntryType.Registration,
                                    Authorised = true,
                                    StartDate = DateTimeOffset.Now.AddDays(-1),
                                    EndDate = DateTimeOffset.Now.AddDays(1)
                                }
                        }
            };
        }

        private void When_I_Build_The_Domain_Model()
        {
            var conferenceBuilder = new ConferenceBuilder(new CalendarEntryBuilder());
            _domainModel = conferenceBuilder.Build(_source);
        }

        private void Then_The_Conference_Is_Closed_For_Submission()
        {
            Assert.That(_domainModel.CanSubmit(), Is.False);
        }

        private void Then_The_Conference_Is_Open_For_Submission()
        {
            Assert.That(_domainModel.CanSubmit(), Is.True);
        }

        private void And_The_Conference_Is_Closed_For_Voting()
        {
            Assert.That(_domainModel.CanVote(), Is.False);
        }

        private void And_The_Conference_Is_Open_For_Voting()
        {
            Assert.That(_domainModel.CanVote(), Is.True);
        }

        private void And_The_Conference_Is_Closed_For_Publishing_The_Agenda()
        {
            Assert.That(_domainModel.CanPublishAgenda(), Is.False);
        }

        private void And_The_Conference_Is_Open_For_Publishing_The_Agenda()
        {
            Assert.That(_domainModel.CanPublishAgenda(), Is.True);
        }

        private void And_The_Conference_Is_Closed_For_Registration()
        {
            Assert.That(_domainModel.CanRegister(), Is.False);
        }

        private void And_The_Conference_Is_Open_For_Registration()
        {
            Assert.That(_domainModel.CanRegister(), Is.True);
        }

        private Conference _domainModel;
        private DDDEastAnglia.DataAccess.EntityFramework.Models.Conference _source;
    }
}