using ContosoCrafts.WebSite.Models;  // Importing models for product representation
using ContosoCrafts.WebSite.Services;  // Importing services for data handling
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;  // Importing Razor Pages components
using System.Linq;                         // Importing LINQ for querying collections

public class ReadModel : PageModel

{

     // Data middle-tier service to interact with product data

    public JsonFileProductService ProductService { get; }

    /// <summary>

    /// Default Constructor for ReadModel.
    /// Initializes the model with the specified product service.

    /// </summary>

    /// <param name="logger"></param>

    /// <param name="productService"></param>

    public ReadModel(JsonFileProductService productService)

    {

        ProductService = productService;   // Assigning the product service to the model

    }

    // // The product data to display on the page

    public ProductModel Product;

    /// <summary>

    /// Handles the GET request for reading a specific product.
    /// Retrieves the product data based on the provided ID.

    /// </summary>

    /// <param name="id"></param>

    public IActionResult OnGet(string id)
    {
        // Fetch all product data and find the first product that matches the given ID
        Product = ProductService.GetAllData().FirstOrDefault(m => m.Id.Equals(id));

        // Check if the product or its title is null; if so, redirect to the Index page
        if (Product == null || string.IsNullOrEmpty(Product.Title))
        {
            return RedirectToPage("./Index"); // Redirect to the Index page
        }

        return Page(); // Otherwise, load the page normally
    }


}


