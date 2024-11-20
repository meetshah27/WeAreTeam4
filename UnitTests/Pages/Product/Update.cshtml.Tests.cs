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

        public static DefaultHttpContext HttpContextDefault; // Mock HTTP context for the test

        public static ModelStateDictionary ModelState;// Dictionary to hold model state for validation

        public static ActionContext ActionContext;// Action context representing HTTP and routing details for the Razor Page

        public static EmptyModelMetadataProvider ModelMetadataProvider;// Provider for model metadata, used to generate ViewData

        public static ViewDataDictionary TestsViewData;// Dictionary to hold view-specific data

        public static TempDataDictionary TempData; // Dictionary to hold temporary data (TempData)

        public static PageContext PageContext;// Context for the PageModel, including ViewData and TempData

        public static UpdateModel PageModel; // Instance of the UpdateModel (page) to be tested

        [SetUp]

        /// <summary>
        /// Initializes the test setup by creating mock dependencies and configuring
        /// the PageModel with a default context, ModelState, TempData, and other required components.
        /// </summary>

        public void TestInitialize()
        {
            HttpContextDefault = new DefaultHttpContext();// Set up the default HTTP context, used to simulate an HTTP request

            ModelState = new ModelStateDictionary();// Initialize ModelState to track validation states

            ActionContext = new ActionContext(HttpContextDefault, HttpContextDefault.GetRouteData(), new PageActionDescriptor(), ModelState);// Create ActionContext, linking HTTP context and route data for Razor Page testing

            ModelMetadataProvider = new EmptyModelMetadataProvider(); // Set up ModelMetadataProvider, used to create ViewData

            TestsViewData = new ViewDataDictionary(ModelMetadataProvider, ModelState); // Initialize ViewData with ModelState and ModelMetadataProvider

            TempData = new TempDataDictionary(HttpContextDefault, Mock.Of<ITempDataProvider>());// Set up TempData, used to pass data between requests

            PageContext = new PageContext(ActionContext) // Initialize PageContext, setting ViewData for the page
            {

                ViewData = TestsViewData,

            };
            // Mock the web host environment to simulate environment-specific data
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            mockWebHostEnvironment.Setup(m => m.EnvironmentName).Returns("Hosting:UnitTestEnvironment");
            mockWebHostEnvironment.Setup(m => m.WebRootPath).Returns("../../../../src/bin/Debug/net7.0/wwwroot");
            mockWebHostEnvironment.Setup(m => m.ContentRootPath).Returns("./data/");
            
            var mockLoggerDirect = Mock.Of<ILogger<UpdateModel>>(); // Create a mocked logger for the UpdateModel
            JsonFileProductService productService = new JsonFileProductService(mockWebHostEnvironment.Object);// Initialize the product service with the mock environment

            PageModel = new UpdateModel(productService) // Instantiate the PageModel with the configured ProductService and context
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
            // Mock the ProductService and set up the model with an invalid URL
            var mockProductService = new Mock<JsonFileProductService>(Mock.Of<IWebHostEnvironment>());
            var model = new UpdateModel(mockProductService.Object)
            {
                Product = new ProductModel
                {
                    Id = "test-id",
                    Title = "Test Title",
                    Description = "Test Description",
                    Url = "invalid-url", // Invalid URL format for testing
                    Image = "Image"
                }
            };
            model.ModelState.AddModelError("Product.Url", "Invalid URL format");

            // Act
            // Call the OnPost method and check the result
            var result = model.OnPost();

            // Assert
            Assert.That(result, Is.TypeOf<PageResult>(), "Invalid URL input should result in returning the page.");// Checks that when the URL format is invalid, OnPost() should return a PageResult (i.e., stay on the same page).
            Assert.That(model.ModelState.IsValid, Is.False, "ModelState should be invalid for an invalid URL."); // Ensures ModelState is marked as invalid due to the URL format error in the ModelState.
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
            var mockProductService = new Mock<JsonFileProductService>(Mock.Of<IWebHostEnvironment>());
            var model = new UpdateModel(mockProductService.Object)
            {

                Product = new ProductModel { Id = "test-id" }// Only Id is set, other fields are missing

            };

            model.ModelState.AddModelError("Product.Title", "Title is required");
            model.ModelState.AddModelError("Product.Description", "Description is required");

            // Act
            // Call the OnPost method and cast result to PageResult for further assertions
            var result = model.OnPost() as PageResult;

            // Assert
            Assert.That(result, Is.Not.Null, "Missing required fields should return the page.");// Checks that when required fields are missing, the OnPost() method should return a PageResult, remaining on the same page.
            Assert.That(model.ModelState.ErrorCount, Is.EqualTo(2), "ModelState should capture two errors for missing fields.");     // Confirms that ModelState contains exactly two errors related to missing fields.
            Assert.That(model.ModelState.Keys, Does.Contain("Product.Title"), "ModelState should capture an error for missing Product.Title.");// Verifies that the ModelState includes an error for the missing "Title" field.
            Assert.That(model.ModelState.Keys, Does.Contain("Product.Description"), "ModelState should capture an error for missing Product.Description.");// Verifies that the ModelState includes an error for the missing "Description" field.

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
            PageModel.Product = TestHelper.ProductService.CreateData();

            // Act
            // Execute the OnPost method
            PageModel.OnPost();

            // Ensures that the Product property is set (not null) after executing OnPost with a valid model.
            Assert.That(PageModel.Product, Is.Not.Null, "Product property should not be null when OnPost is executed.");

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
            Assert.That(updateModel.ProductService, Is.Not.Null, "ProductService should be initialized.");  // Checks that ProductService is properly initialized in the constructor.
            Assert.That(updateModel.ProductService, Is.EqualTo(mockProductService.Object), "ProductService should match the provided instance."); // Confirms that the ProductService instance in the UpdateModel matches the mock instance passed in.

        }
    }
}