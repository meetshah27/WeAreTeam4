// Importing Razor Pages components for building page models and handling requests
using Microsoft.AspNetCore.Mvc.RazorPages;
// Importing logging functionality to log events and errors within the application
using Microsoft.Extensions.Logging;
using System;

namespace ContosoCrafts.WebSite.Pages
{
    /// <summary>
    /// Represents the model for the Privacy page, responsible for handling the page's logic.
    /// </summary>
    public class PrivacyModel : PageModel
    {
        // Private readonly logger instance for logging messages and errors specific to the Privacy page.
        private readonly ILogger<PrivacyModel> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrivacyModel"/> class with a logger.
        /// </summary>
        /// <param name="logger">An <see cref="ILogger"/> instance for logging events and errors on the Privacy page.</param>
        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Handles HTTP GET requests to the Privacy page.
        /// Currently, this method does not perform any actions but can be extended in the future.
        /// </summary>
        public void OnGet()
        {
            // Log that the Privacy page was accessed (optional logging for tracking page loads)
            _logger.LogInformation("Privacy page accessed.");

            // Future logic can be added here, if needed.
        }
    }
}

