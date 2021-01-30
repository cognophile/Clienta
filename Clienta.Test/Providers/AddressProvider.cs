using Clienta.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clienta.Test.Providers
{
    public static class AddressProvider
    {
        public static Address GetPreCreationTestAddress(int clientId)
        {          
            return new Address
            {
                ClientId = clientId, LineOne = "12", LineTwo = "Gerrard Way", City = "Cambridge", County = "Cambridgeshire", Postcode = "CB1 3AB"
            };
        }

        public static Address GetPostCreationTestAddress(int clientId)
        {
            return new Address
            {
                Id = 1, ClientId = clientId, LineOne = "12", LineTwo = "Gerrard Way", City = "Cambridge", County = "Cambridgeshire", Postcode = "CB1 3AB"
            };
        }
    }
}
