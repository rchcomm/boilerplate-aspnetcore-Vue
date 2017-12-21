using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Vue2Spa.Controllers
{
    public class AccountController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult SSOSignOn()
        {
            //var password = "Eptest$00";
            //try
            //{
            //  ApplicationUser user = new ApplicationUser() { Id = "ing", Email = "ing@feelanet.com", EmailConfirmed = true, PhoneNumber = "010-0000-0000", UserName = "ing", TwoFactorEnabled = false };
            //  var claimsPrincipal = await userManager.CreateAsync(user, password);
            //  if (claimsPrincipal.Succeeded)
            //  {

            //  }
            //}catch(Exception ex)
            //{
            //  var message = ex.Message;
            //}

            //var result = await this.loginManager.PasswordSignInAsync(
            //  new ApplicationUser() {
            //      Id = "ing", Email = "ing@feelanet.com", EmailConfirmed = true, PhoneNumber = "010-0000-0000", UserName = "ing", TwoFactorEnabled = false }
            //    , password
            //    , isPersistent: false
            //    , lockoutOnFailure: false);
            //if (result.Succeeded)
            //{
            //}
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SSOSignIn(string chiperUserId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "ing@feelanet"),
                new Claim(ClaimTypes.Email, "ing@feelanet")
            };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            await this.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
                , new ClaimsPrincipal(claimsIdentity)
                , new AuthenticationProperties
                {
                    IsPersistent = false,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(20)
                }
              );

            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult SignOut()
        {
            this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult AccessDenied()
        {
            this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return View();
        }
    }
}
