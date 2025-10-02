using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessEntities
{
    public class Order
    {
        public int _orderId;
        public List<OrderProduct> _orderProduct;
        private DateTime _orderDate; 
        private decimal _orderPrice; 
        private OrderStatus _orderStatus;
        private DateTime _orderUpdateDate;

        public int OrderId 
        { 
            get => _orderId; 
            private set => _orderId = value;
        }
        public List<OrderProduct> OrderProduct 
        { 
            get => _orderProduct; 
            private set => _orderProduct = value;
        }
        public DateTime OrderDate 
        { 
            get => _orderDate; 
            private set => _orderDate = value;
        }
        public decimal OrderPrice 
        { 
            get => _orderPrice; 
            private set => _orderPrice = value;
        }
        public OrderStatus OrderStatus 
        { 
            get => _orderStatus; 
            private set => _orderStatus = value;
        }
        public DateTime OrderUpdateDate
        {
            get => _orderUpdateDate;
            private set => _orderUpdateDate = value;
        }


        public int SetOrderId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException("ID must be a positive integer.");
            }
            _orderId = id;
            return _orderId;
        }
        public List<OrderProduct> SetOrderProduct(List<OrderProduct> orderProducts)
        {
            if (orderProducts == null || orderProducts.Count == 0)
            {
                throw new ArgumentNullException("Order must contain at least one product.");
            }
            _orderProduct = orderProducts;
            return _orderProduct;
        }
        public DateTime SetOrderDate(DateTime orderDate)
        {
            if (orderDate == null)
            {
                throw new ArgumentNullException("Order date was not provided.");
            }
            _orderDate = orderDate;
            return _orderDate;
        }
        public decimal SetOrderPrice(decimal price)
        {
            if (price < 0)
            {
                throw new ArgumentOutOfRangeException("Order price cannot be negative.");
            }
            _orderPrice = price;
            return _orderPrice;
        }
        public OrderStatus SetOrderStatus(OrderStatus status)
        {
            _orderStatus = status;
            return _orderStatus;
        }

        public DateTime SetOrderUpdatedDate(DateTime orderUpdatedDate)
        {
            if (orderUpdatedDate == null)
            {
                throw new ArgumentNullException("Order date was not provided.");
            }
            _orderUpdateDate = orderUpdatedDate;
            return _orderUpdateDate;
        }
    }
}
