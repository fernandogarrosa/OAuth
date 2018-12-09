using Data.EntityConfiguration;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Data
{
    public class DataContext : IdentityDbContext<User, Role, int,
                               IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>,
                               IdentityRoleClaim<int>, IdentityUserToken<int>> , IDbContext
    {        
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleEntityConfiguration());
        }

        public void Initialize()
        {
            var users = new List<User>
            {
                new User { Id = 1, UserName ="fernando" },
                new User { Id = 2, UserName = "carlos" },               
            };

            foreach (User user in users)
                Add(user);

            SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
