using BusinessEntities;
using System.Collections.Generic;

namespace Core.Services.Orders
{
    public interface IOrderService
    {
        List<Order> GetAllOrders();
        Order GetOrderById(int id);
        Order CreateOrder(List<OrderProduct> orderProducts);
        Order UpdateOrder(int orderId, List<OrderProduct> orderProducts);
        Order UpdateOrderStatus(int orderId, int statusId);
        bool OrderEligibleForUpdate(int orderId);
        bool OrderProductQuantityAvailableForUpdate(int orderId, List<OrderProduct> orderProducts);
        OrderProduct GetOrderProductDetails(int ordProdId, int ordProQuantity);
        decimal CalculateTotalProductPrice(decimal price, int quantity);
    }
}
