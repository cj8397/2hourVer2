using _2Hours_Ver2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _2Hours_Ver2.Models
{
    public class ProductRepo
    {
        public IEnumerable<OrderProductVM> GetDetail(int orderNumber)
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

        public OrderProductVM GetOrderProductDetail(int orderNumber,int productId)
        {
            mergedEntities db = new mergedEntities();
            OrderProductVM orderProductVM = new OrderProductVM();

            var query =

             from o in db.OrderDetails
             where (o.orderNumber == orderNumber)
             from op in o.OrderProducts
             where (op.productID == productId)

             select new OrderProductVM()
             //select new
             {
                 OrderNumber = o.orderNumber,
                 ProductId = op.productID,
                 ProductName = op.Product.productName,
                 Quantity = (decimal)op.quantity
             };

            foreach (var item in query)
            {

                orderProductVM.OrderNumber = item.OrderNumber;
                orderProductVM.ProductId = item.ProductId;
                orderProductVM.ProductName = item.ProductName;
                orderProductVM.Quantity = item.Quantity;                
            }


            return orderProductVM;
        }

        public OrderProductVM UpdateOrderProduct(OrderProductVM orderProductVM)
        {
            mergedEntities db = new mergedEntities();
            OrderProduct orderProduct = db.OrderProducts.Where(o => o.orderNumber == orderProductVM.OrderNumber && 
                                                         o.productID == orderProductVM.ProductId).FirstOrDefault();
            orderProduct.productID = orderProductVM.ProductId;
            orderProduct.quantity = orderProductVM.Quantity;

            db.SaveChanges();            
            return orderProductVM;
        }

        public void DeleteOrderProduct(int orderNumber, int productId)
        {
            mergedEntities db = new mergedEntities();
            var selected = db.OrderProducts.Single(o => o.orderNumber == orderNumber &&
                                                        o.productID == productId);
            db.OrderProducts.Remove(selected);
            db.SaveChanges();
        }

    }
}