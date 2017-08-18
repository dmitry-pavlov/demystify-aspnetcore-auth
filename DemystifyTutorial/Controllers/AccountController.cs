using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemystifyTutorial.Controllers {
    public class AccountController : Controller {
        public IActionResult Login(string returnUrl) {
            return View("Login",returnUrl);
        }
        [HttpPost]
        public IActionResult Login(string username, string password, string returnUrl) {
            //TODO: build an authentication that uses the cookie auth handler
            return Json(new {returnUrl, username, password });
        }
    }
}
