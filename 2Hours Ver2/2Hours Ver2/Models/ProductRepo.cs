using _2Hours_Ver2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _2Hours_Ver2.Models
{
    public class ProductRepo
    {

        public OrderProductVM getDetail(int orderNumber)
        {
           OrderProductVM orderProductVM = new OrderProductVM();
            mergedEntities db = new mergedEntities();

           var query =
            from o in db.OrderDetails
            where (o.orderNumber == orderNumber)
            from op in o.OrderProducts
           
            select new OrderProductVM
            {
                OrderNumber = o.orderNumber,
                ProductID = op.productID,
                Quantity = (decimal)op.quantity

            };
            foreach(var item in query)
            {
                orderProductVM.OrderNumber = item.OrderNumber;
                orderProductVM.ProductID = item.ProductID;
                orderProductVM.Quantity = item.Quantity;

            }

            return orderProductVM;
        }
    }
}
        
