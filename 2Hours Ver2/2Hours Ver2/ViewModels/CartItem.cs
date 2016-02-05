using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _2Hours_Ver2.ViewModels
{
    public class CartItem
    {
        public int ProductID { get; set; }

        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        public decimal Price { get; set; }

        [Required]
        [RegularExpression(@"^[1-9]\d*(,\d+)?$", ErrorMessage = "The quantity entered must be a valid number.")]
        [Range(1, Int32.MaxValue, ErrorMessage = "The quantity entered must be greater than zero.")]
        public int? Quantity { get; set; }

        public string SessionID { get; set; }


        //Default constructor.
        public CartItem()
        {
        }

    }//end CartItem
}