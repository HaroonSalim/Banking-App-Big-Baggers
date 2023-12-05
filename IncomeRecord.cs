using Newtonsoft.Json;
using static DashboardForm;


public class IncomeRecordForm : Form
{
    private string email;
    private readonly DataChangedEventHandler dataChangedCallback;

    public IncomeRecordForm(string email, DataChangedEventHandler callback)
    {
        this.email = email;
        this.dataChangedCallback = callback;
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        this.Text = "Income Record";
        this.Size = new System.Drawing.Size(400, 300);
        this.StartPosition = FormStartPosition.CenterScreen;

        Label amountLabel = new Label
        {
            Text = "Amount:",
            Location = new System.Drawing.Point(20, 20),
            Font = new System.Drawing.Font("Arial", 12),
        };

        TextBox amountTextBox = new TextBox
        {
            Location = new System.Drawing.Point(120, 20),
            Size = new System.Drawing.Size(200, 20),
        };

        Label dateLabel = new Label
        {
            Text = "Date Received:",
            Location = new System.Drawing.Point(20, amountTextBox.Bottom + 25),
            Font = new System.Drawing.Font("Arial", 12),
        };

        DateTimePicker datePicker = new DateTimePicker
        {
            Location = new System.Drawing.Point(150, amountTextBox.Bottom + 20),
            Size = new System.Drawing.Size(170, 20),
            Format = DateTimePickerFormat.Short,
        };

        Button saveButton = new Button
        {
            Text = "Save",
            Location = new System.Drawing.Point(150, datePicker.Bottom + 30),
            Size = new System.Drawing.Size(100, 30),
            Font = new System.Drawing.Font("Arial", 12),
        };

        saveButton.Click += (sender, e) =>
        {
            SaveIncomeRecord(amountTextBox.Text, email);
        };

        this.Controls.Add(amountLabel);
        this.Controls.Add(amountTextBox);
        this.Controls.Add(dateLabel);
        this.Controls.Add(datePicker);
        this.Controls.Add(saveButton);
    }

    // Update SaveIncomeRecord method
    private void SaveIncomeRecord(string amount, string email)
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

            if (user.Transactions == null)
            {
                user.Transactions = new List<TransactionInfo>();
            }

            // Updating the Transactions list
            user.Transactions.Add(new TransactionInfo
            {
                Amount = parsedAmount,
                Date = DateTime.UtcNow // Use UTC time for consistency
            });

            string updatedJsonData = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText(filePath, updatedJsonData);

            dataChangedCallback?.Invoke(this, EventArgs.Empty);
            MessageBox.Show("Income record saved!");
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show("An error occurred: " + ex.Message);
        }
    }
}