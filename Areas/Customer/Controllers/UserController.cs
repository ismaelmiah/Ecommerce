using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Online_Shop.Data;
using Online_Shop.Models;

namespace Online_Shop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class UserController : Controller
    {
        UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _db;
        public UserController(UserManager<IdentityUser>userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }
        public IActionResult Index()
        {
            return View(_db.ApplicationUsers.ToList());
        }

        [HttpGet]
        public IActionResult Create(string? id)
        {
            if (id == null)
            {
                // Create
                return View(new ApplicationUser());
            }
            //Update
            var _user = _db.ApplicationUsers.FirstOrDefault(x => x.Id == id);
            if (_user == null)
            {
                return NotFound();
            }

            return View(_user);
        }


        [HttpPost]
        public async Task<IActionResult> Create(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                var _user = _db.ApplicationUsers.FirstOrDefault(x => x.Id == user.Id);
                if (_user == null)
                {
                    var result = await _userManager.CreateAsync(user, user.PasswordHash);
                    if (result.Succeeded)
                    {
                        TempData["save"] = "User has been Registered Successfully";
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    _user.FirstName = user.FirstName;
                    _user.LastName = user.LastName;
                    var res = await _userManager.UpdateAsync(_user);
                    if (res.Succeeded)
                    {
                        TempData["save"] = "Update";
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}