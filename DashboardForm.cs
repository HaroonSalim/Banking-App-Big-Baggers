using System;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json;

public class DashboardForm : Form
{
    private Label cardText;
    public delegate void DataChangedEventHandler(object sender, EventArgs e);
    public event DataChangedEventHandler DataChanged;

    private void OnDataChanged()
    {
        DataChanged?.Invoke(this, EventArgs.Empty);
    }
    private string username;
    private string email;
    private string password;

    public DashboardForm(string username, string email, string password)
    {
        this.username = username;
        this.email = email;
        this.password = password;
        InitializeComponents();
        this.DataChanged += (s, e) => UpdateTotalAmount(email);
    }

    private void InitializeComponents()
    {
        this.Text = "Dashboard";
        this.Size = new System.Drawing.Size(1200, 600);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.FromArgb(44, 62, 80);  // Dark blue background
        this.Font = new Font("Arial", 12);

        // Main layout panel
        Panel mainPanel = new Panel
        {
            Dock = DockStyle.Fill
        };
        this.Controls.Add(mainPanel);

        // Tabs panel
        Panel tabsPanel = new Panel
        {
            Size = new Size(this.Width / 4, this.Height),
            BackColor = Color.Gray  // Just for visualization
        };
        mainPanel.Controls.Add(tabsPanel);

        // Adding tabs
        Button displayTransactionsButton = new Button { Text = "Transactions", Size = new Size(tabsPanel.Width, 60), Location = new Point(0, 0) };
        Button profileButton = new Button { Text = "Profile", Size = new Size(tabsPanel.Width, 60), Location = new Point(0, 60) };
        Button logoutButton = new Button { Text = "Logout", Size = new Size(tabsPanel.Width, 60), Location = new Point(0, 120) };
        Button budgetButton = new Button { Text = "Budget", Size = new Size(tabsPanel.Width, 60), Location = new Point(0, 180) }; 
        Button analyticsButton = new Button { Text = "Analytics", Size = new Size(tabsPanel.Width, 60), Location = new Point(0, 240) };
        tabsPanel.Controls.Add(displayTransactionsButton);
        tabsPanel.Controls.Add(profileButton);
        tabsPanel.Controls.Add(logoutButton);
        tabsPanel.Controls.Add(budgetButton);
        tabsPanel.Controls.Add(analyticsButton);

        // will the analytics button be visible to all?: Yes

        displayTransactionsButton.Click += (sender, e) =>
        {
            TransactionsForm transactionsForm = new TransactionsForm(email);
            transactionsForm.Show();
        };

        profileButton.Click += (sender, e) =>
        {
            ProfileManagementForm profileForm = new ProfileManagementForm(email, password,username);
            profileForm.Show();
        };

        logoutButton.Click += (sender, e) =>
        {
            this.Close(); // Close the dashboard form
            LoginForm loginForm = new LoginForm();
            loginForm.Show(); // Show the login form
        };

        budgetButton.Click += (sender, e) =>
        {
            BudgetForm budgetForm = new BudgetForm(email, DataChanged);
            budgetForm.Show();
            UpdateTotalAmount(email);
            cardText.Refresh();
        };

        analyticsButton.Click += (sender, e) =>
        {
            AnalyticsForm analyticsForm = new AnalyticsForm(username);
            analyticsForm.Show();
        };

        // Card panel
        Panel cardPanel = new Panel
        {
            Size = new Size((this.Width * 3) / 4, this.Height),
            Location = new Point(tabsPanel.Width, 0),
            BackColor = Color.White  // Just for visualization
        };
        mainPanel.Controls.Add(cardPanel);

        // Smaller card panel for content and buttons
        Panel contentCard = new Panel
        {
            Size = new Size(500, 150), // Adjust size as needed
            Location = new Point((cardPanel.Width - 500) / 2, 80), // Positioned at the top middle
            BackColor = Color.LightGray  // For visualization
        };
        cardPanel.Controls.Add(contentCard);

        // Label outside but on top of the smaller card
        Label cardTitle = new Label
        {
            Text = "Hello, " + username + "!",
            AutoSize = true,
            Location = new Point(contentCard.Location.X + 20, 40),
            Font = new Font("Arial", 16),
            ForeColor = Color.Black
        };
        cardPanel.Controls.Add(cardTitle);

        // Content in contentCard panel
        cardText = new Label
        {
            Text = "PKR ",
            AutoSize = true,
            Location = new Point(20, 20),
            Font = new Font("Arial", 32, FontStyle.Bold)
        };
        contentCard.Controls.Add(cardText);
        UpdateTotalAmount(email);
        cardText.Refresh();

        // Place buttons horizontally
        Button incomeButton = new Button { Text = "Add Income", Size = new Size(222, 50), Location = new Point(20, cardText.Bottom + 10) };
        Button expenseButton = new Button { Text = "Add Expense", Size = new Size(222, 50), Location = new Point(incomeButton.Right + 10, cardText.Bottom + 10) };
        contentCard.Controls.Add(incomeButton);
        contentCard.Controls.Add(expenseButton);

        // Add event handlers
        incomeButton.Click += (sender, e) =>
        {
            IncomeRecordForm incomeRecordForm = new IncomeRecordForm(email, DataChanged);
            incomeRecordForm.Show();
            UpdateTotalAmount(email);
            cardText.Refresh();
        };

        expenseButton.Click += (sender, e) =>
        {
            decimal userBudget = GetUserBudget(email);
            ExpenseRecordForm expenseForm = new ExpenseRecordForm(email,userBudget,DataChanged);
            expenseForm.Show();
            UpdateTotalAmount(email);
            cardText.Refresh();
        };

        // Add a transaction list with 5 dummy transactions
        Panel transactionList = new Panel
        {
            Size = new Size(500, 300),
            Location = new Point((cardPanel.Width - 500) / 2, contentCard.Bottom + 20),
            BackColor = Color.LightGray
        };

        // Add a label to the transaction list
        Label transactionListLabel = new Label
        {
            Text = "Recent Transactions",
            AutoSize = true,
            Location = new Point(20, 20),
            Font = new Font("Arial", 16),
            ForeColor = Color.Black
        };

        transactionList.Controls.Add(transactionListLabel);

        // Add a list of transactions
        Panel transactionListContent = new Panel
        {
            Size = new Size(500, 250),
            Location = new Point(0, transactionListLabel.Bottom),
            BackColor = Color.White
        };

        transactionList.Controls.Add(transactionListContent);

        // Call the method to display top transactions
        DisplayTopTransactions(transactionListContent, email);
    }


