using System;
using System.Drawing.Drawing2D;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static LoginForm;

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
    private const string UserFilePath = @"C:\Users\HP\Desktop\DATA.txt";

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
            MessageBox.Show("Password must have at least 8 characters, including one uppercase letter and one digit.");
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
        if (password.Length < 8)
        {
            return false; // Password is too short
        }

        bool hasDigit = false;
        bool hasLower = false;
        bool hasUpper = false;

        foreach (char c in password)
        {
            if (char.IsDigit(c))
            {
                hasDigit = true;
            }
            else if (char.IsLower(c))
            {
                hasLower = true;
            }
            else if (char.IsUpper(c))
            {
                hasUpper = true;
            }
        }

        return hasDigit && hasLower && hasUpper;
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
            Image = Image.FromFile(@"C:\Users\HP\Desktop\LOGO.png"),//change the pc name from "HP" to whatever the name of your pc is 
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
        this.Text = "Welcome to Big Bag";
        this.Size = new System.Drawing.Size(400, 200);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.StartPosition = FormStartPosition.CenterScreen;

        //------------------EMAIL TEXTBOX-----------------//
        emailTextBox = new TextBox
        {
            Location = new System.Drawing.Point(120, 20),
            Size = new System.Drawing.Size(200, 20),
        };

        //-------------------PASSWORD TEXTBOX------------------//
        passwordTextBox = new TextBox
        {
            Location = new System.Drawing.Point(120, emailTextBox.Bottom + 20),
            Size = new System.Drawing.Size(200, 20),
            PasswordChar = '*',
        };

        //----------------REGISTER BUTTON-------------------//
        registerButton = new Button
        {
            Text = "Register",
            Location = new System.Drawing.Point(50, passwordTextBox.Bottom + 20),
        };
        registerButton.Click += new EventHandler(RegisterButtonClick);

        //-------------------LOGIN BUTTON----------------//
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
            this.Hide();
            DashboardForm dashboard = new DashboardForm();
            dashboard.Show();
        }
        else
        {
            MessageBox.Show("Login failed. Invalid email or password.");
        }
    }

    //-------------------DASHBOARD CLASS---------------------//
    public class DashboardForm : Form
    {
        public DashboardForm()
        {
            InitializeComponents();
        }

        private string userEmail;
        private string userPassword;

        public DashboardForm(string userEmail, string userPassword)
        {
            this.userEmail = userEmail;
            this.userPassword = userPassword;
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Text = "Dashboard";
            this.Size = new System.Drawing.Size(500, 300);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(44, 62, 80);  // Dark blue background
            this.Font = new Font("Arial", 12);

            Label welcomeLabel = new Label
            {
                Text = "Welcome to Big Bag!",
                Location = new System.Drawing.Point(100, 20),
                Font = new System.Drawing.Font("Arial", 18, FontStyle.Bold),
                AutoSize = true,
            };
                //---------------TRANSACTIONS----------------------------------//
            Button displayTransactionsButton = CreateButton("Display Transactions", 60, 100);
            displayTransactionsButton.BackColor = Color.FromArgb(46, 139, 87);  
            displayTransactionsButton.ForeColor = Color.White;               
            displayTransactionsButton.FlatStyle = FlatStyle.Flat;             
            displayTransactionsButton.Font = new Font("Arial", 12, FontStyle.Bold);


            displayTransactionsButton.Click += (sender, e) =>
            {
                TransactionsForm transactionsForm = new TransactionsForm();
                transactionsForm.Show();
            };
            //-----------------------------------------------------------//
            //-----------------LOG OUT-------------------------------//
            Button logoutButton = CreateButton("Log Out", 280, 100);
            logoutButton.BackColor = Color.FromArgb(192, 57, 43);  
            logoutButton.ForeColor = Color.White;               
            logoutButton.FlatStyle = FlatStyle.Flat;  
            logoutButton.Font = new Font("Arial", 12, FontStyle.Bold);

            logoutButton.Click += (sender, e) =>
            {
                this.Close(); // Close the dashboard form
                LoginForm loginForm = new LoginForm();
                loginForm.Show(); // Show the login form
            };
            //----------------------------------------------//

            //--------------------PROFILE BUTTON CUSTOMIZATIONS-----------------//
            Button profileButton = CreateButton("Profile Management", 280, 150);
            profileButton.BackColor = Color.FromArgb(52, 73, 94);   
            profileButton.ForeColor = Color.White;              
            profileButton.FlatStyle = FlatStyle.Flat;            
            profileButton.Font = new Font("Arial", 12, FontStyle.Bold);
            profileButton.Click += (sender, e) =>
            {
                ProfileManagementForm profileForm = new ProfileManagementForm(userEmail, userPassword);
                profileForm.Show();
            };
            //-------------------------------------------------------------------------//

            this.Controls.Add(welcomeLabel);
            this.Controls.Add(displayTransactionsButton);
            this.Controls.Add(logoutButton);
            this.Controls.Add(profileButton);
        }

        private Button CreateButton(string text, int x, int y)
        {
            return new Button
            {
                Text = text,
                Location = new System.Drawing.Point(x, y),
                Size = new System.Drawing.Size(150, 30),
                Font = new System.Drawing.Font("Arial", 12),
            };
        }

        private void DisplayTransactionsButton_Click(object sender, EventArgs e)
        {
            TransactionsForm transactionsForm = new TransactionsForm();
            transactionsForm.Show();
        }

        private void LogoutButton_Click(object sender, EventArgs e)
        {
            this.Close(); // Close the dashboard form
            LoginForm loginForm = new LoginForm();
            loginForm.Show(); // Show the login form
        }

        private void ProfileButton_Click(object sender, EventArgs e)
        {
            ProfileManagementForm profileForm = new ProfileManagementForm(userEmail, userPassword);
            profileForm.Show();
        }

    }

    //-----------------------------------------------------//

    //---------------TRANSACTION FORM----------------------//
    public class TransactionsForm : Form
    {
        public TransactionsForm()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Text = "Transactions";
            this.Size = new System.Drawing.Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;


            ListBox transactionsListBox = new ListBox
            {
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(360, 200)
            };


            var random = new Random();
            string[] transactions = new string[10];

            for (int i = 1; i <= 10; i++)
            {
                decimal amountInForeignCurrency = random.Next(1, 1000);
                decimal amountInPKR = ConvertToPKR(amountInForeignCurrency);
                transactions[i - 1] = $"Transaction {i}: PKR {amountInPKR:N2}";
            }

            transactionsListBox.Items.AddRange(transactions);

            this.Controls.Add(transactionsListBox);
        }

        private decimal ConvertToPKR(decimal amountInForeignCurrency)
        {
            decimal conversionRate = 280.50M;
            return amountInForeignCurrency * conversionRate;
        }
    }

    //-------------------------------------------------------------------//

    //---------------------PROFILE MANAGMENT----------------------//
    public class ProfileManagementForm : Form
{
    public ProfileManagementForm(string userEmail, string userPassword)
    {
        InitializeComponents(userEmail, userPassword);
    }

