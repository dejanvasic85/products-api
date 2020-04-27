using System;

namespace Xero.Products.Api.Resources
{
    public class ProductOptionResource
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
