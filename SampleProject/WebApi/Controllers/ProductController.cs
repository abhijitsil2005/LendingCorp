using BusinessEntities;
using Core.Services.Products;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models.Products;

namespace WebApi.Controllers
{
    [RoutePrefix("products")]
    public class ProductController : BaseApiController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [Route("create")]
        [HttpPost]
        public HttpResponseMessage CreateProduct([FromBody] ProductModel proModel)
        {
            Product product = MapProductModel(proModel);
            // Check if Product already exists
            var newProduct = _productService.Add(product);
            if (newProduct == null)
            {
                // if Product exists, return 409 Conflict
                return AlreadyExists("Product exists with ID: " + newProduct.Id);
            }
            return Found(new ProductData(newProduct));
        }

        [Route("{productId:int}/update")]
        [HttpPost]
        public HttpResponseMessage UpdateProduct(int productId, [FromBody] ProductModel proModel)
        {
            Product product = MapProductModel(proModel);
            product.SetId(productId);
            var updatedProduct = _productService.Update(product);
            if (updatedProduct == null)
            {
                string message = "Product not found.";
                return DoesNotExist(message);
            }
            
            return Found(new ProductData(updatedProduct));
        }

        [Route("{productId:int}/delete")]
        [HttpDelete]
        public HttpResponseMessage DeleteProduct(int productId)
        {
            bool product = _productService.Delete(productId);
            if (!product)
            {
                string message = "Product not found.";
                return DoesNotExist(message);
            }
           
            // return 200 OK with success message
            string successMessage = $"Product with ID: {productId} deleted successfully.";
            return TaskSuccess(successMessage);
        }

        [Route("{productId:int}")]
        [HttpGet]
        public HttpResponseMessage GetProduct(int productId)
        {
            var product = _productService.GetProductById(productId);
            if (product == null)
            {
                string message = "Product not found.";
                return DoesNotExist(message);
            }
            return Found(new ProductData(product));
        }

        [Route("list")]
        [HttpGet]
        public HttpResponseMessage GetProducts(int skip, int take, ProductTypes? type = null, string name = null, decimal minPrice = 0, decimal maxPrice = 0)
        {
            if(type == null && name == null && minPrice == 0 && maxPrice == 0)
            {
                string message = "Pass valid parameter value to get results";
                return TaskSuccess(message);
            }
            var products = _productService.GetProducts(type, name, minPrice, maxPrice)
                                       .Skip(skip).Take(take)
                                       .Select(q => new ProductData(q))
                                       .ToList();
            return Found(products);
        }

        public Product MapProductModel(ProductModel proModel)
        {
            Product product = new Product();
            product.SetName(proModel.Name);
            product.SetDescription(proModel.Description);
            product.SetProductType(proModel.Type);
            product.SetPrice(proModel.Price);
            product.SetStockQuantity(proModel.StockQuantity);
            return product;
        }

    }
}