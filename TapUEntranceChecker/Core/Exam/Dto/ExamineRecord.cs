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
        public bool IsValidInput(ExamPassConfiguration examPassConfiguration)
        {
            //validate division
            if (!examPassConfiguration.DivisionConditions.ContainsKey(Division))
            {
                Console.WriteLine("Invalid division for examinee record.");
                return false;
            }

            //validate input length
            if (SubjectScores.Length != examPassConfiguration.Subjects.Count)
            {
                Console.WriteLine("Invalid input format for examinee record");
                return false;
            }

            // validate subject scores
            foreach (var subjectScore in SubjectScores)
            {
                if (subjectScore < 0 || subjectScore > 100)
                {
                    Console.WriteLine("Invalid subject score for examinee record");
                    return false;
                }
            }

            return true;
            
        }

    }
}
