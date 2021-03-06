﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _2Hours_Ver2.ViewModels
{
    public class OrderDetails
    {
        public IEnumerable<CartItem> CartItems { get; set; }

        [Display(Name = "Sub-Total")]
        public decimal SubTotal
        {
            get
            {
                if (CartItems != null)
                {
                    return CartItems.Sum(ci => ci.Price * (ci.Quantity ?? 0));
                }
                else return 0;
            }
        }

        private decimal taxRate;
        private decimal tax;
        public decimal Tax
        {
            get { return tax; }
            set
            {
                taxRate = value;
                Total = SubTotal + (SubTotal * value);
                tax = Total - SubTotal;

            }
        }

        public decimal Total { get; set; }

        public int OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime OrderDelivery { get; set; }
        public bool DeliveryStatus { get; set; }
    }
}