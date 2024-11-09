using Microsoft.AspNetCore.Builder;    // For configuring middleware in the HTTP request pipeline.
using Microsoft.AspNetCore.Hosting;    // For managing the hosting environment.
using Microsoft.Extensions.Configuration;  // For accessing application configuration settings.
using Microsoft.Extensions.DependencyInjection;  // For dependency injection.
using Microsoft.Extensions.Hosting;             // For managing the app’s hosting environment.

using ContosoCrafts.WebSite.Services;      // For accessing the JsonFileProductService class.

namespace ContosoCrafts.WebSite
{
    // Startup class configures services and the request pipeline.
    public class Startup
    {
        // Constructor to inject the configuration settings.
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // Property to access the app configuration.
        public IConfiguration Configuration { get; }

        // This method is called by the runtime to add services to the dependency injection container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Adds Razor Pages with runtime compilation, enabling updates without rebuilding.
            services.AddRazorPages().AddRazorRuntimeCompilation();

            // Adds Blazor server-side support.
            services.AddServerSideBlazor();

            // Registers HttpClient for making HTTP requests.
            services.AddHttpClient();

            // Adds support for MVC controllers.
            services.AddControllers();

            // Registers JsonFileProductService with transient lifetime, creating a new instance each time it’s requested.
            services.AddTransient<JsonFileProductService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) // Checks if the environment is development and displays detailed error pages if true.
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Configures a general error handler page for non-development environments.
                app.UseExceptionHandler("/Error");

                // Enforces HTTPS by adding HSTS (HTTP Strict Transport Security) with a default duration of 30 days.
                app.UseHsts();
            }            
            app.UseHttpsRedirection();  // Redirects HTTP requests to HTTPS.
            app.UseStaticFiles();       // Serves static files (e.g., CSS, JavaScript).

            app.UseRouting();           // Adds routing to the middleware pipeline.

            app.UseAuthorization();           // Adds authorization to the pipeline.

            app.UseEndpoints(endpoints =>   // Configures the endpoint routing for Razor Pages, Controllers, and Blazor Server.
            {
                endpoints.MapRazorPages(); // Maps Razor Pages to endpoints.

                endpoints.MapControllers(); // Maps API controllers to endpoints.

                endpoints.MapBlazorHub();     // Maps Blazor server-side hub to endpoints.

                // Uncommented code that could map a custom endpoint for /products, returning a JSON response with product data.
                // endpoints.MapGet("/products", (context) => 
                // {
                //     var products = app.ApplicationServices.GetService<JsonFileProductService>().GetProducts();
                //     var json = JsonSerializer.Serialize<IEnumerable<Product>>(products);
                //     return context.Response.WriteAsync(json);
                // });
            });
        }
    }
}
