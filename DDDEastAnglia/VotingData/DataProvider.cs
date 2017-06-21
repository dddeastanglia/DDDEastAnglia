using System;
using System.Collections.Generic;
using System.Linq;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.DataAccess.SimpleData.Models;
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
        IList<SessionLeaderBoardEntry> GetLeaderBoard(int limit, bool allowDuplicateSpeakers);
        IList<VotesForIPAddressModel> GetDistinctIPAddresses();
        IList<DateTimeVoteModel> GetVotesPerDate();
        IList<DayOfWeekVoteModel> GetVotesPerDay();
        IList<DateTimeVoteModel> GetVotesPerHour();
        IList<NumberOfUsersWithVotesModel> GetNumberOfVotesCastCounts();
        IList<IPAddressVoterModel> GetVotersPerIPAddress();
        IList<CookieVoteModel> GetVotesPerIPAddress(string ipAddress);
        IList<KnownUserVoteCountModel> GetKnownUserVotes();
        IList<VotedSessionModel> GetVotedForSessions(int userId);
        IList<AnonymousUserVoteCountModel> GetAnonymousUserVotes();
        IList<VotedSessionModel> GetVotedForSessions(Guid cookieId);
        IList<DuplicateVoteModel> GetDuplicateVotes();
        IList<SessionVoterModel> GetVotersForSession(int sessionId);
        IList<VotersPerIPAddressForSessionModel> GetIPAddressesThatVotedForSession(int sessionId);
    }

    public class DataProvider : IDataProvider
    {
        private readonly QueryRunner queryRunner;
        private readonly IVoteRepository voteRepository;
        private readonly ICalendarItemRepository calendarItemRepository;

        public DataProvider(QueryRunner queryRunner, IVoteRepository voteRepository, ICalendarItemRepository calendarItemRepository)
        {
            if (queryRunner == null)
            {
                throw new ArgumentNullException(nameof(queryRunner));
            }

            if (voteRepository == null)
            {
                throw new ArgumentNullException(nameof(voteRepository));
            }

            if (calendarItemRepository == null)
            {
                throw new ArgumentNullException(nameof(calendarItemRepository));
            }

            this.queryRunner = queryRunner;
            this.voteRepository = voteRepository;
            this.calendarItemRepository = calendarItemRepository;
        }

        public int GetTotalVoteCount()
        {
            return voteRepository.GetAllVotes().Count();
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
                    return (int) Math.Max(0, durationUnitVotingCloses.Value.TotalDays);
                });
        }

        private T GetVotingDates<T>(Func<CalendarItem, T> callback)
        {
            var votingDates = calendarItemRepository.GetFromType(CalendarEntryType.Voting);
            return callback(votingDates);
        }

        public int GetNumberOfUsersWhoHaveVoted()
        {
            return voteRepository.GetAllVotes().GroupBy(v => v.CookieId).Count();
        }

        public IList<SessionLeaderBoardEntry> GetLeaderBoard(int limit, bool allowDuplicateSpeakers)
        {
            var leaderBoardSessions = queryRunner.RunQuery(new LeaderBoardQuery(limit, allowDuplicateSpeakers));
            return leaderBoardSessions;
        }

        public IList<VotesForIPAddressModel> GetDistinctIPAddresses()
        {
            var distinctIPAddresses = queryRunner.RunQuery(new DistinctIPAddressQuery());
            return distinctIPAddresses;
        }

        public IList<DateTimeVoteModel> GetVotesPerDate()
        {
            var dateToCountDictionary = voteRepository.GetAllVotes()
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

        public IList<DayOfWeekVoteModel> GetVotesPerDay()
        {
            var dateToCountDictionary = voteRepository.GetAllVotes()
                                                .GroupBy(v => v.TimeRecorded.DayOfWeek)
                                                .ToDictionary(g => g.Key, g => g.Count());

            var dateTimeVoteModels = new List<DayOfWeekVoteModel>();

            var days = (DayOfWeek[]) Enum.GetValues(typeof(DayOfWeek));

            foreach (var day in days)
            {
                int count;
                dateToCountDictionary.TryGetValue(day, out count);
                var model = new DayOfWeekVoteModel
                    {
                        Day = day,
                        VoteCount = count
                    };
                dateTimeVoteModels.Add(model);
            }

            return dateTimeVoteModels;
        }

        public IList<DateTimeVoteModel> GetVotesPerHour()
        {
            var dateToCountDictionary = voteRepository.GetAllVotes()
                                                .GroupBy(v => v.TimeRecorded.TimeOfDay.Hours)
                                                .ToDictionary(g => g.Key, g => g.Count());

            var dateTimeVoteModels = new List<DateTimeVoteModel>();

            for (var hour = 0; hour < 24; hour++)
            {
                int count;
                dateToCountDictionary.TryGetValue(hour, out count);
                var now = DateTime.UtcNow;
                var time = new DateTime(now.Year, now.Month, now.Day, hour, 0, 0, 0);
                var model = new DateTimeVoteModel { Date = time, VoteCount = count };
                dateTimeVoteModels.Add(model);
            }

            return dateTimeVoteModels;
        }

        public IList<NumberOfUsersWithVotesModel> GetNumberOfVotesCastCounts()
        {
            var numberOfVotesCastCount = queryRunner.RunQuery(new NumberOfUsersWhoHaveVotedXTimesQuery());
            return numberOfVotesCastCount;
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

        public IList<DuplicateVoteModel> GetDuplicateVotes()
        {
            var duplicateVotes = queryRunner.RunQuery(new UsersWhoHaveVotedForTheSameSessionMoreThanOnceQuery(new GravatarUrl()));
            return duplicateVotes;
        }

        public IList<SessionVoterModel> GetVotersForSession(int sessionId)
        {
            var votersForSession = queryRunner.RunQuery(new VotersForSessionQuery(new GravatarUrl(), sessionId));
            return votersForSession;
        }

        public IList<VotersPerIPAddressForSessionModel> GetIPAddressesThatVotedForSession(int sessionId)
        {
            var ipAddressesThatVotedForSession = queryRunner.RunQuery(new VotersPerIPAddressForSessionQuery(sessionId));
            return ipAddressesThatVotedForSession;
        }
    }
}
