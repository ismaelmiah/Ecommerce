using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Online_Shop.Data;
using Online_Shop.Models;
using Online_Shop.Utility;
using cloudscribe.Pagination.Models;

namespace Online_Shop.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index(int pagenumber=1, int pagesize=4)
        {
            int ExcludeRecords = (pagesize * pagenumber) - pagesize;
            var data = _db.Product.Include(x => x.ProductTypes).Include(x => x.SpecialTags).Skip(ExcludeRecords).Take(pagesize);

            return View(data.ToList());
        }

        //Details Page
        [HttpGet]
        public IActionResult Details(int? id)
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

        //Details Page
        [HttpPost]
        [ActionName("DetailS")]
        public IActionResult ProductDetails(int? id)
        {
            List<Products> products = new List<Products>();
            if (id == null) return NotFound();
            var Product = _db.Product.Include(x => x.SpecialTags).Include(x => x.ProductTypes).
                FirstOrDefault(x => x.Id == id);
            if (Product == null)
            {
                return NotFound();
            }
            products = HttpContext.Session.Get<List<Products>>("products");
            if (products == null)
            {
                products = new List<Products>();
            }
            products.Add(Product);
            HttpContext.Session.Set("products", products);
            return View(Product);
        }

        [HttpPost]
        public IActionResult Removecart(int? id)
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products != null)
            {
                var product = products.FirstOrDefault(x=>x.Id==id);
                if (product != null)
                {
                    products.Remove(product);
                    HttpContext.Session.Set("products", products);
                }
            }
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public IActionResult Removetocart(int? id)
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products != null)
            {
                var product = products.FirstOrDefault(x => x.Id == id);
                if (product != null)
                {
                    products.Remove(product);
                    HttpContext.Session.Set("products", products);
                }
            }
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public IActionResult Cart()
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products == null)
            {
                products = new List<Products>();
            }
            return View(products);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
