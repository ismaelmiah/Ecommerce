using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Online_Shop.Data;
using Online_Shop.Models;

namespace Online_Shop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment hostingEnvironment;
        public ProductController(ApplicationDbContext db, IWebHostEnvironment he)
        {
            _db = db;
            hostingEnvironment = he;
        }

        //Product Product = new Product();
        public IActionResult Index()
        {
            var data = _db.Product.Include(x=> x.SpecialTags).Include(x=>x.ProductTypes).ToList();
            return View(data);
        }


        //Create GET Method
        [HttpGet]
        public ActionResult Create(int? id)
        {
            ViewData["ProductTypeId"] = new SelectList(_db.ProductType.ToList(), "Id", "ProductType");
            ViewData["SpecialTagId"] = new SelectList(_db.SpecialTags.ToList(), "Id", "SpecialTag");
            if (id == null)
            {
                // Create
                return View(new Products());
            }
            //Update
            var Product = _db.Product.Find(id);
            if (Product == null)
            {
                return NotFound();
            }
            
            return View(Product);
        }

        //Create Post Method

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Products Product, IFormFile photo)
        {
            if (ModelState.IsValid)
            {
                if (photo != null)
                {
                    var name = Path.Combine(hostingEnvironment.WebRootPath + "/images", 
                        Path.GetFileName(photo.FileName));
                    await photo.CopyToAsync(new FileStream(name, FileMode.Create));
                    Product.Image = "images/" + photo.FileName;
                }
                if (Product.Id == 0)
                {
                    TempData["save"] = "Product has been Saved";
                    _db.Product.Add(Product);
                }
                else
                {
                    TempData["save"] = "Update";
                    _db.Product.Update(Product);

                }

                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(Product);
        }

        //Details Page
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null) return NotFound();
            var Product = _db.Product.Find(id);
            if (Product == null)
            {
                return NotFound();
            }
            return View(Product);
        }

        //Delete Get Method
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            var Product = _db.Product.Find(id);
            if (Product == null)
            {
                return NotFound();
            }
            return View(Product);
        }

        //Delete Post Method

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int? Id, Products Products)
        {
            if (Id == null) return NotFound();
            if (Id != Products.Id) return NotFound();
            var Product = _db.Product.Find(Id);
            if (Product == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                TempData["save"] = "Delete";
                _db.Remove(Product);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(Product);
        }

    }
}