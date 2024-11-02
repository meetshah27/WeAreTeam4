using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class CreateModel : PageModel
{
    // Data middle-tier
    public JsonFileProductService ProductService { get; }

    /// <summary> Default Constructor </summary>
    public CreateModel(JsonFileProductService productService)
    {
        ProductService = productService;
    }

    // The data to show, bind to it for the post
    [BindProperty]
    public ProductModel Product { get; set; }

    /// <summary> REST Get request Loads an empty product for creation </summary>
    public void OnGet()
    {
        Product = ProductService.CreateData();
    }
     
    /// <summary> Post the new product data to the service </summary>
    public IActionResult OnPost()
    {
        if (ModelState.IsValid == false)
        {
            return Page();
        }
        ProductService.UpdateData(Product);
        return RedirectToPage("./Index");
    }
}
