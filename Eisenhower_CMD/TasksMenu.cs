namespace Eisenhower_CMD;

public class TasksMenu {
    public static void Menu(List<Tasks> tasks, string filePathTaks, string filePathUsers, Users[] users) {
        const int startX = 0;
        const int startY = 8;

        int currentSelection = 0;

        ConsoleKey key;

        Console.CursorVisible = false;

        do {
            Console.Clear();
            Console.WriteLine("Press C to create a new task");
            Console.WriteLine("Press E to edit a task");
            Console.WriteLine("Press Q to to go back to the main menu");
            Console.WriteLine("Press M to show takss that are assigned to you");

            if (tasks.Count() == 0) {
                Console.WriteLine("No tasks to display");
            }
            else {
                Console.WriteLine("Name\tImportance\tCompleted");
                for (int i = 0; i < tasks.Count(); i++) {
                    Console.SetCursorPosition(startX, startY + i);

                    if (i == currentSelection)
                        Console.ForegroundColor = ConsoleColor.Red;
                    string completed = tasks[i].isCompleted ? "YES" : "NO";
                    Console.WriteLine(tasks[i].Name + "\t" + tasks[i].Importance + "\t" +
                                      completed);

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
                    Menu(tasks, filePathTaks, filePathUsers, users);
                    break;
                }
                case ConsoleKey.C: {
                    Tasks.CreateTask(tasks, filePathTaks, users);
                    break;
                }
                case ConsoleKey.E: {
                    Tasks.EditTasksToCsv(tasks, filePathTaks, currentSelection, users);
                    break;
                }
                case ConsoleKey.Q: {
                    int userInput = LoginMenu.MultipleChoice(true, "List Users", "Login", "Register", "Exit");
                    Console.Clear();
                    LoginMenu.Menu(filePathUsers, userInput, users, tasks, filePathTaks);
                    break;
                }
            }
        } while (key != ConsoleKey.Escape);

        Console.CursorVisible = true;
    }

    public static void DisplayTask(int index, List<Tasks> tasks) {
        Console.Clear();
        Console.WriteLine("Name: " + tasks[index].Name);
        Console.WriteLine("Description: " + tasks[index].Description);
        Console.WriteLine("Creation Date: " + tasks[index].CreationDate);
        Console.WriteLine("Due Date: " + tasks[index].DueDate);
        Console.WriteLine("User Name: " + tasks[index].UserName);
        Console.WriteLine("Importance: " + tasks[index].Importance);
        Console.WriteLine("Is Completed: " + tasks[index].isCompleted);
        Console.ReadLine();
    }
}