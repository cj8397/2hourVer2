using _2Hours_Ver2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _2Hours_Ver2.Models
{
    public class ShoppingCart: IDisposable
    {
        public const decimal TAX_RATE = 0.07m;
        private mergedEntities db = new mergedEntities();


        public IQueryable<CartItem> GetAllItems(string sessionID)
        {
            var products = (from p in db.Products
                            from op in p.OrderProducts
                            where op.sessionID == sessionID && op.productID == p.productID
                            select new CartItem
                            {
                                ProductID = p.productID,
                                ProductName = p.productName,
                                Price = (decimal)p.price,
                                Quantity = (int)op.quantity,
                                SessionID = op.sessionID
                            });
            return products;
        }

        public CartItem GetItem(int productID, string sessionID)
        {
            var item = GetAllItems(sessionID)
                          .Where(op => op.SessionID == sessionID && op.ProductID == productID)
                          .Select(op => new CartItem
                          {
                              ProductID  = op.ProductID,
                              ProductName = op.ProductName,
                              Price       = op.Price,
                              Quantity    = op.Quantity,
                              SessionID   = op.SessionID

                          }).FirstOrDefault();
            return item;
        }

        public CartItem NewCartItem(int productID, string sessionID, int? quantity = null)
        {
            AddItem(productID, quantity, sessionID);
            return GetItem(productID, sessionID);
        }

        public void AddItem(int productID, int? qty, string sessionID)
        {
            try
            {
                var item = new OrderProduct();
                item.sessionID = sessionID;
                item.quantity = qty;
                item.productID = productID;
                item.updatedSession = DateTime.Now;

                if (db.Visits.Any(v => v.sessionID == sessionID))
                {
                    isValidItem(sessionID, productID);
                }

                db.OrderProducts.Add(item);
                db.SaveChanges();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }

        }

        public IEnumerable<CartItem> UpdateOrder(IEnumerable<CartItem> items, string sessionID)
        {
            if (items != null)
            {
                foreach (var item in items)
                {
                    if (isValidItem(sessionID, item.ProductID))
                    {
                        AddItem(item.ProductID, item.Quantity, sessionID);
                    }
                }

                db.SaveChanges();
            }
            return items;
        }


        public void RemoveItem(int productID, string sessionID)
        {
            var selected = db.OrderProducts.Single(p => p.sessionID == sessionID && p.productID == productID);
            db.OrderProducts.Remove(selected);
            db.SaveChanges();
        }


        private bool isValidItem(string sessionID, int productID)
        {
            //check for duplicate sessions and products
            if (db.OrderProducts.Any(s => s.sessionID == sessionID && s.productID == productID))
            {
                RemoveItem(productID, sessionID);
                return true;
            }
            return false;
        }

        public void Dispose()
        {
            db.Dispose();
        }

    }//end ShoppingCart
}