using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using TapUEntranceChecker.App_Start;
using TapUEntranceChecker.Core;
using TapUEntranceChecker.Core.Common.Configuration;
using TapUEntranceChecker.Core.Common.Validation;
using TapUEntranceChecker.Core.Exam;
using TapUEntranceChecker.Core.Exam.Dto;
using TapUEntranceChecker.Infrastructure.Configuration;
using TapUEntranceChecker.Model;

namespace TapUEntranceChecker
{
    internal class Program
    {
        private static IExamService examService;
        private static IExamConfigurationRepository examConfigRepository;
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

                var configuration = ConfigurationFactory.CreateConfiguration();

                var serviceProvider = new ServiceCollection()
                .AddSingleton<IConfiguration>(configuration)
                .AddSingleton<IExamConfigurationRepository, ExamConfigurationRepository>()
                .AddScoped<IExamService, ExamService>()
                .AddSingleton<IValidationService, ValidationService>()
                .BuildServiceProvider();

                // exam configuration repository
                examConfigRepository = serviceProvider.GetService<IExamConfigurationRepository>();
              
                //exam service
                examService = serviceProvider.GetService<IExamService>();

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
                ExamPassConfiguration examConfig = examConfigRepository.GetConfiguration();

                // Read the number of examinees
                int N = int.Parse(Console.ReadLine());
                //validate the number of examinees to be in the range of 1 to 100
                if (N < 1 || N > examConfig.MaxNumberOfStudents)
                {
                    Console.WriteLine($"Invalid number of examinees. N should be between 1 and {examConfig.MaxNumberOfStudents}.");
                    return;
                }

                // Initialize a counter for passing candidates
                int passingCandidates = 0;
                ExamineRecord examineRecord;
                for (int i = 0; i < N; i++)
                {
                    // Read the input line
                    string[] input = Console.ReadLine()?.Split(' ');

                    examineRecord = new ExamineRecord
                    {
                        Division = input[0],
                        SubjectScores = Array.ConvertAll(input.Skip(1).ToArray(), int.Parse)
                    };

                    //validate the input data as per the exam configuration
                    if (!examineRecord.IsValidInput(examConfig))
                    {
                        return;
                    }

                    // Check if the candidate passed the exam based on the exam configuration
                    if (examService.IsCandidatePassedExam(examineRecord, examConfig))
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
                return;
            }
        }

    }

}