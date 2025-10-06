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
            public string? ProductID { get; set; }

            public string? ProductCode { get; set; }

            public string? ProductName { get; set; }

            public decimal Price { get; set; }

            public int Quantity { get; set; }

            public bool DeleteFlag { get; set; }
        }

        public class ProductResponseDto
        {
            public bool IsSuccess { get; set; }

            public String Message { get; set; }

            public ProductDto? Data { get; set; }
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


        [HttpGet("id/{id}")]
        [HttpGet("{id}")]
        public IActionResult GetProduct(string id)
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
                                  FROM [dbo].[tbl_Product] Where ProductId = @ProductId";

                var item = db.Query<ProductDto>(query, new { ProductId = id }).FirstOrDefault();

                if (item is null)
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
                    Data = item
                });
            }
        }


        [HttpPost]
        public IActionResult CreateProduct([FromBody] ProductDto request)
        {
            using (IDbConnection db = new SqlConnection(_stringBuilder.ConnectionString))
            {
                db.Open();

                request.ProductID = Guid.NewGuid().ToString();

                string createQuery = @"INSERT INTO [dbo].[tbl_Product]
                                       ([ProductID]
                                       ,[ProductCode]
                                       ,[ProductName]
                                       ,[Price]
                                       ,[Quantity]
                                       ,[DeleteFlag])
                                 VALUES
                                       (@ProductID
                                       ,@ProductCode
                                       ,@ProductName
                                       ,@Price
                                       ,@Quantity
                                       ,@DeleteFlag)";
                 
                var result = db.Execute(createQuery, request);
                string message = result > 0 ? "Product created successfully." : "Failed to create product.";
                
                ProductResponseDto response = new ProductResponseDto()
                {
                    IsSuccess = result > 0,
                    Message = message
                };

                return Ok(response);
            }
        }

        [HttpPatch("{id}")]
        [HttpPatch("id/{id}")]
        public IActionResult UpdateProduct(string id, [FromBody] ProductDto request)
        {
            using (IDbConnection db = new SqlConnection(_stringBuilder.ConnectionString))
            {
                db.Open();


                //[ProductID]
                //,[ProductCode]
                //,[ProductName]
                //,[Price]
                //,[Quantity]

                //[ProductCode] = @ProductCode
                //  ,[ProductName] = @ProductName
                //  ,[Price] = @Price
                //  ,[Quantity] = @Quantity[ProductCode] = @ProductCode
                //  ,[ProductName] = @ProductName
                //  ,[Price] = @Price
                //  ,[Quantity] = @Quantity


                string conditions = "";
                if (!string.IsNullOrEmpty(request.ProductCode))
                {
                    conditions += "[ProductCode] = @ProductCode, ";
                }
                if (!string.IsNullOrEmpty(request.ProductName))
                {
                    conditions += "[ProductName] = @ProductName, ";
                }
                if (request.Price > 0)
                {
                    conditions += "[Price] = @Price, ";
                }
                if (request.Quantity > 0)
                {
                    conditions += "[Quantity] = @Quantity, ";
                }

                if (conditions.Length == 0)
                {
                    return BadRequest(new ProductResponseDto
                    {
                        IsSuccess = false,
                        Message = "Invalid. Please fill in data to update."
                    });
                }
                
                conditions = conditions.Substring(0, conditions.Length - 2);

                
                string updateQuery = $@"UPDATE [dbo].[tbl_Product]
                                        SET {conditions}
                                        WHERE [ProductID] = @ProductID";

                request.ProductID = id;

                var result = db.Execute(updateQuery, request);
                string message = result > 0 ? "Update Successful." : "Update Failed.";
                return Ok(new ProductResponseDto
                {
                    IsSuccess = true,
                    Message = "Success."
                });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(string id)
        {
            using (IDbConnection db = new SqlConnection(_stringBuilder.ConnectionString))
            {
                db.Open();

                string deleteQuery = @"UPDATE [dbo].[tbl_Product]
                                       SET [DeleteFlag] = 1
                                       WHERE [ProductID] = @ProductID";

                var result = db.Execute(deleteQuery, new { ProductID = id });

                string message = result > 0 ? "Delete Successful." : "Delete Failed.";
                return Ok(new ProductResponseDto
                {
                    IsSuccess = result > 0,
                    Message = message
                });
            }
        }


    }
}
