using System.Diagnostics;

using Microsoft.Extensions.Logging;

using NUnit.Framework;

using Moq;

using ContosoCrafts.WebSite.Pages;

namespace UnitTests.Pages.Error
{

    public class ErrorTests
    {

        #region TestSetup
        // Static instance of the ErrorModel page model used for testing
        public static ErrorModel PageModel;

        [SetUp]
        public void TestInitialize()
        {

            var MockLoggerDirect = Mock.Of<ILogger<ErrorModel>>();// Mock the ILogger to simulate the logging behavior

            PageModel = new ErrorModel(MockLoggerDirect)// Initialize the ErrorModel with the mocked logger
            {
                // Set up context and temporary data for the page model using helper methods
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
            Assert.That(PageModel.ModelState.IsValid, Is.EqualTo(true));// Verify that the model state is valid
            Assert.That(PageModel.RequestId, Is.EqualTo(activity.Id));// Assert that the RequestId matches the activity ID
        }

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
            Assert.That(PageModel.RequestId, Is.EqualTo("trace")); // Assert that the RequestId is set to "trace" (a default identifier)
            Assert.That(PageModel.ShowRequestId, Is.EqualTo(true));// Verify that ShowRequestId is true, indicating the request ID should be displayed
        }
        #endregion OnGet

    }

}