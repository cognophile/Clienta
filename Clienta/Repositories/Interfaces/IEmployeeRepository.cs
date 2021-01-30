using Clienta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clienta.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        public Task<IEnumerable<Employee>> BrowseAsync(string search = null);
        public Task<Employee> ReadAsync(int Id);
        public Task<Employee> EditAsync(int Id, Employee employee);
        public Task<Employee> AddAsync(Employee employee);
        public Task<Employee> DeleteAsync(int Id);
        public Task<int> CountAsync();
    }
}
