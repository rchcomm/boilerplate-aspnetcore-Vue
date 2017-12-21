using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vue2Spa.Models.AuthenticationEvents
{
    public class CustomCookieAuthenticationEvents : CookieAuthenticationEvents
    {

        public CustomCookieAuthenticationEvents()
        {
        }

        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {

        }
    }
}
