using TapUEntranceChecker.Model;

namespace TapUEntranceChecker.Core.Exam.Dto
{
    public interface IExamineRecordValidator
    {
        bool Validate(ExamineRecord examineRecord, ExamPassConfiguration examPassConfiguration);
    }

}