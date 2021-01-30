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
    [Route("addresses")]
    public class AddressController : Controller
    {
        private readonly IAuthenticationService _authentication;
        private readonly IAddressRepository _repository;

        /// <summary>
        /// AddressController constructor
        /// </summary>
        /// <param name="authentication"></param>
        /// <param name="repository"></param>
        public AddressController(IAuthenticationService authentication, IAddressRepository repository)
        {
            _authentication = authentication;
            _repository = repository;
        }

        /// <summary>
        /// Retrieve the Address creation view
        /// </summary>
        /// <returns>PartialView</returns>
        [HttpGet, ActionName("Create")]
        [Route("")]
        public IActionResult Create()
        {
            if (!_authentication.IsAuthenticated(User))
            {
                return RedirectToAction("SignIn", "Authentication");
            }

            return PartialView("_AddressModal", new Address { });
        }

        /// <summary>
        /// Create a new Address and associate it with a client
        /// </summary>
        /// <param name="clientId">The Id of the Client to associate the new Address with</param>
        /// <param name="address">The new Address Model</param>
        /// <returns>PartialView</returns>
        [HttpPost, ActionName("Create")]
        [Route("")]
        public async Task<IActionResult> Create([FromQuery] int clientId, Address address)
        {
            if (!_authentication.IsAuthenticated(User))
            {
                return RedirectToAction("SignIn", "Authentication");
            }

            if (!ModelState.IsValid)
            {
                return PartialView("_AddressModal", address);
            }
            
            address.ClientId = clientId;
            address.Postcode = address.Postcode.ToUpper();

            Address newAddress = await _repository.AddAsync(address);
            return PartialView("_AddressModal", newAddress);
        }
    }
}
