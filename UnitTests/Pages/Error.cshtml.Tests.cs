// System namespace for diagnostics
using System.Diagnostics;
// Microsoft.Extensions.Logging namespace for logging functionality
using Microsoft.Extensions.Logging;
// NUnit framework for unit testing
using NUnit.Framework;
// Moq library for mocking dependencies in unit tests
using Moq;
// ContosoCrafts.WebSite.Pages namespace for the pages in the website
using ContosoCrafts.WebSite.Pages;

namespace UnitTests.Pages.Error
{
    /// <summary>
    /// Unit test class for the ErrorModel page in the Error namespace.
    /// Contains test methods for verifying the behavior of the OnGet method in ErrorModel.
    /// </summary>
    public class ErrorTests
    {

        #region TestSetup
        // Static instance of the ErrorModel page model used for testing
        public static ErrorModel PageModel;

        /// <summary>
        /// Sets up the test environment by initializing the ErrorModel with a mocked ILogger.
        /// Configures necessary properties like PageContext and TempData for testing the Error page.
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            // Mock the ILogger to simulate the logging behavior
            var MockLoggerDirect = Mock.Of<ILogger<ErrorModel>>();
            // Initialize the ErrorModel with the mocked logger
            PageModel = new ErrorModel(MockLoggerDirect)
            {
                // Set up context and temporary data for the page model using helper methods
                PageContext = TestHelper.PageContext,
                TempData = TestHelper.TempData,

            };

        }

        #endregion TestSetup
        /// <summary>
        /// Tests the OnGet method's behavior in the ErrorModel when a valid Activity is set.
        /// Verifies that the ModelState is valid and the RequestId matches the Activity ID, indicating the Activity was properly tracked.
        /// </summary>
        #region OnGet
        [Test]
        public void OnGet_Valid_Activity_Set_Should_Return_RequestId()
        {

            // Arrange
            // Create and start a new Activity with a specific name
            Activity activity = new Activity("activity");
            activity.Start();

            // Act
            // Call the OnGet method to simulate a GET request on the Error page
            PageModel.OnGet();

            // Reset
            // Stop the activity to release resources
            activity.Stop();

            // Assert
            // Verify that the model state is valid
            Assert.That(PageModel.ModelState.IsValid, Is.EqualTo(true));
            // Assert that the RequestId matches the activity ID
            Assert.That(PageModel.RequestId, Is.EqualTo(activity.Id));
        }
        /// <summary>
        /// Tests the OnGet method's behavior in the ErrorModel when no Activity is set (null Activity scenario).
        /// Verifies that ModelState is valid, RequestId is set to a default trace identifier, and ShowRequestId is true, indicating a request ID should be shown.
        /// </summary>
        [Test]
        public void OnGet_InValid_Activity_Null_Should_Return_TraceIdentifier()
        {
            // Arrange
            // No activity is started here, simulating a null activity scenario

            // Act
            // Call the OnGet method to simulate a GET request without an activity
            PageModel.OnGet();

            // Reset
            // No activity to stop here

            // Assert
            // Verify that the model state is valid
            Assert.That(PageModel.ModelState.IsValid, Is.EqualTo(true));
            // Assert that the RequestId is set to "trace" (a default identifier)
            Assert.That(PageModel.RequestId, Is.EqualTo("trace"));
            // Verify that ShowRequestId is true, indicating the request ID should be displayed
            Assert.That(PageModel.ShowRequestId, Is.EqualTo(true));
        }
        #endregion OnGet

    }

}