using NUnit.Framework;
using Clienta.Services;
using Clienta.Services.Interfaces;

namespace Clienta.Test.Services.PaginationServiceTests
{
    [TestFixture]
    public class PaginationServiceTest
    {
        private IPaginationService _pagionator;

        [SetUp]
        public void Setup()
        {
            _pagionator = new PaginationService();
        }

        [TearDown]
        public void TearDown()
        {
            _pagionator = null;
        }

        [Test]
        public void PaginationService_WhenInstantiated_InitialMemberPropertyValuesAreSetToDefaults()
        {
            var expectedPageSize = 10;
            
            Assert.That(expectedPageSize, Is.EqualTo(_pagionator.PageSize));
        }

        [Test]
        public void GetPageCount_WhenGivenZeroValueForCurrentPageArgument_ThenExpectedSinglePageCountReturned()
        {
            var expected = 1;

            var actual = _pagionator.GetPageCount(0);

            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public void GetPageCount_WhenGivenNonZeroValueForCurrentPageArgument_ThenExpectedMultiPageCountReturned()
        {
            var expected = 2;

            var actual = _pagionator.GetPageCount(15);

            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public void GetPreviousPageIndex_WhenGivenZeroValueForCurrentPageArgument_ThenExpectedZeroReturned()
        {
            var expected = 0;

            var actual = _pagionator.GetPreviousPageIndex(0);

            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public void GetPreviousPageIndex_WhenGivenNonZeroValueForCurrentPageArgument_ThenExpectedPreviousPageNumberReturned()
        {
            var expected = 4;

            var actual = _pagionator.GetPreviousPageIndex(5);

            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public void GetNextPageIndex_WhenGivenZeroValueForCurrentPageArgument_ThenExpectedFirstPageIndexReturned()
        {
            var expected = 1;

            var actual = _pagionator.GetNextPageIndex(0);

            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public void GetNextPageIndex_WhenGivenNonZeroValueCurrentPageArgument_ThenExpectedPreviousPageNumberReturned()
        {
            var expected = 5;

            var actual = _pagionator.GetNextPageIndex(4);

            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public void IsPreviousPageAvailable_WhenGivenZeroValueAsPreviousPageArgument_ThenConditionReturnsFalse()
        {
            var expected = false;

            var actual = _pagionator.IsPreviousPageAvailable(0);

            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public void IsPreviousPageAvailable_WhenGivenNonZeroValueAsPreviousPageArgument_ThenConditionReturnsTrue()
        {
            var expected = true;

            var actual = _pagionator.IsPreviousPageAvailable(4);

            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public void IsNextPageAvailable_WhenGivenZeroValuesAsArguments_ThenConditionReturnsFalse()
        {
            var expected = false;

            var actual = _pagionator.IsNextPageAvailable(0, 0);

            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public void IsNextPageAvailable_WhenGivenNonZeroValuesAsArgumentsWhenOnlyOnePageIsRequired_ThenConditionReturnsFalse()
        {
            var expected = false;

            var actual = _pagionator.IsNextPageAvailable(1, 1);

            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public void IsNextPageAvailable_WhenGivenZeroValueForPageCountArgument_ThenConditionReturnsFalse()
        {
            var expected = false;

            var actual = _pagionator.IsNextPageAvailable(3, 0);

            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public void IsNextPageAvailable_WhenGivenNonZeroValueArguments_ThenConditionReturnsTrue()
        {
            var expected = true;

            var actual = _pagionator.IsNextPageAvailable(3, 5);

            Assert.That(expected, Is.EqualTo(actual));
        }

    }
}