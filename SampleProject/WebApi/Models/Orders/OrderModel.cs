using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BusinessEntities;

namespace WebApi.Models.Products
{
    public class OrderModel
    {
        [Required]
        public List<OrderProductModel> OrderProducts { get; set; }
        //public DateTime OrderDate { get; set; }
    }
}