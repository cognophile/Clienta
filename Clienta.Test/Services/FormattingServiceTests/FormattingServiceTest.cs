using Clienta.Services;
using NUnit.Framework;
using System;

namespace Clienta.Test.Services.FormattingServiceTests
{
    [TestFixture]
    public class FormattingServiceTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void ConvertToShortFormUkFormat_WhenGivenDateTimeObject_ThenExpectedDateStringIsReturned()
        {
            var date = DateTime.Parse("1984/01/24");
            var expected = "24/01/1984";

            var actual = FormattingService.ConvertToShortUkFormat(date);

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
