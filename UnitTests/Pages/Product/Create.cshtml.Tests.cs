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
        public static IUrlHelperFactory urlHelperFactory;
        public static DefaultHttpContext httpContextDefault;
        public static IWebHostEnvironment webHostEnvironment;
        public static ModelStateDictionary modelState;
        public static ActionContext actionContext;
        public static EmptyModelMetadataProvider modelMetadataProvider;
        public static ViewDataDictionary viewData;
        public static TempDataDictionary tempData;
        public static PageContext pageContext;

        public static CreateModel pageModel;

        [SetUp]
        public void TestInitialize()
        {
            httpContextDefault = new DefaultHttpContext()
            {
                //RequestServices = serviceProviderMock.Object,
            };

            modelState = new ModelStateDictionary();

            actionContext = new ActionContext(httpContextDefault, httpContextDefault.GetRouteData(), new PageActionDescriptor(), modelState);

            modelMetadataProvider = new EmptyModelMetadataProvider();
            viewData = new ViewDataDictionary(modelMetadataProvider, modelState);
            tempData = new TempDataDictionary(httpContextDefault, Mock.Of<ITempDataProvider>());

            pageContext = new PageContext(actionContext)
            {
                ViewData = viewData,
            };

            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            mockWebHostEnvironment.Setup(m => m.EnvironmentName).Returns("Hosting:UnitTestEnvironment");
            mockWebHostEnvironment.Setup(m => m.WebRootPath).Returns("../../../../src/bin/Debug/net7.0/wwwroot");
            mockWebHostEnvironment.Setup(m => m.ContentRootPath).Returns("./data/");

            var MockLoggerDirect = Mock.Of<ILogger<CreateModel>>();
            JsonFileProductService productService;

            productService = new JsonFileProductService(mockWebHostEnvironment.Object);

            pageModel = new CreateModel(productService)
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
            pageModel.OnGet();

            // Assert
            Assert.That(pageModel.ModelState.IsValid, Is.EqualTo(true));
        }
        [Test]
        public void OnGet_Should_Initialize_Product_Property()
        {
            // Arrange
            var expectedData = TestHelper.ProductService.CreateData();

            // Act
            pageModel.OnGet();

            // Assert
            Assert.That(pageModel.Product, Is.Not.Null);
        }
        #endregion OnGet

        #region onPost
        [Test]
        public void OnPost_InvalidModelState_Should_Return_PageResult()
        {
            // Arrange
            pageModel.ModelState.AddModelError("Product", "Invalid data"); 

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.That(result, Is.TypeOf<PageResult>(), "Expected PageResult when ModelState is invalid.");
        }

        [Test]
        public void OnPost_Should_Have_Product_Not_Null()
        {
            // Arrange
            pageModel.Product = TestHelper.ProductService.CreateData();

            // Act
            pageModel.OnPost();

            // Assert
            Assert.That(pageModel.Product, Is.Not.Null, "Product property should not be null when OnPost is executed.");
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
