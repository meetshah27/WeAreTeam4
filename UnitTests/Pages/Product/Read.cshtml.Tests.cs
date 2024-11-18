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

namespace UnitTests.Pages.Product.Read
{

    public class ReadTests
    {

        #region TestSetup
        // Required dependencies and services for setting up the test environment.
        public static IUrlHelperFactory UrlHelperFactory;

        public static DefaultHttpContext HttpContextDefault;

        public static IWebHostEnvironment WebHostEnvironment;

        public static ModelStateDictionary ModelState;

        public static ActionContext ActionContext;

        public static EmptyModelMetadataProvider ModelMetadataProvider;

        public static ViewDataDictionary TestsViewData;

        public static TempDataDictionary TempData;

        public static PageContext PageContext;

        public static ReadModel PageModel;

        [SetUp]
        public void TestInitialize()
        {

            HttpContextDefault = new DefaultHttpContext()// Initializing HttpContext with default values
            {

                //RequestServices = serviceProviderMock.Object,

            };

            // Setting up ModelState and ActionContext to capture state and context for tests
            ModelState = new ModelStateDictionary();

            ActionContext = new ActionContext(HttpContextDefault, HttpContextDefault.GetRouteData(), new PageActionDescriptor(), ModelState);// Configures the action context using the default HTTP context, route data, and page action descriptor

            ModelMetadataProvider = new EmptyModelMetadataProvider();// Sets up metadata provider for the view

            TestsViewData = new ViewDataDictionary(ModelMetadataProvider, ModelState); // Configures view data with model metadata and model state

            TempData = new TempDataDictionary(HttpContextDefault, Mock.Of<ITempDataProvider>());// Configures TempData dictionary for temporary data storage

            PageContext = new PageContext(ActionContext)// Configures PageContext using action context and view data
            {

                ViewData = TestsViewData,

            };
            // Mocks IWebHostEnvironment for testing environment setup without real file dependency
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            mockWebHostEnvironment.Setup(m => m.EnvironmentName).Returns("Hosting:UnitTestEnvironment");// Sets the mock environment name
            mockWebHostEnvironment.Setup(m => m.WebRootPath).Returns("../../../../src/bin/Debug/net7.0/wwwroot");// Sets a mock web root path
            mockWebHostEnvironment.Setup(m => m.ContentRootPath).Returns("./data/");// Sets a mock content root path

            var mockLoggerDirect = Mock.Of<ILogger<ReadModel>>();
            JsonFileProductService productService;

            productService = new JsonFileProductService(mockWebHostEnvironment.Object);// Initializes the JsonFileProductService with the mocked web host environment

            PageModel = new ReadModel(productService)// Initializes the PageModel (ReadModel) with the product service
            {

            };
        }
        #endregion TestSetup

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
            Assert.That(redirectResult.PageName, Is.EqualTo("./Index"), "Redirect page should be './Index'");
        }
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