// Import collections for managing lists
using System.Collections.Generic;
// Import Razor Pages components
using Microsoft.AspNetCore.Mvc.RazorPages;
// Import models for product representation
using ContosoCrafts.WebSite.Models;
// Import services for data handling
using ContosoCrafts.WebSite.Services;

namespace ContosoCrafts.WebSite.Pages.Product
{
    /// <summary>
    /// Model for the Product Index page, which displays a list of products.
    /// Handles requests to show product data on the main Index page.
    /// </summary>
    public class IndexModel : PageModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IndexModel"/> class with the specified product service.
        /// </summary>
        /// <param name="productService">Service for managing product data.</param>
        public IndexModel(JsonFileProductService productService)
        {
            ProductService = productService;
        }

        /// <summary>
        /// Gets the product service instance for managing product data.
        /// </summary>
        public JsonFileProductService ProductService { get; }

        /// <summary>
        /// Gets the collection of products retrieved from the data service.
        /// </summary>
        public IEnumerable<ProductModel> Products { get; private set; }

        /// <summary>
        /// Handles the HTTP GET request for the Index page by retrieving all products
        /// from the <see cref="ProductService"/> and storing them in the <see cref="Products"/> property.
        /// </summary>
        public void OnGet()
        {
            Products = ProductService.GetAllData();
        }
    }
}
