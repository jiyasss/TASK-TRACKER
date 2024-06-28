using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace test_2
{
    class Validation
    {
        public class DescriptionValidator
        {
            public static string ValidateString(string input)
            {
                while (true)
                {
                    if (!string.IsNullOrWhiteSpace(input) && Regex.IsMatch(input, @"^[a-zA-Z\s]+$"))
                    {
                        return input;
                    }
                    else
                    {
                        Console.WriteLine("Please enter alphabetical letters only.");
                        input = Console.ReadLine();
                    }
                }
            }
        }
        private bool ValidateDate(string date, out DateTime validDate)
        {
            string pattern = @"^(0[1-9]|[12][0-9]|3[01])-(0[1-9]|1[0-2])-\d{4}$";
            if (Regex.IsMatch(date, pattern) && DateTime.TryParseExact(date, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out validDate))
            {
                return true;
            }
            validDate = default(DateTime);
            return false;
        }
    }
    public class Task
    {
        public int TaskId { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsComplete { get; set; }
        public static string directoryPath;

        public static string errorFilePath;

        public override string ToString()
        {
            return $"ID: {TaskId}, Description: {Description}, Due Date: {DueDate}, Complete: {IsComplete}";
        }
        public static void Declare()
        {
            string pathTo = Assembly.GetExecutingAssembly().Location;
            DirectoryInfo Dinfo = new DirectoryInfo(pathTo).Parent.Parent.Parent;
            directoryPath = Path.Combine(Dinfo.FullName, "Output");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            errorFilePath = Path.Combine(directoryPath, "logerror.txt");
        }

        public static void Display()
       {
            List<Task> tasks = new List<Task>();

            while (true)
            {
                Console.WriteLine("WELCOME USER :) ");
                Console.WriteLine("\nMENU IS GIVEN BELOW : ");
                Console.WriteLine("1. Add a new task");
                Console.WriteLine("2. Mark a task as complete");
                Console.WriteLine("3. View all tasks");
                Console.WriteLine("4. View incomplete tasks");
                Console.WriteLine("5. Exit");
                Console.WriteLine("\n----------------------------------------\n");
                Console.Write("Enter your choice from the given MENU : ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddTask(tasks);
                        break;
                    case "2":
                        MarkTaskComplete(tasks);
                        break;
                    case "3":
                        ViewAllTasks(tasks);
                        break;
                    case "4":
                        ViewIncompleteTasks(tasks);
                        break;
                    case "5":
                        return;
                    default:
                        LogError("Invalid menu choice. Please try again :| ");
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
       }
        static void AddTask(List<Task> tasks)
        {
            Console.Write("Enter your Task Description (ALPHABETICAL SENTENCES ONLY): ");
            string description = Console.ReadLine();
            description = Validation.DescriptionValidator.ValidateString(description);

            if (string.IsNullOrWhiteSpace(description))
            {
                LogError("Task description cannot be kept empty.");
                Console.WriteLine("Task description cannot be kept empty.");
                return;
            }

            Console.Write("Enter Due Date (MM-dd-yyyy): ");
            DateTime dueDate;

            while (!DateTime.TryParse(Console.ReadLine(), out dueDate))
            {
                LogError("Invalid date format. PLEASE TRY AGAIN");
                Console.Write("Invalid date format. Please enter Due Date in given format (MM-dd-yyyy): ");
            }

            Task newTask = new Task
            {
                TaskId = tasks.Count + 1,
                Description = description,
                DueDate = dueDate,
                IsComplete = false
            };

            tasks.Add(newTask);
            Console.WriteLine("Task added successfully.");
            Console.WriteLine("\nxxxxxxxxxxxxxxx\n");
        }
        static void MarkTaskComplete(List<Task> tasks)
        {
            Console.Write("Enter Task ID to mark as complete: ");
            int taskId;

            while (!int.TryParse(Console.ReadLine(), out taskId))
            {
                LogError("Invalid Task ID format entered.");
                Console.Write("Invalid input. Enter Task ID to mark as complete: ");
            }

            Task task = tasks.FirstOrDefault(t => t.TaskId == taskId);

            if (task != null)
            {
                task.IsComplete = true;
                Console.WriteLine("Task marked as complete.");
            }
            else
            {
                LogError($"Task with ID {taskId} not found.");
                Console.WriteLine("Task not found.");
                Console.WriteLine("\nxxxxxxxxxxxx\n");
            }
        }
        static void ViewIncompleteTasks(List<Task> tasks)
        {
            Console.WriteLine("Incomplete Tasks:");
            foreach (Task task in tasks.Where(t => !t.IsComplete))
            {
                Console.WriteLine(task);
            }
        }

        static void LogError(string message)
        {
            string logFilePath = "error.txt";
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now}: {message}");
            }
        }

        static void ViewAllTasks(List<Task> tasks)
        {
            Console.WriteLine("All Tasks:");
            foreach (Task task in tasks)
            {
                Console.WriteLine(task);
            }
        }

    }
    
    internal class Program
    {
        static void Main(string[] args)
        {
            Task.Display();
            Console.ReadLine();
        }
    }
}
