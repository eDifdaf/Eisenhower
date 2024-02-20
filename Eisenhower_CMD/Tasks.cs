namespace Eisenhower_CMD {
    public class Tasks {
        public string Name;
        public string Description;
        public string CreationDate;
        public string DueDate;
        public string UserName;
        public eImportance Importance;
        public bool isCompleted;

        public static void SaveTasksToCsv(List<Tasks> tasks, string filePathTasks) {
            using (StreamWriter writer = new StreamWriter(filePathTasks)) {
                writer.WriteLine("Name,Description,CreationDate,DueDate,UserName,Password,Importance");
                foreach (Tasks task in tasks) {
                    string csvLine =
                        $"{task.Name},{task.Description},{task.CreationDate},{task.DueDate},{task.UserName},{task.Importance}, {task.isCompleted}";
                    writer.WriteLine(csvLine);
                }
            }
        }

        public static void CreateTask(List<Tasks> tasks, string filePathTasks, Users[] users) {
            Console.Clear();
            Console.WriteLine("Enter the name of the task: ");
            string name = Console.ReadLine();
            Console.WriteLine("Enter the description of the task: ");
            string description = Console.ReadLine();
            Console.WriteLine("Enter the due date of the task: ");
            string dueDate = Console.ReadLine();
            Console.WriteLine("Enter the user name of the task: ");
            string userName = Console.ReadLine();
            Console.WriteLine("Enter the importance of the task: ");
            int importance = LoginMenu.MultipleChoice(true, "2", "3", "5", "7");
            eImportance eImportance = (eImportance) importance;
            tasks.Add(new Tasks {
                Name = name,
                Description = description,
                CreationDate = DateTime.Now.ToString(),
                DueDate = dueDate,
                UserName = userName,
                Importance = eImportance,
                isCompleted = false
            });
            SaveTasksToCsv(tasks, filePathTasks);
            TasksMenu.Menu(tasks, filePathTasks, filePathTasks, users);
        }

        public static void EditTasksToCsv(List<Tasks> tasks, string filePath, int editIndex, Users[] users) {
            Console.Clear();
            int selection = LoginMenu.MultipleChoice(true, "Name", "Description", "DueDate", "UserName", "Importance",
                "Completed");
            switch (selection) {
                case 0:
                    Console.Clear();
                    Console.WriteLine("Old Name: " + tasks[editIndex].Name);
                    Console.WriteLine("Enter the new name: ");
                    tasks[editIndex].Name = Console.ReadLine();
                    SaveTasksToCsv(tasks, filePath);
                    break;
                case 1:
                    Console.Clear();
                    Console.WriteLine("Old Description: " + tasks[editIndex].Description);
                    Console.WriteLine("Enter the new description: ");
                    tasks[editIndex].Description = Console.ReadLine();
                    SaveTasksToCsv(tasks, filePath);
                    break;
                case 2:
                    Console.Clear();
                    Console.WriteLine("Old DueDate: " + tasks[editIndex].DueDate);
                    Console.WriteLine("Enter the new due date: ");
                    tasks[editIndex].DueDate = Console.ReadLine();
                    SaveTasksToCsv(tasks, filePath);
                    break;
                case 3:
                    Console.Clear();
                    Console.WriteLine("Old assigned user: " + tasks[editIndex].UserName);
                    Console.WriteLine("Enter the new user: ");
                    tasks[editIndex].UserName = Console.ReadLine();
                    SaveTasksToCsv(tasks, filePath);
                    break;
                case 4:
                    Console.Clear();
                    Console.WriteLine("Old Importance: " + tasks[editIndex].Importance);
                    Console.WriteLine("Press 'Enter' to go to Importance Menu");
                    Console.ReadLine();
                    int editImportance = LoginMenu.MultipleChoice(true, "2", "3", "5", "7");
                    switch (editImportance) {
                        case 0:
                            tasks[editIndex].Importance = eImportance.TWO;
                            SaveTasksToCsv(tasks, filePath);
                            break;
                        case 1:
                            tasks[editIndex].Importance = eImportance.THREE;
                            SaveTasksToCsv(tasks, filePath);
                            break;
                        case 2:
                            tasks[editIndex].Importance = eImportance.FIVE;
                            SaveTasksToCsv(tasks, filePath);
                            break;
                        case 3:
                            tasks[editIndex].Importance = eImportance.SEVEN;
                            SaveTasksToCsv(tasks, filePath);
                            break;
                    }

                    break;
                case 5:
                    Console.Clear();
                    tasks[editIndex].isCompleted = !tasks[editIndex].isCompleted;
                    SaveTasksToCsv(tasks, filePath);
                    break;
            }

            SaveTasksToCsv(tasks, filePath);
            TasksMenu.Menu(tasks, filePath, filePath, users);
        }

        public static List<Tasks> LoadTasksFromCsv(string filePath) {
            List<Tasks> tasks = new List<Tasks>();
            //check if the file exists othewise create it
            if (!File.Exists(filePath)) {
                File.Create(filePath);
            }
            else {
                using (StreamReader reader = new StreamReader(filePath)) {
                    reader.ReadLine();
                    while (!reader.EndOfStream) {
                        string[] parts = reader.ReadLine().Split(',');

                        Tasks task = new Tasks {
                            Name = parts[0],
                            Description = parts[1],
                            CreationDate = parts[2],
                            DueDate = parts[3],
                            UserName = parts[4],
                            Importance = (eImportance) Enum.Parse(typeof(eImportance), parts[5]),
                            isCompleted = bool.Parse(parts[6])
                        };
                        tasks.Add(task);
                    }
                }
            }

            return tasks;
        }
    }

    public enum eImportance {
        TWO = 2,
        THREE = 3,
        FIVE = 5,
        SEVEN = 7
    }
}