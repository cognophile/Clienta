using Microsoft.AspNetCore.Mvc;
using Clienta.Models;
using Clienta.Repositories.Interfaces;
using Clienta.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clienta.Controllers
{
    [Route("employees")]
    public class EmployeeController : Controller
    {
        private readonly IAuthenticationService _authentication;
        private readonly IEmployeeRepository _repository;
        private readonly IPaginationService _paginator;

        /// <summary>
        /// EmployeeController constructor
        /// </summary>
        /// <param name="authentication"></param>
        /// <param name="repository"></param>
        /// <param name="paginator"></param>
        public EmployeeController(IAuthenticationService authentication, IEmployeeRepository repository, IPaginationService paginator)
        {
            _authentication = authentication;
            _repository = repository;
            _paginator = paginator;
        }

        /// <summary>
        /// Retrieve the Employee browse page, with data
        /// </summary>
        /// <returns>RedirectToActionResult, or, View</returns>
        [HttpGet, ActionName("Browse")]
        [Route("")]
        public async Task<IActionResult> GetAll(int page = 1, string search = null)
        {
            if (!_authentication.IsAuthenticated(User))
            {
                return RedirectToAction("SignIn", "Authentication");
            }

            IEnumerable<Employee> employees = await _repository.BrowseAsync(search);

            int pageCount = _paginator.GetPageCount(employees.Count());
            int previousPage = _paginator.GetPreviousPageIndex(page);
            int nextPage = _paginator.GetNextPageIndex(page);

            ViewBag.PreviousPage = previousPage;
            ViewBag.NextPage = nextPage;
            ViewBag.HasPreviousPage = _paginator.IsPreviousPageAvailable(previousPage);
            ViewBag.HasNextPage = _paginator.IsNextPageAvailable(nextPage, pageCount);

            employees = employees
                .Skip(_paginator.PageSize * (page - 1))
                .Take(_paginator.PageSize)
                .ToList();

            return View("Browse", employees);
        }

        /// <summary>
        /// Retrieve a single Employee details page
        /// </summary>
        /// <param name="Id">Int: The Id of the record to action</param>
        /// <returns>RedirectToActionResult, or, View</returns>
        [HttpGet, ActionName("Read")]
        [Route("{Id:int}")]
        public async Task<IActionResult> GetOne([FromRoute] int Id)
        {
            if (!_authentication.IsAuthenticated(User))
            {
                return RedirectToAction("SignIn", "Authentication");
            }

            Employee employee = await _repository.ReadAsync(Id);
            return View("Detail", employee);
        }

        /// <summary>
        /// Retrieve the Employee creation view
        /// </summary>
        /// <returns>RedirectToActionResult, or, View</returns>
        [HttpGet]
        [Route("create")]
        public IActionResult Create()
        {
            if (!_authentication.IsAuthenticated(User))
            {
                return RedirectToAction("SignIn", "Authentication");
            }

            return View("Create");
        }

        /// <summary>
        /// Create a new Employee record
        /// </summary>
        /// <returns>RedirectToActionResult, or, View</returns>
        [HttpPost, ActionName("Create")]
        [Route("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (!_authentication.IsAuthenticated(User))
            {
                return RedirectToAction("SignIn", "Authentication");
            }

            if (!ModelState.IsValid)
            {
                return View("Create");
            }

            Employee newEmployee = await _repository.AddAsync(employee);
            return RedirectToAction("Read", "Employee", newEmployee);
        }

        /// <summary>
        /// Edit an existing Employee record
        /// </summary>
        /// <param name="Id">Int: The Id of the record to action</param>
        /// <param name="employee">Employee: The amended Client object</param>
        /// <returns>RedirectToActionResult, or, View</returns>
        [HttpPost, ActionName("Edit")]
        [Route("edit/{Id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int Id, Employee employee)
        {
            if (!_authentication.IsAuthenticated(User))
            {
                return RedirectToAction("SignIn", "Authentication");
            }

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Read", new { Id });
            }

            try
            {
                Employee editedEmployee = await _repository.EditAsync(Id, employee);
                if (editedEmployee != null)
                {
                    return RedirectToAction("Read", new { Id });
                }
                else
                {
                    return View("_NotFound");
                }
            }
            catch (Exception exc)
            {
                ViewBag.ErrorMessage = exc.Message;
                return View("_NotFound");
            }
        }

        /// <summary>
        /// Delete an Employee record
        /// </summary>
        /// <param name="Id">Int: The Id of the record to action</param>
        /// <returns>RedirectToActionResult, or, </returns>
        [HttpGet, ActionName("Delete")]
        [Route("delete/{Id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int Id)
        {
            if (!_authentication.IsAuthenticated(User))
            {
                return RedirectToAction("SignIn", "Authentication");
            }

            try
            {
                Employee removedEmployee = await _repository.DeleteAsync(Id);
                if (removedEmployee != null)
                {
                    return RedirectToAction("Browse");
                }
                else
                {
                    return View("_NotFound");
                }
            }
            catch (Exception exc)
            {
                ViewBag.ErrorMessage = exc.Message;
                return View("_NotFound");
            }
        }
    }
}
