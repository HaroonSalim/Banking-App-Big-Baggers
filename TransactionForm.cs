using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class TransactionsForm : Form
{
    private string email;

    public TransactionsForm(string email)
    {
        this.email = email;
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

        LoadTransactions(transactionsListBox);

        this.Controls.Add(transactionsListBox);
    }

    private void LoadTransactions(ListBox listBox)
    {
        string filePath = "./database.json";
        try
        {
            string jsonData = File.ReadAllText(filePath);
            List<UserInfo> users = JsonConvert.DeserializeObject<List<UserInfo>>(jsonData) ?? new List<UserInfo>();

            var user = users.FirstOrDefault(u => u.Email == email);
            if (user == null || user.Transactions == null)
            {
                MessageBox.Show("No transactions found for this user.");
                return;
            }

            foreach (int transaction in user.Transactions)
            {
                string transactionType = transaction > 0 ? "Income" : "Expense";
                listBox.Items.Add($"{transactionType}: PKR {Math.Abs(transaction):N2}");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("An error occurred while loading transactions: " + ex.Message);
        }
    }
}
