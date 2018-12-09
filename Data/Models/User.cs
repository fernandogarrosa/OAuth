using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Data.Models
{
    public class User : IdentityUser<int>
    {
        public ICollection<UserRole> UserRoles { get; set; }       
    }
}
