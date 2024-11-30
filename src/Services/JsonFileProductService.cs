
using System;
using System.Collections.Generic;

using System.Data;

using System.IO;

using System.Linq;

using System.Text.Json;

using ContosoCrafts.WebSite.Models;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ContosoCrafts.WebSite.Services
{
    // Service class for handling CRUD operations and rating functionality for product data stored in a JSON file
    public class JsonFileProductService
    {
        // Constructor that initializes IWebHostEnvironment to access the web root path
        public JsonFileProductService(IWebHostEnvironment webHostEnvironment)
        {

            WebHostEnvironment = webHostEnvironment;

        }

        // Property to access IWebHostEnvironment, which provides the root path for the application
        public IWebHostEnvironment WebHostEnvironment { get; }

        // Property to get the full path of the products JSON file in the "data" folder under web root
        private string JsonFileName
        {

            get { return Path.Combine(WebHostEnvironment.WebRootPath, "data", "products.json"); }

        }
        // Method to get all product data from the JSON file
        public IEnumerable<ProductModel> GetAllData() 
        {
            // Open and read the JSON file
            using (var jsonFileReader = File.OpenText(JsonFileName)) 
            {

                // Deserialize the JSON data into an array of ProductModel objects
                return JsonSerializer.Deserialize<ProductModel[]>(jsonFileReader.ReadToEnd(),
                    new JsonSerializerOptions
                    {
                        // Ensure property names in JSON are case-insensitive
                        PropertyNameCaseInsensitive = true

                    });

            }

        }


        /// <summary>
        /// Adds a rating to a product identified by its ID.
        /// </summary>
        /// <param name="productId">The ID of the product to be rated.</param>
        /// <param name="rating">The rating value to add, expected between 0 and 5.</param>
        /// <returns>True if the rating was successfully added; otherwise, false.</returns>
        public bool AddRating(string productId, int rating)
        {

            // Return false if product ID is invalid
            if (string.IsNullOrEmpty(productId))
            {
                return false;
            }
            // Get all product data
            var products = GetAllData(); 

            // Find the product by its ID
            var data = products.FirstOrDefault(x => x.Id.Equals(productId));
            // Return false if the product is not found
            if (data == null)
            {
                return false;
            }

            // Check Rating for boundaries, do not allow ratings below 0
            if (rating < 0)
            {
                return false;
            }

            // Check if rating is within valid bounds (0-5)
            if (rating > 5)
            {
                return false;
            }

            // If the product has no existing ratings, initialize an empty array
            if (data.Ratings == null)
            {
                data.Ratings = new int[] { };
            }

            // Convert ratings array to a list to add the new rating
            var ratings = data.Ratings.ToList();
            ratings.Add(rating);

            // Assign the updated ratings back to the product
            data.Ratings = ratings.ToArray();

            // Save the updated product data back to the JSON file
            SaveData(products);

            return true;

        }

        /// <summary>
        /// Find the data record
        /// Update the fields
        /// Save to the data store
        /// </summary>
        /// <param name="data"></param>
        public ProductModel UpdateData(ProductModel data)
        {
            // Get all product data
            var products = GetAllData();

            // Find the product to update by its ID
            var productData = products.FirstOrDefault(x => x.Id.Equals(data.Id));

            // Return null if the product is not found
            if (productData == null)
            {
                // Return null if the product is not found
                return null;
            }

            // Update product properties with new values
            productData.Title = data.Title;
            productData.Description = data.Description.Trim();
            productData.Url = data.Url;
            productData.Image = data.Image;
            productData.GitHub = data.GitHub;
            productData.ProductType = data.ProductType;

            // Save the updated data back to the JSON file
            SaveData(products);

            return productData;

        }

        /// <summary>
        /// Saves all products data to the JSON file.
        /// </summary>
        /// <param name="products">The updated list of products to save.</param>
        private void SaveData(IEnumerable<ProductModel> products)
        {
            // Create the JSON file and write updated data to it
            using (var outputStream = File.Create(JsonFileName))
            {
                // Serialize the products data into the JSON file with indentation
                JsonSerializer.Serialize<IEnumerable<ProductModel>>(
                    new Utf8JsonWriter(outputStream, new JsonWriterOptions
                    {
                        // Skip validation for JSON formatting
                        SkipValidation = true,
                        // Enable indentation for readability
                        Indented = true       

                    }),

                    products

                );

            }

        }

        /// <summary>
        /// Creates a new product with default values and saves it.
        /// </summary>
        /// <returns>The newly created product model.</returns>
        public ProductModel CreateData()
        {
            // Create a new product with default values
            var data = new ProductModel()
            {

                Id = System.Guid.NewGuid().ToString(),
                Title = "",
                Description = "",
                Url = "",
                Image = "",
                GitHub=""

            };

            // Get the current product data and append the new product
            var dataSet = GetAllData();
            dataSet = dataSet.Append(data);

            // Save the updated data back to the JSON file
            SaveData(dataSet); 

            return data;

        }

        /// <summary>
        /// Removes a product from the JSON file by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to remove.</param>
        /// <returns>The deleted product model if successful; otherwise, null.</returns>
        public ProductModel DeleteData(string id)
        {
            // Get all product data
            var dataSet = GetAllData();

            // Find the product to delete by its ID
            var data = dataSet.FirstOrDefault(m => m.Id.Equals(id));

            // Create a new dataset by filtering out the product with the specified ID
            var newDataSet = GetAllData().Where(m => m.Id.Equals(id) == false);

            // Save the updated data back to the JSON file
            SaveData(newDataSet); 

            return data;

        }
        /// <summary>
        /// Increments the visit counter for a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to increment the counter for.</param>
        /// <returns>True if the counter was incremented; otherwise, false.</returns>
        public bool WebsiteCounter(string id)
        {
            // Get all product data
            var products = GetAllData();

            // Find the product by its ID
            var data = products.FirstOrDefault(x => x.Id.Equals(id));

            // Return false if ID is invalid or product is not found
            if (string.IsNullOrEmpty(id)) 
            {
                return false;
            }
            // Check if the product data is not found(null)
            if (data == null) 
            {
                // Return false if the product does not exist
                return false; 
            }
            // Increment the visit counter if valid
            if (data.Counter >= 0)
            {
                data.Counter += 1;
                // Save the updated data
                SaveData(products); 
            }

            return true;

        }
        /// <summary>
        /// Increments the URL click counter for a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to increment the URL counter for.</param>
        /// <returns>True if the counter was incremented; otherwise, false.</returns>
        public bool UrlCounter(string id)
        {
            // Get all product data
            var products = GetAllData();
            // Find the product by its ID
            var data = products.FirstOrDefault(x => x.Id.Equals(id));    

            // Return false if ID is invalid or product is not found
            if (string.IsNullOrEmpty(id)) 
            {
                // Return false if the product does not exist
                return false;
            }
            // Check if the product data is not found (null)
            if (data == null)
            {
                // Return false if the product does not exist
                return false;
            }
            // Increment the URL click counter if valid
            if (data.UrlCounter >= 0)
            {
                data.UrlCounter += 1;
                // Save the updated data
                SaveData(products);
            }

            return true;

        }


    }

}