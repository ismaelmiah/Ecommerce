using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Online_Shop.Data;
using Online_Shop.Models;

namespace Online_Shop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductTypesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductTypesController(ApplicationDbContext db)
        {
            _db = db;
        }

        ProductTypes ProductTypes = new ProductTypes();
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
                return View(new ProductTypes());
            }
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
                if (ProductTypes.Id == 0)
                {
                    _db.ProductType.Add(ProductTypes);
                }
                else
                {
                    _db.ProductType.Update(ProductTypes);

                }

                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ProductTypes);
        }
    }
}