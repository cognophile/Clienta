using Clienta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clienta.Repositories.Interfaces
{
    public interface IAddressRepository
    {

        public Task<IEnumerable<Address>> BrowseAsync(int clientId);
        public Task<Address> ReadAsync(int Id);
        public Task<Address> EditAsync(int Id, Address employee);
        public Task<Address> AddAsync(Address employee);
        public Task<Address> DeleteAsync(int Id);
        public Task<int> CountAsync();
    }
}
