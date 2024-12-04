// Provides classes for handling HTTP requests and responses.
using Microsoft.AspNetCore.Http;
// Includes functionality for MVC pattern, used for controllers and views.
using Microsoft.AspNetCore.Mvc;
// Supports Razor Pages for building page-based web applications.
using Microsoft.AspNetCore.Mvc.RazorPages;
// Provides functionality for binding model data from HTTP requests.
using Microsoft.AspNetCore.Mvc.ModelBinding;
// Contains features for working with HTML helpers, views, and view data.
using Microsoft.AspNetCore.Mvc.ViewFeatures;
// Provides hosting environment-related functionality for ASP.NET Core applications.
using Microsoft.AspNetCore.Hosting;
// Enables routing capabilities, which map URLs to controller actions or Razor Pages.
using Microsoft.AspNetCore.Routing;
// Contains classes for logging within the application.
using Microsoft.Extensions.Logging;
// A mocking library used in unit tests to simulate objects for testing purposes.
using Moq;
// Testing framework for writing and running unit tests. Used for assertions and test structure.
using NUnit.Framework;
// Includes services related to the ContosoCrafts website (likely business logic or data access).
using ContosoCrafts.WebSite.Services;
// Contains the models used in the ContosoCrafts website (e.g., data structures for products).
using ContosoCrafts.WebSite.Models;
using System.Linq;

namespace UnitTests.Pages.Product.Update

{
    /// <summary>
    /// Unit test class for the UpdateModel page in the Update namespace.
    /// Contains test methods to verify the behavior of OnGet and OnPost methods, as well as constructor validation.
    /// </summary>
    public class UpdateTests
    {
        #region TestSetup
        // Define dependencies and properties needed for setting up the PageModel and context in tests

        // Mock HTTP context for the test
        public static DefaultHttpContext HttpContextDefault;

        // Dictionary to hold model state for validation
        public static ModelStateDictionary ModelState;

        // Action context representing HTTP and routing details for the Razor Page
        public static ActionContext ActionContext;

        // Provider for model metadata, used to generate ViewData
        public static EmptyModelMetadataProvider ModelMetadataProvider;

        // Dictionary to hold view-specific data
        public static ViewDataDictionary TestsViewData;

        // Dictionary to hold temporary data (TempData)
        public static TempDataDictionary TempData;

        // Context for the PageModel, including ViewData and TempData
        public static PageContext PageContext;

        // Instance of the UpdateModel (page) to be tested
        public static UpdateModel PageModel; 

        [SetUp]

        /// <summary>
        /// Initializes the test setup by creating mock dependencies and configuring
        /// the PageModel with a default context, ModelState, TempData, and other required components.
        /// </summary>

        public void TestInitialize()
        {
            // Set up the default HTTP context, used to simulate an HTTP request
            HttpContextDefault = new DefaultHttpContext();

            // Initialize ModelState to track validation states
            ModelState = new ModelStateDictionary();

            // Create ActionContext, linking HTTP context and route data for Razor Page testing
            ActionContext = new ActionContext(HttpContextDefault, HttpContextDefault.GetRouteData(), new PageActionDescriptor(), ModelState);

            // Set up ModelMetadataProvider, used to create ViewData
            ModelMetadataProvider = new EmptyModelMetadataProvider();

            // Initialize ViewData with ModelState and ModelMetadataProvider
            TestsViewData = new ViewDataDictionary(ModelMetadataProvider, ModelState);

            // Set up TempData, used to pass data between requests
            TempData = new TempDataDictionary(HttpContextDefault, Mock.Of<ITempDataProvider>());

            // Initialize PageContext, setting ViewData for the page
            PageContext = new PageContext(ActionContext) 
            {

                ViewData = TestsViewData,

            };
            // Mock the web host environment to simulate environment-specific data
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            mockWebHostEnvironment.Setup(m => m.EnvironmentName).Returns("Hosting:UnitTestEnvironment");
            mockWebHostEnvironment.Setup(m => m.WebRootPath).Returns("../../../../src/bin/Debug/net7.0/wwwroot");
            mockWebHostEnvironment.Setup(m => m.ContentRootPath).Returns("./data/");

            // Create a mocked logger for the UpdateModel
            var mockLoggerDirect = Mock.Of<ILogger<UpdateModel>>();

            // Initialize the product service with the mock environment
            JsonFileProductService productService = new JsonFileProductService(mockWebHostEnvironment.Object);

            // Instantiate the PageModel with the configured ProductService and context
            PageModel = new UpdateModel(productService) 
            {

                PageContext = PageContext,
                TempData = TempData,

            };

        }

