using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BusinessEntities;

namespace WebApi.Models.Products
{
    public class ProductModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        public ProductTypes Type { get; set; }
        [Required]
        public int StockQuantity { get; set; }
    }
}