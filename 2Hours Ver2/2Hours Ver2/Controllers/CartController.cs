using _2Hours_Ver2.Models;
using _2Hours_Ver2.ViewModels;
using shoppingCart.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _2Hours_Ver2.Controllers
{
    public class CartController : Controller
    {
        private const int DEFAULT_QTY = 1;
        private mergedEntities db = new mergedEntities();
        private ShoppingCart shoppingCart = new ShoppingCart();
        private Session session = new Session();

        public ActionResult Index()
        {
            var products = db.Products.ToList();
            return View(products);
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
                    shoppingCart.AddItem(item.ProductID, item.Quantity, sessionId);
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
            var order = new OrderDetails
            {
                CartItems = items,
                Tax = ShoppingCart.TAX_RATE
            };

            return View(order);
        }

        [HttpPost]
        public ActionResult ViewCart(IEnumerable<CartItem> cartItem)
        {
            shoppingCart.UpdateOrder(cartItem, session.SessionID);
            return RedirectToAction("ViewCart", shoppingCart);
        }

        public ActionResult ThankYou()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CancelOrder()
        {
            session.Clear();
            return RedirectToAction("ThankYou");
        }


        public ActionResult RemoveItem(int productID)
        {
            shoppingCart.RemoveItem(productID, session.SessionID);
            return RedirectToAction("ViewCart");
        }
    }
}