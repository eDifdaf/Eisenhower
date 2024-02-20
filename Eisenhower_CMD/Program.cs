using Eisenhower_CMD;

#region Variables

string filePathTasks = @".\TestFolder\Tasks.txt";
string filePathUsers = @".\TestFolder\Users.txt";
int userInput;
Users[] users;
List<Tasks> tasks = new List<Tasks>();
bool errorBool = true;

#endregion


Console.WriteLine("Welcome to the Task Manager App!");
Console.ReadLine();

#region get users&tasks

errorBool = ReadUsers();
if (!errorBool) {
    Console.WriteLine("File read successfully");
    tasks = Tasks.LoadTasksFromCsv(filePathTasks);
}
else {
    Console.WriteLine("Error reading file");
}

#endregion


userInput = MultipleChoice(true, "Login", "Register", "Exit");
Console.Clear();
Menu(filePathTasks);

void Menu(string filePath) {
    switch (userInput) {
        case 0:
            LoginMenu.Login(users);
            break;
        case 1:
            errorBool = LoginMenu.Register(users);
            if (errorBool) {
                throw new ApplicationException("Error registering user");
            }
            errorBool = true;
            break;
        case 2:
            LoginMenu.Exit(tasks, filePath);
            break;
        default:
            Console.WriteLine("Invalid Input");
            break;
    }
}

Console.ReadLine();

#region Methodes

int MultipleChoice(bool canCancel, params string[] options) {
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