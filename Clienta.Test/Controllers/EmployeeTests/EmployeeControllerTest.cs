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

namespace Clienta.Test.Controllers.EmployeeTest
{
    [TestFixture]

    public class EmployeeControllerTest
    {
        private List<Employee> _employeeStub;
        private List<Employee> _employeesStub;
        private ClaimsPrincipal _user;
        private Mock<IAuthenticationService> _mockAuthService;
        private Mock<IEmployeeRepository> _mockRepository;
        private Mock<IPaginationService> _mockService;

        [SetUp]
        public void SetUp()
        {
            _employeeStub = new List<Employee>();
            _employeeStub.Add(EmployeeProvider.GetOneTestEmployee());
            _employeesStub = EmployeeProvider.GetAllTestEmployees();

            _mockAuthService = new Mock<IAuthenticationService>();
            _mockRepository = new Mock<IEmployeeRepository>();
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
        public async Task GetAll_WhenAuthenticatedUserRequestsPage_ThenViewResultWithListOfEmployeesReturned()
        {
            _mockAuthService
                .Setup(a => a.IsAuthenticated(_user))
                .Returns(true);
            _mockRepository
                .Setup(r => r.BrowseAsync(null))
                .ReturnsAsync(_employeesStub);
            _mockService
                .Setup(s => s.GetPageCount(_employeesStub.Count))
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

            var controller = new EmployeeController(_mockAuthService.Object, _mockRepository.Object, _mockService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = _user }
            };

            var actual = await controller.GetAll() as ViewResult;
            var modelList = actual.Model as List<Employee>;

            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<ViewResult>(actual);
            Assert.That(2, Is.EqualTo(modelList.Count));
        }

        [Test]
        public async Task GetAll_WhenAuthenticatedAndSearchingForEmployeeByFirstName_ThenViewResultWithListEmployeesWithSingleItemReturned()
        {
            var searchTerm = "Alice";
            _mockAuthService
                .Setup(a => a.IsAuthenticated(_user))
                .Returns(true);
            _mockRepository
                .Setup(r => r.BrowseAsync(searchTerm))
                .ReturnsAsync(_employeeStub);
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

            var controller = new EmployeeController(_mockAuthService.Object, _mockRepository.Object, _mockService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = _user }
            };

            var actual = await controller.GetAll(1, searchTerm) as ViewResult;
            var modelList = actual.Model as List<Employee>;

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
                .ReturnsAsync(_employeesStub);
            _mockService
                .Setup(s => s.GetPageCount(_employeesStub.Count))
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

            var controller = new EmployeeController(_mockAuthService.Object, _mockRepository.Object, _mockService.Object);

