using OmnicellAPI.DTO;
using OmnicellAPI.Interfaces;
using System.Data;
using System.Security.Cryptography;

namespace OmnicellAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductDAO _productDAO;
        private readonly ILogger<ProductService> _logger;
        public ProductService(IProductDAO productDAO, ILogger<ProductService> logger) 
        { 
            _productDAO = productDAO;
            _logger = logger;
        }

        public async Task<List<ProductDetail>> GetProductsByName(string name)
        {
            DataSet dsProducts = _productDAO.GetProductsByName(name);
            var objProducts = new List<ProductDetail>();
            objProducts = (from DataRow dr in dsProducts.Tables[0].Rows
                           select new ProductDetail()
                           {
                               Id = Convert.ToInt32(dr["Id"]),
                               Name = dr["Name"].ToString(),
                               Description = dr["Description"].ToString(),
                               Category = dr["Category"].ToString(),
                               Price = Convert.ToDecimal(dr["Price"])
                           }).ToList();
            return objProducts;
        }

        public async Task<List<ProductDetail>> GetProducts()
        {
            DataSet dsProducts =  _productDAO.GetProducts();
            var objProducts = new List<ProductDetail>();
            objProducts = (from DataRow dr in dsProducts.Tables[0].Rows
                           select new ProductDetail()
                           {
                               Id = Convert.ToInt32(dr["Id"]),
                               Name = dr["Name"].ToString(),
                               Description = dr["Description"].ToString(),
                               Category = dr["Category"].ToString(),
                               Price = Convert.ToDecimal(dr["Price"])
                           }).ToList();
            return objProducts;
        }

        public async Task<int> AddProduct(string name, string description, string category, decimal price)
        {
            int id = _productDAO.AddProduct(name, description, category, price);
            return id;
        }

        public void UpdateProduct(int id, string name, string description, string category, decimal price)
        {
            _productDAO.UpdateProduct(id, name, description, category, price);
        }
        public bool DeleteProduct(int id)
        {
            return _productDAO.DeleteProduct(id);
        }
        public void DeleteAllProducts()
        {
            _productDAO.DeleteAllProducts();
        }
    }
}
