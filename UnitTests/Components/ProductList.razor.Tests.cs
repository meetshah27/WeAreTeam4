// For dependency injection services
using Microsoft.Extensions.DependencyInjection;

// For unit testing attributes and assertions
using NUnit.Framework;

// For accessing components from the ContosoCrafts website project
using ContosoCrafts.WebSite.Components;

// For accessing services from the ContosoCrafts website project
using ContosoCrafts.WebSite.Services;

// For Blazor component testing
using Bunit;

// For LINQ operations on collections
using System.Linq;


// Define a namespace for unit tests related to components
namespace UnitTests.Components
{
    // Test class for the ProductList component
    public class ProductListTests : BunitTestContext
    {
        #region TestSetup

        // Method to set up any necessary data or services before each test
        [SetUp]
        public void TestInitialize()
        {
            // This can be used to initialize values or set up mocks if required
        }

        #endregion TestSetup

        /// <summary>
        /// Verifies that the list of products is properly rendered
        /// Confirms that an entry known to be in the dataset is properly rendered on the page
        /// </summary>
        [Test]
        public void ProductList_Valid_Default_Should_Return_Content()
        {

            // Arrange
            // Register ProductService as a singleton in the service collection for testing
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            // Act
            // Render the ProductList component
            var page = RenderComponent<ProductList>();

            // Get the Cards retrned
            var result = page.Markup;

            // Assert
            // Verify that the rendered markup contains the expected text ("Coursera123")
            Assert.That(result.Contains("Coursera"), Is.EqualTo(true));

        }

        /// <summary>
        /// Verifies that the "more info" tab can be properly rendered
        /// Confirms that the rendered page contains the description for the product specified to be selected
        /// </summary>
        [Test]
        public void SelectProduct_Valid_ID_Should_Return_Content()
        {
            // Arrange
            // Register ProductService as a singleton in the service collection for testing
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            // Specify the ID of the product to select
            var id = "jenlooper-light_MoreInfo";

            // Render the ProductList component
            var page = RenderComponent<ProductList>();

            // Find product blocks
            var buttonList = page.FindAll("A");

            // Find the one that matches the ID looking for, for clicking
            var button = buttonList.First(m => m.OuterHtml.Contains(id));

            // Act
            // Simulate a click on the found anchor tag to select the product
            button.Click();

            // Get the markup page for the assert
            var pageMarkup = page.Markup;

            // Assert
            // Verify that the product's specific description appears in the markup after selection
            Assert.That(pageMarkup.Contains("An online learning platform partnering with universities and organizations to offer courses &amp; specializations. Users can access high-quality educational content across various fields."), Is.EqualTo(true));
            
        }

        /// <summary>
        /// Tests submitting a star rating higher than the current average
        /// Confirms that the rendered stars have changed & the vote count incremented as expected
        /// </summary>
        [Test]
        public void SubmitRating_Valid_ID_Click_Unstarred_Should_Increment_Count_And_Check_Star()
        {
            // Arrange
            // Create a new product for testing with a unique ID
            var data = TestHelper.ProductService.CreateData();
            var id = data.Id + "_MoreInfo";

            // Register ProductService as a singleton in the service collection for testing
            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);

            // Render the ProductList component
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

        /// <summary>
        /// Tests submitting a star rating lower than the current average
        /// Confirms that the rendered stars have changed & the vote count incremented as expected
        /// </summary>
        [Test]
        public void SubmitRating_Valid_ID_Click_Starred_Should_Uncheck_Higher_Stars_And_Increment_Count()
        {
            // Arrange
            var data = TestHelper.ProductService.CreateData();
            var id = data.Id+"_MoreInfo";

            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);
            var page = RenderComponent<ProductList>();

            // Find product blocks
            var buttonList = page.FindAll("A");

            // Find the one that matches the ID looking for and click it
            var button = buttonList.First(m => m.OuterHtml.Contains(id));
            button.Click();

            // Get star buttons
            var starButtonList = page.FindAll("span");

            // Get last star button from the list and click it
            var starButton = starButtonList.Last(m => !string.IsNullOrEmpty(m.ClassName) && m.ClassName.Contains("fa fa-star"));
            starButton.Click();

            // Get the markup page pre-second-click
            var prePageMarkup = page.Markup;

            // Get updated star buttons
            starButtonList = page.FindAll("span");

            // Get the first clicked star button from the list
            starButton = starButtonList.First(m => !string.IsNullOrEmpty(m.ClassName) && m.ClassName.Contains("fa fa-star checked"));

            // Act
            // Click the star button
            starButton.Click();

            // Get the markup page post-second-click
            var postPageMarkup = page.Markup;

            // Assert
            // Confirm that the record has not changed
            Assert.That(prePageMarkup.Contains("1 Vote"), Is.EqualTo(true));
            Assert.That(postPageMarkup.Contains("2 Votes"), Is.EqualTo(true));
        }

        /// <summary>
        /// Tests clicking on the url to visit the product's website
        /// Confirms that the tracker for how many people have visited the product's website has increased accordingly
        /// </summary>
        [Test]
        public void UrlCounter_Valid_ID_Should_Increment_On_Click()
        {
            // Arrange
            var data = TestHelper.ProductService.CreateData();
            var id = data.Id + "_MoreInfo";

            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);
            var page = RenderComponent<ProductList>();

            // Find product blocks
            var buttonList = page.FindAll("A");

            // Find the one that matches the ID looking for and click it
            var button = buttonList.First(m => m.OuterHtml.Contains(id));
            button.Click();

            // Get the markup page pre-url
            var prePageMarkup = page.Markup;

            // Find url
            id = data.Id + "_Url";
            buttonList = page.FindAll("A");
            var url = buttonList.First(m => m.OuterHtml.Contains(id));

            // Act
            url.Click();

            // Get the markup page post-url
            var postPageMarkup = page.Markup;

            // Assert
            // Confirm the counter changed from 0% to 100%
            Assert.That(prePageMarkup.Contains("0 %"), Is.EqualTo(true));
            Assert.That(postPageMarkup.Contains("100 %"), Is.EqualTo(true));
        }

        /// <summary>
        /// Tests clicking the link to the product's website multiple times in one go
        /// Confirms that the "Popularity%" entry does not increase above 100%
        /// </summary>
        [Test]
        public void UrlCounter_Valid_ID_Percent_Should_Cap_At_100()
        {
            // Arrange
            var data = TestHelper.ProductService.CreateData();
            var id = data.Id + "_MoreInfo";

            Services.AddSingleton<JsonFileProductService>(TestHelper.ProductService);
            var page = RenderComponent<ProductList>();

            // Find product blocks
            var buttonList = page.FindAll("A");

            // Find the one that matches the ID looking for and click it
            var button = buttonList.First(m => m.OuterHtml.Contains(id));
            button.Click();

            // Find url
            id = data.Id + "_Url";
            buttonList = page.FindAll("A");
            var url = buttonList.First(m => m.OuterHtml.Contains(id));

            // Act
            url.Click();

            // Get the markup page before extra url click
            var prePageMarkup = page.Markup;

            // Click url again
            url.Click();

            // Get the markup page after extra url click
            var postPageMarkup = page.Markup;

            // Assert
            // Confirm that the counter caps at 100%
            Assert.That(prePageMarkup.Contains("100 %"), Is.EqualTo(true));
            Assert.That(postPageMarkup.Contains("100 %"), Is.EqualTo(true));
        }
    }
}