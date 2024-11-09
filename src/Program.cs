using Microsoft.AspNetCore.Hosting;   // For hosting ASP.NET Core web applications.
using Microsoft.Extensions.Hosting;   // For creating and managing the application host.

namespace ContosoCrafts.WebSite
{
    // Program class, the entry point of the application.
    public class Program
    {
        // Main method, the application entry point.
        public static void Main(string[] args)
        {
            // Creates and runs the host (web server) for the application.
            CreateHostBuilder(args).Build().Run();
        }

        // Configures and returns an IHostBuilder instance, which is used to set up the host for the app.
        public static IHostBuilder CreateHostBuilder(string[] args) =>

            // Creates a default host with pre-configured settings for the app, like logging and configuration sources.
            Host.CreateDefaultBuilder(args)     
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // Specifies that the Startup class should be used to configure the application.
                    webBuilder.UseStartup<Startup>();
                });
    }
}
