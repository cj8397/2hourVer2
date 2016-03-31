using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _2Hours_Ver2.ViewModels
{
    public class Product
    {
        public int ProductID { get; set; }

        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public int? Quantity { get; set; }

        //Default constructor.
        public Product()
        {

        }
    }
}