using System.IO;

using System.Linq;

using Microsoft.AspNetCore.Hosting;

using Microsoft.AspNetCore.Mvc;

using Moq;

using NUnit.Framework;

using ContosoCrafts.WebSite.Controllers;

using ContosoCrafts.WebSite.Services;

namespace UnitTests.Controllers
{
    /// <summary>
    /// Test Cases for the Product Controller.
    /// </summary>
    [TestFixture]
    public class ProductsControllerTests
    {

        private ProductsController _controller;

        private JsonFileProductService _productService;

        private string _testWebRootPath;
        /// <summary>
        /// Data directory to check the controller for the Tests
        /// </summary>
        [SetUp]
        public void Setup()
        {

            // Create a temporary directory to act as WebRootPath
            _testWebRootPath = Path.Combine(Path.GetTempPath(), "TestWebRoot");
            Directory.CreateDirectory(_testWebRootPath);

            // Create a data directory
            string dataDirectory = Path.Combine(_testWebRootPath, "data");
            Directory.CreateDirectory(dataDirectory);

            // Create a test products.json file with sample data
            string productsJsonPath = Path.Combine(dataDirectory, "products.json");
            File.WriteAllText(productsJsonPath, @"
            [
                {
                    ""Id"": ""jenlooper-light"",
                    ""Maker"": ""jenlooper"",
                    ""img"": ""https://about.coursera.org/static/blueCoursera-646f855eae3d677239ea9db93d6c9e17.svg"",
                    ""GitHub"": ""https://github.com/coursera"",
                    ""Url"": ""https://www.coursera.org"",
                    ""Title"": ""Coursera123"",
                    ""Description"": ""An online learning platform partnering with universities and organizations to offer courses \u0026 specializations. Users can access high-quality educational content across various fields."",
                    ""Ratings"": null,
                    ""Counter"": 0,
                    ""UrlCounter"": 0
                },
                {
                    ""Id"": ""jenlooper-lightshow"",
                    ""Maker"": ""@jenlooper"",
                    ""img"": ""https://assets.entrepreneur.com/content/3x2/2000/20180511063849-flipkart-logo-detail-icon.jpeg"",
                    ""GitHub"": ""https://github.com/flipkart-incubator"",
                    ""Url"": ""https://www.flipkart.com"",
                    ""Title"": ""Flipkart"",
                    ""Description"": ""Flipkart offers a wide range of products across categories such as electronics, home appliances, and fashion. This platform has a user-friendly interface, easy checkout process, and a variety of payment options."",
                    ""Ratings"": null,
                    ""Counter"": 0,
                    ""UrlCounter"": 0
                }

            ]");

            // Mock IWebHostEnvironment to return the test WebRootPath
            var mockEnvironment = new Mock<IWebHostEnvironment>();
            mockEnvironment.Setup(m => m.WebRootPath).Returns(_testWebRootPath);

            // Initialize JsonFileProductService with the mock environment
            _productService = new JsonFileProductService(mockEnvironment.Object);

            // Initialize the controller with the product service
            _controller = new ProductsController(_productService);

        }
        /// <summary>
        /// Clean the Directory after the test cases
        /// </summary>
        [TearDown]
        public void TearDown()
        {

            // Clean up the temporary directory after the tests
            if (Directory.Exists(_testWebRootPath))
            {

                Directory.Delete(_testWebRootPath, true);

            }

        }

        /// <summary>
        /// Product Service should be initialized with the contructor.
        /// </summary>
        [Test]
        public void Constructor_Should_Initialize_ProductService()
        {

            // Assert
            Assert.That(_controller.ProductService, Is.EqualTo(_productService), "Expected ProductService to be initialized with the provided service.");

        }

        /// <summary>
        /// Get the count and the products. Return all the products
        /// </summary>
        [Test]
        public void Get_Should_Return_All_Products()
        {

            // Act
            var result = _controller.Get();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(2), "Expected Get() to return two products.");

        }

        /// <summary>
        /// Patch method adding rating to the product and returning the value that are given
        /// </summary>
        [Test]
        public void Patch_Should_Add_Rating_To_Product_And_Return_Ok()
        {

            // Arrange
            var request = new ProductsController.RatingRequest
            {

                ProductId = "jenlooper-light",
                Rating = 4

            };

            // Act
            var result = _controller.Patch(request);

            // Assert
            Assert.That(result, Is.TypeOf<OkResult>(), "Expected Patch() to return Ok result.");

            // Verify that the rating was added
            var products = _productService.GetAllData();
            var product = products.FirstOrDefault(p => p.Id == "jenlooper-light");

            // Check if the product with Id "1" exists
            Assert.That(product, Is.Not.Null, "Product with Id 'jenlooper-light' should exist.");

            // Check if the product's Ratings are not
            Assert.That(product.Ratings, Is.Not.Null, "Product Ratings should not be.");

            // Check if the Ratings array contains the new rating
            Assert.That(product.Ratings, Contains.Item(4), "Product Ratings should contain the new rating.");

        }

    }

}