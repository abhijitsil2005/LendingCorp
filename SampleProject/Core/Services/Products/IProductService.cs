using System;
using System.Collections.Generic;
using BusinessEntities;

namespace Core.Services.Products
{
    public interface IProductService
    {
        List<Product> GetAll();
        Product GetProductById(int id);
        Product Add(Product product);
        Product Update(Product updatedProduct);
        bool Delete(int id);
        List<Product> GetProducts(ProductTypes? productTypes = null, string name = null, decimal? minPrice = null, decimal? maxPrice = null);
    }
}
