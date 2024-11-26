// System namespace for LINQ functionality
using System.Linq;
// Microsoft.Extensions.Logging namespace for logging functionality
using Microsoft.Extensions.Logging;
// A mocking framework used in unit tests to create mock objects that simulate the behavior of real dependencies.
using Moq;
// The NUnit testing framework, used to define unit tests, assertions, and test structures.
using NUnit.Framework;
// Contains the page classes (likely Razor Pages) for the ContosoCrafts website, including the logic for handling page interactions.
using ContosoCrafts.WebSite.Pages;

namespace UnitTests.Pages.Index
{

    /// <summary>
    /// Unit test class for the IndexModel page in the Index namespace.
    /// Contains test methods for verifying the behavior of the OnGet method in IndexModel.
    /// </summary>
    public class IndexTests
    {

        #region TestSetup
        // Static instance of the IndexModel page model used for testing
        public static IndexModel PageModel;

        /// <summary>
        /// Sets up the test environment by initializing the IndexModel with a mocked ILogger and ProductService.
        /// Configures necessary properties and dependencies for testing the Index page.
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            // Mock the ILogger to simulate logging behavior
            var mockLoggerDirect = Mock.Of<ILogger<IndexModel>>();

            // Initialize the IndexModel with the mocked logger and ProductService
            PageModel = new IndexModel(mockLoggerDirect, TestHelper.ProductService)
            {
                // Additional setup can be added here if necessary
            };

        }

        #endregion TestSetup
        /// <summary>
        /// Tests the OnGet method's behavior in the IndexModel.
        /// Verifies that when OnGet is called, 
        /// the ModelState is valid and the Products list is populated, 
        /// indicating that products were successfully loaded.
        /// </summary>
        #region OnGet
        [Test]
        public void OnGet_Valid_Should_Return_Products()
        {
            // Arrange
            // No additional setup is required as we're testing the default behavior

            // Act
            // Call the OnGet method to simulate a GET request on the Index page
            PageModel.OnGet();

            // Assert
            // Verify that the model state is valid after the OnGet call
            Assert.That(PageModel.ModelState.IsValid, Is.EqualTo(true));

            // Assert that the Products collection is not empty, indicating products were loaded
            Assert.That(PageModel.Products.ToList().Any(), Is.EqualTo(true));
        }
        #endregion OnGet

    }

}