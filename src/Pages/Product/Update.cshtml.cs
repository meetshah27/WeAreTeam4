using System.ComponentModel.DataAnnotations;

using System.Linq;                     // Importing LINQ for querying collections

using ContosoCrafts.WebSite.Models;    // Importing models for product representation

using ContosoCrafts.WebSite.Services;  // Importing services for data handling

using Microsoft.AspNetCore.Mvc;         // Importing ASP.NET MVC components

using Microsoft.AspNetCore.Mvc.RazorPages;   // Importing Razor Pages components

public class UpdateModel : PageModel
{
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Description is required.")]
    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
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

    // Data middle-tier service to manage product data
    public JsonFileProductService ProductService { get; }

    /// <summary>  /// Default Constructor for UpdateModel.
    /// Initializes the model with the specified product service. </summary>
    public UpdateModel(JsonFileProductService productService)
    {

        ProductService = productService;   // Assigning the product service to the model

    }

    // The product data to display and bind for form submission
    [BindProperty]
    public ProductModel Product { get; set; }

    /// <summary>   /// Handles the GET request to load the product data for editing.
    /// Retrieves the product based on the provided ID. </summary>
    public void OnGet(string id)
    {
        // Fetch all product data and find the first product that matches the given ID
        Product = ProductService.GetAllData().FirstOrDefault(m => m.Id.Equals(id));

    }

    /// <summary> /// Handles the POST request to update the product data.
    /// Validates the model state and updates the product if valid. </summary>
    public IActionResult OnPost()
    {
        // Check if the model state is valid; if not, return to the same page to display errors
        if (ModelState.IsValid == false)
        {
            
            return Page();

        }

        ProductService.UpdateData(Product);   // Update the product data in the service with the provided Product model

        return RedirectToPage("./Index");     // Redirect to the Index page after successful update

    }

}