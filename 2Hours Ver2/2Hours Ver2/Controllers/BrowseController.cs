using _2Hours_Ver2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _2Hours_Ver2.Controllers
{
    public class BrowseController : Controller
    {
        // GET: Browse
        public ActionResult Index()
        {
            mergedEntities db = new mergedEntities();
            var products = db.Products.ToList();
            //ProductsRepo products = new ProductsRepo();

            return View(products);
        }
    }
}