using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clienta.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Clienta.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Clienta.Controllers
{
    [Route("auth")]
    public class AuthenticationController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthenticationController()
        {
        }

        /// <summary>
        /// AuthenticationController constructor
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        [ActivatorUtilitiesConstructor]
        public AuthenticationController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Retrieve the Sign In view
        /// </summary>
        /// <returns>View</returns>
        [HttpGet]
        [Route("signin")]
        [AllowAnonymous]
        public IActionResult SignIn()
        {
            return View("SignIn");
        }

        /// <summary>
        /// Authenticate the User with a valid session
        /// </summary>
        /// <param name="viewModel">The SignInViewModel containing the POSTed data</param>
        /// <param name="preAuthorisedUrl">A nullable string value representing the requested URL prior to authorisation</param>
        /// <returns>RedirectToActionResult, or, View></returns>
        [HttpPost]
        [Route("signin")]
        public async Task<IActionResult> SignIn(SignInViewModel viewModel, string preAuthorisedUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var authenticationAttempt = await _signInManager
                .PasswordSignInAsync(viewModel.Email, viewModel.Password, viewModel.PersistSession, false);

            if (!authenticationAttempt.Succeeded)
            {
                ModelState.AddModelError("", "Those credentials don't look right. Try again!");
                return View();
            }

            if (!string.IsNullOrWhiteSpace(preAuthorisedUrl))
            {
                return Redirect(preAuthorisedUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Sign out the currently authenticated user
        /// </summary>
        /// <returns>RedirectToActionResult</returns>
        [HttpPost]
        [Route("signout")]
        public async Task<IActionResult> SignOut()
        {
            if (User == null || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "Authentication");
            }

            await _signInManager.SignOutAsync();
            return RedirectToAction("SignIn", "Authentication");
        }

        /// <summary>
        /// Retrieve the Registration view
        /// </summary>
        /// <returns>View</returns>
        [HttpGet]
        [Route("register")]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View("Register");
        }

        /// <summary>
        /// Register the given account
        /// </summary>
        /// <param name="viewModel">The RegistrationViewModel containing the POSTed data</param>
        /// <returns>RedirectToActionResult, or, View</returns>
        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegistrationViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Register", viewModel);
            }

            var newAccount = new IdentityUser { Email = viewModel.Email, UserName = viewModel.Email };
            newAccount.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(newAccount, viewModel.Password);

            var registrationAttempt = await _userManager.CreateAsync(newAccount);
            if (!registrationAttempt.Succeeded)
            {
                foreach (var errorDescription in registrationAttempt.Errors.Select(e => e.Description))
                {
                    ModelState.AddModelError("", errorDescription);
                }

                return View("Register");
            }

            return RedirectToAction("SignIn");
        }
    }
}
