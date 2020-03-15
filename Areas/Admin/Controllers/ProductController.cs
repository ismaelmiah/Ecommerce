﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Online_Shop.Data;
using Online_Shop.Models;

namespace Online_Shop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }

        //Product Product = new Product();
        public IActionResult Index()
        {
            var data = _db.Product.ToList();
            return View(data);
        }


        //Create GET Method
        [HttpGet]
        public ActionResult Create(int? id)
        {
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
        public async Task<ActionResult> Create(Products Product)
        {
            if (ModelState.IsValid)
            {
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