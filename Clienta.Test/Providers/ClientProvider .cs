using Clienta.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clienta.Test.Providers
{
    public static class ClientProvider
    {
        private static List<Address> GetTestAddresses(int clientId, int addressId = 0)
        {          
            if (addressId != 0)
            {
                return new List<Address>
                {
                    new Address { Id = addressId, ClientId = clientId, LineOne = "12", LineTwo = "Gerrard Way", City = "Cambridge", County = "Cambridgeshire", Postcode = "CB1 3AB" }
                };
            }

            return new List<Address>
            {
                new Address { ClientId = clientId, LineOne = "12", LineTwo = "Gerrard Way", City = "Cambridge", County = "Cambridgeshire", Postcode = "CB1 3AB" }
            };
        }

        public static List<Client> GetAllTestClients()
        {
            var clients = new List<Client>();
            clients.Add(new Client
            {
                Id = 1,
                Forename = "Alice",
                Surname = "Smith",
                DateOfBirth = new DateTime(1970, 1, 1),
                Addresses = GetTestAddresses(1, 1)
            });

            clients.Add(new Client
            {
                Id = 2,
                Forename = "Bob",
                Surname = "Smith",
                DateOfBirth = new DateTime(1975, 5, 5),
                Addresses = GetTestAddresses(2, 2)
            });

            return clients;
        }

        public static Client GetOneTestClient()
        {
            return new Client
            {
                Id = 1,
                Forename = "Alice",
                Surname = "Smith",
                DateOfBirth = new DateTime(1970, 1, 1),
                Addresses = GetTestAddresses(1, 1)
            };
        }

        public static Client GetOnePreCreationTestClient()
        {
            return new Client
            {
                Forename = "Jane",
                Surname = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Addresses = GetTestAddresses(3)
            };
        }

        public static Client GetOnePostCreationTestClient()
        {
            return new Client
            {
                Id = 3,
                Forename = "Jane",
                Surname = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Addresses = GetTestAddresses(3, 3)
            };
        }

        public static Client GetOnePreEditTestClient()
        {
            return new Client
            {
                Id = 1,
                Forename = "Alice",
                Surname = "Smith",
                DateOfBirth = new DateTime(1970, 1, 1),
                Addresses = GetTestAddresses(1, 1)
            };
        }

        public static Client GetOnePostEditTestClient()
        {
            return new Client
            {
                Id = 1,
                Forename = "Fran",
                Surname = "Smith",
                DateOfBirth = new DateTime(1970, 1, 1),
                Addresses = GetTestAddresses(1, 1)
            };
        }
    }
}
