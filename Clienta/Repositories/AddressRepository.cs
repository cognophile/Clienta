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
    public class AddressRepository : IAddressRepository
    {
        private readonly ApplicationContext _connection;

        /// <summary>
        /// AddressRepository constructor
        /// </summary>
        /// <param name="connection">ApplicationContext</param>
        public AddressRepository(ApplicationContext connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Retrieve a list of Addresses associated with the given Client Id
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>IEnumerable<Address> Addresses</returns>
        public async Task<IEnumerable<Address>> BrowseAsync(int clientId)
        {
            IQueryable<Address> addresses = _connection.Addresses
                .Where(a => a.ClientId == clientId);

            return await addresses.ToListAsync();
        }

        /// <summary>
        /// Retrieve a single Address by Id
        /// </summary>
        /// <param name="Id">Address Id</param>
        /// <returns>Address</returns>
        public async Task<Address> ReadAsync(int Id)
        {
            var address = await _connection.Addresses
                .SingleOrDefaultAsync(e => e.Id == Id);

            return address;
        }

        /// <summary>
        /// Edit the record corresponding to the given Id with the provided Address instance
        /// </summary>
        /// <param name="Id">Address Id</param>
        /// <param name="address">Address model containing new information</param>
        /// <returns>Edited Address model</returns>
        public async Task<Address> EditAsync(int Id, Address address)
        {
            _connection.Entry(address).State = EntityState.Modified;

            try
            {
                await _connection.SaveChangesAsync();
                return await ReadAsync(Id);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _connection.Addresses.FindAsync(Id) == null)
                {
                    throw new Exception($"Address cannot be edited: Address {Id} Not found");
                }

                throw;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Persist the provided Address
        /// </summary>
        /// <param name="address">Address</param>
        /// <returns>Persisted Address model</returns>
        public async Task<Address> AddAsync(Address address)
        {
            _connection.Addresses.Add(address);
            await _connection.SaveChangesAsync();

            return address;
        }

        /// <summary>
        /// Delete an the Address record identified by the provided Id
        /// </summary>
        /// <param name="Id">Address Id</param>
        /// <returns>Deleted Address model</returns>
        public async Task<Address> DeleteAsync(int Id)
        {
            var address = await _connection.Addresses.FindAsync(Id);
            if (address == null)
            {
                throw new Exception($"Address cannot be deleted: Address {Id} Not found");
            }

            _connection.Addresses.Remove(address);
            await _connection.SaveChangesAsync();

            return address;
        }

        /// <summary>
        /// Count the total number of Addresses
        /// </summary>
        /// <returns>int Count</returns>
        public async Task<int> CountAsync()
        {
            return await _connection.Addresses.CountAsync();
        }
    }
}
