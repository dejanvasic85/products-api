using System;
using System.Data;
using System.Threading.Tasks;
using Xero.Products.BusinessLayer;

namespace Xero.Products.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly IDbConnection _dbConnection;
        private readonly IDbTransaction _dbTransaction;
        private IProductRepository _productRepository;


        public UnitOfWork(IConnectionFactory connectionFactory)
        {
            _dbConnection = connectionFactory.CreateConnection();
            _dbConnection.Open();
            _dbTransaction = _dbConnection.BeginTransaction();
        }

        public IProductRepository ProductRepository => _productRepository ?? (_productRepository = new ProductRepository(_dbTransaction.Connection));

        public void Commit()
        {
            try
            {
                _dbTransaction.Commit();
            }
            catch (Exception ex)
            {
                // Logging should be done centrally in middleware
                throw ex;
            }
            finally
            {
                _dbTransaction.Rollback();
                _dbTransaction.Dispose();
                _dbConnection.Dispose();
            }
        }

        public void Dispose()
        {
            _dbConnection.Close();
            _dbTransaction.Dispose();
            _dbConnection.Dispose();
        }
    }
}
