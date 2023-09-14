using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.IO.Pipes;

namespace TapUEntranceChecker
{
    internal class Program
    {
        // private variables
        private static int totalScoreThreshold;
        private static int scienceSubjectThreshold;
        private static int humanitiesSubjectThreshold;
        private static int subjectCount;
        private static string scienceDivision = "s";
        private static string humanitiesDivision = "l";
        private static int maxNumberOfStudents = 1000; // move to config file
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

                // Setup configuration
                IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                // Read configuration values with null validation and default values
                totalScoreThreshold = config.GetValue<int>("PassingCriteria:TotalScoreThreshold", 350);
                scienceSubjectThreshold = config.GetValue<int>("PassingCriteria:ScienceSubjectThreshold", 160);
                humanitiesSubjectThreshold = config.GetValue<int>("PassingCriteria:HumanitiesSubjectThreshold", 160);
                subjectCount = config.GetValue<int>("PassingCriteria:SubjectCount", 5);
                scienceDivision = config.GetValue<string>("ScienceDivision", "s");
                humanitiesDivision = config.GetValue<string>("HumanitiesDivision", "l");

                // Get the number of passing candidates
                while (true)
                {
                    GetPassingCandidates();

                    // Ask if the user wants to continue
                    Console.Write("Do you want to process another cycle of examinees? (y/n): ");
                    string response = Console.ReadLine().Trim().ToLower();

                    if (response != "y")
                    {
                        break; // Exit the loop if the user enters anything other than "y"
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        //create a function to check if the student passed or not
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

                    if(!ValidateInput(input, i + 1))
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

        private static bool IsCandidatePass(string[] input)
        {
            // Check if the candidate meets the passing conditions
            bool passes = false;

            // Extract data from the input
            string division = input[0];
            int[] subjectScores = new int[subjectCount];
           
            // Parse subject scores
            for (int j = 0; j < subjectCount; j++)
            {
                subjectScores[j] = int.Parse(input[j + 1]);
            }

            // Check the total score
            int totalScore = 0;
            foreach (int score in subjectScores)
            {
                totalScore += score;
            }

            if (totalScore >= totalScoreThreshold)
            {
                if ((division == "s" && subjectScores[1] + subjectScores[2] >= scienceSubjectThreshold) ||
                    (division == "l" && subjectScores[3] + subjectScores[4] >= humanitiesSubjectThreshold))
                {
                    passes = true;
                }
            }

            return passes;
        }

        private static bool ValidateInput(string[] input, int nth)
        {
            //validate number of subjects
            if (input.Length != subjectCount + 1)
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
            int[] subjectScores = new int[subjectCount];
            for (int j = 0; j < subjectCount; j++)
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
    }
}