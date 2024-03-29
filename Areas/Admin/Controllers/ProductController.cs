﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize]
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
        
        [HttpPost]
        public IActionResult Index(decimal? lowPrice, decimal? highPrice)
        {
            var products = _db.Product.Include(x => x.SpecialTags).Include(x => x.ProductTypes).Where(x=>x.Price>=lowPrice && x.Price<=highPrice).ToList();
            if(lowPrice==null || highPrice == null)
            {
                products = _db.Product.Include(x => x.SpecialTags).Include(x => x.ProductTypes).ToList();
            }
            return View(products);
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
            var Product = _db.Product.Include(x => x.SpecialTags).Include(x => x.ProductTypes).
                FirstOrDefault(x => x.Id == id);
            if (Product == null)
            {
                return NotFound();
            }
            
            return View(Product);
        }

        //Create Post Method

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Products Product, IFormFile Image)
        {
            if (ModelState.IsValid)
            {
                var searchProduct = _db.Product.FirstOrDefault(x=> x.Name == Product.Name); 
                if (searchProduct != null)
                {
                    ViewBag.message = "This Product is Already Exists";
                    ViewData["ProductTypeId"] = new SelectList(_db.ProductType.ToList(), "Id", "ProductType");
                    ViewData["SpecialTagId"] = new SelectList(_db.SpecialTags.ToList(), "Id", "SpecialTag");
                    return View(Product);
                }
                if (Image != null)
                {
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Image.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using var fileStream = new FileStream(filePath, FileMode.Create);
                    Image.CopyTo(fileStream);
                    Product.Image = uniqueFileName;
                }
                if (Image == null)
                {
                    Product.Image = "noPhotoFound.png";
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
            var Product = _db.Product.Include(x => x.SpecialTags).Include(x => x.ProductTypes).
                FirstOrDefault(x => x.Id == id);
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
            var Product = _db.Product.Include(x => x.SpecialTags).Include(x => x.ProductTypes).
                FirstOrDefault(x => x.Id == id);
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
            var Product = _db.Product.Include(x => x.SpecialTags).Include(x => x.ProductTypes).
                FirstOrDefault(x => x.Id == Id);
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