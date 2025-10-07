using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApi.Models.Orders
{
    public class OrderData
    {
        public int OrderId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public List<OrderProductData> OrderProducts { get; set; }
        public decimal OrderPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime OrderUpdateDate { get; set; }
        public OrderData(Order order)
        {
            OrderId = order.OrderId;
            OrderStatus = order.OrderStatus;
            OrderProducts = order.OrderProduct?.Select(op => new OrderProductData(op)).ToList();
            OrderPrice = order.OrderPrice;
            OrderDate = order.OrderDate;
            OrderUpdateDate = order.OrderUpdateDate;
        }
    }
}