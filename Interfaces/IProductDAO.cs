using OmnicellAPI.DTO;
using System.Data;

namespace OmnicellAPI.Interfaces
{
    public interface IProductDAO
    {
        DataSet GetProducts();
        DataSet GetProductsByName(string name);
        int AddProduct(string name, string description, string category, decimal price);
        void UpdateProduct(int id, string name, string description, string category, decimal price);
        bool DeleteProduct(int id);
        void DeleteAllProducts();
    }
}
