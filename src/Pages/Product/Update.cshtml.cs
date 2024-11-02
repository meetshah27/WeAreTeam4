using System.Linq;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class UpdateModel : PageModel
{
    // Data middle-tier
    public JsonFileProductService ProductService { get; }

    /// <summary> Default Constructor </summary>
    public UpdateModel(JsonFileProductService productService)
    {
        ProductService = productService;
    }

    // The data to show, bind to it for the post
    [BindProperty]
    public ProductModel Product { get; set; }

    /// <summary> REST Get request Loads the Data </summary>
    public void OnGet(string id)
    {
        Product = ProductService.GetAllData().FirstOrDefault(m => m.Id.Equals(id));
    }
    
    /// <summary> Post the model back to the page The model is in the class variable </summary>
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
