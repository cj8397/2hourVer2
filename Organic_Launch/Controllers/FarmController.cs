using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Organic_Launch.Controllers
{
    public class FarmController : Controller
    {
        private FarmSaleDBEntities db = new FarmSaleDBEntities();
        // GET: Farmer
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            return View(db.Products.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            return View(db.Farms.Where(i => i.farmID == id).FirstOrDefault());
        }

        [HttpPost]
        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            db.Farms.Remove(db.Farms.Where(i => i.farmID == id).FirstOrDefault());
            return View();
        }
    }
}