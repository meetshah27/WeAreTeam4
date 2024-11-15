
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

    public class JsonFileProductService   // Service class for handling product data in JSON file
    {
        // Constructor that initializes IWebHostEnvironment to access web root path
        public JsonFileProductService(IWebHostEnvironment webHostEnvironment)
        {

            WebHostEnvironment = webHostEnvironment;

        }

        // Property to get IWebHostEnvironment, which gives us access to web root path
        public IWebHostEnvironment WebHostEnvironment { get; }

        // Property to get the full path of the products JSON file in the "data" folder under web root
        private string JsonFileName
        {

            get { return Path.Combine(WebHostEnvironment.WebRootPath, "data", "products.json"); }

        }

        public IEnumerable<ProductModel> GetAllData()  // Method to get all product data from the JSON file
        {

            using (var jsonFileReader = File.OpenText(JsonFileName)) // Open and read the JSON file
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
        /// Add Rating
        /// 
        /// Take in the product ID and the rating
        /// If the rating does not exist, add it
        /// Save the update
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="rating"></param>
        public bool AddRating(string productId, int rating)
        {

            // Return false if product ID is invalid
            if (string.IsNullOrEmpty(productId))
            {
                return false;
            }

            var products = GetAllData(); // Get all product data

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
                return null;
            }

            // Update product properties with new values
            productData.Title = data.Title;
            productData.Description = data.Description.Trim();
            productData.Url = data.Url;
            productData.Image = data.Image;
            productData.GitHub = data.GitHub;

            SaveData(products);// Save the updated data back to the JSON file

            return productData;

        }

        /// <summary>
        /// Save All products data to storage
        /// </summary>
        private void SaveData(IEnumerable<ProductModel> products)
        {
            // Create the JSON file and write updated data to it
            using (var outputStream = File.Create(JsonFileName))
            {
                // Serialize the products data into the JSON file with indentation
                JsonSerializer.Serialize<IEnumerable<ProductModel>>(
                    new Utf8JsonWriter(outputStream, new JsonWriterOptions
                    {

                        SkipValidation = true,// Skip validation for JSON formatting
                        Indented = true       // Enable indentation for readability

                    }),

                    products

                );

            }

        }

        /// <summary>
        /// Create a new product using default values
        /// After create the user can update to set values
        /// </summary>
        /// <returns></returns>
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

            SaveData(dataSet); // Save the updated data back to the JSON file

            return data;

        }

        /// <summary>
        /// Remove the item from the system
        /// </summary>
        /// <returns></returns>
        public ProductModel DeleteData(string id)
        {
            // Get all product data
            var dataSet = GetAllData();

            // Find the product to delete by its ID
            var data = dataSet.FirstOrDefault(m => m.Id.Equals(id));

            // Create a new dataset by filtering out the product with the specified ID
            var newDataSet = GetAllData().Where(m => m.Id.Equals(id) == false);

            SaveData(newDataSet); // Save the updated data back to the JSON file

            return data;

        }

        public bool WebsiteCounter(string id)
        {
            // Get all product data
            var products = GetAllData();

            // Find the product by its ID
            var data = products.FirstOrDefault(x => x.Id.Equals(id));

            if (string.IsNullOrEmpty(id))  // Return false if ID is invalid or product is not found
            {
                return false;
            }
            // Check if the product data is not found(null)
            if (data == null) 
            {
                return false; // Return false if the product does not exist
            }
            // Increment the visit counter if valid
            if (data.Counter >= 0)
            {
                data.Counter += 1;
                SaveData(products);  // Save the updated data
            }

            return true;

        }

        public bool UrlCounter(string id)
        {

            var products = GetAllData(); // Get all product data
            var data = products.FirstOrDefault(x => x.Id.Equals(id));    // Find the product by its ID

            if (string.IsNullOrEmpty(id)) // Return false if ID is invalid or product is not found
            {
                return false;// Return false if the product does not exist
            }
            if (data == null)// Check if the product data is not found (null)
            {
                return false;// Return false if the product does not exist
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