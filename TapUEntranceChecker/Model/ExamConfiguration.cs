using System.ComponentModel.DataAnnotations;

namespace TapUEntranceChecker.Model
{
    public class ExamConfiguration
    {
        [Required(ErrorMessage = "PassingCriteria section is missing or incomplete in appsettings.json.")]
        public PassingCriteriaConfiguration PassingCriteria { get; set; }
        [Required(ErrorMessage = "ScienceDivision is missing in appsettings.json.")]
        public string ScienceDivision { get; set; }
        [Required(ErrorMessage = "HumanitiesDivision is missing in appsettings.json.")]
        public string HumanitiesDivision { get; set; }
        [Required(ErrorMessage = "SubjectCount is missing in appsettings.json.")]
        public int SubjectCount { get; set; }
        [Required(ErrorMessage = "MaxNumberOfStudents is missing in appsettings.json.")]
        public int MaxNumberOfStudents { get; set; }
    }

    public class PassingCriteriaConfiguration
    {
        public int TotalScoreThreshold { get; set; }
        public int ScienceSubjectThreshold { get; set; }
        public int HumanitiesSubjectThreshold { get; set; }
    }
}
