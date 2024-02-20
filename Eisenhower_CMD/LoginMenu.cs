namespace Eisenhower_CMD;

public class LoginMenu {
    public static void Menu(string filePathUsers, int userInput, Users[] users, List<Tasks> tasks, string filePathTasks) {
        switch (userInput) {
            case 0:
                LoginMenu.ListUsers(users);
                userInput = LoginMenu.MultipleChoice(true, "List Users", "Login", "Register", "Exit");
                Console.Clear();
                Menu(filePathUsers, userInput, users, tasks, filePathTasks);
                break;
            case 1:
                LoginMenu.Login(users);
                TasksMenu.Menu(tasks, filePathTasks, filePathUsers, users);
                break;
            case 2:
                bool errorBool = true;
                errorBool = LoginMenu.Register(users, filePathUsers);
                if (errorBool) {
                    throw new ApplicationException("Error registering user");
                }
                TasksMenu.Menu(tasks, filePathTasks, filePathUsers, users);
                break;
            case 3:
                LoginMenu.Exit(tasks, filePathTasks);
                break;
            default:
                Console.WriteLine("Invalid Input");
                break;
        }
    }
    public static void ListUsers(Users[] users) {
        Console.WriteLine("Listing users...");
        Console.WriteLine("Users:");
        for (int i = 0; i < users.Length; i++) {
            Console.WriteLine(users[i].Name);
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadLine();
    }

    public static bool Login(Users[] users) {
        //make sure the input is valid
        Console.WriteLine("Enter your username: ");
        string username = Console.ReadLine();
        while (username == "" || username == null) {
            Console.WriteLine("Username cannot be empty. Please enter a username: ");
            username = Console.ReadLine();
        }

        Console.WriteLine("Enter your password: ");
        string password = Console.ReadLine();
        while (password == "" || password == null) {
            Console.WriteLine("Password cannot be empty. Please enter a password: ");
            password = Console.ReadLine();
        }

        for (int i = 0; i < users.Length; i++) {
            if (users[i].Name == username && users[i].Password == password) {
                return true;
            }
        }

        return false;
    }

    private static bool Login(string username, string password, Users[] users) {
        for (int i = 0; i < users.Length; i++) {
            if (users[i].Name == username && users[i].Password == password) {
                username = users[i].Name;
                return false;
            }
        }

        return true;
    }

    public static bool Register(Users[] users, string filePathUsers) {
        Console.WriteLine("Enter your username: ");
        string username = Console.ReadLine();
        //check if username already exists and if it exists, ask for another username in a do while loop
        do {
            for (int i = 0; i < users.Length; i++) {
                if (users[i].Name == username) {
                    Console.WriteLine("Username already exists. Please enter another username: ");
                    username = Console.ReadLine();
                }
            }
        } while (username == null);


        Console.WriteLine("Enter your password: ");
        string password = Console.ReadLine();
        //check if the password is null and if it is, ask for another password in a do while loop
        do {
            if (password == null) {
                Console.WriteLine("Password cannot be empty. Please enter a password: ");
                password = Console.ReadLine();
            }
        } while (password == null);

        //resize the users array to the number of lines in the file + 1
        Users[] temp = new Users[users.Length + 1];
        for (int i = 0; i < users.Length; i++) {
            temp[i] = users[i];
        }

        temp[users.Length] = new Users();
        temp[users.Length].Name = username;
        temp[users.Length].Password = password;
        users = temp;
        using (StreamWriter file = new StreamWriter(filePathUsers)) {
            for (int i = 0; i < users.Length; i++) {
                file.WriteLine(users[i].Name + "," + users[i].Password);
            }
        }

        return Login(username, password, users);
    }

    public static void Exit(List<Tasks> tasks, string filePath) {
        Console.WriteLine("Exiting...");
        Tasks.SaveTasksToCsv(tasks, filePath);
        Environment.Exit(0);
    }

    public static int MultipleChoice(bool canCancel, params string[] options) {
        const int startX = 15;
        const int startY = 8;
        const int optionsPerLine = 3;
        const int spacingPerLine = 14;

        int currentSelection = 0;

        ConsoleKey key;

        Console.CursorVisible = false;

        do {
            Console.Clear();

            for (int i = 0; i < options.Length; i++) {
                Console.SetCursorPosition(startX + (i % optionsPerLine) * spacingPerLine, startY + i / optionsPerLine);

                if (i == currentSelection)
                    Console.ForegroundColor = ConsoleColor.Red;

                Console.Write(options[i]);

                Console.ResetColor();
            }

            key = Console.ReadKey(true).Key;

            switch (key) {
                case ConsoleKey.LeftArrow: {
                    if (currentSelection % optionsPerLine > 0)
                        currentSelection--;
                    break;
                }
                case ConsoleKey.RightArrow: {
                    if (currentSelection % optionsPerLine < optionsPerLine - 1)
                        currentSelection++;
                    break;
                }
                case ConsoleKey.UpArrow: {
                    if (currentSelection >= optionsPerLine)
                        currentSelection -= optionsPerLine;
                    break;
                }
                case ConsoleKey.DownArrow: {
                    if (currentSelection + optionsPerLine < options.Length)
                        currentSelection += optionsPerLine;
                    break;
                }
                case ConsoleKey.Escape: {
                    if (canCancel)
                        return -1;
                    break;
                }
            }
        } while (key != ConsoleKey.Enter);

        Console.CursorVisible = true;

        return currentSelection;
    }
}