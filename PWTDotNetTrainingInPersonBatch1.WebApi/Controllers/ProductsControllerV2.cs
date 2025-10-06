using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;



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
            
            List<ProductDto> products = new List<string>()
            {
                "Product1",
                "Product2",
                "Product3"
            };

            return Ok();
        }

    }
}
