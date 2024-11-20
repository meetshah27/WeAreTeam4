using Microsoft.Extensions.Logging;

using NUnit.Framework;

using Moq;

using ContosoCrafts.WebSite.Pages;

namespace UnitTests.Pages.Privacy
{
    /// <summary>
    /// Unit test class for the PrivacyModel page in the Privacy namespace.
    /// This class contains test methods for verifying the behavior of the OnGet method in PrivacyModel.
    /// </summary>
    public class PrivacyTests
    {

        #region TestSetup
        // Static instance of the PrivacyModel page model used for testing
        public static PrivacyModel PageModel;

        /// <summary>
        /// Sets up the test environment by initializing the PrivacyModel with a mocked ILogger.
        /// Configures the PageContext and TempData for the PrivacyModel.
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {

            var mockLoggerDirect = Mock.Of<ILogger<PrivacyModel>>(); // Mock the ILogger to simulate logging behavior

            PageModel = new PrivacyModel(mockLoggerDirect)// Initialize the PrivacyModel with the mocked logger
            {
                // Set up the page context and temporary data using helper methods
                PageContext = TestHelper.PageContext,
                TempData = TestHelper.TempData,

            };

        }

        #endregion TestSetup
        /// <summary>
        /// Tests the OnGet method's behavior in the PrivacyModel.
        /// Verifies that when OnGet is called, the ModelState remains valid, indicating successful page setup.
        /// </summary>
        #region OnGet
        [Test]
        public void OnGet_Valid_Activity_Set_Should_Return_RequestId()
        {

            // Arrange
            // No additional setup is required as we're testing the default behavior

            // Act
            // Call the OnGet method to simulate a GET request on the Privacy page
            PageModel.OnGet();

            // Reset
            // No additional reset actions needed here

            // Assert
            // Verify that the model state is valid after the OnGet call
            Assert.That(PageModel.ModelState.IsValid, Is.EqualTo(true));

        }

        #endregion OnGet

    }

}