using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Clienta.Models;
using Clienta.Repositories.Interfaces;
using Clienta.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clienta.Controllers
{
    [Route("clients")]
    public class ClientController : Controller
    {
        private readonly IAuthenticationService _authentication;
        private readonly IClientRepository _repository;
        private readonly IPaginationService _paginator;

        /// <summary>
        /// ClientController constructor
        /// </summary>
        /// <param name="authentication"></param>
        /// <param name="repository"></param>
        /// <param name="paginator"></param>
        public ClientController(IAuthenticationService authentication, IClientRepository repository, IPaginationService paginator)
        {
            _authentication = authentication;
            _repository = repository;
            _paginator = paginator;
        }

        /// <summary>
        /// Retrieve the Client browse page, with data
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

            IEnumerable<Client> clients = await _repository.BrowseAsync(search);

            int pageCount = _paginator.GetPageCount(clients.Count());
            int previousPage = _paginator.GetPreviousPageIndex(page);
            int nextPage = _paginator.GetNextPageIndex(page);

            ViewBag.PreviousPage = previousPage;
            ViewBag.NextPage = nextPage;
            ViewBag.HasPreviousPage = _paginator.IsPreviousPageAvailable(previousPage);
            ViewBag.HasNextPage = _paginator.IsNextPageAvailable(nextPage, pageCount);

            clients = clients
                .Skip(_paginator.PageSize * (page - 1))
                .Take(_paginator.PageSize)
                .ToList();

            return View("Browse", clients);
        }

        /// <summary>
        /// Retrieve a single Client details page
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

            Client client = await _repository.ReadAsync(Id);
            return View("Detail", client);
        }

        /// <summary>
        /// Retrieve the Client creation View
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
        /// Create a new Client record
        /// </summary>
        /// <returns>RedirectToActionResult, or, View</returns>
        [HttpPost, ActionName("Create")]
        [Route("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Client client)
        {
            if (!_authentication.IsAuthenticated(User))
            {
                return RedirectToAction("SignIn", "Authentication");
            }

            if (!ModelState.IsValid)
            {
                return View("Create");
            }

            Client newClient = await _repository.AddAsync(client);
            return RedirectToAction("Read", "Client", newClient);
        }

        /// <summary>
        /// Edit an existing client record
        /// </summary>
        /// <param name="Id">Int: The Id of the record to action</param>
        /// <param name="client">Client: The amended Client object</param>
        /// <returns>RedirectToActionResult, or, View</returns>
        [HttpPost, ActionName("Edit")]
        [Route("edit/{Id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int Id, Client client)
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
                Client editedClient = await _repository.EditAsync(Id, client);
                if (editedClient != null)
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
        /// Delete a client record
        /// </summary>
        /// <param name="Id">Int: The Id of the record to action</param>
        /// <returns>RedirectToActionResult, or, View</returns>
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
                Client removedClient = await _repository.DeleteAsync(Id);
                if (removedClient != null)
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
