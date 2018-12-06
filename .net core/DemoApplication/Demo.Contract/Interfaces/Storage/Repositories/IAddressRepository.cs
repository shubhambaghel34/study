namespace Demo.Contract.Interfaces.Storage.Repositories
{
    using Demo.Contract.Models;
    using System.Threading.Tasks;

    public interface IAddressRepository
    {
        Task<int> AddAddressAsync(Address Address);
        Task<Address> GetAddressByTypeAndIdAsync(int EntityType, int EntityId);
    }
}
