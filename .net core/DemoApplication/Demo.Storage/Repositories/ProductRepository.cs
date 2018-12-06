using Demo.Contract.Interfaces.Storage.Repositories;
using Demo.Contract.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Storage.EF.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public List<Product> GetAllProduct()
        {
            return new List<Product> {
            new Product() { productId =1, productCode = "GDN-0011", productName = "Leaf Rake", releaseDate = DateTime.Now.AddYears(-1) , price = 19.85m, starRating = 3.1, imageUrl = "./assets/leafrake.jfif", description ="Leaf rake with 48-inch wooden handle."},
            new Product() { productId =2, productCode = "GDN-0023", productName = "Garden Cart", releaseDate = DateTime.Now.AddYears(-3) , price = 32.99m, starRating = 4.8, imageUrl = "./assets/gardencart.jfif", description = "15 gallon capacity rolling garden cart"},
            new Product() { productId =3, productCode = "TBX-0048", productName = "Hammer", releaseDate = DateTime.Now.AddDays(50) , price = 50.6m, starRating = 2.6, imageUrl = "./assets/hammer.jfif", description ="Curved claw steel hammer" },
            new Product() { productId =4, productCode = "TBX-0022", productName = "Saw", releaseDate = DateTime.Now.AddDays(60).AddYears(-2) , price = 110, starRating = 5.0, imageUrl = "./assets/saw.jfif", description = "15-inch steel blade hand saw" },
            new Product() { productId =5, productCode = "GMG-0042", productName = "Video Game Controller", releaseDate = DateTime.Now.AddMonths(-6) , price = 68.78m, starRating = 1.5, imageUrl = "./assets/gamecontroller.jfif", description ="Standard two-button video game controller"}};
        }

        public Task<IEnumerable<Product>> GetAllProductAsync()
        {
            throw new NotImplementedException();
        }

        public Product GetProductByCode(string Code)
        {
            return new Product()
            {
                productId = 1,
                productCode = "GDN-0011",
                productName = "Leaf Rake",
                releaseDate = DateTime.Now.AddYears(-1),
                price = 19.85m,
                starRating = 3.1,
                imageUrl = "./assets/leafrake.jfif",
                description = "Leaf rake with 48-inch wooden handle."
            };
        }

        public Task<Product> GetProductByCodeAsync(string Code)
        {
            throw new NotImplementedException();
        }

        public Product GetProductById(int Id)
        {
            return new Product()
            {
                productId = 1,
                productCode = "GDN-0011",
                productName = "Leaf Rake",
                releaseDate = DateTime.Now.AddYears(-1),
                price = 19.85m,
                starRating = 3.1,
                imageUrl = "./assets/leafrake.jfif",
                description = "Leaf rake with 48-inch wooden handle."
            };
        }

        public Task<Product> GetProductByIdAsync(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
