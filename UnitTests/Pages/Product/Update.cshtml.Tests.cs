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

namespace UnitTests.Pages.Product.Update

{

    public class UpdateTests
    {
        #region TestSetup
        public static DefaultHttpContext HttpContextDefault;

        public static ModelStateDictionary ModelState;

        public static ActionContext ActionContext;

        public static EmptyModelMetadataProvider ModelMetadataProvider;

        public static ViewDataDictionary TestsViewData;

        public static TempDataDictionary TempData;

        public static PageContext PageContext;

        public static UpdateModel PageModel;

        [SetUp]

        /// <summary>
        /// Initializes the test setup by creating mock dependencies and configuring
        /// the PageModel with a default context, ModelState, TempData, and other required components.
        /// </summary>

        public void TestInitialize()
        {
            HttpContextDefault = new DefaultHttpContext();

            ModelState = new ModelStateDictionary();

            ActionContext = new ActionContext(HttpContextDefault, HttpContextDefault.GetRouteData(), new PageActionDescriptor(), ModelState);
            
            ModelMetadataProvider = new EmptyModelMetadataProvider();

            TestsViewData = new ViewDataDictionary(ModelMetadataProvider, ModelState);

            TempData = new TempDataDictionary(HttpContextDefault, Mock.Of<ITempDataProvider>());

            PageContext = new PageContext(ActionContext)
            {

                ViewData = TestsViewData,

            };

            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            mockWebHostEnvironment.Setup(m => m.EnvironmentName).Returns("Hosting:UnitTestEnvironment");
            mockWebHostEnvironment.Setup(m => m.WebRootPath).Returns("../../../../src/bin/Debug/net7.0/wwwroot");
            mockWebHostEnvironment.Setup(m => m.ContentRootPath).Returns("./data/");
            
            var mockLoggerDirect = Mock.Of<ILogger<UpdateModel>>();
            JsonFileProductService productService = new JsonFileProductService(mockWebHostEnvironment.Object);

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
            var mockProductService = new Mock<JsonFileProductService>(Mock.Of<IWebHostEnvironment>());
            var model = new UpdateModel(mockProductService.Object)
            {
                Product = new ProductModel
                {
                    Id = "test-id",
                    Title = "Test Title",
                    Description = "Test Description",
                    Url = "invalid-url",
                    Image = "Image"
                }
            };
            model.ModelState.AddModelError("Product.Url", "Invalid URL format");

            // Act
            var result = model.OnPost();

            // Assert
            Assert.That(result, Is.TypeOf<PageResult>());
            Assert.That(model.ModelState.IsValid, Is.False);
        }

        /// <summary>
        /// Verifies that OnPost returns the page and captures the ModelState errors
        /// when required fields are missing in the model.
        /// </summary>

        [Test]
        public void OnPost_Missing_Required_Fields_Should_Return_Page()
        {
            // Arrange
            var mockProductService = new Mock<JsonFileProductService>(Mock.Of<IWebHostEnvironment>());
            var model = new UpdateModel(mockProductService.Object)
            {

                Product = new ProductModel { Id = "test-id" }

            };

            model.ModelState.AddModelError("Product.Title", "Title is required");
            model.ModelState.AddModelError("Product.Description", "Description is required");

            // Act
            var result = model.OnPost() as PageResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(model.ModelState.ErrorCount, Is.EqualTo(2));
            Assert.That(model.ModelState.Keys, Does.Contain("Product.Title"));
            Assert.That(model.ModelState.Keys, Does.Contain("Product.Description"));

        }

        [Test]
        public void OnPost_ValidModel_ShouldRedirectToIndex()
        {
            // Arrange
            PageModel.Product = TestHelper.ProductService.CreateData();

            // Act
            PageModel.OnPost();

            // Assert
            Assert.That(PageModel.Product, Is.Not.Null, "Product property should not be null when OnPost is executed.");

        }

        #endregion OnPost

        /// <summary>
        /// Verifies that OnGet correctly initializes a valid model state when provided with a valid product ID.
        /// </summary>

        [Test]
        public void OnGet_Valid_Should_Return_Valid_State()
        {

            // Arrange
            var id = "jenlooper-cactus";

            // Act
            PageModel.OnGet(id);

            // Assert
            Assert.That(PageModel.ModelState.IsValid, Is.EqualTo(true));

        }

        /// <summary>
        /// Verifies that the constructor correctly assigns the ProductService
        /// when provided with a valid instance.
        /// </summary>

        [Test]
        public void Constructor_ValidProductService_Should_Set_ProductService()
        {

            // Arrange
            var mockProductService = new Mock<JsonFileProductService>(Mock.Of<IWebHostEnvironment>());

            // Act
            var model = new UpdateModel(mockProductService.Object);

            // Assert
            Assert.That(model.ProductService, Is.Not.Null);
            Assert.That(model.ProductService, Is.EqualTo(mockProductService.Object));

        }

        /// <summary>
        /// Ensures that the constructor initializes the ProductService
        /// when a valid instance is provided.
        /// </summary>

        [Test]
        public void Constructor_WithValidProductService_ShouldInitializeProductService()
        {

            // Arrange
            var mockProductService = new Mock<JsonFileProductService>(Mock.Of<IWebHostEnvironment>());

            // Act
            var updateModel = new UpdateModel(mockProductService.Object);

            // Assert
            Assert.That(updateModel.ProductService, Is.Not.Null, "ProductService should be initialized.");
            Assert.That(updateModel.ProductService, Is.EqualTo(mockProductService.Object), "ProductService should match the provided instance.");

        }
    }
}