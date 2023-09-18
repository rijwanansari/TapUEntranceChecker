using Microsoft.Extensions.Configuration;

namespace TapUEntranceChecker.App_Start
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
