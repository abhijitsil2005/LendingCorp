using BusinessEntities;

namespace WebApi.Models.Orders
{
    public class OrderProductData
    {
        public OrderProductData(OrderProduct op)
        {
            ProductId = op.ProductId;
            Name = op.Name;
            Description = op.Description;
            Quantity = op.Quantity;
            TotalPrice = op.TotalPrice;
        }

        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }

    }
}