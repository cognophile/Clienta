using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Clienta.Services.Interfaces
{
    public interface IAuthenticationService
    {
        public bool IsAuthenticated(ClaimsPrincipal principal);
    }
}
