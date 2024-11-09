using ContosoCrafts.WebSite.Services;  // Importing services for data handling

using Microsoft.AspNetCore.Mvc;        // Importing ASP.NET MVC components

using Microsoft.AspNetCore.Mvc.RazorPages;  // Importing Razor Pages components


public class DeleteModel : PageModel
{

    // Data middle-tier service to interact with product data
    public JsonFileProductService ProductService { get; }

    /// <summary>  /// Default Constructor that initializes the DeleteModel
    /// with a specified product service. </summary>
    public DeleteModel(JsonFileProductService productService)
    {

        ProductService = productService;

    }

    // The data model for a product, bound for form submission
    [BindProperty]
    public string ProductId { get; set; }

    /// <summary> Save the product id for the product so it can be used later</summary>
    /// <param name="id"></param>
    public void OnGet(string id)
    {

        // Fetch all product data and find the first product that matches the given ID
        ProductId = id;

    }

    /// <summary> Delete the product with the saved id </summary>
    public IActionResult OnPost(string id)
    {
        // Check if the model state is valid; if not, return to the same page
        if (ModelState.IsValid == false)
        {

            return Page();

        }

        // Delete the product with the given id
        ProductService.DeleteData(id);

        // Redirect to the Index page after deletion
        return RedirectToPage("./Index");

    }

}
