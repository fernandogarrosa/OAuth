using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    public class Seed
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;

        public Seed(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public void SeedUsers()
        {
            var roles = new List<Role>
            {
                new Role{Name ="Admin"},
                new Role{Name ="Lawyer"}
            };

            foreach (var role in roles)
                roleManager.CreateAsync(role).Wait();

            var adminUser = new User
            {
                Id = 1,
                UserName = "Admin"
            };

            userManager.CreateAsync(adminUser, "Admin").Wait();
            userManager.AddToRoleAsync(adminUser, "Admin");

            var user = new User
            {
                Id = 2,
                UserName = "Fernando"
            };
            userManager.CreateAsync(user, "fernando").Wait();
            userManager.AddToRoleAsync(user, "Lawyer");
        }
    }
}
