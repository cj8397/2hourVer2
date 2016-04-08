using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _2Hours_Ver2.ViewModels
{
    public class OrderProductVM
    {
        [Required]
        [Display(Name = "Order number")]
        public int OrderNumber { get; set; }

        [Required]
        [Display(Name = "Product Id")]
        public int ProductId { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Required]
        [Display(Name = "Quantity")]
        [Range(0, 10000, ErrorMessage = "Invalid Quantity!")]
        public decimal Quantity { get; set; }
    }
}