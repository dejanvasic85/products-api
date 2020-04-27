using Xero.Products.BusinessLayer;

namespace Xero.Products.Repository
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IConnectionFactory _connectionFactory;

        public UnitOfWorkFactory(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public IUnitOfWork CreateUnitOfWork()
        {
            return new UnitOfWork(_connectionFactory);
        }
    }
}
