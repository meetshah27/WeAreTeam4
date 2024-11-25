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
            // Call AddRating with null product
            var result = TestHelper.ProductService.AddRating(null, 1);

            // Assert
            // Assert that the method returns false for a null product
            Assert.That(result, Is.EqualTo(false));
        }
        /// <summary>
        /// Validation on AddRating method for the product when it is empty.
        /// It should return False since the product is invalid.
        /// </summary>
        [Test]
        public void AddRating_Invalid_Product_Empty_Should_Return_False()
        {
            // Arrange

            // Act
            // Call AddRating with an empty product ID
            var result = TestHelper.ProductService.AddRating("", 1);

            // Assert
            // Assert that the method returns false for an empty product ID
            Assert.That(result, Is.EqualTo(false));
        }
        /// <summary>
        /// Validation on AddRating method for the product when rated 5.
        /// It should return True and add the rating to the product.
        /// </summary>
        [Test]
        public void AddRating_Valid_Product_Rating_5_Should_Return_True()
        {
            // Arrange

            // Get the first product from the service
            var data = TestHelper.ProductService.GetAllData().First();
            // Get the current number of ratings for the product, defaulting to 0 if null
            var countOriginal = 0;
            if(data.Ratings != null)
            { 
                countOriginal = data.Ratings.Length;
            }


            // Act
            // Add a rating of 5 to the product
            var result = TestHelper.ProductService.AddRating(data.Id, 5);
            // Retrieve the updated product data
            var dataNewList = TestHelper.ProductService.GetAllData().First();

            // Assert
            // Assert that the method returns true when the rating is successfully added
            Assert.That(result, Is.EqualTo(true));
            // Assert that the number of ratings increased by 1
            Assert.That(dataNewList.Ratings.Length, Is.EqualTo(countOriginal + 1));
            // Assert that the last rating added is 5
            Assert.That(dataNewList.Ratings.Last(), Is.EqualTo(5));
        }
        /// <summary>
        /// Validation on AddRating method for a nonexistent product.
        /// It should return False since the product does not exist.
        /// </summary>
        [Test]
        public void AddRating_Invalid_Product_Nonexistent_Should_Return_False()
        {
            // Arrange

            // Act
            // Try adding a rating to a nonexistent product
            var result = TestHelper.ProductService.AddRating("NonexistentTestProductDoNotMakeAProductWithThisName", 5);

            // Assert
            // Assert that the method returns false for a nonexistent product
            Assert.That(result, Is.EqualTo(false));
        }
        /// <summary>
        /// Validation on AddRating method for a rating greater than 5.
        /// It should return False since ratings cannot exceed 5.
        /// </summary>
        [Test]
        public void AddRating_Invalid_Product_Rating_Greater_Than_5_Should_Return_False()
        {
            // Arrange

            // Get the first product from the service
            var data = TestHelper.ProductService.GetAllData().First();

            // Try adding a rating greater than 5
            var result = TestHelper.ProductService.AddRating(data.Id, 6);

            // Assert
            // Assert that the method returns false for a rating greater than 5
            Assert.That(result, Is.EqualTo(false));
        }
        /// <summary>
        /// Validation on AddRating method for a rating less than 0.
        /// It should return False since ratings cannot be negative.
        /// </summary>
        [Test]
        public void AddRating_Invalid_Product_Rating_Less_Than_0_Should_Return_False()
        {
            // Arrange

            // Get the first product from the service
            var data = TestHelper.ProductService.GetAllData().First();

            // Act
            // Try adding a negative rating
            var result = TestHelper.ProductService.AddRating(data.Id, -1);

            // Assert
            // Assert that the method returns false for a negative rating
            Assert.That(result, Is.EqualTo(false));
        }
        /// <summary>
        /// Validation on AddRating method for a product with no ratings.
        /// It should create a ratings array and add the first rating to it.
        /// </summary>
        [Test]
        public void AddRating_Valid_Product_No_Ratings_Create_Array()
        {
            // Arrange

            // Create a new product with no ratings
            var data = TestHelper.ProductService.CreateData();

            // Act
            // Add a rating of 5 to the new product
            TestHelper.ProductService.AddRating(data.Id, 5);
            // Retrieve the updated product data
            var dataNewList = TestHelper.ProductService.GetAllData().Last();

            // Assert
            // Assert that the ratings array now contains one rating
            Assert.That(dataNewList.Ratings.Length, Is.EqualTo(1));
            // Assert that the first rating added is 5
            Assert.That(dataNewList.Ratings.Last(), Is.EqualTo(5));
        }
        #endregion AddRating


        #region WebsiteCounter
        /// <summary>
        /// Validation on the WebsiteCounter method with null product id.
        /// It should return False as a null product id is invalid.
        /// </summary>
        [Test]
        public void WebsiteCounter_Invalid_ProductId_Null_Should_Return_False()
        {
            //Act
            // Call WebsiteCounter with a null product ID
            var result = TestHelper.ProductService.WebsiteCounter(null);

            // Assert
            // Assert that the method returns false for a null product ID
            Assert.That(result, Is.EqualTo(false));
        }
        /// <summary>
        /// Validation on the WebsiteCounter method with an invalid product id.
        /// It should return False since the product id is invalid.
        /// </summary>
        [Test]
        public void WebsiteCounter_Invalid_ProductId_Not_Valid_Should_Return_False()
        {
            //Arrange
            // Get the first product from the service
            var data = TestHelper.ProductService.GetAllData().First();
            // Create an invalid product ID by appending "cool" to the valid ID
            var invalidId = data.Id + "cool";

            //Act
            // Call WebsiteCounter with an invalid product ID
            var result = TestHelper.ProductService.WebsiteCounter(invalidId);

            // Assert
            // Assert that the method returns false for an invalid product ID
            Assert.That(result, Is.EqualTo(false));
        }
        /// <summary>
        /// Validation on the WebsiteCounter method with a valid product id.
        /// It should return True indicating the product exists and the counter can be updated.
        /// </summary>
        [Test]
        public void WebsiteCounter_Valid_ProductId_Valid_Should_Return_True()
        {
            //Arrange
            // Get the first product from the service
            var data = TestHelper.ProductService.GetAllData().First();

            //Act
            // Call WebsiteCounter with a valid product ID
            var result = TestHelper.ProductService.WebsiteCounter(data.Id);

            // Assert
            // Assert that the method returns true for a valid product ID
            Assert.That(result, Is.EqualTo(true));
        }
        /// <summary>
        /// Validation on the WebsiteCounter method with counter greater than 0.
        /// It should return True if the counter is greater than 0.
        /// </summary>
        [Test]
        public void WebsiteCounter_Valid_Counter_Greater_Than_Zero_Should_Return_True()
        {
            //Arrange
            // Get the first product from the service
            var data = TestHelper.ProductService.GetAllData().First();

            //Act
            // Call WebsiteCounter for the product
            var result = TestHelper.ProductService.WebsiteCounter(data.Id);

            // Assert
            // Assert that the method returns true when the counter is greater than 0
            Assert.That(result, Is.EqualTo(true));
        }
        #endregion WebsiteCounter

        #region UrlCounter
        /// <summary>
        /// Validation on the UrlCounter method with null product id.
        /// It should return False as a null product id is invalid.
        /// </summary>
        [Test]
        public void UrlCounter_Invalid_ProductId_Null_Should_Return_False()
        {
            //Act
            // Call UrlCounter with a null product ID
            var result = TestHelper.ProductService.UrlCounter(null);

            // Assert
            // Assert that the method returns false for a null product ID
            Assert.That(result, Is.EqualTo(false));
        }

        /// <summary>
        /// Validation on the UrlCounter method with an invalid product id.
        /// It should return False since the product id is invalid.
        /// </summary>
        [Test]
        public void UrlCounter_Invalid_ProductId_Not_Valid_Should_Return_False()
        {
            //Arrange
            // Get the first product from the service
            var data = TestHelper.ProductService.GetAllData().First();
            // Create an invalid product ID by appending "cool" to the valid ID
            var invalidId = data.Id + "cool";

            //Act
            // Call UrlCounter with an invalid product ID
            var result = TestHelper.ProductService.UrlCounter(invalidId);

            // Assert
            // Assert that the method returns false for an invalid product ID
            Assert.That(result, Is.EqualTo(false));
        }
        /// <summary>
        /// Validation on the UrlCounter method with a valid product id.
        /// It should return True indicating the product exists and the counter can be updated.
        /// </summary>
        [Test]
        public void UrlCounter_Valid_ProductId_Valid_Should_Return_True()
        {
            //Arrange
            // Get the first product from the service
            var data = TestHelper.ProductService.GetAllData().First();

            //Act
            // Call UrlCounter with a valid product ID
            var result = TestHelper.ProductService.UrlCounter(data.Id);

            // Assert
            // Assert that the method returns true for a valid product ID
            Assert.That(result, Is.EqualTo(true));
        }
        /// <summary>
        /// Validation on the UrlCounter method having counter greater than 0
        /// It should return True if the counter is greater than 0
        /// </summary>
        [Test]
        public void UrlCounter_Valid_Counter_Greater_Than_Zero_Should_Return_True()
        {
            // Arrange
            // Get the first product from the service to use its ID for testing.
            var data = TestHelper.ProductService.GetAllData().First();

            // Act
            // Call the UrlCounter method with the valid product ID.
            var result = TestHelper.ProductService.UrlCounter(data.Id);

            // Assert
            // Assert that the result is true when the product has a valid counter greater than 0.
            Assert.That(result, Is.EqualTo(true));
        }
        #endregion UrlCounter

        #region UpdateData
        /// <summary>
        /// Validation on the UpdateData method having non-existent product id 
        /// It should return Null since the product does not exist.
        /// </summary>
        [Test]
        public void UpdateData_Invalid_Product_Nonexistent_Should_Return_Null()
        {
            // Arrange
            // Create a new product and append a string to make its ID non-existent.
            var nonexistent = TestHelper.ProductService.CreateData();
            nonexistent.Id += "NonexistentTestProductDoNotMakeAProductWithThisName";

            // Act
            // Try updating the non-existent product.
            var result = TestHelper.ProductService.UpdateData(nonexistent);

            // Assert
            // Assert that the result is null because the product doesn't exist.
            Assert.That(result, Is.Null);
        }
        /// <summary>
        /// Validation on the UpdateData method having a valid product id 
        /// It should return the updated product with the new title.
        /// </summary>
        [Test]
        public void UpdateData_Valid_Product_Should_Return_Updated_Product()
        {
            // Arrange
            // Get the first product from the service and modify its title.
            var newData = TestHelper.ProductService.GetAllData().First();
            var newTitle = newData.Title + " (But very, very cool)";
            newData.Title = newTitle;

            // Act
            // Call the UpdateData method to update the product.
            var result = TestHelper.ProductService.UpdateData(newData);

            // Assert
            // Assert that the title of the updated product matches the new title.
            Assert.That(result.Title, Is.EqualTo(newTitle));
        }

        #endregion UpdateData

        #region CreateData
        /// <summary>
        /// Validation on the CreateData method where a new product is added 
        /// It should return the product added to the set.
        /// </summary>
        [Test]
        public void CreateData_Should_Add_Product_To_Array()
        {
            // Arrange
            // Get the current number of products in the service.
            var originalLength = TestHelper.ProductService.GetAllData().Count();

            // Act
            // Call CreateData to add a new product to the set.
            TestHelper.ProductService.CreateData();

            // Assert
            // Assert that the number of products has increased by 1 after adding a new product.
            Assert.That(TestHelper.ProductService.GetAllData().Count(), Is.EqualTo(originalLength + 1));
        }

        #endregion CreateData

        #region DeleteData
        /// <summary>
        /// Validation on the DeleteData method where a product is removed 
        /// It should return the product removed from the array.
        /// </summary>
        [Test]
        public void DeleteData_Should_Remove_Product_From_Array()
        {
            // Arrange
            // Create a new product and get the current number of products in the service.
            var data = TestHelper.ProductService.CreateData();
            var originalLength = TestHelper.ProductService.GetAllData().Count();

            // Act
            // Call DeleteData to remove the product by its ID.
            TestHelper.ProductService.DeleteData(data.Id);

            // Assert
            // Assert that the number of products has decreased by 1 after deleting the product.
            Assert.That(TestHelper.ProductService.GetAllData().Count(), Is.EqualTo(originalLength - 1));
        }

        #endregion DeleteData



    }
}