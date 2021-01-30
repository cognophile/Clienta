using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Clienta.Controllers;
using Clienta.Models;
using Clienta.Repositories.Interfaces;
using Clienta.Services.Interfaces;
using Clienta.Test.Providers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Clienta.Test.Controllers.AddressTests
{
    [TestFixture]
    public class AddressControllerTest
    {
        private ClaimsPrincipal _user;
        private Mock<IAuthenticationService> _mockAuthService;
        private Mock<IAddressRepository> _mockRepository;

        [SetUp]
        public void SetUp()
        {
            _mockAuthService = new Mock<IAuthenticationService>();
            _mockRepository = new Mock<IAddressRepository>();

            _user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "Alice"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
            }, "mock"));
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void Create_WhenAuthenticatedRequestForViewReceived_ThenPartialViewReturned()
        {
            _mockAuthService
                .Setup(a => a.IsAuthenticated(_user))
                .Returns(true);

            var controller = new AddressController(_mockAuthService.Object, _mockRepository.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = _user }
            };

            var actual = controller.Create() as PartialViewResult;

            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<PartialViewResult>(actual);
            Assert.That("_AddressModal", Is.EqualTo(actual.ViewName));
        }

        [Test]
        public void Create_WhenNonAuthenticatedRequestForViewReceived_ThenRedirectToSignInViewActionReturned()
        {
            _mockAuthService
                .Setup(a => a.IsAuthenticated(_user))
                .Returns(false);

            var controller = new AddressController(_mockAuthService.Object, _mockRepository.Object);

            var actual = controller.Create() as RedirectToActionResult;

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.ActionName == "SignIn");
            Assert.IsTrue(actual.ControllerName == "Authentication");
        }

        [Test]
        public async Task Create_WhenValidPayloadSentAndRequestSuccessful_ThenRedirectToProfileReadViewActionReturned()
        {
            var address = AddressProvider.GetPreCreationTestAddress(1);
            _mockAuthService
                .Setup(a => a.IsAuthenticated(_user))
                .Returns(true);

            _mockRepository
                .Setup(r => r.AddAsync(address))
                .ReturnsAsync(AddressProvider.GetPostCreationTestAddress(1));

            var controller = new AddressController(_mockAuthService.Object, _mockRepository.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = _user }
            };

            var response = await controller.Create(1, address) as PartialViewResult;
            var actual = response.Model as Address;

            Assert.IsNotNull(response);
            Assert.IsInstanceOf<PartialViewResult>(response);
            Assert.That("_AddressModal", Is.EqualTo(response.ViewName));
            Assert.That(1, Is.EqualTo(actual.ClientId));
            Assert.That(address.Postcode, Is.EqualTo(actual.Postcode));
        }
    }
}
