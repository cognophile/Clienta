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

namespace Clienta.Test.Controllers.ClientTest
{
    [TestFixture]
    public class ClientControllerTest
    {
        private List<Client> _clientStub;
        private List<Client> _clientsStub;
        private ClaimsPrincipal _user;
        private Mock<IAuthenticationService> _mockAuthService;
        private Mock<IClientRepository> _mockRepository;
        private Mock<IPaginationService> _mockService;

        [SetUp]
        public void SetUp()
        {
            _clientStub = new List<Client>();
            _clientStub.Add(ClientProvider.GetOneTestClient());
            _clientsStub = ClientProvider.GetAllTestClients();

            _mockAuthService = new Mock<IAuthenticationService>();
            _mockRepository = new Mock<IClientRepository>();
            _mockService = new Mock<IPaginationService>();

            _mockService
                .Setup(s => s.Page)
                .Returns(1);
            _mockService
                .Setup(s => s.PageSize)
                .Returns(10);

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
        public async Task GetAll_WhenAuthenticatedUserRequestsPage_ThenViewResultWithListOfClientsReturned()
        {
            _mockAuthService
                .Setup(a => a.IsAuthenticated(_user))
                .Returns(true);
            _mockRepository
                .Setup(r => r.BrowseAsync(null))
                .ReturnsAsync(_clientsStub);
            _mockService
                .Setup(s => s.GetPageCount(_clientsStub.Count))
                .Returns(1);
            _mockService
                .Setup(s => s.GetPreviousPageIndex(1))
                .Returns(0);
            _mockService
                .Setup(s => s.GetNextPageIndex(1))
                .Returns(1);
            _mockService
                .Setup(s => s.IsPreviousPageAvailable(0))
                .Returns(false);
            _mockService
                .Setup(s => s.IsNextPageAvailable(1, 2))
                .Returns(false);

            var controller = new ClientController(_mockAuthService.Object, _mockRepository.Object, _mockService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = _user }
            };

            var actual = await controller.GetAll() as ViewResult;
            var modelList = actual.Model as List<Client>;

            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<ViewResult>(actual);
            Assert.That(2, Is.EqualTo(modelList.Count));
        }

        [Test]
        public async Task GetAll_WhenAuthenticatedAndSearchingForClientByFirstName_ThenViewResultListClientsWithSingleItemReturned()
        {
            var searchTerm = "Alice";
            _mockAuthService
                .Setup(a => a.IsAuthenticated(_user))
                .Returns(true);
            _mockRepository
                .Setup(r => r.BrowseAsync(searchTerm))
                .ReturnsAsync(_clientStub);
            _mockService
                .Setup(s => s.GetPageCount(1))
                .Returns(1);
            _mockService
                .Setup(s => s.GetPreviousPageIndex(1))
                .Returns(0);
            _mockService
                .Setup(s => s.GetNextPageIndex(1))
                .Returns(1);
            _mockService
                .Setup(s => s.IsPreviousPageAvailable(0))
                .Returns(false);
            _mockService
                .Setup(s => s.IsNextPageAvailable(1, 1))
                .Returns(false);

            var controller = new ClientController(_mockAuthService.Object, _mockRepository.Object, _mockService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = _user }
            };

            var actual = await controller.GetAll(1, searchTerm) as ViewResult;
            var modelList = actual.Model as List<Client>;

            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<ViewResult>(actual);
            Assert.That(1, Is.EqualTo(modelList.Count));
            Assert.That("Alice", Is.EqualTo(modelList[0].Forename));
        }

