using System;
using System.Drawing;
using System.Windows.Forms;

public class DashboardForm : Form
{
    public DashboardForm()
    {
        InitializeComponents();
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
    }

    private void InitializeComponents()
    {
        this.Text = "Dashboard";
        this.Size = new System.Drawing.Size(1200, 600);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.FromArgb(44, 62, 80);  // Dark blue background
        this.Font = new Font("Arial", 12);

        // Label welcomeLabel = new Label
        // {
        //     Text = "Welcome to Big Bag, " + username + "!",
        //     Location = new System.Drawing.Point((ClientSize.Width - Text.Length) / 3, 20),
        //     Font = new System.Drawing.Font("Arial", 18, FontStyle.Bold),
        //     AutoSize = true,
        // };

        // //--------------------ADD INCOME BUTTON-----------------//
        // Button addIncomeButton = CreateButton("Add Income", 280, 50);
        // addIncomeButton.BackColor = Color.FromArgb(255, 193, 7); // Yellow
        // addIncomeButton.ForeColor = Color.White;
        // addIncomeButton.FlatStyle = FlatStyle.Flat;
        // addIncomeButton.Font = new Font("Arial", 12, FontStyle.Bold);
        // addIncomeButton.Click += (sender, e) =>
        // {
        //     IncomeRecordForm incomeRecordForm = new IncomeRecordForm();
        //     incomeRecordForm.Show();
        // };
        // //--------------------------------------------------------//

        // this.Controls.Add(addIncomeButton);


        // //-------------------EXPENSE BUTTON---------------------//
        // Button expenseButton = CreateButton("Add Expense", 60, 200);
        // expenseButton.BackColor = Color.FromArgb(231, 76, 60);
        // expenseButton.ForeColor = Color.White;
        // expenseButton.FlatStyle = FlatStyle.Flat;
        // expenseButton.Font = new Font("Arial", 12, FontStyle.Bold);

        // expenseButton.Click += (sender, e) =>
        // {
        //     ExpenseRecordForm expenseForm = new ExpenseRecordForm();
        //     expenseForm.Show();
        // };
        // //--------------------------------------------------------//

        // this.Controls.Add(expenseButton);


        // //---------------TRANSACTIONS----------------------------------//
        // Button displayTransactionsButton = CreateButton("Display Transactions", 60, 100);
        // displayTransactionsButton.BackColor = Color.FromArgb(46, 139, 87);
        // displayTransactionsButton.ForeColor = Color.White;
        // displayTransactionsButton.FlatStyle = FlatStyle.Flat;
        // displayTransactionsButton.Font = new Font("Arial", 12, FontStyle.Bold);


        // displayTransactionsButton.Click += (sender, e) =>
        // {
        //     TransactionsForm transactionsForm = new TransactionsForm();
        //     transactionsForm.Show();
        // };
        // //-----------------------------------------------------------//
        // //-----------------LOG OUT-------------------------------//
        // Button logoutButton = CreateButton("Log Out", 280, 100);
        // logoutButton.BackColor = Color.FromArgb(192, 57, 43);
        // logoutButton.ForeColor = Color.White;
        // logoutButton.FlatStyle = FlatStyle.Flat;
        // logoutButton.Font = new Font("Arial", 12, FontStyle.Bold);

        // logoutButton.Click += (sender, e) =>
        // {
        //     this.Close(); // Close the dashboard form
        //     LoginForm loginForm = new LoginForm();
        //     loginForm.Show(); // Show the login form
        // };
        // //----------------------------------------------//

        // //--------------------PROFILE BUTTON CUSTOMIZATIONS-----------------//
        // Button profileButton = CreateButton("Profile Management", 280, 150);
        // profileButton.BackColor = Color.FromArgb(52, 73, 94);
        // profileButton.ForeColor = Color.White;
        // profileButton.FlatStyle = FlatStyle.Flat;
        // profileButton.Font = new Font("Arial", 12, FontStyle.Bold);
        // profileButton.Click += (sender, e) =>
        // {
        //     ProfileManagementForm profileForm = new ProfileManagementForm(email, password);
        //     profileForm.Show();
        // };
        // //-------------------------------------------------------------------------//

        // this.Controls.Add(welcomeLabel);
        // this.Controls.Add(displayTransactionsButton);
        // this.Controls.Add(logoutButton);
        // this.Controls.Add(profileButton);

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
        tabsPanel.Controls.Add(displayTransactionsButton);
        tabsPanel.Controls.Add(profileButton);
        tabsPanel.Controls.Add(logoutButton);

        displayTransactionsButton.Click += (sender, e) =>
        {
            TransactionsForm transactionsForm = new TransactionsForm();
            transactionsForm.Show();
        };

        profileButton.Click += (sender, e) =>
        {
            ProfileManagementForm profileForm = new ProfileManagementForm(email, password);
            profileForm.Show();
        };

        logoutButton.Click += (sender, e) =>
        {
            this.Close(); // Close the dashboard form
            LoginForm loginForm = new LoginForm();
            loginForm.Show(); // Show the login form
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
        Label cardText = new Label
        {
            Text = "USD " + 2000,
            AutoSize = true,
            Location = new Point(20, 20),
            Font = new Font("Arial", 32, FontStyle.Bold)
        };
        contentCard.Controls.Add(cardText);

        // Place buttons horizontally
        Button incomeButton = new Button { Text = "Add Income", Size = new Size(222, 50), Location = new Point(20, cardText.Bottom + 10) };
        Button expenseButton = new Button { Text = "Add Expense", Size = new Size(222, 50), Location = new Point(incomeButton.Right + 10, cardText.Bottom + 10) };
        contentCard.Controls.Add(incomeButton);
        contentCard.Controls.Add(expenseButton);

        // Add event handlers
        incomeButton.Click += (sender, e) =>
        {
            IncomeRecordForm incomeRecordForm = new IncomeRecordForm();
            incomeRecordForm.Show();
        };

        expenseButton.Click += (sender, e) =>
        {
            ExpenseRecordForm expenseForm = new ExpenseRecordForm();
            expenseForm.Show();
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

        // Add 5 dummy transactions
        for (int i = 0; i < 5; i++)
        {
            Panel transaction = new Panel
            {
                Size = new Size(500, 50),
                Location = new Point(0, i * 50),
                BackColor = Color.LightGray
            };

            Label transactionText = new Label
            {
                Text = "Transaction " + (i + 1),
                AutoSize = true,
                Location = new Point(20, 20),
                Font = new Font("Arial", 12),
                ForeColor = Color.Black
            };

            transaction.Controls.Add(transactionText);
            transactionListContent.Controls.Add(transaction);
        }

        transactionList.Controls.Add(transactionListContent);
        cardPanel.Controls.Add(transactionList);
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
        TransactionsForm transactionsForm = new TransactionsForm();
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
        ProfileManagementForm profileForm = new ProfileManagementForm(email, password);
        profileForm.Show();
    }

}
