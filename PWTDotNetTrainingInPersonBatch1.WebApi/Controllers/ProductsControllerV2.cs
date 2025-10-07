using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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

            var result = db.TblProducts
                .Where(product => product.DeleteFlag == false)
                .ToList();
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
            var product = db.TblProducts
                .Where(x => x.DeleteFlag == false)
                .FirstOrDefault(Product => Product.ProductId == id);
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


        [HttpPatch("{id}")]
        [HttpPatch("id/{id}")]
        public IActionResult UpdateProduct(string id, [FromBody] ProductDto request)
        {
            var item = db.TblProducts.FirstOrDefault(product => product.ProductId == id);


            if (item is null)
            {
                return NotFound(new ProductResponseDto
                {
                    IsSuccess = false,
                    Message = "Product Not Found!"
                });
            }

            if (!string.IsNullOrEmpty(request.ProductCode))
            {
                item.ProductCode = request.ProductCode;
            }
            if (!string.IsNullOrEmpty(request.ProductName))
            {
                item.ProductName = request.ProductName;
            }
            if (request.Quantity > 0)
            {
                item.Quantity = request.Quantity;
            }
            if (request.Price > 0)
            {
                item.Price = request.Price;
            }

            return Ok(new ProductResponseDto
            {
                IsSuccess = db.SaveChanges() > 0,
                Message = "Product updated successfully."
            });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(string id) 
        {
            var item = db.TblProducts.FirstOrDefault(product => product.ProductId == id);
            
            if (item is null)
            {
                return NotFound(new ProductResponseDto
                {
                    IsSuccess = false,
                    Message = "Product Not Found!"
                });
            }

            item.DeleteFlag = true;
            var result = db.SaveChanges();
            string message = result > 0 ? "Delete Successful." : "Delete Failed.";
            return Ok(new ProductResponseDto
            {
                IsSuccess = result > 0,
                Message = message
            });
            
        }


    }
}
