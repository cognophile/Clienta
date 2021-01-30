using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clienta.Models.Extensions
{
    public static class ModelBuilderExtension
    {
        /// <summary>
        /// Seed the ApplicationContext with data
        ///     For Development purposes, only.
        /// </summary>
        /// <param name="builder">Instance of the calling ModelBuilder</param>
        public static void Seed(this ModelBuilder builder)
        {
            // TODO: Hash the example User passwords via IdentityUser
            builder.Entity<User>().HasData(
                new User { Id = 1, Email = "alice@example.com", Password = "password123", PersistSession = false, Created = DateTime.Now.AddDays(-5), LastLogin = DateTime.Now.AddDays(-1) },
                new User { Id = 2, Email = "bob@example.com", Password = "password123", PersistSession = false, Created = DateTime.Now.AddDays(-3), LastLogin = DateTime.Now.AddDays(-2) }
            );

            builder.Entity<Employee>().HasData(
                new Employee { Id = 1, UserId = 1, EmployeeNumber = new Random().Next(100000, 999999), DateOfBirth = DateTime.Parse("1984/01/24"), Forename = "Alice", Surname = "Doe", StartDate = DateTime.Parse("2000/01/01") },
                new Employee { Id = 2, UserId = 2, EmployeeNumber = new Random().Next(100000, 999999), DateOfBirth = DateTime.Parse("1970/01/01"), Forename = "Bob", Surname = "Smith", StartDate = DateTime.Parse("2013/05/02") }
            );

            builder.Entity<Client>().HasData(
                new Client { Id = 1, EmployeeId = 1, DateOfBirth = DateTime.Parse("1970/01/01"), Forename = "Phil", Surname = "Bloggs" },
                new Client { Id = 2, EmployeeId = 1, DateOfBirth = DateTime.Parse("1971/01/01"), Forename = "Jane", Surname = "Hill" },
                new Client { Id = 3, EmployeeId = 2, DateOfBirth = DateTime.Parse("1972/01/01"), Forename = "Clare", Surname = "Quill" },
                new Client { Id = 4, EmployeeId = 1, DateOfBirth = DateTime.Parse("1973/01/01"), Forename = "Carol", Surname = "Tam" },
                new Client { Id = 5, EmployeeId = 2, DateOfBirth = DateTime.Parse("1974/01/01"), Forename = "Tom", Surname = "Winters" },
                new Client { Id = 6, EmployeeId = 2, DateOfBirth = DateTime.Parse("1975/01/01"), Forename = "Jack", Surname = "Jobs" },
                new Client { Id = 7, EmployeeId = 1, DateOfBirth = DateTime.Parse("1976/01/01"), Forename = "Harriet", Surname = "Storm" },
                new Client { Id = 8, EmployeeId = 1, DateOfBirth = DateTime.Parse("1977/01/01"), Forename = "Doug", Surname = "Quinn" },
                new Client { Id = 9, EmployeeId = 2, DateOfBirth = DateTime.Parse("1978/01/01"), Forename = "Will", Surname = "Hunting" },
                new Client { Id = 10, EmployeeId = 2, DateOfBirth = DateTime.Parse("1979/01/01"), Forename = "Fran", Surname = "Rye" },
                new Client { Id = 11, EmployeeId = 2, DateOfBirth = DateTime.Parse("1980/01/01"), Forename = "Ivy", Surname = "Fall" },
                new Client { Id = 12, EmployeeId = 2, DateOfBirth = DateTime.Parse("1981/01/01"), Forename = "Poppy", Surname = "Smith" }
            );

            builder.Entity<Address>().HasData(
                new Address { Id = 1, ClientId = 1, LineOne = "12", LineTwo = "Gerrard Way", City = "Cambridge", County = "Cambridgeshire", Postcode = "CB1 3AB" },
                new Address { Id = 2, ClientId = 2, LineOne = "17", LineTwo = "Hawk Road", City = "Norwich", County = "Norfolk", Postcode = "NR1 3AB" },
                new Address { Id = 3, ClientId = 2, LineOne = "45", LineTwo = "Heath Street", City = "Norwich", County = "Norfolk", Postcode = "NR1 2CD" },
                new Address { Id = 4, ClientId = 3, LineOne = "74B", LineTwo = "Hawk Avenue", City = "Newmarket", County = "Suffolk", Postcode = "CB8 7AH" },
                new Address { Id = 5, ClientId = 4, LineOne = "1B", LineTwo = "Gerrard Road", City = "Cambridge", County = "Cambridgeshire", Postcode = "CB8 3YU" },
                new Address { Id = 6, ClientId = 5, LineOne = "64", LineTwo = "Caster Street", City = "Norwich", County = "Norfolk", Postcode = "NR2 1XW" },
                new Address { Id = 7, ClientId = 6, LineOne = "43", LineTwo = "Zoo Avenue", City = "Newmarket", County = "Suffolk", Postcode = "CB9 4UU" },
                new Address { Id = 8, ClientId = 7, LineOne = "235", LineTwo = "Caster Avenue", City = "Cambridge", County = "Cambridgeshire", Postcode = "CB4 5QE" },
                new Address { Id = 9, ClientId = 8, LineOne = "45", LineTwo = "Forrest Way", City = "Norwich", County = "Norfolk", Postcode = "NR6 7RP" },
                new Address { Id = 10, ClientId = 9, LineOne = "33", LineTwo = "Ranger Avenue", City = "Newmarket", County = "Suffolk", Postcode = "CB8 7AH" },
                new Address { Id = 11, ClientId = 10, LineOne = "12A", LineTwo = "Reed Road", City = "Cambridge", County = "Cambridgeshire", Postcode = "CB5 8FG" },
                new Address { Id = 12, ClientId = 11, LineOne = "17C", LineTwo = "Storm Street", City = "Newmarket", County = "Suffolk", Postcode = "CB8 7AH" },
                new Address { Id = 13, ClientId = 12, LineOne = "84", LineTwo = "Funnel Way", City = "Cambridge", County = "Cambridgeshire", Postcode = "CB1 3TI" }
            );
        }
    }
}
