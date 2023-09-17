using Microsoft.Extensions.Configuration;
using TapUEntranceChecker.Core.Common.Configuration;
using TapUEntranceChecker.Core.Exam.Dto;
using TapUEntranceChecker.Model;

namespace TapUEntranceChecker.Core.Exam
{
    public class ExamService : IExamService
    {
        private readonly IConfiguration _configuration;
        private readonly IExamConfigurationRepository _configRepository;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="configRepository"></param>
        public ExamService(IConfiguration configuration, IExamConfigurationRepository configRepository)
        {
            _configuration = configuration;
            _configRepository = configRepository;
        }

        /// <summary>
        /// Check if the candidate passed the exam
        /// </summary>
        /// <param name="examineRecord"></param>
        /// <param name="examPassConfiguration"></param>
        /// <returns></returns>
        public bool IsCandidatePassedExam(ExamineRecord examineRecord, ExamPassConfiguration examPassConfiguration)
        {
            try
            {
                var divisionCondition = examPassConfiguration.DivisionConditions[examineRecord.Division];
                int totalScore = examineRecord.SubjectScores.Sum();
                int divisionThreshold = divisionCondition.DivisionThreshold;
                if (totalScore < examPassConfiguration.TotalScoreThreshold)
                {
                    return false;
                }

                int divisionScore = 0;
                foreach (var subjectIndexPosition in divisionCondition.SubjectIndexPositions)
                {
                    divisionScore += examineRecord.SubjectScores[subjectIndexPosition];
                }

                return divisionScore >= divisionThreshold;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
           
        }

    }
}
