using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc.RazorPages;

using ContosoCrafts.WebSite.Models;

using ContosoCrafts.WebSite.Services;

using System.Linq;

using System;

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
        public void OnGet(string sortBy, string sortOrder)
        {
            // Get all products
            Products = ProductService.GetAllData();

            // Default values for sorting parameters if not provided
            sortBy="Title";  // Default to "Title" if no sortBy parameter is passed
            
            // Default to "asc" if no sortOrder parameter is passed
            if (sortOrder=="asc")
            {
                sortOrder = "asc";
            }
            if(sortOrder=="desc")
            {
                sortOrder = "desc";
            }
            // Sorting logic based on the sortBy and sortOrder parameters
            if (sortBy.Equals("Title", StringComparison.OrdinalIgnoreCase))
            {
                // Sorting by Title
                Products = sortOrder == "asc"
                    ? Products.OrderBy(p => p.Title).ToList()
                    : Products.OrderByDescending(p => p.Title).ToList();
            }
            // You can add more sorting conditions for other fields here if needed
        }

    }
}