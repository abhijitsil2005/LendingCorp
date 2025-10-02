using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace BusinessEntities
{
    public class OrderProduct
    {
        private int _productId;
        private string _name;
        private string _description;
        private int _quantityOrdered;
        private decimal _totalPrice;

        public int ProductId
        {
            get => _productId;
            private set => _productId = value;
        }
        public string Name
        {
            get => _name;
            private set => _name = value;
        }
        public string Description
        {
            get => _description;
            private set => _description = value;
        }
        public int Quantity
        {
            get => _quantityOrdered;
            private set => _quantityOrdered = value;
        }
        public decimal TotalPrice
        {
            get => _totalPrice;
            private set => _totalPrice = value;
        }

        public int SetProductId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException("ProductID must be a positive integer.");
            }
            _productId = id;
            return _productId;
        }
        public string SetName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("Name was not provided.");
            }
            _name = name;
            return _name;
        }
        public string SetDescription(string description)
        {
            _description = description;
            return _description;
        }
        public int SetQuantity(int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentOutOfRangeException("Quantity must be a positive integer.");
            }
            _quantityOrdered = quantity;
            return _quantityOrdered;
        }
        public decimal SetTotalPrice(decimal price)
        {
            if (price < 0)
            {
                throw new ArgumentOutOfRangeException("Total Price cannot be negative.");
            }
            _totalPrice = price;
            return _totalPrice;
        }

        //public OrderProduct(int productId, int quantity)
        //{
        //    if (productId <= 0)
        //    {
        //        throw new ArgumentOutOfRangeException("Product ID must be a positive integer.");
        //    }
        //    if (quantity <= 0)
        //    {
        //        throw new ArgumentOutOfRangeException("Quantity must be a positive integer.");
        //    }
        //    ProductId = productId;
        //    Quantity = quantity;
        //}
    }
}
