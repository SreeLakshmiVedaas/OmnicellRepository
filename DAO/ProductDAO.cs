using OmnicellAPI.DTO;
using OmnicellAPI.Interfaces;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace OmnicellAPI.DAO
{
    public class ProductDAO : IProductDAO
    {
        private readonly ILogger<ProductDAO> _logger;
        private IConfiguration _config { get; }
        private readonly string _connString; // connection string
        public ProductDAO(ILogger<ProductDAO> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _connString = _config.GetValue<string>("ConnectionStrings:DbConnection");
        }

        public DataSet GetProductByCategory(string category)
        {
            throw new NotImplementedException();
        }

        public DataSet GetProductById(int id)
        {
            throw new NotImplementedException();
        }

        public DataSet GetProductsByName(string name)
        {
            using (SqlConnection oConn = new SqlConnection(_connString))
            {
                oConn.Open();
                using (SqlCommand command = new SqlCommand("Select Id, Name, Description, Category, Price from dbo.Product  where Name like @name"))
                {

                    command.CommandType = CommandType.Text;
                    command.Connection = oConn;
                    // Add new SqlParameter to the command.
                    command.Parameters.Add(new SqlParameter("@name", "%" + name + "%"));

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        _logger.LogInformation("GetData : Process Completed");
                        return ds;
                    }
                }
            }
        }

        public DataSet GetProducts()
        {
            using (SqlConnection oConn = new SqlConnection(_connString))
            {
                oConn.Open();
                using (SqlCommand command = new SqlCommand("Select Id, Name, Description, Category, Price from dbo.Product"))
                {

                    command.CommandType = CommandType.Text;
                    command.Connection = oConn;

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                        _logger.LogInformation("GetData : Process Completed");
                        return ds;
                    }
                }
            }
        }

        public int AddProduct(string name, string description, string category, decimal price)
        {
            using (SqlConnection con = new SqlConnection(_connString))
            {
                int newID;
                var cmdText = "INSERT INTO dbo.Product(Name, Description, Category, Price) VALUES(@name,@description, @category, @price);SELECT CAST(scope_identity() AS int)";

                using (SqlCommand cmd = new SqlCommand(cmdText, con))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@description", description);
                    cmd.Parameters.AddWithValue("@category", category);
                    cmd.Parameters.AddWithValue("@price", price);

                    con.Open();
                    newID = (int)cmd.ExecuteScalar();

                    if (con.State == System.Data.ConnectionState.Open) con.Close();
                    return newID;
                }
            }
        }

        public void UpdateProduct(int id, string name, string description, string category, decimal price)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                var cmdText = "UPDATE dbo.Product SET Name=@name, Description = @description, Category = @category, Price=@price WHERE ID=@id";
                using (SqlCommand cmd = new SqlCommand(cmdText, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@description", description);
                    cmd.Parameters.AddWithValue("@category", category);
                    cmd.Parameters.AddWithValue("@price", price);
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    cmd.ExecuteNonQuery();
                }

            }
        }

        public bool DeleteProduct(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                var cmdText = "DELETE from dbo.Product WHERE ID=@id";
                using (SqlCommand cmd = new SqlCommand(cmdText, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    int deletedRows = cmd.ExecuteNonQuery();
                    if (deletedRows == 0)
                        return false;
                    else return true;
                }
            }
        }

        public void DeleteAllProducts()
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                var cmdText = "DELETE FROM dbo.Product";
                using (SqlCommand cmd = new SqlCommand(cmdText, conn))
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
