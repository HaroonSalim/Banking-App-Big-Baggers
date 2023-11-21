using System;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;
using static DashboardForm;

public class ExpenseRecordForm : Form
{
    private TextBox amountTextBox;
    private DateTimePicker datePicker;
    private TextBox categoryTextBox;
    private TextBox descriptionTextBox;
    private Button saveButton;
    private string email;
    private readonly DataChangedEventHandler dataChangedCallback;


    public ExpenseRecordForm(string email, DataChangedEventHandler callback)
    {
        this.email = email;
        this.dataChangedCallback = callback;
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        this.Text = "Expense Record";
        this.Size = new System.Drawing.Size(400, 300);
        this.StartPosition = FormStartPosition.CenterScreen;

        Label amountLabel = new Label
        {
            Text = "Amount:",
            Location = new System.Drawing.Point(20, 20),
            Font = new System.Drawing.Font("Arial", 12),
        };

        amountTextBox = new TextBox
        {
            Location = new System.Drawing.Point(120, 20),
            Size = new System.Drawing.Size(200, 20),
        };

        Label dateLabel = new Label
        {
            Text = "Date:",
            Location = new System.Drawing.Point(20, amountTextBox.Bottom + 20),
            Font = new System.Drawing.Font("Arial", 12),
        };

        datePicker = new DateTimePicker
        {
            Location = new System.Drawing.Point(120, amountTextBox.Bottom + 20),
            Size = new System.Drawing.Size(200, 20),
            Format = DateTimePickerFormat.Short,
        };

        Label categoryLabel = new Label
        {
            Text = "Category:",
            Location = new System.Drawing.Point(20, datePicker.Bottom + 20),
            Font = new System.Drawing.Font("Arial", 12),
        };

        categoryTextBox = new TextBox
        {
            Location = new System.Drawing.Point(120, datePicker.Bottom + 20),
            Size = new System.Drawing.Size(200, 20),
        };

        Label descriptionLabel = new Label
        {
            Text = "Description:",
            Location = new System.Drawing.Point(20, categoryTextBox.Bottom + 20),
            Font = new System.Drawing.Font("Arial", 12),
        };

        descriptionTextBox = new TextBox
        {
            Location = new System.Drawing.Point(120, categoryTextBox.Bottom + 20),
            Size = new System.Drawing.Size(200, 20),
        };

        saveButton = new Button
        {
            Text = "Save Expense",
            Location = new System.Drawing.Point(150, descriptionTextBox.Bottom + 30),
            Size = new System.Drawing.Size(150, 30),
            Font = new System.Drawing.Font("Arial", 12),
        };
        saveButton.Click += (sender, e) =>
        {
            // string currentUser = "UsernameOfCurrentUser";  // Replace this with the actual username
            SaveButton_Click(amountTextBox.Text, email);
        };

        this.Controls.Add(amountLabel);
        this.Controls.Add(amountTextBox);
        this.Controls.Add(dateLabel);
        this.Controls.Add(datePicker);
        this.Controls.Add(categoryLabel);
        this.Controls.Add(categoryTextBox);
        this.Controls.Add(descriptionLabel);
        this.Controls.Add(descriptionTextBox);
        this.Controls.Add(saveButton);
    }

    private void SaveButton_Click(string amount, string email)
    {
        if (string.IsNullOrWhiteSpace(amount) || !decimal.TryParse(amount, out decimal parsedAmount) || parsedAmount <= 0)
        {
            MessageBox.Show("Invalid amount. Please enter a positive number.");
            return;
        }

        string filePath = "./database.json";

        try
        {
            string jsonData = File.ReadAllText(filePath);
            List<UserInfo> users = JsonConvert.DeserializeObject<List<UserInfo>>(jsonData) ?? new List<UserInfo>();

            var user = users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                MessageBox.Show("User not found.");
                return;
            }

            if (user.Transactions == null)
            {
                user.Transactions = new List<int>();
            }

            // Expenses are negative, so we add the amount as a negative value
            user.Transactions.Add(-(int)parsedAmount);

            string updatedJsonData = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText(filePath, updatedJsonData);

            dataChangedCallback?.Invoke(this, EventArgs.Empty);
            MessageBox.Show("Expense record saved!");
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show("An error occurred: " + ex.Message);
        }
    }

}