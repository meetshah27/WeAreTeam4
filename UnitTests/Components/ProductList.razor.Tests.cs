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

            // Find product blocks
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

        [Test]
        public void SubmitRating_Valid_ID_Click_Unstarred_Should_Increment_Count_And_Check_Star()
        {
            // Arrange
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);
            var id = "jenlooper-lightshow_MoreInfo";

            var page = RenderComponent<ProductList>();

            // Find product blocks
            var buttonList = page.FindAll("A");

            // Find the one that matches the ID looking for and click it
            var button = buttonList.First(m => m.OuterHtml.Contains(id));
            button.Click();

            // Get the markup page post-click
            var prePageMarkup = page.Markup;

            // Get star buttons
            var starButtonList = page.FindAll("span");

            // Get first star button from the list
            var starButton = starButtonList.First(m => !string.IsNullOrEmpty(m.ClassName) && m.ClassName.Contains("fa fa-star"));

            // Act
            // Click the star button
            starButton.Click();

            // Get the markup page post-star-click
            var postPageMarkup = page.Markup;

            // Assert
            // Confirm that the record had no votes to start, and 1 vote after
            Assert.That(prePageMarkup.Contains("Be the first to vote!"), Is.EqualTo(true));
            Assert.That(postPageMarkup.Contains("1 Vote"), Is.EqualTo(true));
        }
    }
}