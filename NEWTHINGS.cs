using System;
using System.Drawing.Drawing2D;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Windows.Forms;

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

//-------------------BASIC USER MANAGER---------------------//
public class UserManager
{
    private const string UserFilePath = @"C:\Users\HP\Desktop\users.txt";

    public void RegisterUser(string username, string password)
    {
        //------------------CHECKING FOR EMAIL FORMAT-------------------//
        if (!IsEmailValid(username))
        {
            MessageBox.Show("Invalid email format.");
            return;
        }

        //------------------CHECKING FOR PASSWORD COMPLEXITY-------------------//
        if (!IsPasswordComplex(password))
        {
            MessageBox.Show("Password should contain at least one uppercase letter, one lowercase letter, one digit, and be at least 8 characters long.");
            return;
        }

        //------------------PASSWORD HASH GENERATION-------------------//
        string passwordHash = GeneratePasswordHash(password);

        var user = new User(username, passwordHash);
        string userData = $"{user.Username}:{user.PasswordHash}";

        File.AppendAllLines(UserFilePath, new[] { userData });

        MessageBox.Show("Registration complete.");
    }


    //-------------------ALL THE LOGIN PROCEDURE FILE CHECKING---------------------//
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
                        return true;
                    }
                }
            }
        }

        return false;
    }


    //-------------------VALID CHARACTER IN THE EMAIL---------------------//
    private bool IsEmailValid(string email)
    {
        return Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
    }


    //-------------------PASSWORD COMPLEXITY---------------------//
    private bool IsPasswordComplex(string password)
    {
        return Regex.IsMatch(password, @"^(?=.\d)(?=.[a-z])(?=.*[A-Z]).{8,}$");
    }

    //----------------PASSWORD HASH---------------------//
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


    //-------------------VERIFY PASSWORD HASH---------------------//
    private bool VerifyPasswordHash(string password, string storedHash)
    {
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


public class Program
{
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new LoginForm());
    }
}

//-------------------LOGIN FORM---------------------//
public class LoginForm : Form
{
    private UserManager userManager = new UserManager();

    private TextBox emailTextBox;
    private TextBox passwordTextBox;
    private Button registerButton;
    private Button loginButton;

    public LoginForm()
    {
        InitializeComponents();
        this.BackColor = System.Drawing.Color.Black;


        PictureBox pictureBox = new PictureBox
        {
            Image = Image.FromFile(@"C:\Users\HP\Desktop\LOGO.png"),
            SizeMode = PictureBoxSizeMode.AutoSize,
            Location = new System.Drawing.Point(-40, 10),
        };
        this.Controls.Add(pictureBox);


        emailTextBox.Location = new System.Drawing.Point(10, pictureBox.Bottom + 20);
        passwordTextBox.Location = new System.Drawing.Point(10, emailTextBox.Bottom + 30);
        registerButton.Location = new System.Drawing.Point(50, passwordTextBox.Bottom + 30);
        loginButton.Location = new System.Drawing.Point(200, passwordTextBox.Bottom + 30);

        //--------------FOR THE HOVER EFFECT-------------------//
        emailTextBox.Text = "Username";
        passwordTextBox.Text = "Password";
        emailTextBox.Enter += TextBox_Enter;
        emailTextBox.Leave += TextBox_Leave;
        passwordTextBox.Enter += TextBox_Enter;
        passwordTextBox.Leave += TextBox_Leave;
        //-----------------------------------------------------//



        emailTextBox.Location = new System.Drawing.Point(-1000, pictureBox.Bottom + 20);
        passwordTextBox.Location = new System.Drawing.Point(-1000, emailTextBox.Bottom + 30);

        Label emailLabel = new Label
        {
            Text = "Email:",
            TextAlign = ContentAlignment.TopCenter, 
            Location = new System.Drawing.Point(120, emailTextBox.Top - 20),
            ForeColor = System.Drawing.Color.White,
        };

        Label passwordLabel = new Label
        {
            Text = "Password:",
            TextAlign = ContentAlignment.TopCenter, 
            Location = new System.Drawing.Point(120, emailLabel.Bottom + 25), 
            ForeColor = System.Drawing.Color.White,
        };


        emailTextBox.Location = new System.Drawing.Point(80, pictureBox.Bottom + 20); 
        passwordTextBox.Location = new System.Drawing.Point(80, emailTextBox.Bottom + 30);


        registerButton.Location = new System.Drawing.Point(100, passwordTextBox.Bottom + 30); 
        loginButton.Location = new System.Drawing.Point(200, passwordTextBox.Bottom + 30); 


        this.Controls.Add(emailLabel);
        this.Controls.Add(passwordLabel);


        registerButton.BackColor = System.Drawing.Color.LightBlue;
        registerButton.ForeColor = System.Drawing.Color.Black;

        loginButton.BackColor = System.Drawing.Color.LightGreen;
        loginButton.ForeColor = System.Drawing.Color.Black;


        this.Size = new System.Drawing.Size(400, loginButton.Bottom + 100);


        this.StartPosition = FormStartPosition.CenterScreen;
    }

