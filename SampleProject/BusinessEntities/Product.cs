using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessEntities
{
    public class Product
    {
        private int _id;
        private string _name; 
        private string _description; 
        private decimal _price;
        private int _quantity;
        private ProductTypes _productType;

        public int Id 
        { 
            get => _id; 
            private set => _id = value;
        }
        // Product Name
        public string Name 
        { 
            get => _name; 
            private set => _name = value; 
        }

        // Product description
        public string Description 
        { 
            get => _description; 
            private set => _description = value; 
        }

        public ProductTypes ProductType 
        { 
            get => _productType; 
            private set => _productType = value;
        }

        // Price per unit
        public decimal Price 
        { 
            get => _price; 
            private set => _price = value; 
        }

        // Available inventory
        public int StockQuantity 
        { 
            get => _quantity; 
            private set => _quantity = value; 
        }    
        
        public int SetId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException("ID must be a positive integer.");
            }
            _id = id;
            return _id;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("Product name was not provided.");
            }
            _name = name;
        }

        public void SetDescription(string description)
        {
            _description = description ?? string.Empty;
        }

        public void SetPrice(decimal price)
        {
            if (price < 0)
            {
                throw new ArgumentOutOfRangeException("Price cannot be negative.");
            }
            _price = price;
        }

        public void SetProductType(ProductTypes productType)
        {
            _productType = productType;
        }

        public void SetStockQuantity(int quantity)
        {
            if (quantity < 0)
            {
                throw new ArgumentOutOfRangeException("Stock quantity cannot be negative.");
            }
            _quantity = quantity;
        }

        public bool IsAvailable()
        {
            return _quantity > 0;
        }

        public void UpdateStock(int quantityOrdered)
        {
            _quantity = _quantity - quantityOrdered;
        }



    }
}
