// Importing models for product representation
using ContosoCrafts.WebSite.Models; 
// Importing services for data handling
using ContosoCrafts.WebSite.Services;  
// Importing ASP.NET MVC components
using Microsoft.AspNetCore.Mvc;        
// Importing Razor Pages components
using Microsoft.AspNetCore.Mvc.RazorPages;  

using System.ComponentModel.DataAnnotations;

// PageModel for the "Create" page, handling the product creation process
public class CreateModel : PageModel
{
    // Data middle-tier service to interact with product data
    public JsonFileProductService ProductService { get; }

    /// <summary>  
    /// Handles the GET request for the Create page.
    /// Loads an empty product model for user input.
    /// </summary>
    public CreateModel(JsonFileProductService productService)
    {

        ProductService = productService;

    }

    // The data model for a product, bound for form submission
    [BindProperty]
    public ProductModel Product { get; set; }

    /// <summary>  /// Handles the GET request for the Create page.
    /// Loads an empty product model for user input.</summary>
    public void OnGet()
    {
        // Initialize a new ProductModel instance to prepare for product creation
        Product = ProductService.CreateData();

    }
     
    /// <summary> Post the new product data to the service </summary>
    public IActionResult OnPost()
    {
        // Check if the model state is valid; if not, return to the same page
        if (ModelState.IsValid == false)
        {
            return Page();
        }

        // Update the data service with the new product information
        ProductService.UpdateData(Product);

        // Redirect to the Index page after successful creation
        return RedirectToPage("./Index");

    }

    /// <summary>
    /// Delete the product being created and redirect the user back to the index page
    /// </summary>
    public IActionResult OnPostCancel()
    {
        // Logic to delete the product
        if (Product != null)
        {
            ProductService.DeleteData(Product.Id); // Ensure Product.Id exists and is valid
        }

        // Redirect to the Index page or another appropriate page
        return RedirectToPage("./Index");
    }


}
