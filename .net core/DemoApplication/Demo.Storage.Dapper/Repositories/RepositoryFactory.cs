
namespace Demo.Storage.Dapper.Repositories
{
    using Demo.Contract.Interfaces.Storage.Repositories;

    public class RepositoryFactory : IRepositoryFactory
    {
        public IProductRepository ProductRepository { get; set; }
        public IAddressRepository AddressRepository { get; set; }
        public IUserRepository UserRepository { get; set; }
    }
}
