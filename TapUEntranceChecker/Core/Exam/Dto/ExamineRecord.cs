using TapUEntranceChecker.Model;

namespace TapUEntranceChecker.Core.Exam.Dto
{
    public class ExamineRecord
    {
        public string Division { get; set; }
        public int[] SubjectScores { get; set; }

        /// <summary>
        /// validate examinee record
        /// </summary>
        /// <param name="examPassConfiguration"></param>
        /// <returns></returns>
        // validate division, subject length, subject scores
        public bool IsValidInput(ExamPassConfiguration examPassConfiguration)
        {
            var validator = new ExamineRecordValidator();
            return validator.Validate(this, examPassConfiguration);
        }
  

    }
}
