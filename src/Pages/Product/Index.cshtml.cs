using System.Collections.Generic;  // Importing collections for managing lists

using Microsoft.AspNetCore.Mvc.RazorPages;   // Importing Razor Pages components

using ContosoCrafts.WebSite.Models;          // Importing models for product representation
using ContosoCrafts.WebSite.Services;        // Importing services for data handling


namespace ContosoCrafts.WebSite.Pages.Product
{
    /// <summary>
    /// Index Page Model for displaying a list of products.
    /// This page will return all the data to show to the user
    /// </summary>
    public class IndexModel : PageModel
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="productService"></param>
        public IndexModel(JsonFileProductService productService)
        {
            ProductService = productService;  // Assigning the product service to the model
        }

        // / Data service to handle product operations
        public JsonFileProductService ProductService { get; }

        // // Collection to hold the product data retrieved from the service
        public IEnumerable<ProductModel> Products { get; private set; }

        /// <summary>
        /// REST OnGet
        ///  Handles the GET request for the Index page.
        /// Retrieves all product data and stores it in the Products collection.
        /// </summary>
        public void OnGet()
        {
            // Fetch all product data from the service and assign it to the Products property
            Products = ProductService.GetAllData();
        }
    }
}