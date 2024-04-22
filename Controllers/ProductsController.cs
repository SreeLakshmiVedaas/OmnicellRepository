using Microsoft.AspNetCore.Mvc;
using OmnicellAPI.DTO;
using OmnicellAPI.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace OmnicellAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private IConfiguration _config { get; }
        private readonly ILogger<ProductsController> _logger;
        public ProductsController(IProductService productService, IConfiguration config, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _config = config;
            _logger = logger;
        }

        /// <summary>Returns all products</summary>
        [HttpGet]
        [ProducesResponseType(typeof(ProductDetail), StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetProducts();
            if (products == null)
                return NotFound();
            return Ok(products);
        }

        /// <summary>Returns product by Id</summary>
        [HttpGet]
        [Route("{Id}")]
        [ProducesResponseType(typeof(ProductDetail), StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<IActionResult> GetProductById(int Id)
        {
            var products = await _productService.GetProducts();
            var detail = products.Where(p=>p.Id == Id);
            if (detail.Count() == 0)
                return NotFound();
            return Ok(detail);
        }

        /// <summary>Returns total count of the products</summary>
        [HttpGet]
        [Route("total-count")]
        [ProducesResponseType(typeof(ProductDetail), StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<IActionResult> GetProductTotalCount()
        {
            var products = await _productService.GetProducts();
            if (products.Count == 0)
                return NotFound();
            return Ok($"Total Count of the Products: {products.Count}");
        }

        /// <summary>Returns products by categoryName</summary>
        [HttpGet]
        [Route("category/{category}")]
        [ProducesResponseType(typeof(ProductDetail), StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<IActionResult> GetProductByCategory(string category)
        {
            var products = await _productService.GetProducts();
            var detail = products.Where(p => p.Category?.ToLower() == category.ToLower());
            if (detail.Count() == 0)
                return NotFound();
            return Ok(detail);
        }

        /// <summary>Returns products by Name</summary>
        [HttpGet]
        [Route("search")]
        [ProducesResponseType(typeof(ProductDetail), StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<IActionResult> GetProductSearch(string name)
        {
            var products = await _productService.GetProductsByName(name);
            if (products.Count == 0)
                return NotFound();
            return Ok(products);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProductDetail), StatusCodes.Status201Created)]
        public async Task<IActionResult> AddProduct(string name, string description, string category, decimal price)
        {
            int insertedId = await _productService.AddProduct(name, description, category, price);
            return Created("", $"Inserted Product Id is {insertedId}");
        }

        [HttpPut]
        [Route("{Id}")]
        [ProducesResponseType(typeof(ProductDetail), (int)HttpStatusCode.NoContent)]
        public IActionResult UpdateProduct([FromForm][Required] int id, [FromForm]string name, [FromForm] string description, [FromForm] string category, [FromForm] decimal price)
        {
            _productService.UpdateProduct(id, name, description, category, price);
            return  NoContent();
        }

        [HttpDelete]
        [Route("{Id}")]
        [ProducesResponseType(typeof(ProductDetail), (int)HttpStatusCode.NoContent)]
        public IActionResult DeleteProduct([FromForm][Required] int id)
        {
            bool isProductDeleted = _productService.DeleteProduct(id);
            if (isProductDeleted)
                return NoContent();
            else
                return NotFound();
        }

        [HttpDelete]
        [ProducesResponseType(typeof(ProductDetail), (int)HttpStatusCode.NoContent)]
        public IActionResult DeleteAllProducts()
        {
            _productService.DeleteAllProducts();
            return NoContent();
        }

        [Route("sort")]
        /// <summary>Returns all products in Ascending or Descending Order</summary>
        [HttpPost]
        [ProducesResponseType(typeof(ProductDetail), StatusCodes.Status200OK)]
        [Produces("application/json")]
        public async Task<IActionResult> GetSortedProducts([FromForm][Required] string sortparameter, [FromForm][Required] string sortorder)
        {
            var products = await _productService.GetProducts();
            switch (sortparameter.ToLower())
            {
                case "name":
                    if(sortorder.ToLower() == "ascending")
                    products = products.OrderBy(x => x.Name).ToList();
                    else
                        products = products.OrderByDescending(x => x.Name).ToList();
                    break;
                case "category":
                    if (sortorder.ToLower() == "ascending")
                        products = products.OrderBy(x => x.Category).ToList();
                    else
                        products = products.OrderByDescending(x => x.Category).ToList();
                    break;
                case "price":
                    if (sortorder.ToLower() == "ascending")
                        products = products.OrderBy(x => x.Price).ToList();
                    else
                        products = products.OrderByDescending(x => x.Price).ToList();
                    break;
            }
            if (products == null)
                return NotFound();
            return Ok(products);
        }
    }
}
