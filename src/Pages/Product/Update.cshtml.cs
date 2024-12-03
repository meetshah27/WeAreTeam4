// Importing LINQ to enable querying and filtering on collections
using System.Linq;
// Importing models to represent product data
using ContosoCrafts.WebSite.Models;
// Importing services for handling product data operations
using ContosoCrafts.WebSite.Services;
// Importing ASP.NET Core MVC components for controller and view functionality
using Microsoft.AspNetCore.Mvc;
// Importing Razor Pages components to build and manage page models
using Microsoft.AspNetCore.Mvc.RazorPages;   

public class UpdateModel : PageModel
{
    // Service for managing product data interactions
    public JsonFileProductService ProductService { get; }

    /// <summary>
    /// Constructor to initialize the UpdateModel with the specified product service.
    /// </summary>
    /// <param name="productService">The service for managing product data.</param>
    public UpdateModel(JsonFileProductService productService)
    {
        // Assigns the provided product service to the ProductService property
        ProductService = productService;   

    }

    // Property to hold the product data for display and binding during form submission
    [BindProperty]
    public ProductModel Product { get; set; }

    /// <summary>
    /// Handles the HTTP GET request to load product data for editing.
    /// Retrieves the product based on the provided product ID.
    /// </summary>
    /// <param name="id">The ID of the product to retrieve for editing.</param>
    public IActionResult OnGet(string id)
    {
        // Retrieves all products and finds the first one matching the provided ID
        Product = ProductService.GetAllData().FirstOrDefault(m => m.Id == id);

        // Redirects to Index if product is not found or its title is missing
        if (Product == null || string.IsNullOrEmpty(Product.Title))
        {
            return RedirectToPage("../Error");
        }

        // Renders the page displaying the retrieved product
        return Page();
    }

    /// <summary>
    /// Handles the HTTP POST request to update product data.
    /// Validates the model state and, if valid, updates the product data.
    /// </summary>
    public IActionResult OnPost()
    {
        if (!ProductService.GetAllData().Any(p => p.Id.Equals(Product.Id)))
        {
            return RedirectToPage("../Error");
        }

        // Check if the model state is valid; if not, return to the same page to display validation errors
        if (ModelState.IsValid == false)
        {
            return Page();
        }
        // Update the product data in the service using the modified Product model
        ProductService.UpdateData(Product);   
        // Redirect to the Index page after a successful update
        return RedirectToPage("./Index");    

    }

}