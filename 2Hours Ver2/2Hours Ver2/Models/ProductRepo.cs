using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _2Hours_Ver2.Models
{
    public class ProductRepo
    {
        private mergedEntities db = new mergedEntities();
        public IQueryable<Product> GetAllProducts()
        {
            var products = (from p in db.Products
                            where productID == p.productID
                            select new Product
                            {
                                ProductID = p.productID,
                                ProductName = p.productName,
                                Price = (decimal)p.price,
                                Quantity = (int)pv.qtyOrdered,

                            });
            return products;
        }

        public ProductRepo GetProduct(int productID)
        {
            var product = GetAllProducts()
                          .Where(pv => pv.ProductID == productID)
                          .Select(pv => new Product
                          {
                              ProductID = pv.ProductID,
                              ProductName = pv.ProductName,
                              Price = pv.Price,
                              Quantity = pv.Quantity

                          }).FirstOrDefault();
            return product;
        }

        public ProductRepo NewProduct(int productID, string productName)
        {
            StoreProduct(productID, productName);
            return GetProduct(productID);
        }

        public void StoreProduct(int productID, string productName)
        {
            var product = new Product();
            product.productName = productName;
            product.productID = productID;

            db.Products.Add(product);
            db.SaveChanges();

        }

        public void UpdateProduct(int productID)
        {
            if (products != null)
            {
                foreach (var item in products)
                {
                    if (isValidItem(item.ProductID))
                    {
                        StoreProduct(item.ProductID, item.ProductName);
                    }
                }

                db.SaveChanges();
            }
            return products;
        }

        public Product UpdateProduct(int productID, string productName)
        {
            Product product = db.Products.Where(p => p.productID == productID)
                       .FirstOrDefault();
            product.productName = productName;
            db.SaveChanges();
            return product;
        }


        public void RemoveProduct(int productID)
        {
            var selected = db.Products.Single(p => p.productID == productID);
            db.Products.Remove(selected);
            db.SaveChanges();
        }


        private bool isValidItem(int productID)
        {
            //check for duplicate products
            if (db.Products.Any(s => s.productID == productID))
            {
                RemoveProduct(productID);
                return true;
            }
            return false;
        }


        public Product GetProductDetails(int productID)
        {
            return GetAllProducts().Where(p => p.productID == productID).FirstOrDefault();
        }
    }
}