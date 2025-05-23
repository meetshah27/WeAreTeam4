using Microsoft.Extensions.Configuration;

using Microsoft.AspNetCore.Hosting;

using NUnit.Framework;

namespace UnitTests.Pages.Startup
{

    public class StartupTests
    {

        #region TestSetup

        [SetUp]
        public void TestInitialize()
        {

        }

        public class Startup : ContosoCrafts.WebSite.Startup
        {

            public Startup(IConfiguration config) : base(config) { }

        }
        #endregion TestSetup

        #region ConfigureServices
        [Test]
        public void Startup_ConfigureServices_Valid_Defaut_Should_Pass()
        {

            var webHost = Microsoft.AspNetCore.WebHost.CreateDefaultBuilder().UseStartup<Startup>().Build();

            Assert.That(webHost, Is.Not.Null);

        }
        #endregion ConfigureServices

        #region Configure
        [Test]
        public void Startup_Configure_Valid_Defaut_Should_Pass()
        {

            var webHost = Microsoft.AspNetCore.WebHost.CreateDefaultBuilder().UseStartup<Startup>().Build();

            Assert.That(webHost, Is.Not.Null);

        }

        #endregion Configure

    }

}