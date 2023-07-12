using Microsoft.Extensions.Configuration;

namespace Droits.Tests;

public class TestFixture
{
    public TestFixture()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", false, true);

        Configuration = builder.Build();
    }

    public IConfiguration Configuration { get; }
}