using BusinessEntities;
using Core.Services.Orders;
using Core.Services.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models.Orders;
using WebApi.Models.Products;

namespace WebApi.Controllers
{
    [RoutePrefix("order")]
    public class OrderController : BaseApiController
    {
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;

        public OrderController(IProductService productService, IOrderService orderService)
        {
            _productService = productService;
            _orderService = orderService;
        }

        [Route("create")]
        [HttpPost]
        public HttpResponseMessage CreateOrder([FromBody] OrderModel orderModel)
        {
            Order order = _orderService.CreateOrder(MapOrderProductModels(orderModel));

            return Found(new OrderData(order));
        }

        [Route("{orderId:int}")]
        [HttpGet]
        public HttpResponseMessage GetOrder(int orderId)
        {
            var order = _orderService.GetOrderById(orderId);
            if (order == null)
            {
                string message = "Order not found.";
                return DoesNotExist(message);
            }
            return Found(new OrderData(order));
        }

        [Route("listAll")]
        [HttpGet]
        public HttpResponseMessage GetAllOrders()
        {
            var orders = _orderService.GetAllOrders();
            if (orders == null || !orders.Any())
            {
                string message = "No Order found.";
                return DoesNotExist(message);
            }
            var orderDataList = orders.Select(o => new OrderData(o)).ToList();
            return Found(orderDataList);
        }

        [Route("{orderId:int}/update")]
        [HttpPost]
        public HttpResponseMessage UpdateOrder(int orderId, [FromBody] OrderModel orderModel)
        {
            // Only pending orders can be updated
            if (_orderService.OrderEligibleForUpdate(orderId) == false)
            {
                string message = "Only pending orders can be updated.";
                return BadUpdateRequest(message);
            }

            // TODO: Check if updated product quantity is available in stock
            if (_orderService.OrderProductQuantityAvailableForUpdate(orderId, MapOrderProductModels(orderModel)) == false)
            {
                string message = "Updated product quantity is not available in stock.";
                return BadUpdateRequest(message);
            }

            var updatedOrder = _orderService.UpdateOrder(orderId, MapOrderProductModels(orderModel));
            if (updatedOrder == null)
            {
                string message = "Product not found.";
                return DoesNotExist(message);
            }

            return Found(new OrderData(updatedOrder));
        }

        [Route("{orderId:int}/updateStatus/{statusId:int}")]
        [HttpPost]
        public HttpResponseMessage UpdateOrderStatus(int orderId, int statusId)
        {
            var order = _orderService.GetOrderById(orderId);
            if (order == null)
            {
                string message = "Order not found.";
                return DoesNotExist(message);
            }

            order = _orderService.UpdateOrderStatus(orderId, statusId);
            return Found(new OrderData(order));
        }


        public List<OrderProduct> MapOrderProductModels(OrderModel orderModel)
        {
            var orderProducts = new List<OrderProduct>();
            foreach (var ordPro in orderModel.OrderProducts)
            {
                orderProducts.Add(_orderService.GetOrderProductDetails(ordPro.ProductId, ordPro.Quantity));
            }
            return orderProducts;
        }

        
    }
}