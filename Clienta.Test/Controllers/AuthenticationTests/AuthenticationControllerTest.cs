using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Clienta.Controllers;
using Clienta.Services.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Clienta.Test.Controllers.AuthenticationTests
{
    [TestFixture]

    public class AuthenticationControllerTest
    {
        [SetUp]
        public void SetUp()
        {
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void SignOut_WhenRequested_ThenViewResultReturned()
        {
            var controller = new AuthenticationController();

            ViewResult actual = controller.SignIn() as ViewResult;

            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<ViewResult>(actual);
        }

        [Test]
        public void Register_WhenRequested_ThenViewResultReturned()
        {
            var controller = new AuthenticationController();

            ViewResult actual = controller.Register() as ViewResult;

            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<ViewResult>(actual);
        }

        [Test]
        public void SignIn_WhenRequested_ThenViewResultMatchingTheExpectedViewTypeReturned()
        {
            var controller = new AuthenticationController();

            ViewResult actual = controller.SignIn() as ViewResult;

            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<ViewResult>(actual);
            Assert.IsTrue(actual.ViewName == "SignIn");
        }

        [Test]
        public void Register_WhenRequested_ThenViewResultMatchingTheExpectedViewTypeReturned()
        {
            var controller = new AuthenticationController();

            ViewResult actual = controller.Register() as ViewResult;

            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<ViewResult>(actual);
            Assert.IsTrue(actual.ViewName == "Register");
        }

        [Test]
        public void SignOut_WhenUnauthenticatedRequestedReceived_ThenRedirectToSignInViewReturned()
        {
            var controller = new AuthenticationController();

            var response = controller.SignOut();
            var actual = response.Result as RedirectToActionResult;

            Assert.IsNotNull(response);
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.ActionName == "SignIn");
            Assert.IsTrue(actual.ControllerName == "Authentication");
        }

    }
}
