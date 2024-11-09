using System.Collections.Generic;     // For using IEnumerable to hold a collection of products.

using Microsoft.AspNetCore.Mvc.RazorPages;    // For Razor Pages functionality.
using Microsoft.Extensions.Logging;           // For logging functionality.

using ContosoCrafts.WebSite.Models;           // For accessing the ProductModel class.
using ContosoCrafts.WebSite.Services;         // For accessing the JsonFileProductService class.

namespace ContosoCrafts.WebSite.Pages
{
    public class IndexModel : PageModel
    {
        // Private readonly logger instance for logging events and errors.
        private readonly ILogger<IndexModel> _logger;

        // Constructor to initialize dependencies.
        public IndexModel(ILogger<IndexModel> logger,
            JsonFileProductService productService)
        {
            // Assigns injected logger to the local logger instance.
            _logger = logger;

            // Assigns injected productService to the ProductService property.
            ProductService = productService;
        }

        // Public property to access the product service, allowing retrieval of product data.
        public JsonFileProductService ProductService { get; }

        // Public property to store the list of products, which will be populated during a GET request.
        public IEnumerable<ProductModel> Products { get; private set; }

        // Called on a GET request to the page.
        public void OnGet()
        {
            // Retrieves all product data using the ProductService and assigns it to the Products property.
            Products = ProductService.GetAllData();
        }
    }
}