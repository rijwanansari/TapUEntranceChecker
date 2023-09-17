using System.ComponentModel.DataAnnotations;
using TapUEntranceChecker.Model;

namespace TapUEntranceChecker.Core.Common.Validation
{
    public class ValidationService : IValidationService
    {
        public bool ValidateExamConfiguration(ExamConfiguration examConfig, out List<string> validationErrors)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(examConfig);

            bool isValid = Validator.TryValidateObject(examConfig, context, results, true);

            validationErrors = new List<string>();

            if (!isValid)
            {
                foreach (var validationResult in results)
                {
                    validationErrors.Add(validationResult.ErrorMessage);
                }
            }

            return isValid;
        }
    }
}
