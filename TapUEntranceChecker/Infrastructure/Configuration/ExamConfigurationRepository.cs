using Microsoft.Extensions.Configuration;
using TapUEntranceChecker.Core.Common.Configuration;
using TapUEntranceChecker.Model;

namespace TapUEntranceChecker.Infrastructure.Configuration
{
    public class ExamConfigurationRepository : IExamConfigurationRepository
    {
        private readonly IConfiguration _configuration;

        public ExamConfigurationRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Get the exam configuration from appsettings.json
        /// </summary>
        /// <returns></returns>
        public ExamPassConfiguration GetConfiguration()
        {
            var examPassConfiguration = new ExamPassConfiguration {
            TotalScoreThreshold = _configuration.GetValue<int>("TotalScoreThreshold"),
            MaxNumberOfStudents = _configuration.GetValue<int>("MaxNumberOfStudents"),
            DivisionConditions = _configuration.GetSection("DivisionConditions").Get<Dictionary<string, DivisionCondition>>(),
            Subjects = _configuration.GetSection("Subjects").Get<List<string>>()
            };
            
            return examPassConfiguration;
        }

    }
}
