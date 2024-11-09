using Microsoft.AspNetCore.Mvc.RazorPages;   // For Razor Pages functionality.
using Microsoft.Extensions.Logging;          // For logging functionality.

namespace ContosoCrafts.WebSite.Pages
{
    // Model for the Privacy page, handling the logic for that page.
    public class PrivacyModel : PageModel
    {
        // Private readonly logger instance for logging events and errors.
        private readonly ILogger<PrivacyModel> _logger;

        // Constructor to initialize the logger dependency.
        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            // Assigns the injected logger to the local logger instance.
            _logger = logger;
        }

        // Called on a GET request to the Privacy page.
        public void OnGet()
        {
            // Currently, this method does not perform any actions,
            // but could be used for future logic, such as logging page access.
        }
    }
}
