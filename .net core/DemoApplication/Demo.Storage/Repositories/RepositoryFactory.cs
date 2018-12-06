
namespace Demo.Storage.Dapper.Repositories
{
    using Demo.Contract.Interfaces.Storage.Repositories;

    public class RepositoryFactory : IRepositoryFactory
    {
        private IProductRepository _productRepository;

        public IProductRepository ProductRepository
        {
            get { return _productRepository; }
            set { _productRepository = value; }
        }
    }
}
