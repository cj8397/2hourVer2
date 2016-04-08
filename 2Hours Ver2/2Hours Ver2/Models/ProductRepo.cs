using _2Hours_Ver2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _2Hours_Ver2.Models
{
    public class ProductRepo
    {
        public IEnumerable<OrderProductVM> GetDetail(int orderNumber /*, string accountType*/)
        {
            mergedEntities db = new mergedEntities();

            IEnumerable<OrderProductVM> orderProductVM;


            orderProductVM =

            from o in db.OrderDetails
            where (o.orderNumber == orderNumber)
            from op in o.OrderProducts

            select new OrderProductVM()
             //select new
             {
                OrderNumber = o.orderNumber,
                ProductId = op.productID,
                ProductName = op.Product.productName,
                Quantity = (decimal)op.quantity                
            };

            return orderProductVM;
        }
        
    }
}