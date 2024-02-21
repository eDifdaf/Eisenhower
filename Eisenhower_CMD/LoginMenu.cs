namespace Eisenhower_CMD;

public class LoginMenu {
    public static void LMenu(string filePathUsers, int userInput, Users[] users, List<Tasks> tasks,
        string filePathTasks, ref string currentLoggedInUser) {
        switch (userInput) {
            case 0:
                ListUsers(users, filePathUsers);
                userInput = MultipleChoice(true, "List Users", "Login", "Register", "Exit");
                Console.Clear();
                LMenu(filePathUsers, userInput, users, tasks, filePathTasks, ref currentLoggedInUser);
                break;
            case 1:
                bool failedLogin = true;
                failedLogin = Login(users, ref currentLoggedInUser);
                if (!failedLogin) {
                    TasksMenu.ListTasksMenu(tasks, filePathTasks, filePathUsers, users, ref currentLoggedInUser);
                }

                break;
            case 2:
                bool errorBool = true;
                errorBool = Register(users, filePathUsers, ref currentLoggedInUser);
                if (errorBool) {
                    throw new ApplicationException("Error registering user");
                }

                TasksMenu.ListTasksMenu(tasks, filePathTasks, filePathUsers, users, ref currentLoggedInUser);
                break;
            case 3:
                Exit(tasks, filePathTasks);
                break;
            default:
                Console.WriteLine("Invalid Input");
                break;
        }
    }

    public static void ListUsers(Users[] users, string filePathUsers) {
        ReadUsers(ref users, filePathUsers);
        Console.WriteLine("Users: \n");
        for (int i = 0; i < users.Length; i++) {
            Console.WriteLine("\t" + i +". " + users[i].Name);
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadLine();
    }

    public static bool Login(Users[] users, ref string currentLoggedInUser) {
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
                currentLoggedInUser = username;
                return false;
            }
        }

        return true;
    }

    private static bool Login(string username, string password, Users[] users, ref string currentLoggedInUser) {
        for (int i = 0; i < users.Length; i++) {
            if (users[i].Name == username && users[i].Password == password) {
                username = users[i].Name;
                currentLoggedInUser = username;
                return false;
            }
        }

        return true;
    }

    public static bool Register(Users[] users, string filePathUsers, ref string currentLoggedInUser) {
        string username = "";
        Console.WriteLine("Enter your username: ");
        do {
            username = Console.ReadLine();
            for (int i = 0; i < users.Length; i++) {
                if (users[i].Name == username) {
                    Console.WriteLine("Username already exists. Please enter another username: ");
                }
            }

            if (username.Any(c => !char.IsLetterOrDigit(c))) {
                Console.WriteLine("Username can only contain letters and numbers. Please enter another username: ");
                username = null;
            }
        } while (username == null || username=="");

        string password;
        Console.WriteLine("Enter your password (can only contain letters and numbers): ");
        do {
            password = Console.ReadLine();
            if (password.Any(c => !char.IsLetterOrDigit(c))) {
                Console.WriteLine("Password can only contain letters and numbers. Please enter another password: ");
                password = null;
            }
        } while (password == null);

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

        return Login(username, password, users, ref currentLoggedInUser);
    }

    public static void Exit(List<Tasks> tasks, string filePath) {
        Console.WriteLine("Exiting...");
        Tasks.SaveTasksToCsv(tasks, filePath);
        Environment.Exit(0);
    }

    public static int MultipleChoice(bool canCancel, params string[] options) {
        const int startX = 15;
        const int startY = 8;

        int currentSelection = 0;

        ConsoleKey key;

        Console.CursorVisible = false;

        do {
            Console.Clear();

            for (int i = 0; i < options.Length; i++) {
                Console.SetCursorPosition(startX, startY + i);

                if (i == currentSelection)
                    Console.ForegroundColor = ConsoleColor.Red;

                Console.Write(options[i]);

                Console.ResetColor();
            }

            key = Console.ReadKey(true).Key;

            switch (key) {
                case ConsoleKey.UpArrow: {
                    if (currentSelection > 0)
                        currentSelection--;
                    break;
                }
                case ConsoleKey.DownArrow: {
                    if (currentSelection < options.Count() - 1)
                        currentSelection++;
                    break;
                }
                case ConsoleKey.C: {
                    if (canCancel)
                        return -1;
                    break;
                }
            }
        } while (key != ConsoleKey.Enter);

        Console.CursorVisible = true;

        return currentSelection;
    }

    public static bool UserExists(string username, Users[] users) {
        for (int i = 0; i < users.Length; i++) {
            if (users[i].Name == username) {
                return true;
            }
        }

        return false;
    }

    public static void ReadUsers( ref Users[] users, string filePathUsers) {
        //TODO: change to only mysql database
        if (!File.Exists(filePathUsers)) {
            using (StreamWriter file = new StreamWriter(filePathUsers)) {
                file.WriteLine("admin,admin");
                file.Flush();
                file.Close();
            }
        }

        string[] lines = File.ReadAllLines(filePathUsers);
        users = new Users[lines.Length];
        for (int i = 0; i < lines.Length; i++) {
            string[] temp = lines[i].Split(",");
            users[i] = new Users();
            users[i].Name = temp[0];
            users[i].Password = temp[1];
        }
    }
}