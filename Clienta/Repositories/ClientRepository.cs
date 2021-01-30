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
    public class ClientRepository : IClientRepository
    {
        private readonly ApplicationContext _connection;

        /// <summary>
        /// ClientRepository constructor
        /// </summary>
        /// <param name="connection">ApplicationContext</param>
        public ClientRepository(ApplicationContext connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Retrieve a list of Client
        ///     Optional: Filter by search on Name(s)
        /// </summary>
        /// <param name="search">string - Term to search for matching Name(s)</param>
        /// <returns>IEnumerable<Client> Clients</returns>
        public async Task<IEnumerable<Client>> BrowseAsync(string search = null)
        {
            IQueryable<Client> clients = _connection.Clients
                .Include(c => c.Employee)
                .Include(a => a.Addresses);

            foreach (var client in clients)
            {
                client.Addresses = client.Addresses
                    .OrderByDescending(a => a.Id).ToList();
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var loweredSearch = search.ToLower();
                clients = clients.Where(c =>
                       c.Forename.ToLower().Contains(loweredSearch) || 
                       c.Surname.ToLower().Contains(loweredSearch) || 
                       (c.Forename.ToLower() + " " + c.Surname.ToLower()).Contains(loweredSearch)
                );
            }

            return await clients.ToListAsync();
        }

        /// <summary>
        /// Retrieve a single Client by Id
        /// </summary>
        /// <param name="Id">Client Id</param>
        /// <returns>Client</returns>
        public async Task<Client> ReadAsync(int Id)
        {
            var client = await _connection.Clients
                .Include(c => c.Employee)
                .Include(a => a.Addresses)
                .SingleOrDefaultAsync(c => c.Id == Id);

            client.Addresses = client.Addresses
                .OrderByDescending(a => a.Id).ToList();

            return client;
        }

        /// <summary>
        /// Edit the record corresponding to the given Id with the provided Client instance
        /// </summary>
        /// <param name="Id">Client Id</param>
        /// <param name="client">Client model containing new information</param>
        /// <returns>Edited Client model</returns>
        public async Task<Client> EditAsync(int Id, Client client)
        {
            _connection.Entry(client).State = EntityState.Modified;

            try
            {
                await _connection.SaveChangesAsync();
                return await ReadAsync(Id);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _connection.Clients.FindAsync(Id) == null)
                {
                    throw new Exception($"Client cannot be edited: Client {Id} Not found");
                }

                throw;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Persist the provided Client
        /// </summary>
        /// <param name="client">Client</param>
        /// <returns>Persisted Client model</returns>
        public async Task<Client> AddAsync(Client client)
        {
            _connection.Clients.Add(client);
            await _connection.SaveChangesAsync();

            return client;
        }

        /// <summary>
        /// Delete an the Client record identified by the provided Id
        /// </summary>
        /// <param name="Id">Client Id</param>
        /// <returns>Deleted Client model</returns>
        public async Task<Client> DeleteAsync(int Id)
        {
            var client = await _connection.Clients.FindAsync(Id);
            if (client == null)
            {
                throw new Exception($"Client cannot be deleted: Client {Id} Not found"); 
            }

            _connection.Clients.Remove(client);
            await _connection.SaveChangesAsync();

            return client;
        }

        /// <summary>
        /// Count the total number of Clients
        /// </summary>
        /// <returns>int Count</returns>
        public async Task<int> CountAsync()
        {
            return await _connection.Clients.CountAsync();
        }
    }
}
