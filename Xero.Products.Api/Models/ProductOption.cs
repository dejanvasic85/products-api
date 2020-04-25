using System;

namespace Xero.Products.Api.Models
{
    public class ProductOption
    {
        public ProductOption(Guid id)
        {
            Id = id;
        }

        public ProductOption() : this(Guid.NewGuid())
        { }

        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}