        #endregion TestSetup

        #region OnPost

        /// <summary>
        /// Ensures OnPost returns the page when the provided URL is invalid.
        /// Verifies that ModelState captures the error.
        /// </summary>

        [Test]
        public void OnPost_Invalid_Url_Should_Return_Page()
        {
            // Arrange
            // Set up the model with an invalid URL
            var testProduct = TestHelper.ProductService.CreateData();
            var model = new UpdateModel(TestHelper.ProductService)
            {
                Product = new ProductModel
                {
                    Id = testProduct.Id,
                    Title = "Test Title",
                    Description = "Test Description",
                    Url = "invalid-url",
                    Image = "Image",
                    ProductType = ProductTypeEnum.other
                }
            };
            model.ModelState.AddModelError("Product.Url", "Invalid URL format");

            // Act
            // Call the OnPost method and check the result
            var result = model.OnPost();

            // Assert
            // Checks that when the URL format is invalid, OnPost() should return a PageResult (i.e., stay on the same page).
            Assert.That(result, Is.TypeOf<PageResult>(), "Invalid URL input should result in returning the page.");

            // Ensures ModelState is marked as invalid due to the URL format error in the ModelState.
            Assert.That(model.ModelState.IsValid, Is.False, "ModelState should be invalid for an invalid URL."); 
        }

        /// <summary>
        /// Verifies that OnPost returns the page and captures the ModelState errors
        /// when required fields are missing in the model.
        /// </summary>

        [Test]
        public void OnPost_Missing_Required_Fields_Should_Return_Page()
        {
            // Arrange
            // Mock the ProductService and set up the model with missing required fields
            var testProduct = TestHelper.ProductService.CreateData();
            var model = new UpdateModel(TestHelper.ProductService)
            {
                // Only Id is set, other fields are missing
                Product = new ProductModel { Id = testProduct.Id }

            };
            // Simulate errors for missing required fields
            model.ModelState.AddModelError("Product.Title", "Title is required");
            model.ModelState.AddModelError("Product.Description", "Description is required");

            // Act
            // Call the OnPost method and cast result to PageResult for further assertions
            var result = model.OnPost() as PageResult;

            // Assert
            // Checks that when required fields are missing, the OnPost() method should return a PageResult, remaining on the same page.
            Assert.That(result, Is.Not.Null, "Missing required fields should return the page.");

            // Confirms that ModelState contains exactly two errors related to missing fields.
            Assert.That(model.ModelState.ErrorCount, Is.EqualTo(2), "ModelState should capture two errors for missing fields.");

            // Verifies that the ModelState includes an error for the missing "Title" field.
            Assert.That(model.ModelState.Keys, Does.Contain("Product.Title"), "ModelState should capture an error for missing Product.Title.");

            // Verifies that the ModelState includes an error for the missing "Description" field.
            Assert.That(model.ModelState.Keys, Does.Contain("Product.Description"), "ModelState should capture an error for missing Product.Description.");

        }
        /// <summary>
        /// Tests that OnPost redirects to the Index page when the model is valid.
        /// Verifies that the Product property is set and not null after a valid post.
        /// </summary>
        [Test]
        public void OnPost_ValidModel_ShouldRedirectToIndex()
        {
            // Arrange
            // Create a valid product model
            var data = TestHelper.ProductService.GetAllData().First();
            var model = new UpdateModel(TestHelper.ProductService)
            {
                Product = new ProductModel
                {
                    Id = data.Id,
                    Title = data.Title,
                    Description = data.Description,
                    Url = data.Url,
                    Image = data.Image,
                    GitHub = data.GitHub,
                    ProductType = data.ProductType,
                }
            };

            // Act
            // Execute the OnPost method
            var result = model.OnPost();

            // Ensures that the Product property is set (not null) after executing OnPost with a valid model.
            Assert.That(model.Product, Is.Not.Null, "Product property should not be null when OnPost is executed.");

            // Further Assert: confirms that the redirect target page is Index
            var redirectResult = result as RedirectToPageResult;

            Assert.That(redirectResult.PageName, Is.EqualTo("./Index"), "Redirect page should be './Index'");

        }

