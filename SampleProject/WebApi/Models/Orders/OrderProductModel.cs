using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Products
{
    public class OrderProductModel
    {
        [Required]
        public int ProductId { get; set; }
        public string Name { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}