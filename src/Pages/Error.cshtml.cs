using System.Diagnostics;                      // For accessing Activity to get the request ID.

using Microsoft.AspNetCore.Mvc;              // For MVC-related attributes and functionality.
using Microsoft.AspNetCore.Mvc.RazorPages;  // For Razor Pages support.
using Microsoft.Extensions.Logging;       // For logging functionality.

namespace ContosoCrafts.WebSite.Pages
{
    // Specifies that the response should not be cached, ensuring that every request for this page results in a fresh response.
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorModel : PageModel
    {
        // Property to store the unique identifier for the current request.
        public string RequestId { get; set; }

        // Property to indicate if the RequestId should be displayed (if it's not null or empty).
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        // Private readonly logger instance for logging purposes.
        private readonly ILogger<ErrorModel> _logger;

        // Constructor to inject the logger dependency into the ErrorModel class.
        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }

        // This method is called on a GET request to the error page.
        public void OnGet()
        {
            // Sets the RequestId to the current activity ID if available; otherwise, it uses the trace identifier from the HTTP context.
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        }
    }
}