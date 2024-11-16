using Microsoft.Extensions.DependencyInjection;

using NUnit.Framework;

using ContosoCrafts.WebSite.Components;

using ContosoCrafts.WebSite.Services;
using Bunit;
using System.Linq;

namespace UnitTests.Components
{
    public class ProductListTests : BunitTestContext
    {
        #region TestSetup

        [SetUp]
        public void TestInitialize()
        {

        }

        #endregion TestSetup

        [Test]
        public void ProductList_Valid_Default_Should_Return_Content()
        {

            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            // Act
            var page = RenderComponent<ProductList>();

            // Get the Cards retrned
            var result = page.Markup;

            // Assert
            Assert.That(result.Contains("Coursera123"), Is.EqualTo(true));

        }

        [Test]
        public void SelectProduct_Valid_ID_Should_Return_Content()
        {
            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);
            var id = "jenlooper-light_MoreInfo";

            var page = RenderComponent<ProductList>();

            // Find the Buttons (more info)
            var buttonList = page.FindAll("A");

            // Find the one that matches the ID looking for, for clicking
            var button = buttonList.First(m => m.OuterHtml.Contains(id));

            // Act
            button.Click();

            // Get the markup page for the assert
            var pageMarkup = page.Markup;

            // Assert
            Assert.That(true, Is.EqualTo(pageMarkup.Contains("An online learning platform partnering with universities and organizations to offer courses &amp; specializations. Users can access high-quality educational content across various fields.")));
            
        }
    }
}