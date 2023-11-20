using System;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;

public class IncomeRecordForm : Form
{
    public IncomeRecordForm()
    {
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

        Label categoryLabel = new Label
        {
            Text = "Category:",
            Location = new System.Drawing.Point(20, datePicker.Bottom + 25),
            Font = new System.Drawing.Font("Arial", 12),
        };

        TextBox categoryTextBox = new TextBox
        {
            Location = new System.Drawing.Point(120, datePicker.Bottom + 20),
            Size = new System.Drawing.Size(200, 20),
        };

        Button saveButton = new Button
        {
            Text = "Save",
            Location = new System.Drawing.Point(150, categoryTextBox.Bottom + 30),
            Size = new System.Drawing.Size(100, 30),
            Font = new System.Drawing.Font("Arial", 12),
        };

        saveButton.Click += (sender, e) =>
        {
            SaveIncomeRecord(amountTextBox.Text, datePicker.Value, categoryTextBox.Text);
            MessageBox.Show("Income record saved!");
            this.Close();
        };

        this.Controls.Add(amountLabel);
        this.Controls.Add(amountTextBox);
        this.Controls.Add(dateLabel);
        this.Controls.Add(datePicker);
        this.Controls.Add(categoryLabel);
        this.Controls.Add(categoryTextBox);
        this.Controls.Add(saveButton);
    }

    private void SaveIncomeRecord(string amount, DateTime date, string category)
    {
        string filePath = "./IncomeRecords.json";

        // Create an anonymous object to represent the income record
        var incomeRecord = new { Amount = amount, Date = date, Category = category };

        // Serialize the income record to JSON
        string record = JsonConvert.SerializeObject(incomeRecord);

        // Append the JSON data to the file
        File.AppendAllText(filePath, record + Environment.NewLine);
    }
}