namespace Demo.Contract.Interfaces.Storage.Repositories
{
    using Demo.Contract.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByCodeAsync(string Code);
        Task<Product> GetProductByIdAsync(int Id);
        Task<int> AddProductAsync(Product Product);
        Task<int> DeleteProductByIdAsync(int Id);
        Task<int> DeleteProductByCodeAsync(string Code);
        Task<int> UpdateProductByCodeAsync(Product Product);
        Task<int> UpdateProductByIdAsync(Product Product);
    }
}
