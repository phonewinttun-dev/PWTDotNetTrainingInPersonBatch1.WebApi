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

        [HttpPost]
        public IActionResult createProduct()
        {
            return Ok();
        }
    }
}
