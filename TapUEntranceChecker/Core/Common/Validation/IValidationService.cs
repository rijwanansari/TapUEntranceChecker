using TapUEntranceChecker.Model;

namespace TapUEntranceChecker.Core.Common.Validation
{
    public interface IValidationService
    {
        bool ValidateExamConfiguration(ExamConfiguration examConfig, out List<string> validationErrors);
    }
}
