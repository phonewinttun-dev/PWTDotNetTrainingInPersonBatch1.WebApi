using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PWTDotNetTrainingInPersonBatch1.WebApi.Database.AppDbContextModels;
using static PWTDotNetTrainingInPersonBatch1.WebApi.Controllers.ProductsController;

namespace PWTDotNetTrainingInPersonBatch1.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsControllerV2 : ControllerBase
    {
        private readonly AppDbContext db;
        public ProductsControllerV2()
        {
            db = new AppDbContext();
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            //var item = db.TblProducts.FirstOrDefault(Product => Product.ProductId == id);
            //if (item is null)
            //{
            //    return NotFound();
            //}
            //return Ok(item);

            //var result = db.TblProducts.ToList();
            //var lst = result.Select(product => new ProductDto
            //{

            //}).ToList();

            //var result = db.TblProducts.ToList();


            //List<ProductDto> lst = new List<ProductDto>();
            //foreach (TblProduct product in result)
            //{
            //    lst.Add(new ProductDto
            //    {
            //        ProductID = product.ProductId,
            //        ProductCode = product.ProductCode,
            //        ProductName = product.ProductName,
            //        Price = product.Price,
            //        Quantity = product.Quantity,
            //        DeleteFlag = product.DeleteFlag
            //    });
            //}

            var result = db.TblProducts.ToList();
            var lst = result.Select(product => new ProductDto
            {
                ProductID = product.ProductId,
                ProductCode = product.ProductCode,
                ProductName = product.ProductName,
                Price = product.Price,
                Quantity = product.Quantity,
                DeleteFlag = product.DeleteFlag

            }).ToList();


            return Ok(lst);
        }

        [HttpGet("{id}")]
        public IActionResult GetProductById(string id)
        {
            var product = db.TblProducts.FirstOrDefault(Product => Product.ProductId == id);
            if (product is null)
            {
                return NotFound(new ProductResponseDto()
                {
                    IsSuccess = false,
                    Message = "Product not found."
                });
            }

            return Ok(new ProductResponseDto
            {
                IsSuccess = true,
                Message = "Product found.",
                Data = new ProductDto
                {
                    ProductID = product.ProductId,
                    ProductCode = product.ProductCode,
                    ProductName = product.ProductName,
                    Price = product.Price,
                    Quantity = product.Quantity,
                    DeleteFlag = product.DeleteFlag
                }
            });
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody] ProductDto request)
        {
            db.TblProducts.Add(new TblProduct
            {
                ProductId = Guid.NewGuid().ToString(),
                ProductCode = request.ProductCode,
                ProductName = request.ProductName,
                Price = request.Price,
                Quantity = request.Quantity,
                DeleteFlag = request.DeleteFlag
            });

            var result = db.SaveChanges();

            string message = result > 0 ? "Product created successfully." : "Failed to create product.";

            ProductResponseDto response = new ProductResponseDto()
            {
                IsSuccess = result > 0,
                Message = message
            };

            return Ok(response);

        }
    }
}
