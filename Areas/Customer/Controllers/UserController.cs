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
        public IActionResult Active(string id)
        {
            var result = _db.ApplicationUsers.FirstOrDefault(x => x.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Active(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                var result = _db.ApplicationUsers.FirstOrDefault(x => x.Id == user.Id);
                if (result == null)
                {
                    return NotFound();
                }
                result.LockoutEnd = DateTime.Now.AddDays(-1);
                int rowAffected = await _db.SaveChangesAsync();
                if (rowAffected > 0)
                {
                    TempData["save"] = "User has been Active Successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
        }
        [HttpGet]
        public IActionResult Details(string id)
        {
            var result = _db.ApplicationUsers.FirstOrDefault(x=>x.Id==id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new ApplicationUser());
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
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        
        [HttpGet]
        public IActionResult Delete(string id)
        {
            var result = _db.ApplicationUsers.FirstOrDefault(x => x.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                var result = _db.ApplicationUsers.FirstOrDefault(x => x.Id == user.Id);
                if (result == null)
                {
                    return NotFound();
                }
                _db.ApplicationUsers.Remove(result);
                int rowAffected = await _db.SaveChangesAsync();
                if (rowAffected > 0)
                {
                    TempData["save"] = "Delete";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Lockout(string id)
        {

            var result = _db.ApplicationUsers.FirstOrDefault(x => x.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }


        [HttpPost]
        public async Task<IActionResult> Lockout(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                var result = _db.ApplicationUsers.FirstOrDefault(x => x.Id == user.Id);
                if (result == null)
                {
                    return NotFound();
                }
                result.LockoutEnd = DateTime.Now.AddDays(100);
                int rowAffected = await _db.SaveChangesAsync();
                if (rowAffected>0)
                {
                    TempData["save"] = "User has been Locked Out Successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            var _user = _db.ApplicationUsers.FirstOrDefault(x => x.Id == id);
            if (_user == null)
            {
                return NotFound();
            }
            return View(_user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                var _user = _db.ApplicationUsers.FirstOrDefault(x => x.Id == user.Id);
                _user.FirstName = user.FirstName;
                _user.LastName = user.LastName;
                _user.PasswordHash = user.PasswordHash;
                var res = await _userManager.UpdateAsync(_user);
                if (res.Succeeded)
                {
                    TempData["save"] = "Update";
                }
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}