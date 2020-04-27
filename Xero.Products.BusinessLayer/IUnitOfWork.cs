using System;

namespace Xero.Products.BusinessLayer
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository ProductRepository { get; }
        IProductOptionRepository ProductOptionRepository { get; }


        void Commit();
    }
} 
