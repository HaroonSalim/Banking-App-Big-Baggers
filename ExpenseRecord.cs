using Newtonsoft.Json;
using static DashboardForm;

public class ExpenseRecordForm : Form
{
    private TextBox amountTextBox;
    private DateTimePicker datePicker;
    private Button saveButton;
    private string email;
    private readonly DataChangedEventHandler dataChangedCallback;

    private readonly int budget;


    public ExpenseRecordForm(string email, int budget, DataChangedEventHandler callback)
    {
        this.email = email;
        this.dataChangedCallback = callback;
        this.budget = budget;
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

        Label descriptionLabel = new Label
        {
            Text = "Description:",
            Location = new System.Drawing.Point(20, datePicker.Bottom + 20),
            Font = new System.Drawing.Font("Arial", 12),
        };

        saveButton = new Button
        {
            Text = "Save Expense",
            Location = new System.Drawing.Point(150, datePicker.Bottom + 30),
            Size = new System.Drawing.Size(150, 30),
            Font = new System.Drawing.Font("Arial", 12),
        };
        saveButton.Click += (sender, e) =>
        {
            SaveButton_Click(amountTextBox.Text, email);
        };

        this.Controls.Add(amountLabel);
        this.Controls.Add(amountTextBox);
        this.Controls.Add(dateLabel);
        this.Controls.Add(datePicker);
        this.Controls.Add(saveButton);
    }

    //Updated
    private void SaveButton_Click(string amount, string email)
    {
        if (string.IsNullOrWhiteSpace(amount) || !int.TryParse(amount, out int parsedAmount) || parsedAmount <= 0)
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

            if (user.Budget < Math.Abs(parsedAmount))
            {
                MessageBox.Show("Expense exceeds the budget. Cannot save.");
                return;
            }

            if (user.Transactions == null)
            {
                user.Transactions = new List<TransactionInfo>();
            }

            // Expenses are negative, so we add the amount as a negative value
            user.Transactions.Add(new TransactionInfo
            {
                Amount = -parsedAmount,
                Date = DateTime.UtcNow // Use UTC time for consistency
            });

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