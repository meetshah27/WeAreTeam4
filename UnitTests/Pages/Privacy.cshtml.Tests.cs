using Microsoft.Extensions.Logging;

using NUnit.Framework;

using Moq;

using ContosoCrafts.WebSite.Pages;

namespace UnitTests.Pages.Privacy
{

    public class PrivacyTests
    {

        #region TestSetup
        public static PrivacyModel PageModel;

        [SetUp]
        public void TestInitialize()
        {

            var mockLoggerDirect = Mock.Of<ILogger<PrivacyModel>>();

            PageModel = new PrivacyModel(mockLoggerDirect)
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

            // Act
            PageModel.OnGet();

            // Reset

            // Assert
            Assert.That(PageModel.ModelState.IsValid, Is.EqualTo(true));

        }

        #endregion OnGet

    }

}