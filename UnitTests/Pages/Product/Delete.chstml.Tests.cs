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

namespace UnitTests.Pages.Product
{

    class DeleteTests
    {

        #region TestSetup
        public static DefaultHttpContext HttpContextDefault;

        public static ModelStateDictionary ModelState;

        public static ActionContext ActionContext;

        public static EmptyModelMetadataProvider ModelMetadataProvider;

        public static ViewDataDictionary TestsViewData;

        public static TempDataDictionary TempData;

        public static PageContext PageContext;

        public static DeleteModel PageModel;

        [SetUp]
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

            PageModel = new DeleteModel(productService)
            {

                PageContext = PageContext,
                TempData = TempData,

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
            PageModel.OnGet(data.Id);

            // Assert
            Assert.That(PageModel.ModelState.IsValid, Is.EqualTo(true));
            Assert.That(PageModel.ProductId, Is.EqualTo(data.Id));
            Assert.That(PageModel.ProductId, Is.Not.Null);  //Ensures that the product is loaded and not null.
        }
        #endregion OnGet

        #region OnPost

        [Test]
        public void OnPost_Invalid_Id_Should_Return_Invalid_State()
        {
            // Arrange
            PageModel.OnGet(null);
            PageModel.ModelState.AddModelError("ProductId", "Id does not exist");

            // Act
            var result = PageModel.OnPost(null);

            // Assert
            Assert.That(PageModel.ModelState.IsValid, Is.False, "PageModel should have invalid state when error exists.");
            Assert.That(result, Is.InstanceOf<PageResult>(), "Should return PageResult in event of an error.");
            Assert.That(PageModel.ProductId, Is.Null, "PageModel Product Id should be null.");
        }


        [Test]
        public void OnPost_Valid_Id_Should_Return_Valid_Id()
        {
            // Arrange
            var data = TestHelper.ProductService.CreateData();
            PageModel.OnGet(data.Id);

            // Act
            var result = PageModel.OnPost(data.Id);

            // Assert
            Assert.That(PageModel.ModelState.IsValid, Is.True, "Valid input should return valid ModelState.");
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>(), "Valid input should return a RedirectToPageResult, as the page is redirecting to a new page.");
        }
        #endregion OnPost

    }

}