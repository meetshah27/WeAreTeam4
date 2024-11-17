using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc.RazorPages;

using Microsoft.AspNetCore.Mvc.ModelBinding;

using Microsoft.AspNetCore.Mvc.ViewFeatures;

using Microsoft.AspNetCore.Hosting;

using Microsoft.AspNetCore.Routing;

using Microsoft.Extensions.Logging;

using Moq;

using NUnit.Framework;

using ContosoCrafts.WebSite.Services;

namespace UnitTests.Pages.Product
{

    class DeleteTests
    {

        #region TestSetup
        // Declare necessary properties for setting up tests
        public static DefaultHttpContext HttpContextDefault;

        public static ModelStateDictionary ModelState;

        public static ActionContext ActionContext;

        public static EmptyModelMetadataProvider ModelMetadataProvider;

        public static ViewDataDictionary TestsViewData;

        public static TempDataDictionary TempData;

        public static PageContext PageContext;

        public static DeleteModel PageModel;

        [SetUp]
        public void TestInitialize()
        {

            HttpContextDefault = new DefaultHttpContext(); // Initializes a default HTTP context for testing

            ModelState = new ModelStateDictionary();// Initializes ModelState for model validation during tests

            ActionContext = new ActionContext(HttpContextDefault, HttpContextDefault.GetRouteData(), new PageActionDescriptor(), ModelState);// Sets up ActionContext, required by PageContext for Razor Pages testing

            ModelMetadataProvider = new EmptyModelMetadataProvider();// Configures ModelMetadataProvider, used for providing metadata to view data

            TestsViewData = new ViewDataDictionary(ModelMetadataProvider, ModelState);// Initializes ViewData with the ModelMetadataProvider and ModelState

            TempData = new TempDataDictionary(HttpContextDefault, Mock.Of<ITempDataProvider>()); // Configures TempData, used to store temporary data in Razor Pages

            PageContext = new PageContext(ActionContext)// Sets up the PageContext with the ActionContext and ViewData for the page model
            {

                ViewData = TestsViewData,

            };
            // Mocks the hosting environment to simulate settings like root paths for testing purposes
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            mockWebHostEnvironment.Setup(m => m.EnvironmentName).Returns("Hosting:UnitTestEnvironment");
            mockWebHostEnvironment.Setup(m => m.WebRootPath).Returns("../../../../src/bin/Debug/net7.0/wwwroot");
            mockWebHostEnvironment.Setup(m => m.ContentRootPath).Returns("./data/");

            // Mocks the logger to avoid actual logging in tests
            var mockLoggerDirect = Mock.Of<ILogger<UpdateModel>>();
            JsonFileProductService productService = new JsonFileProductService(mockWebHostEnvironment.Object);

            PageModel = new DeleteModel(productService)// Initializes the DeleteModel page model with ProductService and other test contexts
            {

                PageContext = PageContext,
                TempData = TempData,

            };

        }

        #endregion TestSetup

        #region OnGet
        [Test]
        public void OnGet_Valid_Should_Return_Valid_State()
        {

            // Arrange
            var data = TestHelper.ProductService.CreateData();// Set up necessary product data for testing

            // Act
            PageModel.OnGet(data.Id);// Calls the OnGet method with a valid product ID

            // Assert
            Assert.That(PageModel.ModelState.IsValid, Is.EqualTo(true));// Ensures ModelState is valid
            Assert.That(PageModel.ProductId, Is.EqualTo(data.Id));// Verifies the correct product ID is set
            Assert.That(PageModel.ProductId, Is.Not.Null);  //Ensures that the product is loaded and not null.
        }
        #endregion OnGet

        #region OnPost

        [Test]
        public void OnPost_Invalid_Id_Should_Return_Invalid_State()
        {
            // Arrange
            PageModel.OnGet(null);// Sets an invalid (null) ProductId
            PageModel.ModelState.AddModelError("ProductId", "Id does not exist"); // Adds a model error for invalid ProductId

            // Act
            var result = PageModel.OnPost(null);// Calls OnPost with an invalid ID

            // Assert
            Assert.That(PageModel.ModelState.IsValid, Is.False, "PageModel should have invalid state when error exists."); // Verifies ModelState is invalid
            Assert.That(result, Is.InstanceOf<PageResult>(), "Should return PageResult in event of an error.");// Checks that the result is a PageResult
            Assert.That(PageModel.ProductId, Is.Null, "PageModel Product Id should be null.");// Confirms ProductId is null
        }


        [Test]
        public void OnPost_Valid_Id_Should_Return_Valid_Id()
        {
            // Arrange
            var data = TestHelper.ProductService.CreateData();// Creates valid product data
            PageModel.OnGet(data.Id);// Sets up the page with a valid ProductId by calling OnGet

            // Act
            var result = PageModel.OnPost(data.Id); // Calls OnPost with a valid ID

            // Assert
            Assert.That(PageModel.ModelState.IsValid, Is.True, "Valid input should return valid ModelState."); // Ensures ModelState is valid
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>(), "Valid input should return a RedirectToPageResult, as the page is redirecting to a new page.");// Checks that a redirection occurs
        }
        #endregion OnPost

    }

}