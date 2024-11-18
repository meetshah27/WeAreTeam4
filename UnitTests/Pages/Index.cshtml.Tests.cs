using System.Linq;

using Microsoft.Extensions.Logging;

using Moq;

using NUnit.Framework;

using ContosoCrafts.WebSite.Pages;

namespace UnitTests.Pages.Index
{

    public class IndexTests
    {

        #region TestSetup
        // Static instance of the IndexModel page model used for testing
        public static IndexModel PageModel;

        [SetUp]
        public void TestInitialize()
        {
            var mockLoggerDirect = Mock.Of<ILogger<IndexModel>>(); // Mock the ILogger to simulate logging behavior

            PageModel = new IndexModel(mockLoggerDirect, TestHelper.ProductService)// Initialize the IndexModel with the mocked logger and ProductService
            {
                // Additional setup can be added here if necessary
            };

        }

        #endregion TestSetup

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
            Assert.That(PageModel.ModelState.IsValid, Is.EqualTo(true));// Verify that the model state is valid after the OnGet call
            Assert.That(PageModel.Products.ToList().Any(), Is.EqualTo(true));// Assert that the Products collection is not empty, indicating products were loaded
        }
        #endregion OnGet

    }

}