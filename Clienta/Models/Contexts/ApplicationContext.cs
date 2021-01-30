using Microsoft.EntityFrameworkCore;
using Clienta.Models.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clienta.Models.Contexts
{
    public class ApplicationContext : DbContext
    {
        // Define the tables in this context
        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Address> Addresses { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasOne(u => u.Employee)
                .WithOne(s => s.User)
                .HasForeignKey<Employee>(s => s.UserId);

            builder.Entity<Employee>()
                .HasMany(s => s.Clients)
                .WithOne(c => c.Employee)
                .HasForeignKey(c => c.EmployeeId)
                .OnDelete(DeleteBehavior.SetNull);
                
            builder.Entity<Client>()
                .HasMany(c => c.Addresses)
                .WithOne(a => a.Client)
                .HasForeignKey(a => a.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Address>()
                .HasOne(a => a.Client)
                .WithMany(c => c.Addresses)
                .HasForeignKey(a => a.ClientId);

            builder.Seed();
        }
    }
}
