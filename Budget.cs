using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using static DashboardForm;

public class BudgetForm : Form
{
    private TextBox budgetTextBox;
    private Button saveBudgetButton;
    private string email;
    private readonly DataChangedEventHandler dataChangedCallback;

    public BudgetForm(string email, DataChangedEventHandler callback)
    {
        this.email = email;
        this.dataChangedCallback = callback;
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        this.Text = "Budget";
        this.Size = new System.Drawing.Size(300, 200);
        this.StartPosition = FormStartPosition.CenterScreen;

        Label budgetLabel = new Label
        {
            Text = "Set Budget:",
            Location = new System.Drawing.Point(20, 20),
            Font = new System.Drawing.Font("Arial", 12),
        };

        budgetTextBox = new TextBox
        {
            Location = new System.Drawing.Point(120, 20),
            Size = new System.Drawing.Size(150, 20),
        };

        saveBudgetButton = new Button
        {
            Text = "Save Budget",
            Location = new System.Drawing.Point(80, 70),
            Size = new System.Drawing.Size(120, 30),
            Font = new System.Drawing.Font("Arial", 12),
        };
        saveBudgetButton.Click += (sender, e) =>
        {
            SaveBudget(budgetTextBox.Text, email);
        };

        this.Controls.Add(budgetLabel);
        this.Controls.Add(budgetTextBox);
        this.Controls.Add(saveBudgetButton);
    }

    // Update SaveBudget method
    private void SaveBudget(string budget, string email)
    {
        if (string.IsNullOrWhiteSpace(budget) || !int.TryParse(budget, out int parsedBudget) || parsedBudget <= 0)
        {
            MessageBox.Show("Invalid budget. Please enter a positive number.");
            return;
        }

        string filePath = "./database.json";

        try
        {
            string jsonData = File.ReadAllText(filePath);
            List<UserInfo> users = JsonConvert.DeserializeObject<List<UserInfo>>(jsonData) ?? new List<UserInfo>();

            var user = users.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                user.Budget = parsedBudget;
            }
            else
            {
                MessageBox.Show("User not found.");
            }

            string updatedJsonData = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText(filePath, updatedJsonData);

            dataChangedCallback?.Invoke(this, EventArgs.Empty);
            MessageBox.Show("Budget saved!");
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show("An error occurred: " + ex.Message);
        }
    }
}
