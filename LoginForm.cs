using System;
using System.Drawing;
using System.Windows.Forms;

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
        this.FormBorderStyle = FormBorderStyle.Sizable;
        this.Size = new System.Drawing.Size(400, 200);
        this.BackColor = Color.FromArgb(31, 31, 31);


        PictureBox pictureBox = new PictureBox
        {
            Image = Image.FromFile(@"C:\Users\ammar\Desktop\LOGO.png"),//change the pc name from "HP" to whatever the name of your pc is 
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
            TextAlign = ContentAlignment.TopLeft,
            Location = new System.Drawing.Point(88, emailTextBox.Top - 20),
            ForeColor = System.Drawing.Color.White,
        };

        Label passwordLabel = new Label
        {
            Text = "Password:",
            TextAlign = ContentAlignment.TopLeft,
            Location = new System.Drawing.Point(88, emailTextBox.Bottom + 20),
            ForeColor = System.Drawing.Color.White,
        };


        emailTextBox.Location = new System.Drawing.Point(90, emailLabel.Bottom);
        passwordTextBox.Location = new System.Drawing.Point(90, passwordLabel.Bottom);


        registerButton.Location = new System.Drawing.Point(88, passwordTextBox.Bottom + 30);
        registerButton.Padding = new Padding(4, 2, 4, 2);
        loginButton.Location = new System.Drawing.Point(88, registerButton.Bottom + 10);
        loginButton.Padding = new Padding(4, 2, 4, 2);


        this.Controls.Add(emailLabel);
        this.Controls.Add(passwordLabel);


        registerButton.BackColor = System.Drawing.Color.LightBlue;
        registerButton.ForeColor = System.Drawing.Color.Black;

        loginButton.BackColor = System.Drawing.Color.LightGreen;
        loginButton.ForeColor = System.Drawing.Color.Black;



        this.Size = new System.Drawing.Size(400, loginButton.Bottom + 80);


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
        this.FormBorderStyle = FormBorderStyle.Sizable;
        this.Size = new System.Drawing.Size(400, 200);
        this.MaximizeBox = false;
        //this.StartPosition = FormStartPosition.CenterScreen;

        //------------------EMAIL TEXTBOX-----------------//
        emailTextBox = new TextBox
        {
            Location = new System.Drawing.Point(200, 20),
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
            Location = new System.Drawing.Point(40, passwordTextBox.Bottom + 20),
            Size = new System.Drawing.Size(200, 35),
        };
        registerButton.Click += new EventHandler(RegisterButtonClick);

        //-------------------LOGIN BUTTON----------------//
        loginButton = new Button
        {
            Text = "Login",
            Location = new System.Drawing.Point(200, passwordTextBox.Bottom + 20),
            Size = new System.Drawing.Size(200, 35),
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
}