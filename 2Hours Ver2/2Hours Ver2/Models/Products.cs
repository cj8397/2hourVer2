using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _2Hours_Ver2.ViewModels;

namespace _2Hours_Ver2.Models
{
    public class Products
    {
        public const string PRODUCT_NAME = "name";
        private mergedEntities db = new mergedEntities();

        //IEnumerable<ProductSearch> FilterProducts(IEnumerable<ProductSearch> products, string searchString)
        //{
        //    // Filter results based on search.
        //    if (!String.IsNullOrEmpty(searchString))
        //        products = products.Where(
        //                    p => p.ProductName.ToUpper().Contains(searchString.ToUpper()));
        //    return products;
        //}


        //public IQueryable<ProductSearch> GetAllProducts(string searchString)
        //{
        //    var products = (from p in db.Products
        //                    from op in p.OrderProducts
        //                    where op.productID == p.productID
        //                    select new ProductSearch
        //                    {
        //                        ProductID = p.productID,
        //                        ProductName = p.productName,
        //                        Price = (decimal)p.price
        //                    });

        //    products = FilterProducts(products, searchString);
        //    return products;
        //}

    }//end product repo
}