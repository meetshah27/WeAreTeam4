// Importing models to define the structure of product data
using ContosoCrafts.WebSite.Models;
// Importing services to manage data operations for products
using ContosoCrafts.WebSite.Services;
// Importing ASP.NET Core MVC for controller functionality
using Microsoft.AspNetCore.Mvc;
// Importing Razor Pages components for page model functionality
using Microsoft.AspNetCore.Mvc.RazorPages;
// Importing LINQ for data querying capabilities
using System.Linq;

namespace ContosoCrafts.WebSite.Pages.Product
{
    /// <summary>
    /// Page model for reading a specific product's details.
    /// Handles HTTP GET requests to retrieve and display product information.
    /// </summary>
    public class ReadModel : PageModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadModel"/> class with the specified product service.
        /// </summary>
        /// <param name="productService">Service for managing product data.</param>
        public ReadModel(JsonFileProductService productService)
        {
            ProductService = productService;
        }

        /// <summary>
        /// Gets the product service instance for interacting with product data.
        /// </summary>
        public JsonFileProductService ProductService { get; }

        /// <summary>
        /// Gets or sets the product to be displayed on the page.
        /// </summary>
        public ProductModel Product { get; private set; }

        /// <summary>
        /// Handles the HTTP GET request for a specific product.
        /// Retrieves the product data based on the provided product ID.
        /// </summary>
        /// <param name="id">The ID of the product to retrieve.</param>
        /// <returns>The page displaying the product or a redirect to the Index page if not found.</returns>
        public IActionResult OnGet(string id)
        {
            // Retrieves all products and finds the first one matching the provided ID
            Product = ProductService.GetAllData().FirstOrDefault(m => m.Id == id);

            // Redirects to Index if product is not found or its title is missing
            if (Product == null || string.IsNullOrEmpty(Product.Title))
            {
                return RedirectToPage("./Index");
            }

            // Renders the page displaying the retrieved product
            return Page();
        }
    }
}



