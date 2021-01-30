using Microsoft.EntityFrameworkCore;
using Clienta.Models;
using Clienta.Models.Contexts;
using Clienta.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clienta.Services
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationContext _connection;

        /// <summary>
        /// EmployeeRepository constructor
        /// </summary>
        /// <param name="connection">ApplicationContext</param>
        public EmployeeRepository(ApplicationContext connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Retrieve a list of Employee
        ///     Optional: Filter by search on Name(s)
        /// </summary>
        /// <param name="search">string - Term to search for matching Name(s)</param>
        /// <returns>IEnumerable<Employee> Employees</returns>
        public async Task<IEnumerable<Employee>> BrowseAsync(string search = null)
        {
            IQueryable<Employee> employees = _connection.Employees
                .Include(e => e.Clients);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var loweredSearch = search.ToLower();
                employees = employees.Where(e => 
                       e.Forename.ToLower().Contains(loweredSearch) || 
                       e.Surname.ToLower().Contains(loweredSearch) || 
                       (e.Forename.ToLower() + " " + e.Surname.ToLower()).Contains(loweredSearch)
                );
            }

            return await employees.ToListAsync();
        }

        /// <summary>
        /// Retrieve a single Employee by Id
        /// </summary>
        /// <param name="Id">Employee Id</param>
        /// <returns>Employee</returns>
        public async Task<Employee> ReadAsync(int Id)
        {
            var client = await _connection.Employees
                .Include(c => c.Clients)
                .SingleOrDefaultAsync(e => e.Id == Id);

            return client;
        }

        /// <summary>
        /// Edit the record corresponding to the given Id with the provided Employee instance
        /// </summary>
        /// <param name="Id">Employee Id</param>
        /// <param name="employee">Employee model containing new information</param>
        /// <returns>Edited Employee model</returns>
        public async Task<Employee> EditAsync(int Id, Employee employee)
        {
            _connection.Entry(employee).State = EntityState.Modified;

            try
            {
                await _connection.SaveChangesAsync();
                return await ReadAsync(Id);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _connection.Employees.FindAsync(Id) == null)
                {
                    throw new Exception($"Employee cannot be edited: Employee {Id} Not found");
                }

                throw;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Persist the provided Employee
        /// </summary>
        /// <param name="employee">Employee</param>
        /// <returns>Persisted Employee model</returns>
        public async Task<Employee> AddAsync(Employee employee)
        {
            _connection.Employees.Add(employee);
            await _connection.SaveChangesAsync();

            return employee;
        }

        /// <summary>
        /// Delete an the Employee record identified by the provided Id
        /// </summary>
        /// <param name="Id">Employee Id</param>
        /// <returns>Deleted Employee model</returns>
        public async Task<Employee> DeleteAsync(int Id)
        {
            var employee = await _connection.Employees.FindAsync(Id);
            if (employee == null)
            {
                throw new Exception($"Employee cannot be deleted: Employee {Id} Not found");
            }

            _connection.Employees.Remove(employee);
            await _connection.SaveChangesAsync();

            return employee;
        }

        /// <summary>
        /// Count the total number of Employees
        /// </summary>
        /// <returns>int Count</returns>
        public async Task<int> CountAsync()
        {
            return await _connection.Employees.CountAsync();
        }
    }
}
