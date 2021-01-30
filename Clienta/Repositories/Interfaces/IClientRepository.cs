using Clienta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clienta.Repositories.Interfaces
{
    public interface IClientRepository
    {
        public Task<IEnumerable<Client>> BrowseAsync(string search = null);
        public Task<Client> ReadAsync(int Id);
        public Task<Client> EditAsync(int Id, Client client);
        public Task<Client> AddAsync(Client client);
        public Task<Client> DeleteAsync(int Id);
        public Task<int> CountAsync();
    }
}
