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
        public static ErrorModel PageModel;

        [SetUp]
        public void TestInitialize()
        {
            var MockLoggerDirect = Mock.Of<ILogger<ErrorModel>>();

            PageModel = new ErrorModel(MockLoggerDirect)
            {
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

            Activity activity = new Activity("activity");
            activity.Start();

            // Act
            PageModel.OnGet();

            // Reset
            activity.Stop();

            // Assert
            Assert.That(PageModel.ModelState.IsValid, Is.EqualTo(true));
            Assert.That(PageModel.RequestId, Is.EqualTo(activity.Id));
        }

        [Test]
        public void OnGet_InValid_Activity_Null_Should_Return_TraceIdentifier()
        {
            // Arrange

            // Act
            PageModel.OnGet();

            // Reset

            // Assert
            Assert.That(PageModel.ModelState.IsValid, Is.EqualTo(true));
            Assert.That(PageModel.RequestId, Is.EqualTo("trace"));
            Assert.That(PageModel.ShowRequestId, Is.EqualTo(true));
        }
        #endregion OnGet
    }
}