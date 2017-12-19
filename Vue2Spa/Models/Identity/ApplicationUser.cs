using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vue2Spa.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime Timestamp { get; set; }
    }
}
