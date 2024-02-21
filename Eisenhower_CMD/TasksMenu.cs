namespace Eisenhower_CMD;

public class TasksMenu {
    public static void ListTasksMenu(List<Tasks> tasks, string filePathTaks, string filePathUsers, Users[] users,
        ref string currentLoggedInUser) {
        const int startX = 0;
        const int startY = 8;

        int currentSelection = 0;

        ConsoleKey key;

        Console.CursorVisible = false;

        do {
            Console.Clear();
            Console.WriteLine("C - Create new task");
            Console.WriteLine("E - Edit task");
            Console.WriteLine("D - Delete task");
            Console.WriteLine("Q - Quit to the main menu");
            Console.WriteLine("M - Sort to show MY tasks");
            Console.WriteLine("A - Show ALL tasks (sorted by creation date)");
            Console.WriteLine("S - Sort by status");

            if (tasks.Count() == 0) {
                Console.WriteLine("No tasks to display");
            }
            else {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("Nr\tName\t\tImportance\tCompleted\tUser");

                for (int i = 0; i < tasks.Count(); i++) {
                    Console.ResetColor();
                    Console.SetCursorPosition(startX, startY + i);

                    if (i == currentSelection) {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.Gray;
                    }
                    
                    Console.WriteLine(tasks[i].ID + "\t" + tasks[i].Name + "\t" + tasks[i].Importance + "\t" + "\t" + (tasks[i].isCompleted
                        ? "YES"
                        : "NO")+ "\t\t" + tasks[i].UserName);

                    Console.ResetColor();
                }
            }

            key = Console.ReadKey(true).Key;

            switch (key) {
                case ConsoleKey.UpArrow: {
                    if (currentSelection > 0)
                        currentSelection--;
                    break;
                }
                case ConsoleKey.DownArrow: {
                    if (currentSelection < tasks.Count() - 1)
                        currentSelection++;
                    break;
                }
                case ConsoleKey.Enter: {
                    DisplayTask(currentSelection, tasks);
                    ListTasksMenu(tasks, filePathTaks, filePathUsers, users, ref currentLoggedInUser);
                    break;
                }
                case ConsoleKey.C: {
                    Tasks.CreateTask(tasks, filePathTaks, users, ref currentLoggedInUser);
                    break;
                }
                case ConsoleKey.E: {
                    Tasks.EditTasks(tasks, filePathTaks, currentSelection, users, ref currentLoggedInUser);
                    break;
                }
                case ConsoleKey.D: {
                    Console.WriteLine("Are you sure you want to delete this task? (Y/N)");
                    int userInput = LoginMenu.MultipleChoice(true, "Yes", "No");
                    if (userInput == 0) {
                        Tasks.DeleteTask(tasks, filePathTaks, currentSelection, users, ref currentLoggedInUser);
                    }

                    break;
                }
                case ConsoleKey.Q: {
                    int userInput = LoginMenu.MultipleChoice(true, "List Users", "Login", "Register", "Exit");
                    Console.Clear();
                    currentLoggedInUser = "";
                    //TODO: Load Users again
                    LoginMenu.LMenu(filePathUsers, userInput, users, tasks, filePathTaks, ref currentLoggedInUser);
                    break;
                }
                case ConsoleKey.M: {
                    string currUser = currentLoggedInUser;
                    var currentUserTasks = tasks.Where(task => task.UserName == currUser).ToList();
                    tasks.RemoveAll(task => task.UserName == currUser);
                    tasks.InsertRange(0, currentUserTasks);

                    ListTasksMenu(tasks, filePathTaks, filePathUsers, users, ref currentLoggedInUser);
                    break;
                }
                case ConsoleKey.A: {
                    tasks = tasks.OrderBy(task => task.CreationDate).ToList();
                    ListTasksMenu(tasks, filePathTaks, filePathUsers, users, ref currentLoggedInUser);
                    break;
                }
                case ConsoleKey.S: {
                    tasks = tasks.OrderBy(task => task.isCompleted).ThenBy(task => task.CreationDate).ToList();
                    ListTasksMenu(tasks, filePathTaks, filePathUsers, users, ref currentLoggedInUser);
                    break;
                }
            }
        } while (key != ConsoleKey.Escape);

        Console.CursorVisible = true;
    }


    public static void DisplayTask(int index, List<Tasks> tasks) {
        Console.Clear();
        Console.WriteLine("Name:\t\t" + tasks[index].Name);
        Console.WriteLine("Description:\t" + tasks[index].Description);
        Console.WriteLine("Creation Date:\t" + tasks[index].CreationDate);
        Console.WriteLine("Due Date:\t" + tasks[index].DueDate);
        Console.WriteLine("User Name:\t" + tasks[index].UserName);
        Console.WriteLine("Importance:\t" + tasks[index].Importance);
        Console.WriteLine("Is Completed:\t" + tasks[index].isCompleted);
        Console.ReadLine();
    }
}