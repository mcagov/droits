using Microsoft.Extensions.Configuration;

namespace Droits.Tests.Helpers;

public class TestFixture
{

    public IConfiguration Configuration {get;}


    protected TestFixture()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        Configuration = builder.Build();
    }

}
