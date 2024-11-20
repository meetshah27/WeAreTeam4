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
        // Declares necessary properties for testing
        public static IUrlHelperFactory UrlHelperFactory;

        public static DefaultHttpContext HttpContextDefault;

        public static IWebHostEnvironment WebHostEnvironment;

        public static ModelStateDictionary ModelState;

        public static ActionContext ActionContext;

        public static EmptyModelMetadataProvider ModelMetadataProvider;

        public static ViewDataDictionary TestsViewData;

        public static TempDataDictionary TempData;

        public static PageContext PageContext;

        public static IndexModel PageModel;

        [SetUp]
        /// <summary>
        /// Initializes the test setup by creating mock dependencies and configuring the PageModel
        /// with a default HTTP context, ModelState, TempData, and other required components.
        /// </summary>
        public void TestInitialize()
        {

            HttpContextDefault = new DefaultHttpContext()// Sets up a default HTTP context for the page model
            {

                //RequestServices = serviceProviderMock.Object,

            };

            ModelState = new ModelStateDictionary();// Initializes ModelState to enable validation within tests

            ActionContext = new ActionContext(HttpContextDefault, HttpContextDefault.GetRouteData(), new PageActionDescriptor(), ModelState);// Sets up ActionContext for Razor Page testing, linking HTTP context and route data

            ModelMetadataProvider = new EmptyModelMetadataProvider();// Initializes ModelMetadataProvider for providing metadata to ViewData

            TestsViewData = new ViewDataDictionary(ModelMetadataProvider, ModelState);// Configures ViewData with the ModelMetadataProvider and ModelState

            TempData = new TempDataDictionary(HttpContextDefault, Mock.Of<ITempDataProvider>());// Configures TempData, used for temporary data storage in Razor Pages

            PageContext = new PageContext(ActionContext)// Initializes PageContext with the ActionContext and ViewData for the page model
            {

                ViewData = TestsViewData,

            };
            // Mocks the hosting environment (e.g., setting root paths for testing)
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            mockWebHostEnvironment.Setup(m => m.EnvironmentName).Returns("Hosting:UnitTestEnvironment");
            mockWebHostEnvironment.Setup(m => m.WebRootPath).Returns("../../../../src/bin/Debug/net7.0/wwwroot");
            mockWebHostEnvironment.Setup(m => m.ContentRootPath).Returns("./data/");

            var mockLoggerDirect = Mock.Of<ILogger<IndexModel>>(); // Mocks the logger to prevent actual logging in test environment
            JsonFileProductService productService;

            productService = new JsonFileProductService(mockWebHostEnvironment.Object);// Creates an instance of JsonFileProductService with mocked environment settings

            PageModel = new IndexModel(productService) // Initializes the IndexModel page model with the ProductService and other mock dependencies
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
            // (No specific arrangement needed as we are testing a general page load)

            // Act
            PageModel.OnGet();// Calls the OnGet method to test page load functionality

            // Assert
            Assert.That(PageModel.ModelState.IsValid, Is.EqualTo(true), "Index page should return a valid state");// Ensures ModelState is valid after OnGet
            Assert.That(PageModel.Products.ToList().Any(), Is.EqualTo(true), "Index page's list of products should exist");// Verifies that products are loaded and non-empty

        }
        #endregion OnGet

    }

}