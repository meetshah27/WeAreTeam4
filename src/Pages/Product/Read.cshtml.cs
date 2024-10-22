using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

public class ReadModel : PageModel

{

    // Data middle tier

    public JsonFileProductService ProductService { get; }

    /// <summary>

    /// Default Constructor

    /// </summary>

    /// <param name="logger"></param>

    /// <param name="productService"></param>

    public ReadModel(JsonFileProductService productService)

    {

        ProductService = productService;

    }

    // The data to show

    public ProductModel Product;

    /// <summary>

    /// REST Get request

    /// </summary>

    /// <param name="id"></param>

    public void OnGet(string id)

    {

        Product = ProductService.GetAllData().FirstOrDefault(m => m.Id.Equals(id));

    }

}


