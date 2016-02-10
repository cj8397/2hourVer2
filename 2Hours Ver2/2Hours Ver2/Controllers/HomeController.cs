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

        public ActionResult Search(string searchString)
        {
            //Products product = new Products();
            //IEnumerable<ProductSearch> products = product.GetAllProducts(searchString);

            //// Store current search and sort filter parameters.
            //ViewBag.CurrentFilter = searchString;
            return View(/*products*/);
        }


    }//end home controller
}