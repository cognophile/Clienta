using Microsoft.AspNetCore.Mvc.Rendering;
using Clienta.Repositories.Interfaces;
using Clienta.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clienta.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _repository;

        /// <summary>
        /// ClientService constructor
        /// </summary>
        /// <param name="repository">IClientRepository</param>
        public ClientService(IClientRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Get a list of SelectItem Clients
        /// </summary>
        /// <returns>List<SelectListItem></returns>
        public async Task<IEnumerable<SelectListItem>> GetClientSelectListAsync()
        {
            var clients = await _repository.BrowseAsync();

            if (clients.Count() == 0)
            {
                return new List<SelectListItem>();
            }

            return clients.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.GetFullname()
            }).ToList();
        }
    }
}
