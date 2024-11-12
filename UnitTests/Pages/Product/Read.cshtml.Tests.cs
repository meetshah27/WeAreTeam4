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

            var mockLoggerDirect = Mock.Of<ILogger<ReadModel>>();
            JsonFileProductService productService;

            productService = new JsonFileProductService(mockWebHostEnvironment.Object);

            PageModel = new ReadModel(productService)
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
            PageModel.OnGet(data.Id);

            // Assert
            Assert.That(PageModel.ModelState.IsValid, Is.EqualTo(true), "Read Page should return a valid state");

        }
        #endregion OnGet

    }

}