using Microsoft.AspNetCore.Mvc;
using Clienta.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Clienta.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        /// <summary>
        /// Assert whether the given Principal (User) is authenticated
        /// </summary>
        /// <param name="principle">ClaimsPrincipal</param>
        /// <returns>bool</returns>
        public bool IsAuthenticated(ClaimsPrincipal principle)
        {
            if (principle != null && principle.Identity.IsAuthenticated)
            {
                return true;
            }

            return false;
        }
    }
}
