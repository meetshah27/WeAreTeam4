// Importing models for defining the structure of product data
using ContosoCrafts.WebSite.Models;
//Importing services to handle data operations for products
using ContosoCrafts.WebSite.Services;
// Importing ASP.NET Core MVC components for controller functionality
using Microsoft.AspNetCore.Mvc;
// Importing Razor Pages components for building page models
using Microsoft.AspNetCore.Mvc.RazorPages;
// Importing LINQ to support data querying functionality
using System.Linq;                         

public class ReadModel : PageModel

{

    // Property for the data service that interacts with product data
    public JsonFileProductService ProductService { get; }

    /// <summary>
    /// Constructor to initialize the ReadModel with the specified product service.
    /// </summary>
    /// <param name="productService">The service for managing product data.</param>
  
    public ReadModel(JsonFileProductService productService)

    {
        // Assigns the provided product service to the ProductService property
        ProductService = productService;  
    }

    // Property to hold a single product's data to be displayed on the page

    public ProductModel Product;

    /// <summary>

    /// <summary>
    /// Handles the HTTP GET request for reading a specific product.
    /// Retrieves the product data based on the provided product ID.
    /// </summary>
    /// <param name="id">The ID of the product to retrieve.</param>

    public IActionResult OnGet(string id)
    {
        // Fetch all products and find the first one that matches the given ID
        Product = ProductService.GetAllData().FirstOrDefault(m => m.Id.Equals(id));

        // If no product is found or its title is null, redirect to the Index page
        if (Product == null || string.IsNullOrEmpty(Product.Title))
        {
            // Redirect to the Index page if product is missing
            return RedirectToPage("./Index"); 
        }
        // Load the page to display the found product
        return Page(); 
    }


}


