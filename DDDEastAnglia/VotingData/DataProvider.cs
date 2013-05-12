using System;
using System.Collections.Generic;
using System.Linq;
using DDDEastAnglia.DataAccess.EntityFramework;
using DDDEastAnglia.DataAccess.EntityFramework.Models;
using DDDEastAnglia.Domain.Calendar;
using DDDEastAnglia.Helpers;
using DDDEastAnglia.VotingData.Models;
using DDDEastAnglia.VotingData.Queries;

namespace DDDEastAnglia.VotingData
{
    public interface IDataProvider
    {
        int GetTotalVoteCount();
        DateTime GetVotingStartDate();
        DateTime GetVotingEndDate();
        int GetNumberOfDaysOfVoting();
        int GetNumberOfDaysSinceVotingOpened();
        int GetNumberOfDaysUntilVotingCloses();
        int GetNumberOfUsersWhoHaveVoted();
        IList<SessionLeaderBoardEntry> GetLeaderBoard(int limit);
        IList<VotesForIPAddressModel> GetDistinctIPAddresses();
        IList<DateTimeVoteModel> GetVotesPerDay();
        IList<DateTimeVoteModel> GetVotesPerHour();
        IList<IPAddressVoterModel> GetVotersPerIPAddress();
        IList<CookieVoteModel> GetVotesPerIPAddress(string ipAddress);
        IList<KnownUserVoteCountModel> GetKnownUserVotes();
        IList<VotedSessionModel> GetVotedForSessions(int userId);
        IList<AnonymousUserVoteCountModel> GetAnonymousUserVotes();
        IList<VotedSessionModel> GetVotedForSessions(Guid cookieId);
    }

    public class DataProvider : IDataProvider
    {
        private readonly QueryRunner queryRunner;

        public DataProvider(QueryRunner queryRunner)
        {
            if (queryRunner == null)
            {
                throw new ArgumentNullException("queryRunner");
            }
            
            this.queryRunner = queryRunner;
        }

        public int GetTotalVoteCount()
        {
            using (var context = new DDDEAContext())
            {
                return context.Votes.Count();
            }
        }

        public DateTime GetVotingStartDate()
        {
            return GetVotingDates(votingDates => votingDates.StartDate.Date);
        }

        public DateTime GetVotingEndDate()
        {
            return GetVotingDates(votingDates => votingDates.EndDate.Value.Date);
        }

        public int GetNumberOfDaysOfVoting()
        {
            return GetVotingDates(votingDates =>
                {
                    var durationOfVoting = votingDates.EndDate - votingDates.StartDate;
                    return (int) durationOfVoting.Value.TotalDays;
                });
        }

        public int GetNumberOfDaysSinceVotingOpened()
        {
            return GetVotingDates(votingDates =>
                {
                    var durationSinceVotingOpened = DateTime.Today - votingDates.StartDate;
                    return (int) durationSinceVotingOpened.TotalDays;
                });
        }

        public int GetNumberOfDaysUntilVotingCloses()
        {
            return GetVotingDates(votingDates =>
                {
                    var durationUnitVotingCloses = votingDates.EndDate - DateTime.Today;
                    return (int) durationUnitVotingCloses.Value.TotalDays;
                });
        }

        private T GetVotingDates<T>(Func<CalendarItem, T> callback)
        {
            using (var context = new DDDEAContext())
            {
                CalendarItem votingDates = context.Conferences.Include("CalendarItems")
                                                  .First()
                                                  .CalendarItems
                                                  .Single(c => c.EntryType == CalendarEntryType.Voting);
                return callback(votingDates);
            }
        }

        public int GetNumberOfUsersWhoHaveVoted()
        {
            using (var context = new DDDEAContext())
            {
                return context.Votes.GroupBy(v => v.CookieId).Count();
            }
        }

        public IList<SessionLeaderBoardEntry> GetLeaderBoard(int limit)
        {
            var leaderBoardSessions = queryRunner.RunQuery(new LeaderBoardQuery(limit));
            return leaderBoardSessions;
        }

        public IList<VotesForIPAddressModel> GetDistinctIPAddresses()
        {
            var distinctIPAddresses = queryRunner.RunQuery(new DistinctIPAddressQuery());
            return distinctIPAddresses;
        }

        public IList<DateTimeVoteModel> GetVotesPerDay()
        {
            using (var context = new DDDEAContext())
            {
                var dateToCountDictionary = context.Votes.AsEnumerable()
                                                   .GroupBy(v => v.TimeRecorded.Date)
                                                   .ToDictionary(g => g.Key, g => g.Count());

                var votingStartDate = GetVotingStartDate();
                var votingEndDate = GetVotingEndDate();
                var dateTimeVoteModels = new List<DateTimeVoteModel>();

                for (var day = votingStartDate.Date; day <= votingEndDate; day = day.AddDays(1))
                {
                    int count;
                    dateToCountDictionary.TryGetValue(day, out count);
                    var model = new DateTimeVoteModel
                        {
                            Date = day, 
                            VoteCount = count
                        };
                    dateTimeVoteModels.Add(model);
                }

                return dateTimeVoteModels;
            }
        }

        public IList<DateTimeVoteModel> GetVotesPerHour()
        {
            using (var context = new DDDEAContext())
            {
                var dateToCountDictionary = context.Votes.AsEnumerable()
                                                   .GroupBy(v => v.TimeRecorded.TimeOfDay.Hours)
                                                   .ToDictionary(g => g.Key, g => g.Count());

                var dateTimeVoteModels = new List<DateTimeVoteModel>();

                for (var hour = 0; hour < 24; hour++)
                {
                    int count;
                    dateToCountDictionary.TryGetValue(hour, out count);
                    var now = DateTime.UtcNow;
                    var time = new DateTime(now.Year, now.Month, now.Day, hour, 0, 0, 0);
                    var model = new DateTimeVoteModel
                        {
                            Date = time, 
                            VoteCount = count
                        };
                    dateTimeVoteModels.Add(model);
                }

                return dateTimeVoteModels;
            }
        }

        public IList<IPAddressVoterModel> GetVotersPerIPAddress()
        {
            var votersPerIPAddress = queryRunner.RunQuery(new VotersPerIPAddressQuery());
            return votersPerIPAddress;
        }

        public IList<CookieVoteModel> GetVotesPerIPAddress(string ipAddress)
        {
            var votesPerIPAddress = queryRunner.RunQuery(new VotesPerIPAddressQuery(ipAddress));
            return votesPerIPAddress;
        }

        public IList<KnownUserVoteCountModel> GetKnownUserVotes()
        {
            var knownUserVotes = queryRunner.RunQuery(new KnownUsersVotingQuery(new GravatarUrl()));
            return knownUserVotes;
        }

        public IList<VotedSessionModel> GetVotedForSessions(int userId)
        {
            var sessionsVotedFor = queryRunner.RunQuery(new KnownUserVotedSessionsQuery(userId));
            return sessionsVotedFor;
        }

        public IList<AnonymousUserVoteCountModel> GetAnonymousUserVotes()
        {
            var anonymousUserVotes = queryRunner.RunQuery(new AnonymousUsersVotingQuery(new GravatarUrl()));
            return anonymousUserVotes;
        }

        public IList<VotedSessionModel> GetVotedForSessions(Guid cookieId)
        {
            var sessionsVotedFor = queryRunner.RunQuery(new AnonymousUserVotedSessionsQuery(cookieId));
            return sessionsVotedFor;
        }
    }
}
