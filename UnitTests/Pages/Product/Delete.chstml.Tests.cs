// ASP.NET Core namespace for HTTP abstractions, such as HttpRequest and HttpResponse
using Microsoft.AspNetCore.Http;
// ASP.NET Core namespace for MVC-related features, such as controllers and action results
using Microsoft.AspNetCore.Mvc;
// ASP.NET Core namespace for Razor Pages, a page-based programming model for MVC
using Microsoft.AspNetCore.Mvc.RazorPages;
// ASP.NET Core namespace for handling model validation, including binding and validation errors
using Microsoft.AspNetCore.Mvc.ModelBinding;
// ASP.NET Core namespace for view rendering and properties, like TempData and ViewData
using Microsoft.AspNetCore.Mvc.ViewFeatures;
// ASP.NET Core namespace for hosting environment abstractions, such as IHostingEnvironment
using Microsoft.AspNetCore.Hosting;
// ASP.NET Core namespace for URL routing and route handling
using Microsoft.AspNetCore.Routing;
// ASP.NET Core namespace for logging, allowing for structured and configurable logging
using Microsoft.Extensions.Logging;
// Moq namespace, a popular mocking framework for creating mock objects in unit tests
using Moq;
// NUnit framework namespace for writing unit tests
using NUnit.Framework;
// ContosoCrafts service namespace, likely containing custom services used in the project
using ContosoCrafts.WebSite.Services;
using System.Linq;
// Namespace for unit tests specific to the Product page in the ContosoCrafts application
namespace UnitTests.Pages.Product
{
    /// <summary>
    /// Unit test class for the DeleteModel page in the Product namespace.
    /// Contains test methods to verify the behavior of the OnGet and OnPost methods.
    /// </summary>
    class DeleteTests
    {

        #region TestSetup
        // Declares a default HTTP context used to simulate HTTP-related information for testing
        public static DefaultHttpContext HttpContextDefault;
        // ModelState dictionary for managing validation state, used in testing model validation
        public static ModelStateDictionary ModelState;
        // ActionContext holds contextual information about the current HTTP request, used in testing action execution
        public static ActionContext ActionContext;
        // Provides model metadata and validation information, useful for testing view rendering and validation
        public static EmptyModelMetadataProvider ModelMetadataProvider;
        // Dictionary for storing view data, allows testing data passed to views
        public static ViewDataDictionary TestsViewData;
        // TempData dictionary, used to pass temporary data between requests, allowing for testing temporary data handling
        public static TempDataDictionary TempData;
        // Context for a Razor page, containing information about the HTTP request, user, and view data
        public static PageContext PageContext;
        // The DeleteModel page model being tested, representing the Delete page functionality
        public static DeleteModel PageModel;

        [SetUp]
        /// <summary>
        /// Initializes the setup for tests, creating mock dependencies and configuring the DeleteModel
        /// with necessary contexts such as HTTP context, ModelState, TempData, and PageContext.
        /// </summary>
        public void TestInitialize()
        {
            // Initializes a default HTTP context for testing
            HttpContextDefault = new DefaultHttpContext();
            // Initializes ModelState for model validation during tests
            ModelState = new ModelStateDictionary();
            // Sets up ActionContext, required by PageContext for Razor Pages testing
            ActionContext = new ActionContext(HttpContextDefault, HttpContextDefault.GetRouteData(), new PageActionDescriptor(), ModelState);
            // Configures ModelMetadataProvider, used for providing metadata to view data
            ModelMetadataProvider = new EmptyModelMetadataProvider();
            // Initializes ViewData with the ModelMetadataProvider and ModelState
            TestsViewData = new ViewDataDictionary(ModelMetadataProvider, ModelState);
            // Configures TempData, used to store temporary data in Razor Pages
            TempData = new TempDataDictionary(HttpContextDefault, Mock.Of<ITempDataProvider>());
            // Sets up the PageContext with the ActionContext and ViewData for the page model
            PageContext = new PageContext(ActionContext)
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
            // Initializes the DeleteModel page model with ProductService and other test contexts
            PageModel = new DeleteModel(productService)
            {

                PageContext = PageContext,
                TempData = TempData,

            };

        }

        #endregion TestSetup

        #region OnGet
        /// <summary>
        /// Verifies that OnGet with a valid product ID loads the Delete page with a valid ModelState
        /// and the specified product ID. Confirms that the ModelState is valid, the ProductId is set correctly, and is not null.
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Return_Valid_State()
        {

            // Arrange
            // Retrieve an extant product for testing
            var data = TestHelper.ProductService.GetAllData().First();

            // Act
            // Calls the OnGet method with a valid product ID
            PageModel.OnGet(data.Id);

            // Assert
            // Ensures ModelState is valid
            Assert.That(PageModel.ModelState.IsValid, Is.EqualTo(true));
            // Verifies the correct product ID is set
            Assert.That(PageModel.ProductId, Is.EqualTo(data.Id));
            //Ensures that the product is loaded and not null.
            Assert.That(PageModel.ProductId, Is.Not.Null);  
        }
        #endregion OnGet

        #region OnPost
        /// <summary>
        /// Verifies that OnPost with an invalid (null) ProductId returns an invalid ModelState,
        /// sets an error message in ModelState, and returns a PageResult. Confirms that the ProductId remains null.
        /// </summary>
        [Test]
        public void OnPost_Invalid_Id_Should_Return_Invalid_State()
        {
            // Arrange
            // Sets an invalid (null) ProductId
            PageModel.OnGet(null);
            // Adds a model error for invalid ProductId
            PageModel.ModelState.AddModelError("ProductId", "Id does not exist");

            // Act
            // Calls OnPost with an invalid ID
            var result = PageModel.OnPost(null);

            // Assert// Verifies ModelState is invalid
            Assert.That(PageModel.ModelState.IsValid, Is.False, "PageModel should have invalid state when error exists.");
            // Checks that the result is a PageResult
            Assert.That(result, Is.InstanceOf<PageResult>(), "Should return PageResult in event of an error.");
            // Confirms ProductId is null
            Assert.That(PageModel.ProductId, Is.Null, "PageModel Product Id should be null.");
        }

        /// <summary>
        /// Verifies that OnPost with a valid product ID redirects to another page, indicating successful deletion.
        /// Ensures ModelState is valid and that the result is a RedirectToPageResult, confirming the redirect behavior.
        /// </summary>
        [Test]
        public void OnPost_Valid_Id_Should_Return_Valid_Id()
        {
            // Arrange
            // Creates valid product data
            var data = TestHelper.ProductService.CreateData();
            // Sets up the page with a valid ProductId by calling OnGet
            PageModel.OnGet(data.Id);

            // Act
            // Calls OnPost with a valid ID
            var result = PageModel.OnPost(data.Id);

            // Assert
            // Ensures ModelState is valid
            Assert.That(PageModel.ModelState.IsValid, Is.True, "Valid input should return valid ModelState.");
            // Checks that a redirection occurs
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>(), "Valid input should return a RedirectToPageResult, as the page is redirecting to a new page.");
        }
        #endregion OnPost

    }

}