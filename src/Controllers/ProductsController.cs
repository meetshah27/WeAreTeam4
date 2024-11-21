using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;

namespace ContosoCrafts.WebSite.Controllers
{
    // ApiController attribute simplifies model validation and automatically sets up HTTP responses
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        // Constructor to inject the product service dependency.
        public ProductsController(JsonFileProductService productService)
        {
            ProductService = productService;
        }

        // Read-only property to access the product service.
        public JsonFileProductService ProductService { get; }

        // GET: /products - Retrieves all products from the system
        [HttpGet]
        public IEnumerable<ProductModel> GetAllProducts()
        {
            return ProductService.GetAllData();
        }

        // PATCH: /products - Updates a product's rating
        [HttpPatch]
        public ActionResult UpdateProductRating([FromBody] RatingRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request body cannot be null.");
            }

            if (request.Rating < 1 || request.Rating > 5)
            {
                return BadRequest("Rating must be between 1 and 5.");
            }

            ProductService.AddRating(request.ProductId, request.Rating);

            // Return a success response indicating the rating update was successful
            return Ok();
        }

        // Model to hold the product rating request data
        public class RatingRequest
        {
            // The product ID to update the rating for
            public string ProductId { get; set; }

            // The rating to be added for the product
            public int Rating { get; set; }
        }
    }
}

