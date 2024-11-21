// Importing services for data handling
using ContosoCrafts.WebSite.Services;  
// Importing ASP.NET MVC components
using Microsoft.AspNetCore.Mvc;
// Importing Razor Pages components                                                                                 
using Microsoft.AspNetCore.Mvc.RazorPages;


public class DeleteModel : PageModel
{

    // Property for injecting the product service to interact with product data
    public JsonFileProductService ProductService { get; }

    /// <summary>
    /// Default constructor that initializes the DeleteModel
    /// with the specified product service.
    /// </summary>
    public DeleteModel(JsonFileProductService productService)
    {
        // Assign the provided product service to the ProductService property
        ProductService = productService;

    }

    // Bound property for capturing the product ID from form submissions
    [BindProperty]
    public string ProductId { get; set; }

    /// <summary> 
    /// HTTP GET method to retrieve the product ID from the URL 
    /// and store it in the ProductId property.
    /// </summary>
    /// <param name="id">The ID of the product to be deleted.</param>
    public void OnGet(string id)
    {
        // Assign the received ID to the ProductId property for later use
        ProductId = id;

    }

    /// <summary> 
    /// HTTP POST method to delete the product with the specified ID 
    /// if the model state is valid.
    /// </summary>
    /// <param name="id">The ID of the product to delete.</param>
    public IActionResult OnPost(string id)
    {
        // Validate the model state; if invalid, return to the same page
        if (ModelState.IsValid == false)
        {
            return Page();
        }

        // Call the ProductService to delete the product with the provided ID
        ProductService.DeleteData(id);

        // Redirect to the Index page after a successful deletion
        return RedirectToPage("./Index");

    }

}