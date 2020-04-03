using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Online_Shop.Data;
using Online_Shop.Models;

namespace Online_Shop.Areas.Admin.Controllers
{
    [Area("Admin")]

    [Authorize]
    public class ProductTypesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductTypesController(ApplicationDbContext db)
        {
            _db = db;
        }

        //ProductTypes ProductTypes = new ProductTypes();
        public IActionResult Index()
        {
            var data = _db.ProductType.ToList();
            return View(data);
        }


        //Create GET Method
        [HttpGet]
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                // Create
                return View(new ProductTypes());
            }
            //Update
            var productType = _db.ProductType.Find(id);
            if (productType == null)
            {
                return NotFound();
            }
            return View(productType);
        }

        //Create Post Method

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductTypes ProductTypes)
        {
            if (ModelState.IsValid)
            {
                var searchProductType= _db.ProductType.FirstOrDefault(x => x.ProductType == ProductTypes.ProductType);
                if (searchProductType != null)
                {
                    ViewBag.message = "This Product Type is Already Exists";
                    return View(ProductTypes);
                }
                if (ProductTypes.Id == 0)
                {
                    TempData["save"] = "Product Type has been Saved";
                    _db.ProductType.Add(ProductTypes);
                }
                else
                {
                    TempData["save"] = "Update";
                    _db.ProductType.Update(ProductTypes);

                }

                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ProductTypes);
        }

        //Details Page
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null) return NotFound();
            var productType = _db.ProductType.Find(id);
            if (productType == null)
            {
                return NotFound();
            }
            return View(productType);
        }

        //Delete Get Method
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            var productType = _db.ProductType.Find(id);
            if (productType == null)
            {
                return NotFound();
            }
            return View(productType);
        }

        //Delete Post Method

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int?Id, ProductTypes ProductTypes)
        {
            if (Id == null) return NotFound();
            if (Id != ProductTypes.Id) return NotFound();
            var productType = _db.ProductType.Find(Id);
            if (productType == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                TempData["save"] = "Delete";
                _db.Remove(productType);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ProductTypes);
        }

    }
}