            var actual = await controller.GetAll() as RedirectToActionResult;

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.ActionName == "SignIn");
            Assert.IsTrue(actual.ControllerName == "Authentication");
        }

        [Test]
        public async Task GetOne_WhenAuthenticatedUserRequestsPage_ThenADetailViewResultWithEmployeesReturned()
        {
            _mockAuthService
                .Setup(a => a.IsAuthenticated(_user))
                .Returns(true);
            _mockRepository
                .Setup(r => r.ReadAsync(1))
                .ReturnsAsync(EmployeeProvider.GetOneTestEmployee());

            var controller = new EmployeeController(_mockAuthService.Object, _mockRepository.Object, _mockService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = _user }
            };

            var actual = await controller.GetOne(1) as ViewResult;
            var actualModel = actual.Model as Employee;

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
                .ReturnsAsync(EmployeeProvider.GetOneTestEmployee());

            var controller = new EmployeeController(_mockAuthService.Object, _mockRepository.Object, _mockService.Object);

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

            var controller = new EmployeeController(_mockAuthService.Object, _mockRepository.Object, _mockService.Object);
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

            var controller = new EmployeeController(_mockAuthService.Object, _mockRepository.Object, _mockService.Object);

            var actual = controller.Create() as RedirectToActionResult;

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.ActionName == "SignIn");
            Assert.IsTrue(actual.ControllerName == "Authentication");
        }

        [Test]
        public async Task Create_WhenAuthenticatedRequestReceived_ThenRedirectToReadViewActionReturned()
        {
            var employee = EmployeeProvider.GetOnePreCreationTestEmployee();
            
            _mockAuthService
                .Setup(a => a.IsAuthenticated(_user))
                .Returns(true);
            _mockRepository
                .Setup(r => r.AddAsync(employee))
                .ReturnsAsync(EmployeeProvider.GetOnePostCreationTestEmployee());

            var controller = new EmployeeController(_mockAuthService.Object, _mockRepository.Object, _mockService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = _user }
            };

            var actual = await controller.Create(employee) as RedirectToActionResult;

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.ActionName == "Read");
            Assert.IsTrue(actual.ControllerName == "Employee");
        }

        [Test]
        public async Task Create_WhenAuthenticatedRequestReceivedWithInvalidModelState_ThenRedirectToCreateViewActionReturned()
        {
            var employee = EmployeeProvider.GetOnePreCreationTestEmployee();

            _mockAuthService
                .Setup(a => a.IsAuthenticated(_user))
                .Returns(true);
            _mockRepository
                .Setup(r => r.AddAsync(employee))
                .ReturnsAsync(EmployeeProvider.GetOnePostCreationTestEmployee());

            var controller = new EmployeeController(_mockAuthService.Object, _mockRepository.Object, _mockService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = _user }
            };

            controller.ModelState.AddModelError("Forename", "Forename maximum length reached");
            var actual = await controller.Create(employee) as ViewResult;

            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<ViewResult>(actual);
            Assert.That("Create", Is.EqualTo(actual.ViewName));
        }

        [Test]
        public async Task Create_WhenUnAuthenticatedRequestReceived_ThenRedirectToSignInViewActionReturned()
        {
            var employee = EmployeeProvider.GetOnePreCreationTestEmployee();

            _mockAuthService
                .Setup(a => a.IsAuthenticated(_user))
                .Returns(false);
            _mockRepository
                .Setup(r => r.AddAsync(employee))
                .ReturnsAsync(EmployeeProvider.GetOnePostCreationTestEmployee());

            var controller = new EmployeeController(_mockAuthService.Object, _mockRepository.Object, _mockService.Object);

            var actual = await controller.Create(employee) as RedirectToActionResult;

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.ActionName == "SignIn");
            Assert.IsTrue(actual.ControllerName == "Authentication");
        }

        [Test]
        public async Task Edit_WhenAuthenticatedRequestReceived_ThenRedirectToReadProfileViewActionReturned()
        {
            var employee = EmployeeProvider.GetOnePreEditTestEmployee();
            var editedEmployee = EmployeeProvider.GetOnePostEditTestEmployee();

            _mockAuthService
                .Setup(a => a.IsAuthenticated(_user))
                .Returns(true);
            _mockRepository
                .Setup(r => r.EditAsync(employee.Id, editedEmployee))
                .ReturnsAsync(editedEmployee);

            var controller = new EmployeeController(_mockAuthService.Object, _mockRepository.Object, _mockService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = _user }
            };

            var actual = await controller.Edit(employee.Id, editedEmployee) as RedirectToActionResult;

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.ActionName == "Read");
        }


        [Test]
        public async Task Edit_WhenAuthenticatedRequestReceivedWithInvalidModelState_ThenRedirectToReadViewActionReturned()
        {
            var employee = EmployeeProvider.GetOnePreEditTestEmployee();
            var editedEmployee = EmployeeProvider.GetOnePostEditTestEmployee();

            _mockAuthService
                .Setup(a => a.IsAuthenticated(_user))
                .Returns(true);
            _mockRepository
                .Setup(r => r.EditAsync(employee.Id, editedEmployee))
                .ReturnsAsync(editedEmployee);

            var controller = new EmployeeController(_mockAuthService.Object, _mockRepository.Object, _mockService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = _user }
            };

            controller.ModelState.AddModelError("Forename", "Forename maximum length reached");
            var actual = await controller.Edit(employee.Id, editedEmployee) as RedirectToActionResult;

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.ActionName == "Read");
        }

        [Test]
        public async Task Edit_WhenUnAuthenticatedRequestReceived_ThenRedirectToSignInViewActionReturned()
        {
            var employee = EmployeeProvider.GetOnePreEditTestEmployee();
            var editedEmployee = EmployeeProvider.GetOnePostEditTestEmployee();

            _mockAuthService
                .Setup(a => a.IsAuthenticated(_user))
                .Returns(false);
            _mockRepository
                .Setup(r => r.EditAsync(employee.Id, editedEmployee))
                .ReturnsAsync(editedEmployee);

            var controller = new EmployeeController(_mockAuthService.Object, _mockRepository.Object, _mockService.Object);

            var actual = await controller.Edit(employee.Id, editedEmployee) as RedirectToActionResult;

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.ActionName == "SignIn");
            Assert.IsTrue(actual.ControllerName == "Authentication");
        }

        [Test]
        public async Task Delete_WhenEmployeeRecordFound_ThenRedirectToBrowseViewReturned()
        {
            _mockAuthService
                .Setup(a => a.IsAuthenticated(_user))
                .Returns(true);
            _mockRepository
                .Setup(r => r.DeleteAsync(1))
                .ReturnsAsync(EmployeeProvider.GetOneTestEmployee);

            var controller = new EmployeeController(_mockAuthService.Object, _mockRepository.Object, _mockService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = _user }
            };

            var actual = await controller.Delete(1) as RedirectToActionResult;

            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.ActionName == "Browse");
        }

        [Test]
        public async Task Delete_WhenEmployeeRecordNotFound_ThenNotFoundErrorViewReturned()
        {
            _mockAuthService
                .Setup(a => a.IsAuthenticated(_user))
                .Returns(true);
            _mockRepository
                .Setup(r => r.DeleteAsync(1))
                .ReturnsAsync(EmployeeProvider.GetOneTestEmployee);

            var controller = new EmployeeController(_mockAuthService.Object, _mockRepository.Object, _mockService.Object);
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
