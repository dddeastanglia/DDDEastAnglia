using System;
using DDDEastAnglia.Helpers;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Helpers
{
    [TestFixture]
    public sealed class DateTimePassedEvaluatorTests
    {
        [Test]
        public void HasDatePassed_ReturnsTrue_WhenTheSuppliedDateHasPassed()
        {
            var currentDate = DateTimeOffset.Now;
            var dateToEvaluate = currentDate - TimeSpan.FromSeconds(1);
            var evaluator = CreateEvaluator(currentDate);

            var hasDatePassed = evaluator.HasDatePassed(dateToEvaluate);
            Assert.That(hasDatePassed, Is.True);
        }

        [Test]
        public void HasDatePassed_ReturnsTrue_WhenTheSuppliedDateIsOnTheThreshold()
        {
            var currentDate = DateTimeOffset.Now;
            var dateToEvaluate = currentDate;
            var evaluator = CreateEvaluator(currentDate);

            var hasDatePassed = evaluator.HasDatePassed(dateToEvaluate);
            Assert.That(hasDatePassed, Is.True);
        }

        [Test]
        public void HasDatePassed_ReturnsFalse_WhenTheSuppliedDateHasNotPassed()
        {
            var currentDate = DateTimeOffset.Now;
            var dateToEvaluate = currentDate + TimeSpan.FromSeconds(1);
            var evaluator = CreateEvaluator(currentDate);

            var hasDatePassed = evaluator.HasDatePassed(dateToEvaluate);
            Assert.That(hasDatePassed, Is.False);
        }

        [Test]
        public void HasDatePassed_HandlesDifferentTimeZonesCorrectly_WhenTheSuppliedDateHasPassed()
        {
            var currentDate = new DateTimeOffset(2010, 07, 21, 12, 00, 00, TimeSpan.FromHours(1));    // 11:00 UTC
            var dateToEvaluate = new DateTimeOffset(2010, 07, 21, 16, 30, 00, TimeSpan.FromHours(6)); // 10:30 UTC
            var evaluator = CreateEvaluator(currentDate);

            var hasDatePassed = evaluator.HasDatePassed(dateToEvaluate);
            Assert.That(hasDatePassed, Is.True);
        }

        [Test]
        public void HasDatePassed_HandlesDifferentTimeZonesCorrectly_WhenTheSuppliedDateIsOnTheThreshold()
        {
            var currentDate = new DateTimeOffset(2010, 07, 21, 12, 00, 00, TimeSpan.FromHours(1));    // 11:00 UTC
            var dateToEvaluate = new DateTimeOffset(2010, 07, 21, 17, 00, 00, TimeSpan.FromHours(6)); // 11:00 UTC
            var evaluator = CreateEvaluator(currentDate);

            var hasDatePassed = evaluator.HasDatePassed(dateToEvaluate);
            Assert.That(hasDatePassed, Is.True);
        }

        [Test]
        public void HasDatePassed_HandlesDifferentTimeZonesCorrectly_WhenTheSuppliedDateHasNotPassed()
        {
            var currentDate = new DateTimeOffset(2010, 07, 21, 12, 00, 00, TimeSpan.FromHours(1));    // 11:00 UTC
            var dateToEvaluate = new DateTimeOffset(2010, 07, 21, 17, 30, 00, TimeSpan.FromHours(6)); // 11:30 UTC
            var evaluator = CreateEvaluator(currentDate);

            var hasDatePassed = evaluator.HasDatePassed(dateToEvaluate);
            Assert.That(hasDatePassed, Is.False);
        }

        private DateTimePassedEvaluator CreateEvaluator(DateTimeOffset currentDate)
        {
            var dateTimeOffsetProvider = new CannedResponseDateTimeOffsetProvider();
            dateTimeOffsetProvider.SetCurrentValue(currentDate);
            return new DateTimePassedEvaluator(dateTimeOffsetProvider);
        }
    }
}
