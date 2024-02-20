namespace Eisenhower_CMD;

public class LoginMenu {
    public static bool Login(Users[] users) {
        //make sure the input is valid
        Console.WriteLine("Enter your username: ");
        string username = Console.ReadLine();
        while (username == null) {
            Console.WriteLine("Username cannot be empty. Please enter a username: ");
            username = Console.ReadLine();
        }

        Console.WriteLine("Enter your password: ");
        string password = Console.ReadLine();
        while (password == null) {
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
                return true;
            }
        }

        return false;
    }

    public static bool Register(Users[] users) {
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

        return Login(username, password, users);
    }

    public static void Exit(List<Tasks> tasks, string filePath) {
        Console.WriteLine("Exiting...");
        Tasks.SaveTasksToCsv(tasks, filePath);
        Environment.Exit(0);
    }
}