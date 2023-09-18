using System.ComponentModel.DataAnnotations;

namespace TapUEntranceChecker.Model
{
    /// <summary>
    /// exam configuration model
    /// </summary>
    public class ExamPassConfiguration
    {
        public int MaxNumberOfStudents { get; set; }
        public int TotalScoreThreshold { get; set; }
        public Dictionary<string, DivisionCondition> DivisionConditions { get; set; }
        public List<string> Subjects { get; set; }
    }
    /// <summary>
    /// division condition model
    /// </summary>
    public class DivisionCondition
    {
        public string Division { get; set; }
        public int DivisionThreshold { get; set; }
        public List<int> SubjectIndexPositions { get; set; }
    }
}
