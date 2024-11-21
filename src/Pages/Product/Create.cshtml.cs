// Importing models for product representation
using ContosoCrafts.WebSite.Models;
// Importing services for data handling
using ContosoCrafts.WebSite.Services;
// Importing ASP.NET MVC components
using Microsoft.AspNetCore.Mvc;
// Importing Razor Pages components
using Microsoft.AspNetCore.Mvc.RazorPages;

/// <summary>
/// PageModel for the "Create" page, handling the product creation process.
/// </summary>
public class CreateModel : PageModel
{
    // Data middle-tier service to interact with product data
    public JsonFileProductService ProductService { get; }

    /// <summary>
    /// Constructor for CreateModel, initializes the service for interacting with product data.
    /// </summary>
    /// <param name="productService">Service for handling product data</param>
    public CreateModel(JsonFileProductService productService)
    {
        ProductService = productService;
    }

    // The data model for a product, bound for form submission
    [BindProperty]
    public ProductModel Product { get; set; }

    /// <summary>
    /// Handles the GET request for the Create page.
    /// Initializes an empty product model for user input.
    /// </summary>
    public void OnGet()
    {
        // Initialize a new ProductModel instance to prepare for product creation
        Product = ProductService.CreateData();
    }

    /// <summary>
    /// Handles the POST request to create a new product.
    /// Validates the form and updates the product data if valid.
    /// </summary>
    public IActionResult OnPost()
    {
        // Check if the model state is valid; if not, return to the same page
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Update the data service with the new product information
        ProductService.UpdateData(Product);

        // Redirect to the Index page after successful creation
        return RedirectToPage("./Index");
    }

    /// <summary>
    /// Handles the POST request for canceling the product creation process.
    /// Deletes the product if it exists and redirects to the Index page.
    /// </summary>
    public IActionResult OnPostCancel()
    {
        // Check if the product exists and delete it
        if (Product != null)
        {
            ProductService.DeleteData(Product.Id); // Ensure Product.Id exists and is valid
        }

        // Redirect to the Index page after cancellation
        return RedirectToPage("./Index");
    }
}

