using Microsoft.AspNetCore.Mvc.Rendering;
using Clienta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clienta.Services.Interfaces
{
    public interface IClientService
    {
        public Task<IEnumerable<SelectListItem>> GetClientSelectListAsync();
    }
}
