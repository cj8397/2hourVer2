using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2Hours_Ver2.ViewModels
{
    public class ProductVM
    {
        [Required]
        [Display(Name = "Product ID")]
        public int ProductId { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }


        [Required]
        [Display(Name = "Unit")]
        public string Unit { get; set; }

        [Required]
        [Display(Name = "Price")]
        public decimal Price { get; set; }


        [Required]
        [Display(Name = "Product Type")]
        public string ProductType { get; set; }

        [Required]
        [Display(Name = "Supplier ID")]
        public int SupplierId { get; set; }
    }
}
