using System;
using System.Drawing;
using System.Windows.Forms;

public class ProfileManagementForm : Form
{
    public ProfileManagementForm(string userEmail, string userPassword, string usern)
    {
        InitializeComponents(userEmail, userPassword, usern);
    }

    private void InitializeComponents(string userEmail, string userPassword, string user)
    {
        this.Text = "Profile Management";
        this.Size = new System.Drawing.Size(300, 200);
        this.StartPosition = FormStartPosition.CenterScreen;


        TextBox emailTextBox = new TextBox
        {
            Text = userEmail,
            Location = new System.Drawing.Point(120, 20),
            Size = new System.Drawing.Size(200, 20),
            ReadOnly = true,
        };

        TextBox passwordTextBox = new TextBox
        {
            Text = userPassword,
            Location = new System.Drawing.Point(120, emailTextBox.Bottom + 20),
            Size = new System.Drawing.Size(200, 20),
            ReadOnly = true,
            // PasswordChar = '*',
            
        };

        
        TextBox usernameTextBox = new TextBox
        {
            Text = user,
            Location = new System.Drawing.Point(120, passwordTextBox.Bottom + 20),
            Size = new System.Drawing.Size(200, 20),
            ReadOnly = true,
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

        
        Label usernameLabel = new Label
        {
            Text = "Username:",
            Location = new System.Drawing.Point(20, passwordLabel.Bottom + 25),
            Font = new System.Drawing.Font("Arial", 12),
        };
        

        this.Controls.Add(emailLabel);
        this.Controls.Add(emailTextBox);
        this.Controls.Add(passwordLabel);
        this.Controls.Add(passwordTextBox);
        this.Controls.Add(usernameLabel);
        this.Controls.Add(usernameTextBox);
        
    }
    
}
