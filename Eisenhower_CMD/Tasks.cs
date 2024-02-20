namespace Eisenhower_CMD {
    public class Tasks {
        public string Name;
        public string Description;
        public int CreationDate;
        public int DueDate;
        public string UserName;
        public eImportance Importance;

        public static void SaveTasksToCsv(List<Tasks> tasks, string filePath) {
            using (StreamWriter writer = new StreamWriter(filePath)) {
                writer.WriteLine("Name,Description,CreationDate,DueDate,UserName,Password,Importance");
                foreach (Tasks task in tasks) {
                    string csvLine =
                        $"{task.Name},{task.Description},{task.CreationDate},{task.DueDate},{task.UserName},{task.Importance}";
                    writer.WriteLine(csvLine);
                }
            }
        }
        
        public static List<Tasks> LoadTasksFromCsv(string filePath) {
            List<Tasks> tasks = new List<Tasks>();
            //check if the file exists othewise create it
            if (!File.Exists(filePath)) {
                File.Create(filePath);
            }else {
                using (StreamReader reader = new StreamReader(filePath)) {
                    reader.ReadLine();
                    while (!reader.EndOfStream) {
                        string[] parts = reader.ReadLine().Split(',');

                        Tasks task = new Tasks {
                            Name = parts[0],
                            Description = parts[1],
                            CreationDate = int.Parse(parts[2]),
                            DueDate = int.Parse(parts[3]),
                            UserName = parts[4],
                            Importance = (eImportance) Enum.Parse(typeof(eImportance), parts[5])
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