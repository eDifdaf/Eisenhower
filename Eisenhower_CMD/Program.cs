using Eisenhower_CMD;
using Microsoft.Win32.SafeHandles;

#region Variables

string filePathTasks = @".\TestFolder\Tasks.txt";
string filePathUsers = @".\TestFolder\Users.txt";
string currentLoggedInUser = "";

int userInput;
Users[] users = new Users[1];
users[0] = new Users() { Name = "admin", Password = "admin" };
List<Tasks> tasks = new List<Tasks>();

#endregion


Console.WriteLine("Welcome to the Task Manager App!");
Console.ReadLine();

#region get users&tasks

LoginMenu.ReadUsers(ref users, filePathUsers);
tasks = Tasks.LoadTasksFromCsv(filePathTasks);

#endregion

while (true) {
    userInput = LoginMenu.MultipleChoice(true, "List Users", "Login", "Register", "Exit");
    Console.Clear();
    LoginMenu.LMenu(filePathUsers, userInput, users, tasks, filePathTasks, ref currentLoggedInUser);
}





