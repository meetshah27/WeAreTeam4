using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

using ContosoCrafts.WebSite.Models;

using ContosoCrafts.WebSite.Services;

namespace ContosoCrafts.WebSite.Controllers
{

    /// <summary>
    /// API Controller to manage product data and operations.
    /// Provides endpoints to retrieve all products and add ratings to a product.
    /// </summary>

    [ApiController]
    [Route("[controller]")]

    public class ProductsController : ControllerBase
    {
        /// <summary>
        /// Constructor to inject the JsonFileProductService.
        /// </summary>
        /// <param name="productService">Service for handling product data operations.</param>

        public ProductsController(JsonFileProductService productService)
        {
            ProductService = productService;
        }

        /// <summary>
        /// The product service used to interact with product data.
        /// </summary>

        public JsonFileProductService ProductService { get; }

        /// <summary>
        /// HTTP GET endpoint to retrieve all products.
        /// </summary>
        /// <returns>A collection of all product models.</returns>
        [HttpGet]
        public IEnumerable<ProductModel> Get()
        {
            return ProductService.GetAllData();
        }
        /// <summary>
        /// HTTP PATCH endpoint to add a rating to a specific product.
        /// </summary>
        /// <param name="request">The rating request containing the product ID and rating value.</param>
        /// <returns>An HTTP status indicating the success of the operation.</returns>
        [HttpPatch]
        public ActionResult Patch([FromBody] RatingRequest request)
        {
            // Add the rating to the specified product
            ProductService.AddRating(request.ProductId, request.Rating);
            return Ok();
        }
        /// <summary>
        /// Represents a request to add a rating to a product.
        /// </summary>
        public class RatingRequest
        {
            /// <summary>
            /// The ID of the product to rate.
            /// </summary>
            public string ProductId { get; set; }

            /// <summary>
            /// The rating value to assign to the product.
            /// </summary>
            public int Rating { get; set; }
        }
    }
}