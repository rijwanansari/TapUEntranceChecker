using System.ComponentModel.DataAnnotations;
using TapUEntranceChecker.Model;

namespace TapUEntranceChecker.Core.Exam.Dto
{
    public class DivisionValidator : IExamineRecordValidator
    {
        private readonly IExamineRecordValidator _nextValidator;

        public DivisionValidator(IExamineRecordValidator nextValidator)
        {
            _nextValidator = nextValidator;
        }

        public bool Validate(ExamineRecord examineRecord, ExamPassConfiguration examPassConfiguration)
        {
            if (!examPassConfiguration.DivisionConditions.ContainsKey(examineRecord.Division))
            {
                Console.WriteLine("Invalid division for examinee record, division should be s or l.");
                return false;
            }

            return _nextValidator == null || _nextValidator.Validate(examineRecord, examPassConfiguration);
        }
    }

    public class SubjectLengthValidator : IExamineRecordValidator
    {
        private readonly IExamineRecordValidator _nextValidator;

        public SubjectLengthValidator(IExamineRecordValidator nextValidator)
        {
            _nextValidator = nextValidator;
        }

        public bool Validate(ExamineRecord examineRecord, ExamPassConfiguration examPassConfiguration)
        {
            if (examineRecord.SubjectScores.Length != examPassConfiguration.Subjects.Count)
            {
                Console.WriteLine("Invalid input format for examinee record");
                return false;
            }

            return _nextValidator == null || _nextValidator.Validate(examineRecord, examPassConfiguration);
        }
    }

    public class SubjectScoresValidator : IExamineRecordValidator
    {
        private readonly IExamineRecordValidator _nextValidator;

        public SubjectScoresValidator(IExamineRecordValidator nextValidator)
        {
            _nextValidator = nextValidator;
        }

        public bool Validate(ExamineRecord examineRecord, ExamPassConfiguration examPassConfiguration)
        {
            foreach (var subjectScore in examineRecord.SubjectScores)
            {
                if (subjectScore < 0 || subjectScore > 100)
                {
                    Console.WriteLine($"Invalid subject score for examinee record, the score should be 0 and 100.");
                    return false;
                }
            }

            return _nextValidator == null || _nextValidator.Validate(examineRecord, examPassConfiguration);
        }
    }

    public class ExamineRecordValidator : IExamineRecordValidator
    {
        private readonly IExamineRecordValidator _validator;

        public ExamineRecordValidator()
        {
            _validator = new DivisionValidator(
                new SubjectLengthValidator(
                    new SubjectScoresValidator(null)));
        }

        public bool Validate(ExamineRecord examineRecord, ExamPassConfiguration examPassConfiguration)
        {
            return _validator.Validate(examineRecord, examPassConfiguration);
        }
    }
}
