using Eisenhower_CMD;
using Microsoft.Win32.SafeHandles;

#region Variables

//TODO: check if folder exists, if not create, else initialize with folder values

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
LoginMenu.ReadTasks(ref tasks, filePathTasks);
//tasks = Tasks.LoadTasksFromCsv(filePathTasks);  //TODO: was wenn nicht da?

#endregion

while (true) {
    userInput = LoginMenu.MultipleChoice(true, "List Users", "Login", "Register", "Exit");
    Console.Clear();
    LoginMenu.LMenu(filePathUsers, userInput, users, tasks, filePathTasks, ref currentLoggedInUser);
}





