using Microsoft.AspNetCore.Mvc.Rendering;
using Clienta.Repositories.Interfaces;
using Clienta.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clienta.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;

        /// <summary>
        /// EmployeeService constructor
        /// </summary>
        /// <param name="repository">IEmployeeRepository</param>
        public EmployeeService(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Get a list of SelectItem Employees
        /// </summary>
        /// <returns>List<SelectListItem></returns>
        public async Task<IEnumerable<SelectListItem>> GetEmployeeSelectListAsync()
        {
            var employees = await _repository.BrowseAsync();

            if (employees.Count() == 0)
            {
                return new List<SelectListItem>();
            }

            return employees.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.GetFullname()
            }).ToList();
        }
    }
}
