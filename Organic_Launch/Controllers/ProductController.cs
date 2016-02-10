using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Organic_Launch.Controllers
{
    public class ProductController : Controller
    {
        private FarmSaleDBEntities db = new FarmSaleDBEntities();
        // GET: Product
        public ActionResult Single(int id)
        {
            return View(db.Products.Where(x => x.productID == id).FirstOrDefault());
        }

        public ActionResult List()
        {
            return View(db.Products.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(int id)
        {
            return View();
        }

        public ActionResult Delete()
        {
            return View();
        }
    }
}