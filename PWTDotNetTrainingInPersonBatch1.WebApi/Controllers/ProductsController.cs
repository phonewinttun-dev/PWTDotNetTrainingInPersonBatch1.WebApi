using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace PWTDotNetTrainingInPersonBatch1.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly SqlConnectionStringBuilder _stringBuilder = new SqlConnectionStringBuilder()
        {
            DataSource = ".",
            InitialCatalog = "InPersonBatch1MiniPOS",
            UserID = "sa",
            Password = "sasa@123",
            TrustServerCertificate = true
        };


        public class ProductDto()
        {
            public string ProductID { get; set; }

            public string ProductCode { get; set; }

            public string ProductName { get; set; }

            public decimal Price { get; set; }

            public int Quantity { get; set; }

            public bool DeleteFlag { get; set; }
        }

        public class ProductResponseDto
        {
            public bool IsSuccess { get; set; }

            public String Message { get; set; }
        }

        [HttpGet]
        public IActionResult GetProducts()
        {

            using (IDbConnection db = new SqlConnection(_stringBuilder.ConnectionString))
            {
                db.Open();

                string query = @"SELECT [ProductID]
                                      ,[ProductCode]
                                      ,[ProductName]
                                      ,[Price]
                                      ,[Quantity]
                                      ,[DeleteFlag]
                                  FROM [dbo].[tbl_Product]";

                var lst = db.Query<ProductDto>(query).ToList();


                return Ok(lst);

            }
        }



        [HttpPost]
        public IActionResult CreateProduct(ProductDto request)
        {
            using (IDbConnection db = new SqlConnection(_stringBuilder.ConnectionString))
            {
                db.Open();

                request.ProductID = Guid.NewGuid().ToString();

                string query = @"INSERT INTO [dbo].[Tbl_Product]
                                       ([ProductId]
                                       ,[ProductCode]
                                       ,[ProductName]
                                       ,[Price]
                                       ,[Quantity]
                                       ,[DeleteFlag])
                                 VALUES
                                       (@ProductId
                                       ,@ProductCode
                                       ,@ProductName
                                       ,@Price
                                       ,@Quantity
                                       ,@DeleteFlag)";
                 
                var result = db.Execute(query, request);
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
}
