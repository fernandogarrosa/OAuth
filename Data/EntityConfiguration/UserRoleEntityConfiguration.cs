using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.EntityConfiguration
{
    public class UserRoleEntityConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(ur => new { ur.UserId, ur.RoleId });
            builder.HasOne(ur => ur.Role).
                WithMany(r => r.UserRoles).
                HasForeignKey(ur => ur.RoleId).
                IsRequired();

            builder.HasOne(ur => ur.User).
                WithMany(r => r.UserRoles).
                HasForeignKey(ur => ur.UserId).
                IsRequired();

        }
    }
}
