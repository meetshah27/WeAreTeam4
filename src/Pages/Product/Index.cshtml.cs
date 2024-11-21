// Importing collections for managing lists
using System.Collections.Generic;  
// Importing Razor Pages components
using Microsoft.AspNetCore.Mvc.RazorPages;   
// Importing models for product representation
using ContosoCrafts.WebSite.Models;
// Importing services for data handling
using ContosoCrafts.WebSite.Services;    


namespace ContosoCrafts.WebSite.Pages.Product
{
    /// <summary>
    /// Index Page Model for displaying a list of products.
    /// This model handles requests to display all product data to the user
    /// on the main Index page.
    /// </summary>
    public class IndexModel : PageModel
    {
        /// <summary>
        /// Constructor to initialize the IndexModel with the specified product service.
        /// </summary>
        /// <param name="productService">The service for managing product data.</param>
        public IndexModel(JsonFileProductService productService)
        {
            // Assigns the provided product service to the ProductService property
            ProductService = productService;  
        }

        // Property to hold the product service for interacting with product data
        public JsonFileProductService ProductService { get; }

        // Property to store the collection of products retrieved from the data service
        public IEnumerable<ProductModel> Products { get; private set; }

        /// <summary>
        /// Handles the HTTP GET request for the Index page.
        /// Retrieves all product data from the ProductService and stores it
        /// in the Products property to display on the page.
        /// </summary>
        public void OnGet()
        {
            // Fetch all products using the service and assign them to the Products collection.
            Products = ProductService.GetAllData();
        }
    }
}