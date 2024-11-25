using System.Linq;

using NUnit.Framework;

namespace UnitTests.Pages.Product.AddRating
{
    public class JsonFileProductServiceTests
    {
        #region TestSetup
        /// <summary>
        /// Setup Initialization for the Test file of JsonFileProductService.
        /// This method will be called before each test to prepare the necessary test environment.
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
        }

        #endregion TestSetup

        #region AddRating
        /// <summary>
        /// Validation on AddRating method for the product when it is null.
        /// It should return False since the product is invalid.
        /// </summary>
        [Test]
        public void AddRating_Invalid_Product_Null_Should_Return_False()
        {
            // Arrange

            // Act

            var result = TestHelper.ProductService.AddRating(null, 1);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }
        /// <summary>
        /// validation on Addrating method for the product when it is Empty,
        /// it should return False
        /// </summary>
        [Test]
        public void AddRating_Invalid_Product_Empty_Should_Return_False()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.AddRating("", 1);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }
        /// <summary>
        /// Validation on AddRating method for the product when rated 5
        /// return True
        /// </summary>
        [Test]
        public void AddRating_Valid_Product_Rating_5_Should_Return_True()
        {
            // Arrange

            // Get the First data item
            var data = TestHelper.ProductService.GetAllData().First();
            var countOriginal = 0;
            if(data.Ratings != null)
            { 
                countOriginal = data.Ratings.Length;
            }
            

            // Act
            var result = TestHelper.ProductService.AddRating(data.Id, 5);
            var dataNewList = TestHelper.ProductService.GetAllData().First();

            // Assert
            Assert.That(result, Is.EqualTo(true));
            Assert.That(dataNewList.Ratings.Length, Is.EqualTo(countOriginal + 1));
            Assert.That(dataNewList.Ratings.Last(), Is.EqualTo(5));
        }
        /// <summary>
        /// Validation on the AddRating method for the Nonexistent product
        /// Return False
        /// </summary>
        [Test]
        public void AddRating_Invalid_Product_Nonexistent_Should_Return_False()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.AddRating("NonexistentTestProductDoNotMakeAProductWithThisName", 5);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }
        /// <summary>
        /// Validation on the AddRating method for rating going greater than 5 for any product
        /// Return False
        /// </summary>
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
        /// <summary>
        /// Validation on the AddRating method for rating going less than 0 for any product
        /// Return False
        /// </summary>
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
        /// <summary>
        /// Validation on the AddRating method for rating going having no rating to any product
        /// Return False
        /// </summary>
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
        /// <summary>
        /// Validation on the WebsiteCounter method having null product id 
        /// return False
        /// </summary>
        [Test]
        public void WebsiteCounter_Invalid_ProductId_Null_Should_Return_False()
        {
            //Act
            var result = TestHelper.ProductService.WebsiteCounter(null);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }
        /// <summary>
        /// Validation on the WebsiteCounter method having invalid product id 
        /// return False
        /// </summary>
        [Test]
        public void WebsiteCounter_Invalid_ProductId_Not_Valid_Should_Return_False()
        {
            //Arrange
            var data = TestHelper.ProductService.GetAllData().First();
            var invalidId = data.Id + "cool";

            //Act
            var result = TestHelper.ProductService.WebsiteCounter(invalidId);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }
        /// <summary>
        /// Validation on the WebsiteCounter method having valid product id 
        /// return True
        /// </summary>
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
        /// <summary>
        /// Validation on the WebsiteCounter method having counter greater than 0
        /// return True
        /// </summary>
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

        #region UrlCounter
        /// <summary>
        /// Validation on the UrlCounter method having null product id 
        /// return False
        /// </summary>
        [Test]
        public void UrlCounter_Invalid_ProductId_Null_Should_Return_False()
        {
            //Act
            var result = TestHelper.ProductService.UrlCounter(null);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }
        /// <summary>
        /// Validation on the UrlCounter method having invalid product id 
        /// return False
        /// </summary>
        [Test]
        public void UrlCounter_Invalid_ProductId_Not_Valid_Should_Return_False()
        {
            //Arrange
            var data = TestHelper.ProductService.GetAllData().First();
            var invalidId = data.Id + "cool";

            //Act
            var result = TestHelper.ProductService.UrlCounter(invalidId);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }
        /// <summary>
        /// Validation on the UrlCounter method having valid product id 
        /// return True
        /// </summary>
        [Test]
        public void UrlCounter_Valid_ProductId_Valid_Should_Return_True()
        {
            //Arrange
            var data = TestHelper.ProductService.GetAllData().First();

            //Act
            var result = TestHelper.ProductService.UrlCounter(data.Id);

            // Assert
            Assert.That(result, Is.EqualTo(true));
        }
        /// <summary>
        /// Validation on the UrlCounter method having counter greater than 0
        /// return True
        /// </summary>
        [Test]
        public void UrlCounter_Valid_Counter_Greater_Than_Zero_Should_Return_True()
        {
            //Arrange
            var data = TestHelper.ProductService.GetAllData().First();

            //Act
            var result = TestHelper.ProductService.UrlCounter(data.Id);

            // Assert
            Assert.That(result, Is.EqualTo(true));
        }
        #endregion UrlCounter

        #region UpdateData
        /// <summary>
        /// Validation on the UpdateData method having non existence product id 
        /// return False
        /// </summary>
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
        /// <summary>
        /// Validation on the UpdateData method having valid product id 
        /// return Updated Value
        /// </summary>
        [Test]
        public void UpdateData_Valid_Product_Should_Return_Updated_Product()
        {
            // Arrange

            // Get the First data item
            var newData = TestHelper.ProductService.GetAllData().First();
            var newTitle = newData.Title + " (But very, very cool)";
            newData.Title = newTitle;

            // Act
            var result = TestHelper.ProductService.UpdateData(newData);

            // Assert
            Assert.That(result.Title, Is.EqualTo(newTitle));
        }

        #endregion UpdateData

        #region CreateData
        /// <summary>
        /// Validation on the CreateData method having product added 
        /// return product added to the set
        /// </summary>
        [Test]
        public void CreateData_Should_Add_Product_To_Array()
        {
            // Arrange
            var originalLength = TestHelper.ProductService.GetAllData().Count();

            // Act
            TestHelper.ProductService.CreateData();

            // Assert
            Assert.That(TestHelper.ProductService.GetAllData().Count(), Is.EqualTo(originalLength + 1));
        }

        #endregion CreateData

        #region DeleteData
        /// <summary>
        /// Validation on the DeleteData method having valid product id 
        /// return deleted product
        /// </summary>
        [Test]
        public void DeleteData_Should_Remove_Product_From_Array()
        {
            // Arrange
            var data = TestHelper.ProductService.CreateData();
            var originalLength = TestHelper.ProductService.GetAllData().Count();

            // Act
            TestHelper.ProductService.DeleteData(data.Id);

            // Assert
            Assert.That(TestHelper.ProductService.GetAllData().Count(), Is.EqualTo(originalLength - 1));
        }

        #endregion DeleteData



    }
}