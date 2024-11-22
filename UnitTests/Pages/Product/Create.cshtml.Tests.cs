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

using System.Linq;




namespace UnitTests.Pages.Product.Create
{
    /// <summary>
    /// Unit test class for the CreateModel page in the Product namespace.
    /// Contains test methods to verify the behavior of the OnGet, OnPost methods, and the constructor.
    /// </summary>
    public class CreateTests
    {

        #region TestSetup
        /// Declares necessary properties for testing
        public static IUrlHelperFactory UrlHelperFactory;

        /// Simulates HTTP requests and responses for testing purposes.
        public static DefaultHttpContext HttpContextDefault;

        /// Mocked web hosting environment providing simulated environment details, such as paths and environment names.
        public static IWebHostEnvironment WebHostEnvironment;

        /// Represents the state and validation errors of the model in Razor Pages during testing.
        public static ModelStateDictionary ModelState;

        /// Provides the context for executing Razor Page actions, including HTTP request, route data, and model state.
        public static ActionContext ActionContext;

        /// Supplies metadata about models used in Razor Pages for testing purposes.
        public static EmptyModelMetadataProvider ModelMetadataProvider;

        /// Contains view data for rendering Razor Pages, mimicking the ViewData functionality in tests.
        public static ViewDataDictionary TestsViewData;

        /// Temporary data storage for passing information between page requests in a Razor Page during tests.
        public static TempDataDictionary TempData;

        /// Represents the context of a Razor Page during execution, including action context and view data.
        public static PageContext PageContext;

        /// Instance of the CreateModel page being tested, initialized with required dependencies and services.
        public static CreateModel PageModel;


        [SetUp]
        /// <summary>
        /// Initializes the setup for tests, creating mock dependencies and configuring the CreateModel
        /// with necessary contexts such as HTTP context, ModelState, TempData, and PageContext.
        /// </summary>
        public void TestInitialize()
        {
            // Sets up an HTTP context for the page model
            HttpContextDefault = new DefaultHttpContext() 
            {

                //RequestServices = serviceProviderMock.Object,

            };

            // Initializes ModelState for model validation
            ModelState = new ModelStateDictionary();

            // Sets up the ActionContext for the PageContext (used in Razor Pages)                                        
            ActionContext = new ActionContext(HttpContextDefault, HttpContextDefault.GetRouteData(), new PageActionDescriptor(), ModelState);

            // Configures ModelMetadataProvider for view data
            ModelMetadataProvider = new EmptyModelMetadataProvider();

            // Sets up ViewData with the ModelMetadataProvider and ModelState
            TestsViewData = new ViewDataDictionary(ModelMetadataProvider, ModelState);

            // Configures TempData (temporary data storage for views)
            TempData = new TempDataDictionary(HttpContextDefault, Mock.Of<ITempDataProvider>());

            // Initializes PageContext for the page model
            PageContext = new PageContext(ActionContext) 
            {

                ViewData = TestsViewData,

            };
            // Mocks the web hosting environment (e.g., setting root paths for testing)
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            mockWebHostEnvironment.Setup(m => m.EnvironmentName).Returns("Hosting:UnitTestEnvironment");
            mockWebHostEnvironment.Setup(m => m.WebRootPath).Returns("../../../../src/bin/Debug/net7.0/wwwroot");
            mockWebHostEnvironment.Setup(m => m.ContentRootPath).Returns("./data/");

            // Mocks the logger to avoid real logging during testing
            var mockLoggerDirect = Mock.Of<ILogger<CreateModel>>();
            JsonFileProductService productService;

            // Creates an instance of the ProductService with the mocked environment
            productService = new JsonFileProductService(mockWebHostEnvironment.Object);

            // Initializes the CreateModel page model with the mocked ProductService
            PageModel = new CreateModel(productService)
            {

            };

        }

        #endregion TestSetup

