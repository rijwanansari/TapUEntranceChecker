using TapUEntranceChecker.Model;

namespace TapUEntranceChecker.Core.Common.Configuration
{
    public interface IExamConfigurationRepository
    {
        ExamPassConfiguration GetConfiguration();
    }
}
