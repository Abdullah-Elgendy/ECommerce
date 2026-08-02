using ECommerce.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        //Id is inherited from IdentityUser, it's a string GUID

        public string DisplayName { get; set; } = default!;

        public Address? Address { get; set; }
    }
}
