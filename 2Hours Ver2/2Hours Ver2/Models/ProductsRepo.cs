using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _2Hours_Ver2.Models
{
    public class ProductsRepo
    {
        mergedEntities db = new mergedEntities();

        public IEnumerable<Product> GetAll()
        {
            return db.Products;
        }
    }
}