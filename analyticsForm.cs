using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ScottPlot;

public class AnalyticsForm : Form
{
    private string username;
    private List<UserInfo> userData;

    public AnalyticsForm(string username)
    {
        this.username = username;
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        this.Text = "Analytics";
        this.Size = new Size(800, 600);
        this.StartPosition = FormStartPosition.CenterScreen;

        // Getting transactions for the specific user
        var user = GetUserData(username);
        if (user == null)
        {
            MessageBox.Show("User not found.");
            return;
        }

        var transactions = user.Transactions;

        // Converting transactions to double[]
        double[] transactionValues = transactions.Select(t => (double)t.Amount).ToArray();

        // Creating a ScottPlot.WinFormsPlot
        var winFormsPlot = new ScottPlot.FormsPlot();
        winFormsPlot.Location = new Point(10, 10);
        winFormsPlot.Size = new Size(780, 480);

        // Add the control to the form
        this.Controls.Add(winFormsPlot);

        // Plotting the data as separate bar charts for income and expenses
        double[] xValues = new double[transactionValues.Length];
        double[] incomeValues = transactionValues.Select(t => Math.Max(0, t)).ToArray();
        double[] expenseValues = transactionValues.Select(t => Math.Min(0, t)).ToArray();

        for (int i = 0; i < transactionValues.Length; i++)
        {
            xValues[i] = i + 1; // Use transaction index as X value
        }

        winFormsPlot.plt.PlotBar(xValues, incomeValues, label: "Income");
        winFormsPlot.plt.PlotBar(xValues, expenseValues, label: "Expenses");

        // Customizing the appearance
        winFormsPlot.plt.Title($"Transactions for {username}");
        winFormsPlot.plt.YLabel("Amount");
        winFormsPlot.plt.XLabel("Transaction Number");
        winFormsPlot.plt.Legend(location: ScottPlot.legendLocation.upperRight);

        winFormsPlot.Render();
    }

    private UserInfo GetUserData(string username)
    {
        string json = System.IO.File.ReadAllText("database.json");
        List<UserInfo> users = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserInfo>>(json);

        // Finding the user with the specified username
        UserInfo user = users.FirstOrDefault(u => u.Username.Trim().Equals(username.Trim(), StringComparison.OrdinalIgnoreCase));

        return user;
    }
}
