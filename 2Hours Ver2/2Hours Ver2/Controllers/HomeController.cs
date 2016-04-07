using _2Hours_Ver2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _2Hours_Ver2.Controllers
{
    public class HomeController : Controller
    {
        private mergedEntities db = new mergedEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AboutUs()
        {
            return View();
        }
        public ActionResult FAQ()
        {
            return View();
        }

        public ActionResult ShopProducts()
        {
            var products = db.Products.ToList();
            return View(products);
        }

        public ActionResult ContactUs()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string search)
        {
            var product = db.Products
                .Where(x => x.productName.StartsWith(search)
                       || search == null
                       || x.productName.Contains(search) // search by productName
            );

            ViewBag.search = true;

            return View("ShopProducts", product);
        }


    }//end home controller
}