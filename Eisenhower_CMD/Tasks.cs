using System;
using System.Collections.Generic;
using System.IO;

namespace Eisenhower_CMD {
    public class TasksManager
{
    // Method to save tasks to a CSV file
    public static void SaveTasksToCsv(List<Tasks> tasks, string filePath)
    {
        // Open the file for writing
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            // Write header
            writer.WriteLine("Name,Description,CreationDate,DueDate,UserName,Password,Importance");

            // Write each task to the file
            foreach (Tasks task in tasks)
            {
                // Serialize the task to a CSV string
                string csvLine = $"{task.Name},{task.Description},{task.CreationDate},{task.DueDate},{task.UserAcc.Name},{task.UserAcc.Password},{task.Importance}";

                // Write the CSV string to the file
                writer.WriteLine(csvLine);
            }
        }
    }

    // Method to load tasks from a CSV file
    public static List<Tasks> LoadTasksFromCsv(string filePath)
    {
        List<Tasks> tasks = new List<Tasks>();

        // Open the file for reading
        using (StreamReader reader = new StreamReader(filePath))
        {
            // Skip the header line
            reader.ReadLine();

            // Read each line of the file
            while (!reader.EndOfStream)
            {
                // Read a line and split it by comma
                string[] parts = reader.ReadLine().Split(',');

                // Create a new Users object
                Users user = new Users
                {
                    Name = parts[4], // User name is at index 4 in CSV line
                    Password = parts[5] // Password is at index 5 in CSV line
                };

                // Create a new Tasks object and populate its properties
                Tasks task = new Tasks
                {
                    Name = parts[0],
                    Description = parts[1],
                    CreationDate = int.Parse(parts[2]),
                    DueDate = int.Parse(parts[3]),
                    UserAcc = user,
                    Importance = (eImportance)Enum.Parse(typeof(eImportance), parts[6])
                };

                // Add the task to the list
                tasks.Add(task);
            }
        }

        return tasks;
    }
}

// Define the Tasks class
public class Tasks
{
    public string Name;
    public string Description;
    public int CreationDate;
    public int DueDate;
    public Users UserAcc;
    public eImportance Importance;
}


// Define the eImportance enum
public enum eImportance
{
    TWO,
    THREE,
    FIVE,
    SEVEN
}
}