    //---------------------------------------------------------------//
    private Button CreateButton(string text, int x, int y)
    {
        return new Button
        {
            Text = text,
            Location = new System.Drawing.Point(x, y),
            Size = new System.Drawing.Size(150, 30),
            Font = new System.Drawing.Font("Arial", 12),
        };
    }

    private void DisplayTransactionsButton_Click(object sender, EventArgs e)
    {
        TransactionsForm transactionsForm = new TransactionsForm(email);
        transactionsForm.Show();
    }

    private void LogoutButton_Click(object sender, EventArgs e)
    {
        this.Close(); // Close the dashboard form
        LoginForm loginForm = new LoginForm();
        loginForm.Show(); // Show the login form
    }

    private void ProfileButton_Click(object sender, EventArgs e)
    {
        ProfileManagementForm profileForm = new ProfileManagementForm(email, password,username);
        profileForm.Show();
    }

    private void DisplayTopTransactions(Panel transactionListContent, string email)
    {
        string filePath = "./database.json";
        try
        {
            string jsonData = File.ReadAllText(filePath);
            List<UserInfo> users = JsonConvert.DeserializeObject<List<UserInfo>>(jsonData) ?? new List<UserInfo>();

            var user = users.FirstOrDefault(u => u.Email == email);
            if (user == null || user.Transactions == null || !user.Transactions.Any())
            {
                MessageBox.Show("No transactions found for this user.");
                return;
            }

            // Clear existing items
            transactionListContent.Controls.Clear();

            // Fetch top 5 transactions
            var topTransactions = user.Transactions.Take(5).ToList();

            // Display each transaction
            for (int i = 0; i < topTransactions.Count; i++)
            {
                Label transactionLabel = new Label
                {
                    Text = (topTransactions[i] > 0 ? "Income: $" : "Expense: $") + Math.Abs(topTransactions[i]),
                    AutoSize = true,
                    Location = new Point(10, i * 30), // Adjust location as needed
                    Font = new Font("Arial", 10),
                    ForeColor = Color.Black
                };

                transactionListContent.Controls.Add(transactionLabel);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("An error occurred while loading transactions: " + ex.Message);
        }
    }


    private void UpdateTotalAmount(string email)
    {
        string filePath = "./database.json";
        try
        {
            string jsonData = File.ReadAllText(filePath);
            List<UserInfo> users = JsonConvert.DeserializeObject<List<UserInfo>>(jsonData) ?? new List<UserInfo>();

            var user = users.FirstOrDefault(u => u.Email == email);
            if (user == null || user.Transactions == null || !user.Transactions.Any())
            {
                MessageBox.Show("No transactions found for this user.");
                return;
            }

            // Calculate the sum of all transactions
            int totalAmount = user.Transactions.Sum();

            // Update the cardText label with the total amount
            // Assuming cardText is a Label control already defined in your DashboardForm
            cardText.Text = "PKR " + totalAmount;
            cardText.Refresh();
        }
        catch (Exception ex)
        {
            MessageBox.Show("An error occurred while calculating the total amount: " + ex.Message);
        }
    }


    private decimal GetUserBudget(string email)
    {
        string filePath = "./database.json";
        try
        {
            string jsonData = File.ReadAllText(filePath);
            List<UserInfo> users = JsonConvert.DeserializeObject<List<UserInfo>>(jsonData) ?? new List<UserInfo>();

            var user = users.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                return user.Budget;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("An error occurred while getting user's budget: " + ex.Message);
        }

        // Return a default value if unable to get the budget
        return 0;
    }


}
