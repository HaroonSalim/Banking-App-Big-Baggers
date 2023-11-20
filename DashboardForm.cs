using System;
using System.Drawing;
using System.Windows.Forms;

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
