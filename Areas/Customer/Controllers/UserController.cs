using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Online_Shop.Models;

namespace Online_Shop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class UserController : Controller
    {
        UserManager<IdentityUser> _userManager;
        public UserController(UserManager<IdentityUser>userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                var result = await _userManager.CreateAsync(user, user.PasswordHash);
                if (result.Succeeded)
                {
                    TempData["save"] = "User has been Registered Successfully";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

            }
            return View();
        }
    }
}