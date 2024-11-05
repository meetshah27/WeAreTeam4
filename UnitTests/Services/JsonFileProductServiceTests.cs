using System.Linq;

using Microsoft.AspNetCore.Mvc;

using NUnit.Framework;

using ContosoCrafts.WebSite.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics.Metrics;

namespace UnitTests.Pages.Product.AddRating
{
    public class JsonFileProductServiceTests
    {
        #region TestSetup

        [SetUp]
        public void TestInitialize()
        {
        }

        #endregion TestSetup

        #region AddRating
        //[Test]
        //public void AddRating_InValid_....()
        //{
        //    // Arrange

        //    // Act
        //    //var result = TestHelper.ProductService.AddRating(null, 1);

        //    // Assert
        //    //Assert.AreEqual(false, result);
        //}

        // ....

        [Test]
        public void AddRating_Invalid_Product_Null_Should_Return_False()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.AddRating(null, 1);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void AddRating_Invalid_Product_Empty_Should_Return_False()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.AddRating("", 1);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void AddRating_Valid_Product_Rating_5_Should_Return_True()
        {
            // Arrange

            // Get the First data item
            var data = TestHelper.ProductService.GetAllData().First();
            var countOriginal = data.Ratings.Length;

            // Act
            var result = TestHelper.ProductService.AddRating(data.Id, 5);
            var dataNewList = TestHelper.ProductService.GetAllData().First();

            // Assert
            Assert.That(result, Is.EqualTo(true));
            Assert.That(dataNewList.Ratings.Length, Is.EqualTo(countOriginal + 1));
            Assert.That(dataNewList.Ratings.Last(), Is.EqualTo(5));
        }

        [Test]
        public void AddRating_Invalid_Product_Nonexistent_Should_Return_False()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.AddRating("NonexistentTestProductDoNotMakeAProductWithThisName", 5);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void AddRating_Invalid_Product_Rating_Greater_Than_5_Should_Return_False()
        {
            // Arrange

            // Get the First data item
            var data = TestHelper.ProductService.GetAllData().First();

            // Act
            var result = TestHelper.ProductService.AddRating(data.Id, 6);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void AddRating_Invalid_Product_Rating_Less_Than_0_Should_Return_False()
        {
            // Arrange

            // Get the First data item
            var data = TestHelper.ProductService.GetAllData().First();

            // Act
            var result = TestHelper.ProductService.AddRating(data.Id, -1);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void AddRating_Valid_Product_No_Ratings_Create_Array()
        {
            // Arrange

            // Create dummy product with no ratings
            var data = TestHelper.ProductService.CreateData();

            // Act
            TestHelper.ProductService.AddRating(data.Id, 5);
            var dataNewList = TestHelper.ProductService.GetAllData().Last();

            // Assert
            Assert.That(dataNewList.Ratings.Length, Is.EqualTo(1));
            Assert.That(dataNewList.Ratings.Last(), Is.EqualTo(5));
        }
        #endregion AddRating

        #region WebsiteCounter
        [Test]
        public void WebsiteCounter_Invalid_ProductId_Null_Should_Return_False()
        {
            //Act
            var result = TestHelper.ProductService.WebsiteCounter(null);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void WebsiteCounter_Invalid_ProductId_Not_Valid_Should_Return_False()
        {
            //Arrange
            var data = TestHelper.ProductService.GetAllData().First();
            var Invalid_id = data.Id + "cool";

            //Act
            var result = TestHelper.ProductService.WebsiteCounter(Invalid_id);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }
        [Test]
        public void WebsiteCounter_Valid_ProductId_Valid_Should_Return_True()
        {
            //Arrange
            var data = TestHelper.ProductService.GetAllData().First();

            //Act
            var result = TestHelper.ProductService.WebsiteCounter(data.Id);

            // Assert
            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void WebsiteCounter_Valid_Counter_Greater_Than_Zero_Should_Return_True()
        {
            //Arrange
            var data = TestHelper.ProductService.GetAllData().First();

            //Act
            var result = TestHelper.ProductService.WebsiteCounter(data.Id);

            // Assert
            Assert.That(result, Is.EqualTo(true));
        }
        #endregion WebsiteCounter

        #region UpdateData

        [Test]
        public void UpdateData_Invalid_Product_Nonexistent_Should_Return_Null()
        {
            // Arrange
            var nonexistent = TestHelper.ProductService.CreateData();
            nonexistent.Id += "NonexistentTestProductDoNotMakeAProductWithThisName";

            // Act
            var result = TestHelper.ProductService.UpdateData(nonexistent);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void UpdateData_Valid_Product_Should_Return_Updated_Product()
        {
            // Arrange

            // Get the First data item
            var NewData = TestHelper.ProductService.GetAllData().First();
            var NewTitle = NewData.Title + " (But very, very cool)";
            NewData.Title = NewTitle;

            // Act
            var result = TestHelper.ProductService.UpdateData(NewData);

            // Assert
            Assert.That(result.Title, Is.EqualTo(NewTitle));
        }

        #endregion UpdateData

        #region CreateData
        [Test]
        public void CreateData_Should_Add_Product_To_Array()
        {
            // Arrange
            var OriginalLength = TestHelper.ProductService.GetAllData().Count();

            // Act
            TestHelper.ProductService.CreateData();

            // Assert
            Assert.That(TestHelper.ProductService.GetAllData().Count(), Is.EqualTo(OriginalLength + 1));
        }

        #endregion CreateData

        #region DeleteData
        [Test]
        public void DeleteData_Should_Remove_Product_From_Array()
        {
            // Arrange
            var data = TestHelper.ProductService.CreateData();
            var OriginalLength = TestHelper.ProductService.GetAllData().Count();

            // Act
            TestHelper.ProductService.DeleteData(data.Id);

            // Assert
            Assert.That(TestHelper.ProductService.GetAllData().Count(), Is.EqualTo(OriginalLength - 1));
        }

        #endregion DeleteData



    }
}