using _2Hours_Ver2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _2Hours_Ver2.Models
{
    public class OrderRepo
    {
        //Get order details
        public OrderDetail GetOrderDetail()
        {
            mergedEntities db = new mergedEntities();
            OrderDetails repo = new OrderDetails();
            OrderDetail order = new OrderDetail();

            var query =
            from a in db.OrderDetails
            where (a.orderNumber == repo.OrderNumber)
            select new
            {
                OrderNumber = a.orderNumber,
                OrderDate = a.orderDate,
                Delivery = a.deliveryStatus,
            };

            foreach (var item in query)
            {
                order.orderNumber = item.OrderNumber;
                order.orderDate = item.OrderDate;
                order.deliveryStatus = item.Delivery;
            }

            return order;
        }






    }//end class OrderRepo
}