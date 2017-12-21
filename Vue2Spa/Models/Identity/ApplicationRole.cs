using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Vue2Spa.Models.Identity
{
    public class ApplicationRole : IdentityRole<string>
    {
        public string Description { get; set; }

        public ApplicationRole()
        {
        }
    }
}