    private void InitializeComponents(string userEmail, string userPassword)
    {
        this.Text = "Profile Management";
        this.Size = new System.Drawing.Size(300, 200);
        this.StartPosition = FormStartPosition.CenterScreen;

        TextBox emailTextBox = new TextBox
        {
            Text = userEmail,
            Location = new System.Drawing.Point(120, 20),
            Size = new System.Drawing.Size(200, 20),
        };

        TextBox passwordTextBox = new TextBox
        {
            Text = userPassword,
            Location = new System.Drawing.Point(120, emailTextBox.Bottom + 20),
            Size = new System.Drawing.Size(200, 20),
            PasswordChar = '*',
        };

        Label emailLabel = new Label
        {
            Text = "Email:",
            Location = new System.Drawing.Point(20, 20),
            Font = new System.Drawing.Font("Arial", 12),
        };

        Label passwordLabel = new Label
        {
            Text = "Password:",
            Location = new System.Drawing.Point(20, emailLabel.Bottom + 25),
            Font = new System.Drawing.Font("Arial", 12),
        };

        this.Controls.Add(emailLabel);
        this.Controls.Add(emailTextBox); 
        this.Controls.Add(passwordLabel);
        this.Controls.Add(passwordTextBox); 
    }
}
    //--------------------------------------------------------------//
}