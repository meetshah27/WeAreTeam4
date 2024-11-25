using System.Linq;

using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc.Routing;

using Microsoft.AspNetCore.Mvc.ModelBinding;

using Microsoft.AspNetCore.Mvc.ViewFeatures;

using Microsoft.AspNetCore.Mvc.RazorPages;

using Microsoft.AspNetCore.Routing;

using Microsoft.AspNetCore.Hosting;

using Microsoft.Extensions.Logging;

using Moq;

using NUnit.Framework;

using ContosoCrafts.WebSite.Pages.Product;

using ContosoCrafts.WebSite.Services;

namespace UnitTests.Pages.Product.Index
{
    /// <summary>
    /// Unit test class for the IndexModel page in the Product namespace.
    /// Contains test methods to verify the behavior of the OnGet method.
    /// </summary>
    public class IndexTests
    {

        #region TestSetup
        // Factory for creating URL helpers, used to test URL generation within the page model
        public static IUrlHelperFactory UrlHelperFactory;
        // Default HTTP context, simulating HTTP request/response properties in tests
        public static DefaultHttpContext HttpContextDefault;
        // Mocked web hosting environment, allows configuring paths and environment settings in tests
        public static IWebHostEnvironment WebHostEnvironment;
        // Dictionary to manage validation state for testing model binding and validation
        public static ModelStateDictionary ModelState;
        // Provides context about the current HTTP request, enabling testing of request-specific actions
        public static ActionContext ActionContext;
        // Metadata provider for model validation, used to simulate metadata for model properties in tests
        public static EmptyModelMetadataProvider ModelMetadataProvider;
        // Stores view-specific data passed from controllers to views in tests
        public static ViewDataDictionary TestsViewData;
        // Temporary data dictionary for holding short-lived data between requests
        public static TempDataDictionary TempData;
        // Context for a Razor Page, containing request information, user data, and view data
        public static PageContext PageContext;
        // The page model instance of IndexModel, representing the page logic to be tested
        public static IndexModel PageModel;

        [SetUp]
        /// <summary>
        /// Initializes the test setup by creating mock dependencies and configuring the PageModel
        /// with a default HTTP context, ModelState, TempData, and other required components.
        /// </summary>
        public void TestInitialize()
        {
            // Sets up a default HTTP context for the page model
            HttpContextDefault = new DefaultHttpContext()
            {

                //RequestServices = serviceProviderMock.Object,

            };
            // Initializes ModelState to enable validation within tests
            ModelState = new ModelStateDictionary();
            // Sets up ActionContext for Razor Page testing, linking HTTP context and route data
            ActionContext = new ActionContext(HttpContextDefault, HttpContextDefault.GetRouteData(), new PageActionDescriptor(), ModelState);
            // Initializes ModelMetadataProvider for providing metadata to ViewData
            ModelMetadataProvider = new EmptyModelMetadataProvider();
            // Configures ViewData with the ModelMetadataProvider and ModelState
            TestsViewData = new ViewDataDictionary(ModelMetadataProvider, ModelState);
            // Configures TempData, used for temporary data storage in Razor Pages
            TempData = new TempDataDictionary(HttpContextDefault, Mock.Of<ITempDataProvider>());
            // Initializes PageContext with the ActionContext and ViewData for the page model
            PageContext = new PageContext(ActionContext)
            {

                ViewData = TestsViewData,

            };
            // Mocks the hosting environment (e.g., setting root paths for testing)
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            mockWebHostEnvironment.Setup(m => m.EnvironmentName).Returns("Hosting:UnitTestEnvironment");
            mockWebHostEnvironment.Setup(m => m.WebRootPath).Returns("../../../../src/bin/Debug/net7.0/wwwroot");
            mockWebHostEnvironment.Setup(m => m.ContentRootPath).Returns("./data/");
            // Mocks the logger to prevent actual logging in test environment
            var mockLoggerDirect = Mock.Of<ILogger<IndexModel>>(); 
            JsonFileProductService productService;
            // Creates an instance of JsonFileProductService with mocked environment settings
            productService = new JsonFileProductService(mockWebHostEnvironment.Object);
            // Initializes the IndexModel page model with the ProductService and other mock dependencies
            PageModel = new IndexModel(productService) 
            {

            };

        }

        #endregion TestSetup

        #region OnGet
        /// <summary>
        /// Verifies that OnGet loads the Index page with a valid ModelState and a populated list of products.
        /// Confirms that the ModelState is valid and that Products are correctly loaded.
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Return_Products()
        {

            // Arrange
            var sortBy = "title";
            var sortOrder = "asc";
            // Act
            // Calls the OnGet method to test page load functionality
            PageModel.OnGet(sortBy, sortOrder);

            // Assert
            // Ensures ModelState is valid after OnGet
            Assert.That(PageModel.ModelState.IsValid, Is.EqualTo(true), "Index page should return a valid state");
            // Verifies that products are loaded and non-empty.
            Assert.That(PageModel.Products.ToList().Any(), Is.EqualTo(true), "Index page's list of products should exist");

        }
        #endregion OnGet

    }

}