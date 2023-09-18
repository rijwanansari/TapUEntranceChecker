using TapUEntranceChecker.Model;

namespace TapUEntranceChecker.Core.Common.Validation
{
    public interface IValidationService
    {
        bool ValidateExamConfiguration(ExamPassConfiguration examConfig, out List<string> validationErrors);
    }
}
