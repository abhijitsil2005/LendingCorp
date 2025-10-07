using BusinessEntities;
using Common;
using Core.Services.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Services.Orders
{
    [AutoRegister(AutoRegisterTypes.Singleton)]
    public class OrderService : IOrderService
    {
        private readonly List<Order> _orders = new List<Order>();
        private readonly IProductService _productService;
        private int _nextOrderId = 1;

        public List<Order> GetAll() => _orders;

        public OrderService(IProductService productService)
        {
            _productService = productService;
        }

        public Order CreateOrder(List<OrderProduct> orderProducts)
        {
            Order order = new Order();
            order.SetOrderId(_nextOrderId++);
            // Set OrderProduct details
            order.SetOrderProduct(orderProducts);
            order.SetOrderDate(DateTime.Now);
            order.SetOrderStatus(OrderStatus.Pending);
            order.SetOrderPrice(orderProducts.Sum(op => op.TotalPrice));
            _orders.Add(order);
            return order;
        }

        public Order UpdateOrder(int orderId, List<OrderProduct> orderProducts)
        {
            var existingOrder = _orders.FirstOrDefault(o => o.OrderId == orderId);
            if (existingOrder == null)
                return null;

            // update order is permissible only for pending orders
            if (OrderEligibleForUpdate(orderId))
            {
                // Set OrderProduct with new details
                existingOrder.SetOrderProduct(orderProducts);
                existingOrder.SetOrderUpdatedDate(DateTime.Now);
                existingOrder.SetOrderPrice(orderProducts.Sum(op => op.TotalPrice));
            }
            else
            {
                throw new InvalidOperationException("Only pending orders can be updated.");
            }

            return existingOrder;
        }

        public Order UpdateOrderStatus(int orderId, int statusId)
        {
            var existingOrder = _orders.FirstOrDefault(o => o.OrderId == orderId);
            if (existingOrder == null)
                return null;

            // Convert statusId to OrderStatus enum
            if (!Enum.IsDefined(typeof(OrderStatus), statusId))
                throw new ArgumentOutOfRangeException(nameof(statusId), "Invalid status ID.");

            var newStatus = (OrderStatus)statusId;
            existingOrder.SetOrderStatus(newStatus);
            existingOrder.SetOrderUpdatedDate(DateTime.Now);

            return existingOrder;
        }

        public bool OrderEligibleForUpdate(int orderId)
        {
            var existingOrder = GetOrderById(orderId);
            // update order is permissible only for pending orders
            if (existingOrder != null && existingOrder.OrderProduct != null && existingOrder.OrderProduct.Count > 0 && existingOrder.OrderStatus == OrderStatus.Pending)
            {
                return true;
            }
            return false;
        }

        public bool OrderProductQuantityAvailableForUpdate(int orderId, List<OrderProduct> orderProducts)
        {
            var existingOrder = GetOrderById(orderId);
            if (existingOrder == null || existingOrder.OrderProduct == null || existingOrder.OrderProduct.Count == 0)
            {
                throw new ArgumentNullException("Order not found or does not contain any products.");
            }
            foreach (var ordProd in orderProducts)
            {
                var product = _productService.GetProductById(ordProd.ProductId);
                if (product == null)
                {
                    throw new ArgumentNullException($"Product with ID: {ordProd.ProductId} not found.");
                }
                // TODO: Check for available stock including already allocated stock for the order
                var existingOrderProduct = existingOrder.OrderProduct
                    .FirstOrDefault(op => op.ProductId == ordProd.ProductId);

                int alreadyAllocated = existingOrderProduct != null ? existingOrderProduct.Quantity : 0;
                int availableStock = product.StockQuantity + alreadyAllocated;

                if (ordProd.Quantity > availableStock)
                {
                    throw new ArgumentOutOfRangeException(
                        $"Only {availableStock} items are available (including already allocated) for product ID: {product.Id}");
                }
            }
            return true;
        }

        public OrderProduct GetOrderProductDetails(int ordProdId, int ordProQuantity)
        {
            var ordProduct = new OrderProduct();
            ordProduct.SetProductId(ordProdId);
            // get the product details from Product service based on product id
            Product product = _productService.GetProductById(ordProdId);

            // Check for available stock
            if (product.IsAvailable())
            {
                if (ordProQuantity > product.StockQuantity)
                {
                    throw new ArgumentOutOfRangeException($"Only {product.StockQuantity} items are available in stock for product ID: {product.Id}");
                }
                else
                {
                    // set product detail in order
                    ordProduct.SetName(product.Name);
                    ordProduct.SetDescription(product.Description);
                    ordProduct.SetQuantity(ordProQuantity);
                    ordProduct.SetTotalPrice(CalculateTotalProductPrice(product.Price, ordProQuantity));

                    // reduce the stock quantity for the product
                    product.SetStockQuantity(product.StockQuantity - ordProQuantity);
                    _productService.Update(product);
                }
            }
            else
            {
                throw new InvalidOperationException($"Product with ID: {product.Id} is out of stock.");
            }
            return ordProduct;
        }

        public Order GetOrderById(int id)
        {
            return _orders.FirstOrDefault(o => o.OrderId == id);
        }

        public decimal CalculateTotalProductPrice(decimal price, int quantity)
        {
            if (price > 0 && quantity > 0)
            {
                return price * quantity;
            }
            return 0;
        }

        public List<Order> GetAllOrders()
        {
            return _orders;
        }

        

        

    }
}