        /// <summary>
        /// Tests that OnPost redirects to the Index page when the model is valid.
        /// Verifies that the Product property is set and not null after a valid post.
        /// </summary>
        [Test]
        public void OnPost_InvalidID_ShouldRedirectToError()
        {
            // Arrange
            // Create a product model not extant in the main dataset
            PageModel.Product = TestHelper.ProductService.CreateData();

            // Act
            // Execute the OnPost method
            var result = PageModel.OnPost();

            //Assert: confirms that the redirect target page is Error
            var redirectResult = result as RedirectToPageResult;

            //Verifies that the target page of the redirect result is '../Error',
            Assert.That(redirectResult.PageName, Is.EqualTo("../Error"), "Redirect page should be '../Error'");

        }

        #endregion OnPost

        //// <summary>
        /// Tests that OnGet initializes a valid model state when provided with a valid product ID.
        /// Verifies that the ModelState is valid and data is correctly loaded.
        /// </summary>

        [Test]
        public void OnGet_Valid_Should_Return_Valid_State()
        {

            // Arrange
            // Use a valid product ID for testing
            var id = "jenlooper-cactus";

            // Act
            // Execute OnGet with the valid ID
            PageModel.OnGet(id);

            // Confirms that the ModelState is valid after calling OnGet with a valid product ID, ensuring the correct data was loaded.
            Assert.That(PageModel.ModelState.IsValid, Is.EqualTo(true), "OnGet should result in a valid ModelState for a valid product ID.");

        }

        /// <summary>
        /// Tests that the constructor assigns the ProductService when a valid instance is provided.
        /// Verifies that ProductService is initialized and matches the provided mock instance.
        /// </summary>

        [Test]
        public void Constructor_ValidProductService_Should_Set_ProductService()
        {

            // Arrange
            // Mock ProductService instance
            var mockProductService = new Mock<JsonFileProductService>(Mock.Of<IWebHostEnvironment>());

            // Act
            // Initialize the UpdateModel with the mocked ProductService
            var model = new UpdateModel(mockProductService.Object);

            // Assert
            Assert.That(model.ProductService, Is.Not.Null, "ProductService should be initialized and not null.");  // Ensures that ProductService is not null, meaning it was properly initialized in the constructor.
            Assert.That(model.ProductService, Is.EqualTo(mockProductService.Object), "ProductService should match the provided mock instance."); // Confirms that the ProductService instance in the model matches the mock instance that was passed to the constructor.

        }

        /// <summary>
        /// Ensures that the constructor initializes the ProductService
        /// when a valid instance is provided.
        /// </summary>

        [Test]
        public void Constructor_WithValidProductService_ShouldInitializeProductService()
        {

            // Arrange
            // Mock ProductService instance
            var mockProductService = new Mock<JsonFileProductService>(Mock.Of<IWebHostEnvironment>());

            // Act
            // Create an UpdateModel instance with the mock service
            var updateModel = new UpdateModel(mockProductService.Object);

            // Assert
            // Checks that ProductService is properly initialized in the constructor.
            Assert.That(updateModel.ProductService, Is.Not.Null, "ProductService should be initialized.");

            // Confirms that the ProductService instance in the UpdateModel matches the mock instance passed in.
            Assert.That(updateModel.ProductService, Is.EqualTo(mockProductService.Object), "ProductService should match the provided instance."); 

        }
    }
}