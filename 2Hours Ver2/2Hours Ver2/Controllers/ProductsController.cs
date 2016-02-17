using _2Hours_Ver2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _2Hours_Ver2.Controllers
{
    public class ProductsController : Controller
    {
        private mergedEntities db = new mergedEntities();
        private ProductRepo productRepo = new ProductRepo();
        private Product prod = new Product();

        public ActionResult Add(int productID, string productName)
        {
            var product = this.productRepo.GetProduct(productID);
            if (product == null)
            {
                product = this.productRepo.NewProduct(productID, productName);
            }

            return View(product);
        }
        [HttpPost]
        public ActionResult Add(Product product)
        {
            using (var productRepo = new ProductRepo())
            {
                if (ModelState.IsValid)
                {
                    productRepo.StoreProduct(product.productID, product.productName);
                }
                return View("Add");
            }
        }
        
        [HttpGet]
        public ActionResult Details(int productID)
        {
            prod = productRepo.GetProductDetails(productID);
            return View(prod);
        }

        public ActionResult UpdateProduct(int productID)
        {
            productRepo.UpdateProduct(productID);
            return RedirectToAction("Details");
        }

        public ActionResult RemoveProduct(int productID)
        {
            productRepo.RemoveProduct(productID);
            return RedirectToAction("Details");
        }

    }
}