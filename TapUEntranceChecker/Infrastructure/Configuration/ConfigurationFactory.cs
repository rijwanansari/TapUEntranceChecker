using Microsoft.Extensions.Configuration;

namespace TapUEntranceChecker.Infrastructure.Configuration
{
    public class ConfigurationFactory
    {
        public static IConfiguration CreateConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }
    }
}
