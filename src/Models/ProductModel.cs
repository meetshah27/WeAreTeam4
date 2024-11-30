// Importing namespace for data annotations to validate properties of the model.
using System.ComponentModel.DataAnnotations;
// Importing namespace for JSON serialization and deserialization attributes.
using System.Text.Json.Serialization;

namespace ContosoCrafts.WebSite.Models
{
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
    }
}
