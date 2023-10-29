using System;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

public class User
{
    public string Username
    {
        get;
        set;
    }

    public string PasswordHash
    {
        get;
        set;
    }

    public User(string username, string passwordHash)
    {
        Username = username;
        PasswordHash = passwordHash;
    }
}

public class UserManager
{
    // Text file to store user data
    private const string UserFilePath = @"C:\Users\HP\Desktop\users.txt";



    //--------------------USER REGISTRATION------------------------//
    public void RegisterUser(string username, string password)
    {
        if (!IsEmailValid(username))
        {
            Console.WriteLine("Invalid email format.");
            return;
        }

        if (!IsPasswordComplex(password))
        {
            Console.WriteLine("Password should contain at least one uppercase letter, one lowercase letter, one digit, and be at least 8 characters long.");
            return;
        }

        // Generate a secure password hash using PBKDF2
        string passwordHash = GeneratePasswordHash(password);

        var user = new User(username, passwordHash);
        string userData = $"{user.Username}:{user.PasswordHash}";

        File.AppendAllLines(UserFilePath, new[] { userData });

        Console.WriteLine("Registration complete.\n");
    }


    //--------------------LOGIN FUNCTION-----------------------//
    public bool Login(string username, string password)
    {
        if (File.Exists(UserFilePath))
        {
            string[] lines = File.ReadAllLines(UserFilePath);
            foreach (var line in lines)
            {
                string[] parts = line.Split(':');
                if (parts.Length == 2)
                {
                    string storedUsername = parts[0];
                    string storedPasswordHash = parts[1];

                    if (storedUsername == username && VerifyPasswordHash(password, storedPasswordHash))
                    {
                        return true; // User found and password matches
                    }
                }
            }
        }

        return false; // User not found or password doesn't match
    }



    //---------------------------EMAIL FORMAT-------------------//
    private bool IsEmailValid(string email)
    {
        // Use a simple regular expression to validate email format
        return Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
    }


    //-----------------------PASSWORD COMPLEXITY------------------//
    private bool IsPasswordComplex(string password)
    {
        // Check if the password meets complexity requirements
        return Regex.IsMatch(password, @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}$");
    }



    //-------------------GENERATING THE HASH-----------------------//
    private string GeneratePasswordHash(string password)
    {
        byte[] salt;
        new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
        byte[] hash = pbkdf2.GetBytes(20);

        byte[] saltAndHash = new byte[36];
        Array.Copy(salt, 0, saltAndHash, 0, 16);
        Array.Copy(hash, 0, saltAndHash, 16, 20);

        return Convert.ToBase64String(saltAndHash);
    }


    //--------------------------------------------------------------//



    //-------------------PASSWORD HASH VERIFICATION--------------------//
    private bool VerifyPasswordHash(string password, string storedHash)
    {
        // Verify a password against a stored password hash
        byte[] hashBytes = Convert.FromBase64String(storedHash);
        byte[] salt = new byte[16];
        Array.Copy(hashBytes, 0, salt, 0, 16);
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);

        byte[] hash = pbkdf2.GetBytes(20);

        for (int i = 0; i < 20; i++)
        {
            if (hashBytes[i + 16] != hash[i])
            {
                return false;
            }
        }

        return true;
    }
}
//---------------------------------------------------------//

public class Program
{
    public static void Main()
    {
        var userManager = new UserManager();

        while (true)
        {
            //--------------------REGISTRATION MENU------------------//
            Console.WriteLine("-----Welcome to User Registration-------");
            Console.Write("Please enter email: ");
            string regEmail = Console.ReadLine();

            Console.Write("Please enter password: ");
            string regPassword = Console.ReadLine(); //Reading password normally at the time of registration

            userManager.RegisterUser(regEmail, regPassword);

            //------------------------LOGIN MENU---------------//
            Console.WriteLine("---------Welcome to User Login----------");
            Console.Write("Please enter email: ");
            string loginEmail = Console.ReadLine();

           
            Console.Write("Please enter password: ");
            string loginPassword = MaskPasswordInput();//Login password masking

            //------------CHECKING FOR THE LOGIN EMAIL AND PASSWORD----------------//
            if (userManager.Login(loginEmail, loginPassword))
            {
                Console.WriteLine("Login successful!");
            }
            else
            {
                Console.WriteLine("Login failed. Invalid email or password.");
            }

            //----------------ASKING IF USER WISHES TO LOGIN/REGISTER AGAIN-------------//
            Console.WriteLine("Do you want to register/login again? (y/n): ");
            string choice = Console.ReadLine().ToLower();

            if (choice != "y")
            {
                break;
            }
        }
    }

    //---------------FOR MASKING THE PASSWORD----------------//
    private static string MaskPasswordInput()
    {
        string password = "";
        ConsoleKeyInfo keyInfo;

        do
        {
            keyInfo = Console.ReadKey(intercept: true);

            if (keyInfo.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                // Backspace removing character but not being added to password
                Console.Write("\b \b");
                password = password.Substring(0, password.Length - 1);
            }
            else if (keyInfo.Key != ConsoleKey.Enter)
            {
                // Mask the password with an asterisk and add the character to the password
                Console.Write("*");
                password += keyInfo.KeyChar;
            }
        } while (keyInfo.Key != ConsoleKey.Enter);

        Console.WriteLine(); // Newline after password entry
        return password;
    }

}
