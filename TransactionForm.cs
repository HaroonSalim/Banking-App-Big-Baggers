using System;
using System.Drawing;
using System.Windows.Forms;

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
