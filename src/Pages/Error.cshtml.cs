// Importing System.Diagnostics to access diagnostic tools like Activity for tracking request details
using System.Diagnostics;
// Importing ASP.NET Core MVC components for controller and response handling
using Microsoft.AspNetCore.Mvc;
// Importing Razor Pages components for building and managing page models
using Microsoft.AspNetCore.Mvc.RazorPages;
// Importing logging utilities for application logging
using Microsoft.Extensions.Logging; 

namespace ContosoCrafts.WebSite.Pages
{
    // Disables caching for this page, ensuring a fresh response is generated for each request.
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorModel : PageModel
    {
        // Property to store the unique identifier for the current request.
        public string RequestId { get; set; }

        // Property to check if the RequestId is not null or empty, indicating it should be displayed on the page.
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        // Private readonly field to hold the logger instance for logging within this class.
        private readonly ILogger<ErrorModel> _logger;

        // Constructor to initialize the ErrorModel with the provided logger.
        public ErrorModel(ILogger<ErrorModel> logger)
        {
            // Assigns the provided logger to the _logger field for use within the class
            _logger = logger;
        }

        // This method is called on a GET request to the error page.
        public void OnGet()
        {
            // Sets the RequestId property to the current activity ID if available;
            // otherwise, it defaults to the trace identifier from the HTTP context.
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        }
    }
}