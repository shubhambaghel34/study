namespace Demo.Contract.Interfaces.Storage.Repositories
{
    using Demo.Contract.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetALLUsersAsync();
        Task<int> AddUserAsync(User User);
        Task<User> GetUserByCodeAsync(string Code);
    }
}
