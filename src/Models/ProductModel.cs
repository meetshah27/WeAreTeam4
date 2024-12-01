// Importing namespace for data annotations to validate properties of the model.
using System.ComponentModel.DataAnnotations;
// Importing namespace for JSON serialization and deserialization attributes.
using System.Text.Json.Serialization;

namespace ContosoCrafts.WebSite.Models
{
    /// <summary>
    /// Table of enums values for designating the type of product
    /// </summary>
    public enum ProductTypeEnum
    {
        undefined = 0,
        [Display(Name = "Educational")]
        education = 1,
        [Display(Name = "Banking & Finance")]
        finance = 3,
        [Display(Name = "Social Media")]
        social_media = 5,
        [Display(Name = "Shopping")]
        shopping = 7,
        [Display(Name = "Transport & Travel")]
        transport = 9,
        [Display(Name = "Food Delivery")]
        food_delivery = 11,
        [Display(Name = "Software Development")]
        software_development = 13,
        [Display(Name = "Other")]
        other = 12345,
    }

    /// <summary>
    /// Extensions of ProductTypeEnum to allow 
    /// </summary>
    public static class ProductTypeEnumExtensions
    {
        /// <summary>
        /// Returns string format of enum type for front-facing representation
        /// </summary>
        /// <param name="data">The enum being referenced</param>
        /// <returns>String representation of enum type if enum is valid & defined, blank string otherwise</returns>
        public static string DisplayName(this ProductTypeEnum data)
        {
            return data switch
            {
                ProductTypeEnum.education => "Educational",
                ProductTypeEnum.finance => "Banking & Finance",
                ProductTypeEnum.social_media => "Social Media",
                ProductTypeEnum.shopping => "Shopping",
                ProductTypeEnum.transport => "Transport & Travel",
                ProductTypeEnum.food_delivery => "Food Delivery",
                ProductTypeEnum.software_development => "Software Development",
                ProductTypeEnum.other => "Other",


                // Default, Unknown
                _ => "",
            };
        }
    }


    /// <summary>
    /// Represents a product model with relevant details such as ID, maker, title, description, ratings, and counters.
    /// </summary>
    public class ProductModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the product.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the maker of the product.
        /// </summary>
        public string Maker { get; set; }

        /// <summary>
        /// Gets or sets the image associated with the product.
        /// </summary>
        [JsonPropertyName("img")]
        public string Image { get; set; }

        /// <summary>
        /// Gets or sets the GitHub link related to the product.
        /// </summary>
        public string GitHub { get; set; }

        /// <summary>
        /// Gets or sets the URL of the product.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the title of the product. Length must be between 1 and 33 characters.
        /// </summary>
        [StringLength(maximumLength: 33, MinimumLength = 1, ErrorMessage = "The Title should have a length of more than {2} and less than {1}")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description of the product.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the ratings of the product.
        /// </summary>
        public int[] Ratings { get; set; }

        /// <summary>
        /// Gets or sets the counter representing the number of views of the product.
        /// </summary>
        public int Counter { get; set; }

        /// <summary>
        /// Gets or sets the counter representing the number of URL accesses for the product.
        /// </summary>
        public int UrlCounter { get; set; }

        /// <summary>
        /// Gets or sets the enum representing what type of service the product provides
        /// </summary>
        public ProductTypeEnum ProductType { get; set; }
    }
}