        [Test]
        public async Task GetAll_WhenUnAuthenticatedRequestReceived_ThenRedirectToSignInViewActionReturned()
        {
            _mockAuthService
                .Setup(a => a.IsAuthenticated(_user))
                .Returns(false);
            _mockRepository
                .Setup(r => r.BrowseAsync(null))
                .ReturnsAsync(_clientsStub);
            _mockService
                .Setup(s => s.GetPageCount(_clientsStub.Count))
                .Returns(1);
            _mockService
                .Setup(s => s.GetPreviousPageIndex(1))
                .Returns(0);
            _mockService
                .Setup(s => s.GetNextPageIndex(1))
                .Returns(1);
            _mockService
                .Setup(s => s.IsPreviousPageAvailable(0))
                .Returns(false);
            _mockService
                .Setup(s => s.IsNextPageAvailable(1, 2))
                .Returns(false);

            var controller = new ClientController(_mockAuthService.Object, _mockRepository.Object, _mockService.Object);

            var actual = await controller.GetAll() as RedirectToActionResult;

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.ActionName == "SignIn");
            Assert.IsTrue(actual.ControllerName == "Authentication");
        }

        [Test]
        public async Task GetOne_WhenAuthenticatedUserRequestsPage_ThenDetailViewResultWithClientsReturned()
        {
            _mockAuthService
                .Setup(a => a.IsAuthenticated(_user))
                .Returns(true);
            _mockRepository
                .Setup(r => r.ReadAsync(1))
                .ReturnsAsync(ClientProvider.GetOneTestClient());

            var controller = new ClientController(_mockAuthService.Object, _mockRepository.Object, _mockService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = _user }
            };

            var actual = await controller.GetOne(1) as ViewResult;
            var actualModel = actual.Model as Client;

            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<ViewResult>(actual);
            Assert.That("Detail", Is.EqualTo(actual.ViewName));
            Assert.That("Alice", Is.EqualTo(actualModel.Forename));
        }

        [Test]
        public async Task GetOne_WhenUnAuthenticatedRequestReceived_ThenRedirectToSignInViewActionReturned()
        {
            _mockAuthService
                .Setup(a => a.IsAuthenticated(_user))
                .Returns(false);
            _mockRepository
                .Setup(r => r.ReadAsync(1))
                .ReturnsAsync(ClientProvider.GetOneTestClient());

            var controller = new ClientController(_mockAuthService.Object, _mockRepository.Object, _mockService.Object);

            var actual = await controller.GetOne(1) as RedirectToActionResult;

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.ActionName == "SignIn");
            Assert.IsTrue(actual.ControllerName == "Authentication");
        }

        [Test]
        public void Create_WhenAuthenticatedRequestForViewReceived_ThenViewReturned()
        {
            _mockAuthService
                .Setup(a => a.IsAuthenticated(_user))
                .Returns(true);

            var controller = new ClientController(_mockAuthService.Object, _mockRepository.Object, _mockService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = _user }
            };

            var actual = controller.Create() as ViewResult;

            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<ViewResult>(actual);
            Assert.That("Create", Is.EqualTo(actual.ViewName));
        }

        [Test]
        public void Create_WhenNonAuthenticatedRequestForViewReceived_ThenRedirectToSignInViewActionReturned()
        {
            _mockAuthService
                .Setup(a => a.IsAuthenticated(_user))
                .Returns(false);

            var controller = new ClientController(_mockAuthService.Object, _mockRepository.Object, _mockService.Object);

            var actual = controller.Create() as RedirectToActionResult;

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.ActionName == "SignIn");
            Assert.IsTrue(actual.ControllerName == "Authentication");
        }

        [Test]
        public async Task Create_WhenAuthenticatedRequestReceived_ThenRedirectToReadViewActionReturned()
        {
            var client = ClientProvider.GetOnePreCreationTestClient();
            
            _mockAuthService
                .Setup(a => a.IsAuthenticated(_user))
                .Returns(true);
            _mockRepository
                .Setup(r => r.AddAsync(client))
                .ReturnsAsync(ClientProvider.GetOnePostCreationTestClient());

            var controller = new ClientController(_mockAuthService.Object, _mockRepository.Object, _mockService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = _user }
            };

            var actual = await controller.Create(client) as RedirectToActionResult;

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.ActionName == "Read");
            Assert.IsTrue(actual.ControllerName == "Client");
        }

        [Test]
        public async Task Create_WhenAuthenticatedRequestReceivedWithInvalidModelState_ThenRedirectToCreateViewActionReturned()
        {
            var client = ClientProvider.GetOnePreCreationTestClient();

            _mockAuthService
                .Setup(a => a.IsAuthenticated(_user))
                .Returns(true);
            _mockRepository
                .Setup(r => r.AddAsync(client))
                .ReturnsAsync(ClientProvider.GetOnePostCreationTestClient());

            var controller = new ClientController(_mockAuthService.Object, _mockRepository.Object, _mockService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = _user }
            };

            controller.ModelState.AddModelError("Forename", "Forename maximum length reached");
            var actual = await controller.Create(client) as ViewResult;

            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<ViewResult>(actual);
            Assert.That("Create", Is.EqualTo(actual.ViewName));
        }

        [Test]
        public async Task Create_WhenUnAuthenticatedRequestReceived_ThenRedirectToSignInViewActionReturned()
        {
            var client = ClientProvider.GetOnePreCreationTestClient();

            _mockAuthService
                .Setup(a => a.IsAuthenticated(_user))
                .Returns(false);
            _mockRepository
                .Setup(r => r.AddAsync(client))
                .ReturnsAsync(ClientProvider.GetOnePostCreationTestClient());

            var controller = new ClientController(_mockAuthService.Object, _mockRepository.Object, _mockService.Object);

            var actual = await controller.Create(client) as RedirectToActionResult;

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.ActionName == "SignIn");
            Assert.IsTrue(actual.ControllerName == "Authentication");
        }

        [Test]
        public async Task Edit_WhenAuthenticatedRequestReceived_ThenRedirectToReadProfileViewActionReturned()
        {
            var client = ClientProvider.GetOnePreEditTestClient();
            var editedClient = ClientProvider.GetOnePostEditTestClient();

            _mockAuthService
                .Setup(a => a.IsAuthenticated(_user))
                .Returns(true);
            _mockRepository
                .Setup(r => r.EditAsync(client.Id, editedClient))
                .ReturnsAsync(editedClient);

            var controller = new ClientController(_mockAuthService.Object, _mockRepository.Object, _mockService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = _user }
            };

            var actual = await controller.Edit(client.Id, editedClient) as RedirectToActionResult;

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.ActionName == "Read");
        }


        [Test]
        public async Task Edit_WhenAuthenticatedRequestReceivedWithInvalidModelState_ThenRedirectToReadViewActionReturned()
        {
            var client = ClientProvider.GetOnePreEditTestClient();
            var editedClient = ClientProvider.GetOnePostEditTestClient();

            _mockAuthService
                .Setup(a => a.IsAuthenticated(_user))
                .Returns(true);
            _mockRepository
                .Setup(r => r.EditAsync(client.Id, editedClient))
                .ReturnsAsync(editedClient);

            var controller = new ClientController(_mockAuthService.Object, _mockRepository.Object, _mockService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = _user }
            };

            controller.ModelState.AddModelError("Forename", "Forename maximum length reached");
            var actual = await controller.Edit(client.Id, editedClient) as RedirectToActionResult;

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.ActionName == "Read");
        }

        [Test]
        public async Task Edit_WhenUnAuthenticatedRequestReceived_ThenRedirectToSignInViewActionReturned()
        {
            var client = ClientProvider.GetOnePreEditTestClient();
            var editedClient = ClientProvider.GetOnePostEditTestClient();

            _mockAuthService
                .Setup(a => a.IsAuthenticated(_user))
                .Returns(false);
            _mockRepository
                .Setup(r => r.EditAsync(client.Id, editedClient))
                .ReturnsAsync(editedClient);

            var controller = new ClientController(_mockAuthService.Object, _mockRepository.Object, _mockService.Object);

            var actual = await controller.Edit(client.Id, editedClient) as RedirectToActionResult;

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.ActionName == "SignIn");
            Assert.IsTrue(actual.ControllerName == "Authentication");
        }

        [Test]
        public async Task Delete_WhenClientRecordFound_ThenRedirectToBrowseViewReturned()
        {
            _mockAuthService
                .Setup(a => a.IsAuthenticated(_user))
                .Returns(true);
            _mockRepository
                .Setup(r => r.DeleteAsync(1))
                .ReturnsAsync(ClientProvider.GetOneTestClient);

            var controller = new ClientController(_mockAuthService.Object, _mockRepository.Object, _mockService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = _user }
            };

            var actual = await controller.Delete(1) as RedirectToActionResult;

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.ActionName == "Browse");
        }

        [Test]
        public async Task Delete_WhenClientRecordNotFound_ThenNotFoundErrorViewReturned()
        {
            _mockAuthService
                .Setup(a => a.IsAuthenticated(_user))
                .Returns(true);
            _mockRepository
                .Setup(r => r.DeleteAsync(1))
                .ReturnsAsync(ClientProvider.GetOneTestClient);

            var controller = new ClientController(_mockAuthService.Object, _mockRepository.Object, _mockService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = _user }
            };

            var actual = await controller.Delete(5) as ViewResult;

            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<ViewResult>(actual);
            Assert.IsTrue(actual.ViewName == "_NotFound");
        }
    }
}
