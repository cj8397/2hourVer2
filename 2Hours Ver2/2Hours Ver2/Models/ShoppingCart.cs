using _2Hours_Ver2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _2Hours_Ver2.Models
{
    public class ShoppingCart
    {
        public const decimal TAX_RATE = 0.07m;
        private mergedEntities db = new mergedEntities();


        public IQueryable<CartItem> GetAllItems(string sessionID)
        {
            var products = (from p in db.Products
                            from od in p.OrderDetails
                            where od.sessionID == sessionID && od.productID == p.productID
                            select new CartItem
                            {
                                ProductID = p.productID,
                                ProductName = p.productName,
                                Price = (decimal)p.price,
                                Quantity = (int)od.qtyOrdered,
                                SessionID = od.sessionID

                            });
            return products;
        }

        public CartItem GetItem(int productID, string sessionID)
        {
            var product = GetAllItems(sessionID)
                          .Where(pv => pv.SessionID == sessionID && pv.ProductID == productID)
                          .Select(pv => new CartItem
                          {
                              ProductID = pv.ProductID,
                              ProductName = pv.ProductName,
                              Price = pv.Price,
                              Quantity = pv.Quantity,
                              SessionID = pv.SessionID

                          }).FirstOrDefault();
            return product;
        }

        public CartItem NewCartItem(int productID, string sessionID, int? quantity = null)
        {
            StoreItem(productID, quantity, sessionID);
            return GetItem(productID, sessionID);
        }

        public void StoreItem(int productID, int? qty, string sessionID)
        {
            var item = new OrderDetail();
            item.sessionID = sessionID;
            item.qtyOrdered = qty;
            item.productID = productID;
            item.updated = DateTime.Now;

            if (db.Visits.Any(s => s.sessionID == sessionID))
            {
                isValidItem(sessionID, productID);
            }

            db.OrderDetails.Add(item);
            db.SaveChanges();

        }

        public IEnumerable<CartItem> UpdateOrder(IEnumerable<CartItem> items, string sessionID)
        {
            if (items != null)
            {
                foreach (var item in items)
                {
                    if (isValidItem(sessionID, item.ProductID))
                    {
                        StoreItem(item.ProductID, item.Quantity, sessionID);
                    }
                }

                db.SaveChanges();
            }
            return items;
        }


        public void RemoveItem(int productID, string sessionID)
        {
            var selected = db.OrderProduct.Single(p => p.sessionID == sessionID && p.productID == productID);
            db.OrderDetails.Remove(selected);
            db.SaveChanges();
        }


        private bool isValidItem(string sessionID, int productID)
        {
            //check for duplicate sessions and products
            if (db.OrderDetails.Any(s => s.sessionID == sessionID && s.productID == productID))
            {
                RemoveItem(productID, sessionID);
                return true;
            }
            return false;
        }
    }
}