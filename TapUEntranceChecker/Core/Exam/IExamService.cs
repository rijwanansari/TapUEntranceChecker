using TapUEntranceChecker.Core.Exam.Dto;
using TapUEntranceChecker.Model;

namespace TapUEntranceChecker.Core.Exam
{
    public interface IExamService
    {
        bool IsCandidatePassedExam(ExamineRecord examineRecord, ExamPassConfiguration examPassConfiguration);
    }
}
