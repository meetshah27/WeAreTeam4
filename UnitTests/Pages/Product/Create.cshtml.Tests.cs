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
using System;


namespace UnitTests.Pages.Product.Create
{
    public class CreateTests
    {
        #region TestSetup
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
        public void TestInitialize()
        {
            HttpContextDefault = new DefaultHttpContext()
            {
                //RequestServices = serviceProviderMock.Object,
            };

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

            var mockLoggerDirect = Mock.Of<ILogger<CreateModel>>();
            JsonFileProductService productService;

            productService = new JsonFileProductService(mockWebHostEnvironment.Object);

            PageModel = new CreateModel(productService)
            {
            };
        }

        #endregion TestSetup

        #region OnGet
        [Test]
        public void OnGet_Valid_Should_Return_Valid_State()
        {
            // Arrange
            var data = TestHelper.ProductService.CreateData();

            // Act
            PageModel.OnGet();

            // Assert
            Assert.That(PageModel.ModelState.IsValid, Is.EqualTo(true));
        }
        [Test]
        public void OnGet_Should_Initialize_Product_Property()
        {
            // Arrange
            var expectedData = TestHelper.ProductService.CreateData();

            // Act
            PageModel.OnGet();

            // Assert
            Assert.That(PageModel.Product, Is.Not.Null);
        }
        #endregion OnGet

        #region onPost
        [Test]
        public void OnPost_InvalidModelState_Should_Return_PageResult()
        {
            // Arrange
            PageModel.ModelState.AddModelError("Product", "Invalid data"); 

            // Act
            var result = PageModel.OnPost();

            // Assert
            Assert.That(result, Is.TypeOf<PageResult>(), "Expected PageResult when ModelState is invalid.");
        }

        [Test]
        public void OnPost_Should_Have_Product_Not_Null()
        {
            // Arrange
            PageModel.Product = TestHelper.ProductService.CreateData();

            // Act
            PageModel.OnPost();

            // Assert
            Assert.That(PageModel.Product, Is.Not.Null, "Product property should not be null when OnPost is executed.");
        }
        #endregion onPost

        #region CreateModel
        [Test]
        public void Constructor_WithNullProductService_Should_NotThrowException()
        {
            // Arrange
            JsonFileProductService productService = null;

            // Act
            var pageModel = new CreateModel(productService);

            // Assert
            Assert.That(pageModel.ProductService, Is.Null, "ProductService should be null when passed as null to the constructor.");
        }
        #endregion CreateModel
    }
}
