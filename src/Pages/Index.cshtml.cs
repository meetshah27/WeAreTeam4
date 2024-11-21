// Importing collections namespace to work with IEnumerable and manage product collections
using System.Collections.Generic;
// Importing Razor Pages components for building page models and handling requests
using Microsoft.AspNetCore.Mvc.RazorPages;
// Importing logging functionality to log events and errors
using Microsoft.Extensions.Logging;
// Importing models to define the structure of product data
using ContosoCrafts.WebSite.Models;
// Importing services to handle data operations for products
using ContosoCrafts.WebSite.Services;      

namespace ContosoCrafts.WebSite.Pages
{
    /// <summary>
    /// Index Page Model for displaying a list of products.
    /// Handles the logic for loading and displaying product data.
    /// </summary>
    public class IndexModel : PageModel
    {
        // Private readonly logger instance for logging messages and errors within this class
        private readonly ILogger<IndexModel> _logger;

        /// <summary>
        /// Constructor to initialize the IndexModel with injected dependencies.
        /// </summary>
        /// <param name="logger">Logger instance for logging events and errors.</param>
        /// <param name="productService">Service for managing product data operations.</param>
        public IndexModel(ILogger<IndexModel> logger,
            JsonFileProductService productService)
        {
            // Assigns the injected logger instance to the local _logger field
            _logger = logger;

            // Assigns the injected product service to the ProductService property
            ProductService = productService;
        }

        /// Public property to access the product service, which provides methods to retrieve and manage product data
        public JsonFileProductService ProductService { get; }

        // Public property to hold the collection of products, populated when a GET request is made
        public IEnumerable<ProductModel> Products { get; private set; }

        /// <summary>
        /// Handles the HTTP GET request to load product data for display.
        /// Retrieves all products using the ProductService and assigns them to the Products property.
        /// </summary>
        public void OnGet()
        {
            // Fetches all products and assigns them to the Products property for display on the page
            Products = ProductService.GetAllData();
        }
    }
}