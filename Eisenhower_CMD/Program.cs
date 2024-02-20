string userInput;
string[,] users = new string[1,2];
bool errorBool = false;



Console.WriteLine("Wecome to the Task Manager App!");
//create a file with the user login details if it does not exist
//read the file and write it to the users array
errorBool = ReadUsersFile();
if (errorBool) {
    Console.WriteLine("File read successfully");
} else {
    Console.WriteLine("Error reading file");
}

Console.WriteLine("Do you want to Login or Register?");
Console.WriteLine("1. Login");
Console.WriteLine("2. Register");
Console.WriteLine("3. Exit");
//read the one key input from the user and put it in userInput
userInput = Console.ReadLine();
Login(userInput);


void Login(string userInput) {
    
    
    switch (userInput)
    {
        case "1":
            Console.WriteLine("Login");
            break;
        case "2":
            Console.WriteLine("Register");
            Register();
            break;
        case "3":
            Console.WriteLine("Exit");
            break;
        default:
            Console.WriteLine("Invalid Input");
            break;
    }
}

bool ReadUsersFile() {
    if (!File.Exists(@".\TestFolder\WriteLines2.txt")) {
        using (StreamWriter file = new StreamWriter(@".\TestFolder\WriteLines2.txt")) {
            file.WriteLine("admin,admin");
        }
    }
    string[] lines = File.ReadAllLines(@".\TestFolder\WriteLines2.txt");
    for (int i = 0; i < lines.Length; i++) {
        string[] temp = lines[i].Split(",");
        users[i,0] = temp[0];
        users[i,1] = temp[1];
    }
    return true;
}

bool Register() {
    do {
        Console.WriteLine("Enter your desired username: ");
        string username = Console.ReadLine();
        //check if username already exists
        //check if users is empty
        if (users[0,0] == null) {
            break;
        }
        for (int i = 0; i < users.Length-1; i++) {
            if (users[i, 0] == username) {
                Console.WriteLine("Username already exists");
                errorBool = false;
            }
        }
    }while (!errorBool);

    Console.WriteLine("Please enter");
    return true;
}