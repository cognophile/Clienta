using Microsoft.AspNetCore.Mvc;
using Clienta.Models.Contexts;
using Clienta.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clienta.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        private IAuthenticationService _authentication;

        /// <summary>
        /// HomeController constructor
        /// </summary>
        /// <param name="authentication"></param>
        public HomeController(IAuthenticationService authentication) : base()
        {
            _authentication = authentication;
        }

        /// <summary>
        /// Retrieve the Home view
        /// </summary>
        /// <returns>RedirectToActionResult, or, View</returns>
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            if (!_authentication.IsAuthenticated(User))
            {
                return RedirectToAction("SignIn", "Authentication");
            }

            return View("Index");
        }
    }
}