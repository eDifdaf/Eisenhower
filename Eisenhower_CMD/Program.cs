using Eisenhower_CMD;

#region Variables

string filePathTasks = @".\TestFolder\Tasks.txt";
string filePathUsers = @".\TestFolder\Users.txt";
int userInput;
Users[] users;
List<Tasks> tasks = new List<Tasks>();

#endregion


Console.WriteLine("Welcome to the Task Manager App!");
Console.ReadLine();

#region get users&tasks

ReadUsers();
tasks = Tasks.LoadTasksFromCsv(filePathTasks);

#endregion

userInput = LoginMenu.MultipleChoice(true, "List Users", "Login", "Register", "Exit");
Console.Clear();
LoginMenu.Menu(filePathUsers, userInput, users, tasks, filePathTasks);

Console.ReadLine();

#region Methodes

bool ReadUsers() {
    //TODO: change to only mysql database
    if (!File.Exists(filePathUsers)) {
        using (StreamWriter file = new StreamWriter(filePathUsers)) {
            file.WriteLine("admin,admin");
            users = new Users[1];
            users[0] = new Users();
            users[0].Name = "admin";
            users[0].Password = "admin";
        }
    }

    string[] lines = File.ReadAllLines(filePathUsers);
    //resize the users array to the number of lines in the file
    users = new Users[lines.Length];
    for (int i = 0; i < lines.Length; i++) {
        string[] temp = lines[i].Split(",");
        users[i] = new Users();
        users[i].Name = temp[0];
        users[i].Password = temp[1];
    }

    return false;
}

#endregion