﻿using Microsoft.AspNetCore.Http;

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

        public static CreateModel PageModel;


        [SetUp]
        /// <summary>
        /// Initializes the setup for tests, creating mock dependencies and configuring the CreateModel
        /// with necessary contexts such as HTTP context, ModelState, TempData, and PageContext.
        /// </summary>
        public void TestInitialize()
        {

            HttpContextDefault = new DefaultHttpContext() // Sets up an HTTP context for the page model
            {

                //RequestServices = serviceProviderMock.Object,

            };
           
            ModelState = new ModelStateDictionary();// Initializes ModelState for model validation
                                                    
            ActionContext = new ActionContext(HttpContextDefault, HttpContextDefault.GetRouteData(), new PageActionDescriptor(), ModelState);// Sets up the ActionContext for the PageContext (used in Razor Pages)
           
            ModelMetadataProvider = new EmptyModelMetadataProvider(); // Configures ModelMetadataProvider for view data

            TestsViewData = new ViewDataDictionary(ModelMetadataProvider, ModelState);// Sets up ViewData with the ModelMetadataProvider and ModelState

            TempData = new TempDataDictionary(HttpContextDefault, Mock.Of<ITempDataProvider>());  // Configures TempData (temporary data storage for views)

            PageContext = new PageContext(ActionContext) // Initializes PageContext for the page model
            {

                ViewData = TestsViewData,

            };
            // Mocks the web hosting environment (e.g., setting root paths for testing)
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            mockWebHostEnvironment.Setup(m => m.EnvironmentName).Returns("Hosting:UnitTestEnvironment");
            mockWebHostEnvironment.Setup(m => m.WebRootPath).Returns("../../../../src/bin/Debug/net7.0/wwwroot");
            mockWebHostEnvironment.Setup(m => m.ContentRootPath).Returns("./data/");

            var mockLoggerDirect = Mock.Of<ILogger<CreateModel>>();// Mocks the logger to avoid real logging during testing
            JsonFileProductService productService;

            productService = new JsonFileProductService(mockWebHostEnvironment.Object);// Creates an instance of the ProductService with the mocked environment

            PageModel = new CreateModel(productService)// Initializes the CreateModel page model with the mocked ProductService
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
            var data = TestHelper.ProductService.CreateData();// Setup any necessary data

            // Act
            PageModel.OnGet(); // Calls the OnGet method to test its behavior

            // Assert
            Assert.That(PageModel.ModelState.IsValid, Is.EqualTo(true), "Valid input should return valid ModelState");// Verifies that ModelState is valid after OnGet is called

        }
        /// <summary>
        /// Verifies that the OnGet method initializes the Product property.
        /// Ensures that Product is not null after calling OnGet.
        /// </summary>
        [Test]
        public void OnGet_Should_Initialize_Product_Property()
        {

            // Arrange
            var expectedData = TestHelper.ProductService.CreateData();// Expected data for validation

            // Act
            PageModel.OnGet();// Calls the OnGet method to ensure product property is initialized

            // Assert
            Assert.That(PageModel.Product, Is.Not.Null, "Valid input should result in a page with a non-null product");// Checks that the Product property is not null after OnGet is executed

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
            PageModel.ModelState.AddModelError("Product", "Invalid data");  // Introduces a model state error to simulate invalid input

            // Act
            var result = PageModel.OnPost();// Calls the OnPost method to check the behavior with invalid model state

            // Assert
            Assert.That(result, Is.TypeOf<PageResult>(), "Expected PageResult when ModelState is invalid."); // Ensures that OnPost returns a PageResult when ModelState is invalid

        }
        /// <summary>
        /// Verifies that OnPost maintains a non-null Product property.
        /// Confirms that Product is not null after executing OnPost.
        /// </summary>
        [Test]
        public void OnPost_Should_Have_Product_Not_Null()
        {

            // Arrange
            PageModel.Product = TestHelper.ProductService.CreateData(); // Sets a product object for the test

            // Act
            PageModel.OnPost();// Calls the OnPost method to check if product is retained

            // Assert
            Assert.That(PageModel.Product, Is.Not.Null, "Product property should not be null when OnPost is executed.");// Confirms that the Product property is not null after OnPost

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
            JsonFileProductService productService = null; // Null ProductService to test handling in constructor

            // Act
            var pageModel = new CreateModel(productService);// Creates a page model instance with a null ProductService

            // Assert
            Assert.That(pageModel.ProductService, Is.Null, "ProductService should be null when passed as null to the constructor.");// Checks that ProductService is null when passed null in constructor

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
            var expectedData = TestHelper.ProductService.CreateData();// Expected data for validation
            PageModel.OnGet(); // Calls the OnGet method to ensure product property is initialized

            // Act
            PageModel.OnPostCancel(); // Calls the OnPostCancel method to simulate cancelling the creation process

            // Assert
            Assert.That(TestHelper.ProductService.GetAllData().Any(m => m.Id == PageModel.Product.Id), Is.EqualTo(false), "Product should not exist after OnGetCancel() has been called");// Checks that product is properly deleted after OnPostCancel() is called

        }
        #endregion OnPostCancel

    }

}