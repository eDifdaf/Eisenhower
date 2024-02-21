using System.Runtime.InteropServices.JavaScript;

namespace Eisenhower_CMD {
    public class Tasks {
        public string Name;
        public string Description;
        public DateTime CreationDate;
        public DateTime DueDate;
        public string UserName;
        public eImportance Importance;
        public bool isCompleted;
        public int ID;

        public static void SaveTasksToCsv(List<Tasks> tasks, string filePathTasks) {
            using StreamWriter writer = new StreamWriter(filePathTasks);
            writer.WriteLine("Name,Description,CreationDate,DueDate,UserName,Password,Importance");
            foreach (Tasks task in tasks) {
                string csvLine =
                    $"{task.Name},{task.Description},{task.CreationDate},{task.DueDate},{task.UserName},{task.Importance}, {task.isCompleted}, {task.ID}";
                writer.WriteLine(csvLine);
            }
            writer.Flush();
            writer.Close();
        }

        public static void CreateTask(List<Tasks> tasks, string filePathTasks, Users[] users,
            ref string currentLoggedInUser) {
            string name = "";
            string description = "";
            string userName = "";
            DateTime dueDate = DateTime.Now;
            int daysToAdd;
            int importance;
            Console.Clear();
            do {
                Console.WriteLine("Enter the name of the task(max 8 chars): ");
                name = Console.ReadLine();
            } while (name.Length > 8 || name == "");

            if (name.Length < 8) {
                name = name.PadRight(8, ' ');
            }

            do {
                Console.WriteLine("Enter the description of the task: ");
                description = Console.ReadLine();
            } while (description == "" || description == null);

            do {
                Console.WriteLine("How many days from now is this due: ");
                daysToAdd = int.Parse(Console.ReadLine());
            } while (dueDate == DateTime.Now || dueDate == null);

            dueDate.AddDays(daysToAdd);

            do {
                Console.WriteLine("Users:");
                for (int i = 0; i < users.Length; i++) {
                    Console.WriteLine(users[i].Name);
                }

                Console.WriteLine();
                Console.WriteLine("Enter the user name of the task: ");
                userName = Console.ReadLine();
            } while (userName == "" || userName == null || LoginMenu.UserExists(userName, users) == false);

            do {
                Console.WriteLine("Enter the importance of the task: ");
                importance = LoginMenu.MultipleChoice(true, "2", "3", "5", "7");
            } while (importance == null);

            eImportance createImportance;
            switch (importance) {
                case 0:
                    createImportance = eImportance.TWO;
                    break;
                case 1:
                    createImportance = eImportance.THREE;
                    break;
                case 2:
                    createImportance = eImportance.FIVE;
                    break;
                case 3:
                    createImportance = eImportance.SEVEN;
                    break;
                default:
                    createImportance = eImportance.FIVE;
                    break;
            }

            int id = tasks.Count + 1;

            tasks.Add(new Tasks {
                Name = name,
                Description = description,
                CreationDate = DateTime.Now,
                DueDate = dueDate,
                UserName = userName,
                Importance = createImportance,
                isCompleted = false,
                ID = id
            });
            SaveTasksToCsv(tasks, filePathTasks);
            TasksMenu.ListTasksMenu(tasks, filePathTasks, filePathTasks, users, ref currentLoggedInUser);
        }

        public static void EditTasks(List<Tasks> tasks, string filePath, int editIndex, Users[] users,
            ref string currentLoggedInUser) {
            Console.Clear();
            int selection = LoginMenu.MultipleChoice(true, "Name", "Description", "DueDate", "UserName", "Importance",
                "Completed");
            switch (selection) {
                case 0:
                    Console.Clear();
                    Console.WriteLine("Old Name: " + tasks[editIndex].Name);
                    string name = "";

                    Console.WriteLine("Enter the new name (max 8 chars): ");
                    do {
                        Console.WriteLine("Enter the name of the task(max 8 chars): ");
                        name = Console.ReadLine();
                    } while (name.Length > 8 && name != "");

                    if (name.Length < 8) {
                        name = name.PadRight(8, ' ');
                    }

                    tasks[editIndex].Name = name;
                    SaveTasksToCsv(tasks, filePath);
                    break;
                case 1:
                    Console.Clear();
                    Console.WriteLine("Old Description: " + tasks[editIndex].Description);
                    Console.WriteLine("Enter the new description: ");
                    string description = "";

                    do {
                        Console.WriteLine("Enter the description of the task: ");
                        description = Console.ReadLine();
                    } while (description != "" && description != null);

                    tasks[editIndex].Description = description;
                    SaveTasksToCsv(tasks, filePath);
                    break;
                case 2:
                    Console.Clear();
                    Console.WriteLine("Old DueDate: " + tasks[editIndex].DueDate);

                    int daysToAdd;
                    do {
                        Console.WriteLine("How many days do you want to add/reduce: ");
                        daysToAdd = int.Parse(Console.ReadLine());
                    } while (tasks[editIndex].DueDate.AddDays(daysToAdd) != DateTime.Now &&
                             tasks[editIndex].DueDate != null);

                    tasks[editIndex].DueDate.AddDays(daysToAdd);
                    SaveTasksToCsv(tasks, filePath);
                    break;
                case 3:
                    Console.Clear();
                    Console.WriteLine("Old assigned user: " + tasks[editIndex].UserName);

                    string userName = "";
                    do {
                        Console.WriteLine("Enter the new user: ");
                        userName = Console.ReadLine();
                    } while (userName != "" && userName != null && LoginMenu.UserExists(userName, users) == false);

                    tasks[editIndex].UserName = userName;
                    SaveTasksToCsv(tasks, filePath);
                    break;
                case 4:
                    Console.Clear();
                    Console.WriteLine("Old Importance: " + tasks[editIndex].Importance);
                    do {
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
                    } while (editIndex == -1 && editIndex != null);

                    SaveTasksToCsv(tasks, filePath);
                    break;
                case 5:
                    Console.Clear();
                    tasks[editIndex].isCompleted = !tasks[editIndex].isCompleted;
                    SaveTasksToCsv(tasks, filePath);
                    break;
            }

            SaveTasksToCsv(tasks, filePath);
            TasksMenu.ListTasksMenu(tasks, filePath, filePath, users, ref currentLoggedInUser);
        }

        public static void DeleteTask(List<Tasks> tasks, string filePath, int deleteIndex, Users[] users,
            ref string currentLoggedInUser) {
            Console.Clear();
            tasks.RemoveAt(deleteIndex);
            SaveTasksToCsv(tasks, filePath);
            TasksMenu.ListTasksMenu(tasks, filePath, filePath, users, ref currentLoggedInUser);
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
                            CreationDate = DateTime.Parse(parts[2]),
                            DueDate = DateTime.Parse(parts[3]),
                            UserName = parts[4],
                            Importance = (eImportance) Enum.Parse(typeof(eImportance), parts[5]),
                            isCompleted = bool.Parse(parts[6]),
                            ID = int.Parse(parts[7])
                        };
                        tasks.Add(task);
                    }
                    reader.Close();
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