    //------------------------THE ENTER AND LEAVE PASSWORD EFFECT-----------//
    private void TextBox_Enter(object sender, EventArgs e)
    {
        TextBox textBox = (TextBox)sender;
        if (textBox.Text == "Username" || textBox.Text == "Password")
        {
            textBox.Text = "";
            textBox.ForeColor = System.Drawing.Color.Black;
        }
    }

    private void TextBox_Leave(object sender, EventArgs e)
    {
        TextBox textBox = (TextBox)sender;
        if (string.IsNullOrWhiteSpace(textBox.Text))
        {
            if (textBox == emailTextBox)
                textBox.Text = "Username";
            else if (textBox == passwordTextBox)
                textBox.Text = "Password";
            textBox.ForeColor = System.Drawing.Color.Gray;
        }
    }
    //-----------------------------------------------------------------------//


    private void InitializeComponents()
    {
        this.Text = "User Registration and Login";
        this.Size = new System.Drawing.Size(400, 200);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.StartPosition = FormStartPosition.CenterScreen;

        emailTextBox = new TextBox
        {
            Location = new System.Drawing.Point(120, 20),
            Size = new System.Drawing.Size(200, 20),
        };

        passwordTextBox = new TextBox
        {
            Location = new System.Drawing.Point(120, emailTextBox.Bottom + 20),
            Size = new System.Drawing.Size(200, 20),
            PasswordChar = '*',
        };

        registerButton = new Button
        {
            Text = "Register",
            Location = new System.Drawing.Point(50, passwordTextBox.Bottom + 20),
        };
        registerButton.Click += new EventHandler(RegisterButtonClick);

        loginButton = new Button
        {
            Text = "Login",
            Location = new System.Drawing.Point(200, passwordTextBox.Bottom + 20),
        };
        loginButton.Click += new EventHandler(LoginButtonClick);

        this.Controls.Add(emailTextBox);
        this.Controls.Add(passwordTextBox);
        this.Controls.Add(registerButton);
        this.Controls.Add(loginButton);
    }

    //--------------REGISTER BUTTON----------------//
    private void RegisterButtonClick(object sender, EventArgs e)
    {
        string email = emailTextBox.Text;
        string password = passwordTextBox.Text;
        userManager.RegisterUser(email, password);
    }

    //--------------LOGIN BUTTON------------------//
    private void LoginButtonClick(object sender, EventArgs e)
    {
        string email = emailTextBox.Text;
        string password = passwordTextBox.Text;

        if (userManager.Login(email, password))
        {
            MessageBox.Show("Login successful!");
        }
        else
        {
            MessageBox.Show("Login failed. Invalid email or password.");
        }
    }
}