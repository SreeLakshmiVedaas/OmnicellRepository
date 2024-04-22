using OmnicellAPI.DTO;

namespace OmnicellAPI.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDetail>> GetProducts();
        Task<List<ProductDetail>> GetProductsByName(string name);
        Task<int> AddProduct(string name, string description, string category, decimal price);
        void UpdateProduct(int Id, string name, string description, string category, decimal price);
        bool DeleteProduct(int id);
        void DeleteAllProducts();
    }
}
