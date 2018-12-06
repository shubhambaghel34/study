namespace Demo.Contract.Interfaces.Storage.Repositories
{
    public interface IRepositoryFactory
    {
        IProductRepository ProductRepository { get; set; }
        IAddressRepository AddressRepository { get; set; }
        IUserRepository UserRepository { get; set; }
    }
}
