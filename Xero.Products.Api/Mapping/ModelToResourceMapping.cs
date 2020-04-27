using AutoMapper;
using Xero.Products.Api.Resources;
using Xero.Products.BusinessLayer;

namespace Xero.Products.Api.Mapping
{
    public class ModelToResourceMapping : Profile
    {
        public ModelToResourceMapping()
        {
            CreateMap<Product, ProductResource>();

            CreateMap<CreateUpdateProductResource, Product>();

            CreateMap<ProductResource, Product>();

            CreateMap<ProductOption, ProductOptionResource>();

            CreateMap<CreateUpdateProductOptionResource, ProductOption>();

            CreateMap<ProductOptionResource, ProductOption>();
        }
    }
}
