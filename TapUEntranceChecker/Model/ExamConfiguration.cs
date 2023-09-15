namespace TapUEntranceChecker.Model
{
    public class ExamConfiguration
    {
        public PassingCriteriaConfiguration PassingCriteria { get; set; }
        public string ScienceDivision { get; set; }
        public string HumanitiesDivision { get; set; }
        public int SubjectCount { get; set; }
    }

    public class PassingCriteriaConfiguration
    {
        public int TotalScoreThreshold { get; set; }
        public int ScienceSubjectThreshold { get; set; }
        public int HumanitiesSubjectThreshold { get; set; }
    }
}
