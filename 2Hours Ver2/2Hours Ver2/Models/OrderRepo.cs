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

        // for admin area
        public OrderDetails GetOrderDetail(int orderNumber)
        {
            mergedEntities db = new mergedEntities();
            OrderDetails orderDetails = new OrderDetails();

            var query =

             from o in db.OrderDetails
             where (o.orderNumber == orderNumber)

             select new OrderDetails()
             //select new
             {
                 OrderNumber = o.orderNumber,
                 OrderDate = (DateTime)o.orderDate,
                 OrderDelivery = (DateTime)o.deliveryTime,
                 DeliveryStatus = (bool)o.deliveryStatus
             };

            foreach (var item in query)
            {

                orderDetails.OrderNumber = item.OrderNumber;
                orderDetails.OrderDate = item.OrderDate;
                orderDetails.OrderDelivery = item.OrderDelivery;
                orderDetails.DeliveryStatus = item.DeliveryStatus;
            }


            return orderDetails;
        }

        public OrderDetails UpdateOrder(OrderDetails orderDetails)
        {
            mergedEntities db = new mergedEntities();
            OrderDetail orderDetail = db.OrderDetails.Where(o => o.orderNumber == orderDetails.OrderNumber).FirstOrDefault();
            orderDetail.orderNumber = orderDetails.OrderNumber;
            orderDetail.orderDate = orderDetails.OrderDate;
            orderDetail.deliveryTime = orderDetails.OrderDelivery;
            orderDetail.deliveryStatus = orderDetails.DeliveryStatus;
            db.SaveChanges();
            return orderDetails;
        }






    }//end class OrderRepo
}