        #region OnGet
        /// <summary>
        /// Verifies that OnGet initializes with a valid ModelState.
        /// Ensures that the ModelState is valid after calling OnGet.
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Return_Valid_State()
        {

            // Arrange
            // Setup any necessary data
            var data = TestHelper.ProductService.CreateData();

            // Act
            // Calls the OnGet method to test its behavior
            PageModel.OnGet();

            // Assert
            // Verifies that ModelState is valid after OnGet is called
            Assert.That(PageModel.ModelState.IsValid, Is.EqualTo(true), "Valid input should return valid ModelState");

        }
        /// <summary>
        /// Verifies that the OnGet method initializes the Product property.
        /// Ensures that Product is not null after calling OnGet.
        /// </summary>
        [Test]
        public void OnGet_Should_Initialize_Product_Property()
        {

            // Arrange
            // Expected data for validation
            var expectedData = TestHelper.ProductService.CreateData();

            // Act
            // Calls the OnGet method to ensure product property is initialized
            PageModel.OnGet();

            // Assert
            // Checks that the Product property is not null after OnGet is executed
            Assert.That(PageModel.Product, Is.Not.Null, "Valid input should result in a page with a non-null product");

        }
        #endregion OnGet

        #region onPost
        /// <summary>
        /// Verifies that OnPost with an invalid ModelState returns a PageResult.
        /// Introduces a model state error to simulate invalid input and checks for PageResult return.
        /// </summary>
        [Test]
        public void OnPost_InvalidModelState_Should_Return_PageResult()
        {

            // Arrange
            // Introduces a model state error to simulate invalid input
            PageModel.ModelState.AddModelError("Product", "Invalid data");

            // Act
            // Calls the OnPost method to check the behavior with invalid model state
            var result = PageModel.OnPost();

            // Assert
            // Ensures that OnPost returns a PageResult when ModelState is invalid
            Assert.That(result, Is.TypeOf<PageResult>(), "Expected PageResult when ModelState is invalid."); 

        }
        /// <summary>
        /// Verifies that OnPost maintains a non-null Product property.
        /// Confirms that Product is not null after executing OnPost.
        /// </summary>
        [Test]
        public void OnPost_Should_Have_Product_Not_Null()
        {

            // Arrange
            // Sets a product object for the test
            PageModel.Product = TestHelper.ProductService.CreateData();

            // Act
            // Calls the OnPost method to check if product is retained
            PageModel.OnPost();

            // Assert
            // Confirms that the Product property is not null after OnPost
            Assert.That(PageModel.Product, Is.Not.Null, "Product property should not be null when OnPost is executed.");

        }
        #endregion onPost

        #region CreateModel
        /// <summary>
        /// Tests that the constructor of CreateModel does not throw an exception when given a null ProductService.
        /// Verifies that ProductService remains null when passed as null in the constructor.
        /// </summary>
        [Test]
        public void Constructor_WithNullProductService_Should_NotThrowException()
        {

            // Arrange
            // Null ProductService to test handling in constructor
            JsonFileProductService productService = null;

            // Act
            // Creates a page model instance with a null ProductService
            var pageModel = new CreateModel(productService);

            // Assert
            // Checks that ProductService is null when passed null in constructor
            Assert.That(pageModel.ProductService, Is.Null, "ProductService should be null when passed as null to the constructor.");

        }
        #endregion CreateModel


        /// <summary>
        /// Tests that the product being created is not acually added to the overall list of products in the event that the user cancels the creation process
        /// </summary>
        #region OnPostCancel
        [Test]
        public void OnPostCancel_Should_Delete_Product_In_Progress()
        {

            // Arrange
            // Expected data for validation
            var expectedData = TestHelper.ProductService.CreateData();
            // Calls the OnGet method to ensure product property is initialized
            PageModel.OnGet(); 

            // Act
            // Calls the OnPostCancel method to simulate cancelling the creation process
            PageModel.OnPostCancel();

            // Assert
            // Checks that product is properly deleted after OnPostCancel() is called
            Assert.That(TestHelper.ProductService.GetAllData().Any(m => m.Id == PageModel.Product.Id), Is.EqualTo(false), "Product should not exist after OnGetCancel() has been called");

        }
        #endregion OnPostCancel

    }

}