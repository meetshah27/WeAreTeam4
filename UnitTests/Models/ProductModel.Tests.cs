using NUnit.Framework;
using ContosoCrafts.WebSite.Models;
using Moq;

namespace UnitTests.Models
{
    internal class ProductModelTests
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

        #region DisplayName

        /// <summary>
        /// Validation of DisplayName() extension for product type enums
        /// Should return the expected string when called on the corresponding enum
        /// </summary>
        [Test]
        public void Displayname_Should_Return_Correct_String_Representation()
        {
            //Arrange (Nothing needs done)

            //Act (Nothing needs done)

            //Assert
            //Check that DisplayName() returns the expected string for each enum
            Assert.That(ProductTypeEnum.education.DisplayName(), Is.EqualTo("Educational"));
            Assert.That(ProductTypeEnum.finance.DisplayName(), Is.EqualTo("Banking & Finance"));
            Assert.That(ProductTypeEnum.social_media.DisplayName(), Is.EqualTo("Social Media"));
            Assert.That(ProductTypeEnum.shopping.DisplayName(), Is.EqualTo("Shopping"));
            Assert.That(ProductTypeEnum.transport.DisplayName(), Is.EqualTo("Transport & Travel"));
            Assert.That(ProductTypeEnum.food_delivery.DisplayName(), Is.EqualTo("Food Delivery"));
            Assert.That(ProductTypeEnum.software_development.DisplayName(), Is.EqualTo("Software Development"));
            Assert.That(ProductTypeEnum.housing.DisplayName(), Is.EqualTo("Homes & Apartments"));
            Assert.That(ProductTypeEnum.other.DisplayName(), Is.EqualTo("Other"));
        }

        #endregion DisplayName
    }
}
