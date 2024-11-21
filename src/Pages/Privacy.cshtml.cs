// Importing Razor Pages components for building page models and handling requests
using Microsoft.AspNetCore.Mvc.RazorPages;
// Importing logging functionality to log events and errors within the application
using Microsoft.Extensions.Logging;          

namespace ContosoCrafts.WebSite.Pages
{
    // Represents the model for the Privacy page, responsible for handling the page's logic
    public class PrivacyModel : PageModel
    {
        // Private readonly logger instance for logging messages and errors specific to the Privacy page
        private readonly ILogger<PrivacyModel> _logger;

        /// <summary>
        /// Constructor to initialize the PrivacyModel with the injected logger dependency.
        /// </summary>
        /// <param name="logger">Logger instance for logging events and errors on the Privacy page.</param>
        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            // Assigns the injected logger to the _logger field for use within the class
            _logger = logger;
        }

        /// <summary>
        /// Handles the HTTP GET request to the Privacy page.
        /// Currently, this method does not perform any actions but can be extended in the future.
        /// </summary>
        public void OnGet()
        {
            // Currently, this method does not perform any actions, but it can be used to log page access
            // or handle any other logic needed when the Privacy page is loaded.
        }
    }
}
