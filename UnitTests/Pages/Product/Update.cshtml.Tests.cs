﻿using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc.RazorPages;

using Microsoft.AspNetCore.Mvc.ModelBinding;

using Microsoft.AspNetCore.Mvc.ViewFeatures;

using Microsoft.AspNetCore.Hosting;

using Microsoft.Extensions.Logging;

using Moq;

using NUnit.Framework;

using ContosoCrafts.WebSite.Services;

using ContosoCrafts.WebSite.Models;

using Microsoft.AspNetCore.Routing;

namespace UnitTests.Pages.Product.Update

{

    public class UpdateTests

    {

        #region TestSetup

        public static DefaultHttpContext httpContextDefault;

        public static ModelStateDictionary modelState;

        public static ActionContext actionContext;

        public static EmptyModelMetadataProvider modelMetadataProvider;

        public static ViewDataDictionary viewData;

        public static TempDataDictionary tempData;

        public static PageContext pageContext;

        public static UpdateModel pageModel;

        [SetUp]

        public void TestInitialize()

        {

            httpContextDefault = new DefaultHttpContext();

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

            var MockLoggerDirect = Mock.Of<ILogger<UpdateModel>>();

            JsonFileProductService productService = new JsonFileProductService(mockWebHostEnvironment.Object);

            pageModel = new UpdateModel(productService)

            {

                PageContext = pageContext,

                TempData = tempData,

            };

        }

        #endregion TestSetup

      

        #region OnPost

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

        #endregion OnPost

        [Test]

        public void OnGet_Valid_Should_Return_Valid_State()

        {

            // Arrange

            var id = "jenlooper-cactus";

            // Act

            pageModel.OnGet(id);

            // Assert

            Assert.That(pageModel.ModelState.IsValid, Is.EqualTo(true));

        }
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

