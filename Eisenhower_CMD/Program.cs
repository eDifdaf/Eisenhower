#region Variables
using Eisenhower_CMD;
int userInput;
Users[] users;
bool errorBool = false;

#endregion


Console.WriteLine("Welcome to the Task Manager App!");
#region get users
errorBool = ReadUsers();
if (errorBool) {
    Console.WriteLine("File read successfully");
} else {
    Console.WriteLine("Error reading file");
}
#endregion
userInput = MultipleChoice(true, "Login", "Register", "Exit");
Console.Clear();
LoginMenu(userInput);

void LoginMenu(int userInput) {
    
    
    switch (userInput)
    {
        case 0:
            Console.WriteLine("Login");
            break;
        case 1:
            errorBool = Register();
            if (!errorBool) {
                throw new ApplicationException ("Error registering user");
            }
            break;
        case 2:
            Exit();
            break;
        default:
            Console.WriteLine("Invalid Input");
            break;
    }
}

Console.ReadLine();

//TODO: LOGIN FUNCTION

bool ReadUsers() {
    //TODO: change to only mysql database
    if (!File.Exists(@".\TestFolder\WriteLines2.txt")) {
        using (StreamWriter file = new StreamWriter(@".\TestFolder\WriteLines2.txt")) {
            file.WriteLine("admin,admin");
            users = new Users[1];
            users[0] = new Users();
            users[0].Name = "admin";
            users[0].Password = "admin";
        }
    }
    string[] lines = File.ReadAllLines(@".\TestFolder\WriteLines2.txt");
    //resize the users array to the number of lines in the file
    users = new Users[lines.Length];
    for (int i = 0; i < lines.Length; i++) {
        string[] temp = lines[i].Split(",");
        users[i] = new Users();
        users[i].Name = temp[0];
        users[i].Password = temp[1];
    }
    return true;
}

bool Register() {
    
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
    using (StreamWriter file = new StreamWriter(@".\TestFolder\WriteLines2.txt")) {
        for (int i = 0; i < users.Length; i++) {
            file.WriteLine(users[i].Name + "," + users[i].Password);
        }
    }
    return true;
    
    
}

void Exit() {
    Console.WriteLine("Exiting...");
    Environment.Exit(0);
}

int MultipleChoice(bool canCancel, params string[] options) 
{
        const int startX = 15;
        const int startY = 8;
        const int optionsPerLine = 3;
        const int spacingPerLine = 14;

        int currentSelection = 0;

        ConsoleKey key;

        Console.CursorVisible = false;

        do
        {
            Console.Clear();

            for (int i = 0; i < options.Length; i++)
            {
                Console.SetCursorPosition(startX + (i % optionsPerLine) * spacingPerLine, startY + i / optionsPerLine);

                if(i == currentSelection)
                    Console.ForegroundColor = ConsoleColor.Red;

                Console.Write(options[i]);

                Console.ResetColor();
            }

            key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.LeftArrow:
                {
                    if (currentSelection % optionsPerLine > 0)
                        currentSelection--;
                    break;
                }
                case ConsoleKey.RightArrow:
                {
                    if (currentSelection % optionsPerLine < optionsPerLine - 1)
                        currentSelection++;
                    break;
                }
                case ConsoleKey.UpArrow:
                {
                    if (currentSelection >= optionsPerLine)
                        currentSelection -= optionsPerLine;
                    break;
                }
                case ConsoleKey.DownArrow:
                {
                    if (currentSelection + optionsPerLine < options.Length)
                        currentSelection += optionsPerLine;
                    break;
                }
                case ConsoleKey.Escape:
                {
                    if (canCancel)
                        return -1;
                    break;
                }
            }
        } while (key != ConsoleKey.Enter);

        Console.CursorVisible = true;

        return currentSelection;
}

