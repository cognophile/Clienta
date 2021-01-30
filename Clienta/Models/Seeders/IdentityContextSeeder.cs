using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Clienta.Models.Contexts;

namespace Clienta.Models.Seeders
{
    public class IdentityContextSeeder
    {
        /// <summary>
        /// Seed theIdentityContext with user data
        ///     For Development purposes, only.
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public async Task Initialise(IdentityContext connection)
        {
            var roleStore = new RoleStore<IdentityRole>(connection);
            var initialUser = new IdentityUser
            {
                UserName = "alice@example.com",
                NormalizedUserName = "alice@example.com",
                Email = "alice@example.com",
                NormalizedEmail = "alice@example.com",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            if (!connection.Roles.Any(r => r.Name == "admin"))
            {
                await roleStore.CreateAsync(new IdentityRole { Name = "admin", NormalizedName = "admin" });
            }

            if (!connection.Users.Any(u => u.UserName == initialUser.UserName))
            {
                initialUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(initialUser, "password123");

                var userStore = new UserStore<IdentityUser>(connection);
                await userStore.CreateAsync(initialUser);
                await userStore.AddToRoleAsync(initialUser, "admin");
            }

            await connection.SaveChangesAsync();
        }
    }
}
