using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WhatWasRead_Angular.Controllers
{
   public class AccountController : Controller
   {
      //just for testing
      private const string adminUser = "admin";
      private const string adminPassword = "123456";

      [HttpPost("/api/account/login")]
      public async Task<IActionResult> Login([FromBody] LoginViewModel creds)
      {
         if (ModelState.IsValid && creds.Name == adminUser && creds.Password == adminPassword)
         {
            await Authenticate(adminUser);
            return Ok("true");
         }
         return BadRequest();
      }

      private async Task Authenticate(string userName)
      {
         List<Claim> claims = new List<Claim> 
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
         ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
         await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
      }

      [HttpPost("/api/account/logout")]
      public async Task<IActionResult> Logout()
      {
         await LogoutInner();
         return Ok();
      }

      private async Task<IActionResult> LogoutInner()
      {
         await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
         return RedirectToAction("Login", "Account");
      }

   }
   public class LoginViewModel
   {
      [Required]
      public string Name { get; set; }
      [Required]
      public string Password { get; set; }
   }

}
