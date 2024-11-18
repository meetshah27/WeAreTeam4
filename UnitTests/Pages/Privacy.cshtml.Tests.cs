using Microsoft.Extensions.Logging;

using NUnit.Framework;

using Moq;

using ContosoCrafts.WebSite.Pages;

namespace UnitTests.Pages.Privacy
{

    public class PrivacyTests
    {

        #region TestSetup
        // Static instance of the PrivacyModel page model used for testing
        public static PrivacyModel PageModel;

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