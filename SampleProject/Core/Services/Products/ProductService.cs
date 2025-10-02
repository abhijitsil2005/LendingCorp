using BusinessEntities;
using Common;
using Core.Factories;
using Data.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Services.Products
{
    [AutoRegister(AutoRegisterTypes.Singleton)]
    public class ProductService : IProductService
    {
        private readonly List<Product> _products = new List<Product>();
        private int _nextId = 1;

        public List<Product> GetAll() => _products;

        public Product Add(Product product)
        {
            product.SetId(_nextId++);
            _products.Add(product);
            return product;
        }

        public Product Update(Product product)
        {
            var existingProduct = _products.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct == null)
                return null;

            existingProduct.SetName(product.Name);
            existingProduct.SetDescription(product.Description);
            existingProduct.SetProductType(product.ProductType);
            existingProduct.SetPrice(product.Price);
            existingProduct.SetStockQuantity(product.StockQuantity);
            return product;
        }

        public bool Delete(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return false;
            _products.Remove(product);
            return true;
        }

        public Product GetProductById(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public List<Product> GetProducts(ProductTypes? productType = null, string name = null, decimal? minPrice = null, decimal? maxPrice = null)
        {
            return _products.FindAll(p =>
                ((productType != null) ? p.ProductType == productType : productType == null) &&
                ((name !=null) ? p.Name.IndexOf(name,StringComparison.OrdinalIgnoreCase) >= 0 : name == null) &&
                ((minPrice > 0 && maxPrice > 0) 
                    ? (p.Price >= minPrice && p.Price <= maxPrice) 
                    : (minPrice > 0) 
                        ? (p.Price >= minPrice) 
                        : (maxPrice > 0)
                            ? (p.Price <= maxPrice)
                            : (minPrice == 0 && maxPrice == 0)
                )
                //(minPrice == null || p.Price >= minPrice)
                //(maxPrice == null || p.Price <= maxPrice)
            );
        }
    }
}
