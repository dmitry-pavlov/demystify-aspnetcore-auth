using DemystifyTutorial.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DemystifyTutorial.Controllers {
    public class AccountController : Controller {

        public List<ApplicationUser> Users { get; set; } = new List<ApplicationUser> {
            new ApplicationUser{UserName = "darkhelmet", Password = "vespa" },
            new ApplicationUser{UserName = "prezscroob", Password = "12345"}
        };

        public IActionResult Login(string returnUrl = null) {
            TempData["returnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(ApplicationUser user, string returnUrl = null) {
            const string badUserNameOrPasswordMessage = "Username or password is incorrect.";
            if (user == null) {
                return BadRequest(badUserNameOrPasswordMessage);
            }
            var lookupUser = Users.Where(u => u.UserName == user.UserName).FirstOrDefault();

            if (lookupUser?.Password != user.Password) {
                return BadRequest(badUserNameOrPasswordMessage);
            }

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, lookupUser.UserName));

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            if (returnUrl == null) {
                returnUrl = TempData["returnUrl"]?.ToString();
            }
            if (returnUrl != null) {
                return Redirect(returnUrl);
            } else {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        public async Task<IActionResult> Logout() {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
