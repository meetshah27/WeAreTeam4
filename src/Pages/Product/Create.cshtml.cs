using ContosoCrafts.WebSite.Models; // Importing models for product representation

using ContosoCrafts.WebSite.Services;  // Importing services for data handling

using Microsoft.AspNetCore.Mvc;        // Importing ASP.NET MVC components

using Microsoft.AspNetCore.Mvc.RazorPages;  // Importing Razor Pages components

using System.ComponentModel.DataAnnotations;


public class CreateModel : PageModel
{
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Description is required.")]
    [StringLength(1000, ErrorMessage = "Description cannot exceed 500 characters.")]
    public string Description { get; set; }

    [Required(ErrorMessage = "URL is required.")]
    [Url(ErrorMessage = "Please enter a valid URL.")]
    public string ProductUrl { get; set; }

    [Required(ErrorMessage = "Image URL is required.")]
    [Url(ErrorMessage = "Please enter a valid Image URL.")]
    public string Image { get; set; }

    [Required(ErrorMessage = "GitHub URL is required.")]
    [Url(ErrorMessage = "Please enter a valid GitHub URL.")]
    public string GitHub { get; set; }

    // Data middle-tier service to interact with product data
    public JsonFileProductService ProductService { get; }

    /// <summary>  /// Default Constructor that initializes the CreateModel
    /// with a specified product service. </summary>
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
        // Initialize a new ProductModel instance for the creation form
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

}
