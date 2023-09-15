using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using TapUEntranceChecker.Model;

namespace TapUEntranceChecker
{
    internal class Program
    {
        private static ExamConfiguration examConfig;
        private static int maxNumberOfStudents = 1000; // can move to config file
        static void Main(string[] args)
        {
            try
            {
                // Check if appsettings.json file exists
                if (!File.Exists("appsettings.json"))
                {
                    Console.WriteLine("Error: The 'appsettings.json' file is missing.");
                    return;
                }

                // Load and bind configuration
                examConfig = LoadAndBindConfiguration();

                // Validate the configuration
                List<string> validationErrors;
                if (!ValidateExamConfiguration(examConfig, out validationErrors))
                {
                    foreach (string error in validationErrors)
                    {
                        Console.WriteLine("Configuration Validation Error: " + error);
                    }
                    return;
                }

                // Get the number of passing candidates
                while (true)
                {
                    GetPassingCandidates();

                    // Ask if the user wants to continue
                    Console.Write("Do you want to process another cycle of examinees? (y/n): ");
                    string response = Console.ReadLine().Trim().ToLower();

                    if (response != "y")
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        /// <summary>
        /// Get the number of passing candidates
        /// </summary>
        private static void GetPassingCandidates()
        {
            try
            {
                // Read the number of examinees
                int N = int.Parse(Console.ReadLine());
                //validate the number of examinees to be in the range of 1 to 100
                if (N < 1 || N > maxNumberOfStudents)
                {
                    Console.WriteLine($"Invalid number of examinees. N should be between 1 and {maxNumberOfStudents}.");
                    return;
                }

                // Initialize a counter for passing candidates
                int passingCandidates = 0;

                // Loop through the examinees
                for (int i = 0; i < N; i++)
                {
                    // Read the input line
                    string[] input = Console.ReadLine().Split(' ');

                    if (!ValidateInput(input, i + 1))
                    {
                        return;
                    }

                    if (IsCandidatePass(input))
                    {
                        passingCandidates++;
                    }
                }

                // Print the number of passing candidates
                Console.WriteLine(passingCandidates);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        /// <summary>
        /// Check if the candidate meets the passing conditions
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static bool IsCandidatePass(string[] input)
        {
            // Check if the candidate meets the passing conditions
            bool passes = false;

            // Extract data from the input
            string division = input[0];
            int[] subjectScores = new int[examConfig.SubjectCount];

            // Parse subject scores
            for (int j = 0; j < examConfig.SubjectCount; j++)
            {
                subjectScores[j] = int.Parse(input[j + 1]);
            }

            // Check the total score
            int totalScore = 0;
            foreach (int score in subjectScores)
            {
                totalScore += score;
            }

            if (totalScore >= examConfig.PassingCriteria.TotalScoreThreshold)
            {
                if ((division == examConfig.ScienceDivision && subjectScores[1] + subjectScores[2] >= examConfig.PassingCriteria.ScienceSubjectThreshold) ||
                    (division == examConfig.HumanitiesDivision && subjectScores[3] + subjectScores[4] >= examConfig.PassingCriteria.HumanitiesSubjectThreshold))
                {
                    passes = true;
                }
            }

            return passes;
        }

        /// <summary>
        /// Validate the input for each examinee
        /// </summary>
        /// <param name="input"></param>
        /// <param name="nth"></param>
        /// <returns></returns>
        private static bool ValidateInput(string[] input, int nth)
        {
            //validate number of subjects
            if (input.Length != examConfig.SubjectCount + 1)
            {
                Console.WriteLine("Invalid input format for examinee " + nth);
                return false;
            }

            //validate division
            if (input[0] != "s" && input[0] != "l")
            {
                Console.WriteLine("Invalid division for examinee " + nth);
                return false;
            }

            //validate subject scores
            int[] subjectScores = new int[examConfig.SubjectCount];
            for (int j = 0; j < examConfig.SubjectCount; j++)
            {
                subjectScores[j] = int.Parse(input[j + 1]);

                if (subjectScores[j] < 0 || subjectScores[j] > 100)
                {
                    Console.WriteLine("Invalid score for subject " + (j + 1) + " of examinee " + nth);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// load and bind exam configuration
        /// </summary>
        /// <returns></returns>
        static ExamConfiguration LoadAndBindConfiguration()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var examConfig = new ExamConfiguration();

            // Use the Get method to access configuration values
            examConfig.PassingCriteria = new PassingCriteriaConfiguration
            {
                TotalScoreThreshold = config.GetValue<int>("PassingCriteria:TotalScoreThreshold"),
                ScienceSubjectThreshold = config.GetValue<int>("PassingCriteria:ScienceSubjectThreshold"),
                HumanitiesSubjectThreshold = config.GetValue<int>("PassingCriteria:HumanitiesSubjectThreshold"),
            };

            examConfig.ScienceDivision = config.GetValue<string>("ScienceDivision");
            examConfig.HumanitiesDivision = config.GetValue<string>("HumanitiesDivision");
            examConfig.SubjectCount = config.GetValue<int>("SubjectCount");

            return examConfig;
        }
        /// <summary>
        /// Validate exam configuration
        /// </summary>
        /// <param name="examConfig"></param>
        /// <param name="validationErrors"></param>
        /// <returns></returns>
        static bool ValidateExamConfiguration(ExamConfiguration examConfig, out List<string> validationErrors)
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