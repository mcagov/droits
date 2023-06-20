using Microsoft.Extensions.Configuration;

namespace tests
{
    public class TestFixture
    {

        public IConfiguration Configuration {get;}

        public TestFixture()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();
        }

    }
}
