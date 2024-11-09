using System.Text.Json.Serialization;

using System.ComponentModel.DataAnnotations;

namespace ContosoCrafts.WebSite.Models
{

    public class ProductModel
    {

        public string Id { get; set; }

        public string Maker { get; set; }

        [JsonPropertyName("img")]
        public string Image { get; set; }

        public string GitHub { get; set; }

        public string Url { get; set; }
        
        [StringLength (maximumLength: 33, MinimumLength = 1, ErrorMessage = "The Title should have a length of more than {2} and less than {1}")]
        public string Title { get; set; }

        public string Description { get; set; }

        public int[] Ratings { get; set; }

        public int Counter {  get; set; }

        public int UrlCounter { get; set; }

    }

}