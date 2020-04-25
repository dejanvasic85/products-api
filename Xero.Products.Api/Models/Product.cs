using System;

namespace Xero.Products.Api.Models
{
    public class Product
    {
       
        public Product(Guid id)
        {
            Id = id;
        }

        public Product() : this(Guid.NewGuid())
        {

        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }
    }
}