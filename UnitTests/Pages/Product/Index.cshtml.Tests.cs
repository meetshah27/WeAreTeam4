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

namespace UnitTests.Pages.Product.Index
{

    public class IndexTests
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

        public static IndexModel PageModel;

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

            var mockLoggerDirect = Mock.Of<ILogger<IndexModel>>();
            JsonFileProductService productService;

            productService = new JsonFileProductService(mockWebHostEnvironment.Object);

            PageModel = new IndexModel(productService)
            {

            };

        }

        #endregion TestSetup

        #region OnGet
        [Test]
        public void OnGet_Valid_Should_Return_Products()
        {

            // Arrange

            // Act
            PageModel.OnGet();

            // Assert
            Assert.That(PageModel.ModelState.IsValid, Is.EqualTo(true));
            Assert.That(PageModel.Products.ToList().Any(), Is.EqualTo(true));

        }
        #endregion OnGet

    }

}