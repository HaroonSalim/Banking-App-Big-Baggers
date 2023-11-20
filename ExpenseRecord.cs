using System;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;

public class ExpenseRecordForm : Form
{
    private TextBox amountTextBox;
    private DateTimePicker datePicker;
    private TextBox categoryTextBox;
    private TextBox descriptionTextBox;
    private Button saveButton;

    public ExpenseRecordForm()
    {
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
        saveButton.Click += new EventHandler(SaveButton_Click);

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

    private void SaveButton_Click(object sender, EventArgs e)
    {
        try
        {
            // Use a verbatim string literal to simplify the path
            string expenseFilePath = "./ExpenseRecords.json";

            // Read existing JSON data from the file
            string jsonData = File.ReadAllText(expenseFilePath);

            // Deserialize existing JSON data into a list of anonymous objects
            List<object> expenseRecords = JsonConvert.DeserializeObject<List<object>>(jsonData) ?? new List<object>();

            // Create a new anonymous object to represent the current expense record
            var expenseRecord = new { Amount = amountTextBox.Text, Date = datePicker.Value, Category = categoryTextBox.Text, Description = descriptionTextBox.Text };

            // Add the current expense record to the list
            expenseRecords.Add(expenseRecord);

            // Serialize the updated list of expense records back to JSON
            string updatedJsonData = JsonConvert.SerializeObject(expenseRecords, Formatting.Indented);

            // Write the updated JSON data back to the file
            File.WriteAllText(expenseFilePath, updatedJsonData);

            MessageBox.Show("Expense saved!");
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred while saving the expense: {ex.Message}");
        }
    }

}