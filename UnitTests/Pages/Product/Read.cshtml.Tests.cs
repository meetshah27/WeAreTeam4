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

using ContosoCrafts.WebSite.Services;
using ContosoCrafts.WebSite.Pages.Product;

namespace UnitTests.Pages.Product.Read
{
    /// <summary>
    /// Unit test class for the ReadModel page in the Product namespace.
    /// This class contains test methods for verifying the behavior of the OnGet method in ReadModel.
    /// </summary>
    public class ReadTests
    {

        #region TestSetup
   
        // Required dependencies and services for setting up the test environment.
        public static IUrlHelperFactory UrlHelperFactory;

        // Simulates HTTP requests and responses for testing purposes.
        public static DefaultHttpContext HttpContextDefault;

        // Mocked web hosting environment providing simulated environment details.
        public static IWebHostEnvironment WebHostEnvironment;

        // Tracks the state and validation errors of the model in Razor Pages.
        public static ModelStateDictionary ModelState;

        // Encapsulates HTTP request, route data, and model state for executing Razor Pages.
        public static ActionContext ActionContext;

        // Provides metadata about models in Razor Pages during testing.
        public static EmptyModelMetadataProvider ModelMetadataProvider;

        // Holds view data, simulating the ViewData functionality in Razor Pages.
        public static ViewDataDictionary TestsViewData;

        // Temporary data storage for passing data between requests in Razor Pages.
        public static TempDataDictionary TempData;

        // Represents the context of a Razor Page, including action context and view data.
        public static PageContext PageContext;

        // Instance of the ReadModel page being tested, initialized with necessary dependencies
        public static ReadModel PageModel;

        [SetUp]
        public void TestInitialize()
        {
            // Initializing HttpContext with default values
            HttpContextDefault = new DefaultHttpContext()
            {

                //RequestServices = serviceProviderMock.Object,

            };

            // Setting up ModelState and ActionContext to capture state and context for tests
            ModelState = new ModelStateDictionary();

            // Configures the action context using the default HTTP context, route data, and page action descriptor
            ActionContext = new ActionContext(HttpContextDefault, HttpContextDefault.GetRouteData(), new PageActionDescriptor(), ModelState);

            // Sets up metadata provider for the view
            ModelMetadataProvider = new EmptyModelMetadataProvider();

            // Configures view data with model metadata and model state
            TestsViewData = new ViewDataDictionary(ModelMetadataProvider, ModelState);

            // Configures TempData dictionary for temporary data storage
            TempData = new TempDataDictionary(HttpContextDefault, Mock.Of<ITempDataProvider>());

            // Configures PageContext using action context and view data
            PageContext = new PageContext(ActionContext)
            {

                ViewData = TestsViewData,

            };
            // Mocks IWebHostEnvironment for testing environment setup without real file dependency
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            // Sets the mock environment name
            mockWebHostEnvironment.Setup(m => m.EnvironmentName).Returns("Hosting:UnitTestEnvironment");
            // Sets a mock web root path
            mockWebHostEnvironment.Setup(m => m.WebRootPath).Returns("../../../../src/bin/Debug/net7.0/wwwroot");
            // Sets a mock content root path
            mockWebHostEnvironment.Setup(m => m.ContentRootPath).Returns("./data/");

            var mockLoggerDirect = Mock.Of<ILogger<ReadModel>>();
            JsonFileProductService productService;

            // Initializes the JsonFileProductService with the mocked web host environment
            productService = new JsonFileProductService(mockWebHostEnvironment.Object);

            // Initializes the PageModel (ReadModel) with the product service
            PageModel = new ReadModel(productService)
            {

            };
        }
        #endregion TestSetup
        /// <summary>
        /// Test to verify that the OnGet method returns a valid model state when a valid product ID is provided.
        /// </summary>
        #region OnGet
        [Test]
        public void OnGet_Valid_Should_Return_Valid_State()
        {

            //Assert: creates mock product data for testing
            var data = TestHelper.ProductService.CreateData();

            // Act: calls the OnGet method with the product ID to load data
            PageModel.OnGet(data.Id);

            // Assert: checks if the model state is valid, indicating data was successfully loaded
            Assert.That(PageModel.ModelState.IsValid, Is.EqualTo(true), "Read Page should return a valid state");

        }
        /// <summary>
        /// Test to verify that the OnGet method redirects to the Index page when a non-existent product ID is provided.
        /// </summary>
        [Test]
        public void OnGet_Product_Null_Should_Redirect_To_Index()
        {
            // Arrange: provides an ID that doesn't match any product
            var invalidId = "nonexistent-id"; // ID that doesn't match any product

            // Act: calls the OnGet method with an invalid product ID
            var result = PageModel.OnGet(invalidId);

            // Assert: verifies that the result is a redirect to Index if the product ID is invalid
            Assert.That(result, Is.TypeOf<RedirectToPageResult>(), "Should redirect to the Index page");

            // Further Assert: confirms that the redirect target page is Index
            var redirectResult = result as RedirectToPageResult;

            // Verifies that the target page of the redirect result is './Index',
            Assert.That(redirectResult.PageName, Is.EqualTo("./Index"), "Redirect page should be './Index'");
        }
        /// <summary>
        /// Test to verify that the OnGet method successfully returns the page when a valid product ID is provided.
        /// </summary>
        [Test]
        public void OnGet_Valid_Product_Should_Return_Page()
        {
            // Arrange: provides a valid product ID for testing
            string Id = "sailorhg-corsage"; // Ensure the product has a valid title

            // Act: calls the OnGet method with a valid product ID
            var result = PageModel.OnGet(Id);

            // Assert: checks if OnGet() returns a PageResult, confirming successful page load
            Assert.That(result, Is.TypeOf<PageResult>(), "Should return the page when the product and title are valid");
        }

        #endregion OnGet

    }

}