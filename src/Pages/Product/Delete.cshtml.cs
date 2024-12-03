using ContosoCrafts.WebSite.Models;

using ContosoCrafts.WebSite.Services;

using Microsoft.AspNetCore.Mvc;
                                                                               
using Microsoft.AspNetCore.Mvc.RazorPages;

using System.Linq;


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
    /// Gets or sets the product to be displayed on the page.
    /// </summary>
    public ProductModel Product { get; private set; }
    /// <summary> 
    /// HTTP GET method to retrieve the product ID from the URL 
    /// and store it in the ProductId property.
    /// </summary>
    /// <param name="id">The ID of the product to be deleted.</param>
    public IActionResult OnGet(string id)
    {
        // Retrieves all products and finds the first one matching the provided ID
        Product = ProductService.GetAllData().FirstOrDefault(m => m.Id == id);

        // Redirects to Index if product is not found or its title is missing
        if (Product == null || string.IsNullOrEmpty(Product.Title))
        {
            return RedirectToPage("../Error");
        }

        // Assign the received ID to the ProductId property for later use
        ProductId = id;

        // Renders the page displaying the retrieved product
        return Page();

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