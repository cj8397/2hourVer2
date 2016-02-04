using _2Hours_Ver2.ViewModels;
using shoppingCart.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _2Hours_Ver2.Controllers
{
    public class HomeController : Controller
    {
        private const int DEFAULT_QTY = 1;
        private ShoppingCartEntities db = new ShoppingCartEntities();
        private ShoppingCart shoppingCart = new ShoppingCart();
        private Session session = new Session();

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add(int productID)
        {
            var item = shoppingCart.GetItem(productID, session.SessionID);
            if (item == null)
            {
                item = shoppingCart.NewCartItem(productID, session.SessionID, DEFAULT_QTY);
            }
            else {
                //default quantity to one
                item.Quantity = item.Quantity ?? DEFAULT_QTY;
                //shoppingCart.UpdateCartItem(item);
            }

            return View(item);
        }
        [HttpPost]
        public ActionResult Add(CartItem item)
        {
            using (var shoppingCart = new ShoppingCart())
            {
                if (ModelState.IsValid)
                {
                    string sessionId = session.SessionID;
                    shoppingCart.StoreItem(item.ProductID, item.Quantity, sessionId);
                    return RedirectToAction("ViewCart");
                }
                return View("Add");
            }
        }

        public ActionResult ViewCart()
        {
            var items = shoppingCart.GetAllItems(session.SessionID);

            if (items.Any())
            {
                Session["shoppingCart"] = items.Count();
            }
            else
            {
                Session["shoppingCart"] = null;
            }
            var order = new Order
            {
                CartItems = items,
                Tax = ShoppingCart.TAX_RATE
            };

            return View(order);
        }
    }
}