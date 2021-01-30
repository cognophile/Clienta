using Clienta.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clienta.Test.Providers
{
    public static class EmployeeProvider
    {
        public static List<Employee> GetAllTestEmployees()
        {
            var employees = new List<Employee>();
            employees.Add(new Employee
            {
                Id = 1,
                Forename = "Alice",
                Surname = "Smith",
                DateOfBirth = new DateTime(1970, 1, 1),
                StartDate = new DateTime(2016, 6, 6),
            });

            employees.Add(new Employee
            {
                Id = 2,
                Forename = "Bob",
                Surname = "Smith",
                DateOfBirth = new DateTime(1975, 5, 5),
                StartDate = new DateTime(2015, 5, 5),
            });

            return employees;
        }

        public static Employee GetOneTestEmployee()
        {
            return new Employee
            {
                Id = 1,
                Forename = "Alice",
                Surname = "Smith",
                DateOfBirth = new DateTime(1970, 1, 1),
                StartDate = new DateTime(2016, 6, 6),
            };
        }

        public static Employee GetOnePreCreationTestEmployee()
        {
            return new Employee
            {
                Forename = "Jane",
                Surname = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                StartDate = new DateTime(2014, 4, 4),
            };
        }

        public static Employee GetOnePostCreationTestEmployee()
        {
            return new Employee
            {
                Id = 3,
                Forename = "Jane",
                Surname = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                StartDate = new DateTime(2014, 4, 4),
            };
        }

        public static Employee GetOnePreEditTestEmployee()
        {
            return new Employee
            {
                Id = 1,
                Forename = "Alice",
                Surname = "Smith",
                DateOfBirth = new DateTime(1970, 1, 1),
                StartDate = new DateTime(2016, 6, 6),
            };
        }

        public static Employee GetOnePostEditTestEmployee()
        {
            return new Employee
            {
                Id = 1,
                Forename = "Fran",
                Surname = "Smith",
                DateOfBirth = new DateTime(1970, 1, 1),
                StartDate = new DateTime(2016, 6, 6),
            };
        }
    }
}
