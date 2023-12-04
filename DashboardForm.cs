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
        };
        mainPanel.Controls.Add(tabsPanel);
        tabsPanel.Paint += TabsPanel_Paint;

        // Adding tabs
        string[] buttonNames = { "Transactions", "Budget", "Analytics", "Currency Conversion", "Profile", "Logout"};
        EventHandler[] eventHandlers = { DisplayTransactionsButton_Click, ProfileButton_Click, LogoutButton_Click, BudgetButton_Click, AnalyticsButton_Click, CurrencyConversionButton_Click };

        for (int i = 0; i < buttonNames.Length; i++)
        {
            var button = CreateTabButton(buttonNames[i], new Point(0, 60 * i), eventHandlers[i]);
            tabsPanel.Controls.Add(button);
        }

        // cardPanel is added before transactionList for correct Z-order
        Panel cardPanel = new Panel
        {
            Size = new Size((this.Width * 3) / 4, this.Height),
            Location = new Point(tabsPanel.Width, 0),
            BackColor = Color.White  // Card panel background
        };
        mainPanel.Controls.Add(cardPanel);

        // Smaller card panel for content and buttons
        Panel contentCard = new Panel
        {
            Size = new Size(500, 150), // Adjust size as needed
            Location = new Point((cardPanel.Width - 500) / 2, 80), // Positioned at the top middle
            BackColor = ColorTranslator.FromHtml("#eef1f4")
        };
        cardPanel.Controls.Add(contentCard);

        // Label outside but on top of the smaller card
        Label cardTitle = new Label
        {
            Text = "Hello, " + username + "!",
            AutoSize = true,
            Location = new Point(contentCard.Location.X + 20, 40),
            Font = new Font("Arial", 16),
            ForeColor = ColorTranslator.FromHtml("#545f71")
        };
        cardPanel.Controls.Add(cardTitle);

        // Content in contentCard panel
        cardText = new Label
        {
            Text = "PKR ",
            AutoSize = true,
            Location = new Point(20, 20),
            Font = new Font("Arial", 32, FontStyle.Bold),
            ForeColor = ColorTranslator.FromHtml("#545f71")
        };
        contentCard.Controls.Add(cardText);
        UpdateTotalAmount(email);
        cardText.Refresh();

        Button incomeButton = CreateButton("Add Income", 20, cardText.Bottom + 10, Color.White, ColorTranslator.FromHtml("#545f71"), "./assets/income.png");
        Button expenseButton = CreateButton("Add Expense", incomeButton.Right + 10, cardText.Bottom + 10, ColorTranslator.FromHtml("#545f71"), Color.White, "./assets/expense.png");
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
            ExpenseRecordForm expenseForm = new ExpenseRecordForm(email, userBudget, DataChanged);
            expenseForm.Show();
            UpdateTotalAmount(email);
            cardText.Refresh();
        };

        Panel transactionList = new Panel
        {
            Size = new Size(500, 300),
            Location = new Point((cardPanel.Width - 500) / 2, contentCard.Bottom + 20),
            BackColor = Color.White  // White background for consistency
        };
        cardPanel.Controls.Add(transactionList);

        Label transactionListLabel = new Label
        {
            Text = "Recent Transactions",
            AutoSize = true,
            Location = new Point(0, 8),
            Font = new Font("Arial", 12, FontStyle.Bold),  // Consistent font style
            ForeColor = ColorTranslator.FromHtml("#545f71")  // Consistent text color
        };
        transactionList.Controls.Add(transactionListLabel);

        Panel transactionListContent = new Panel
        {
            Size = new Size(500, 250),
            Location = new Point(0, transactionListLabel.Bottom),
            BackColor = Color.White // Light gray background for a subtle contrast
        };
        transactionList.Controls.Add(transactionListContent);

        // Call the method to display top transactions
        DisplayTopTransactions(transactionListContent, email);

        this.BackColor = Color.White; // Set the form's background color to white

        // Set the color of all text-containing controls
        Color textColor = ColorTranslator.FromHtml("#545f71");
    }


    //---------------------------------------------------------------//

    private void TabsPanel_Paint(object sender, PaintEventArgs e)
    {
        Panel panel = sender as Panel;
        if (panel != null)
        {
            // Draw right border
            using (Pen pen = new Pen(ColorTranslator.FromHtml("#545f71"), 1)) // You can change the color and width as needed
            {
                e.Graphics.DrawLine(pen, panel.Width - 1, 0, panel.Width - 1, panel.Height);
            }
        }
    }

    private Button CreateButton(string text, int x, int y, Color textColor, Color backgroundColor, string icon)
    {
        var button = new Button
        {
            Text = text,
            Location = new Point(x, y),
            Size = new Size(222, 50),
            Font = new Font("Arial", 12),
            ForeColor = textColor,
            BackColor = backgroundColor,
            FlatStyle = FlatStyle.Flat,
            TextAlign = ContentAlignment.MiddleCenter,
            Image = Image.FromFile(icon),
            ImageAlign = ContentAlignment.MiddleRight
        };

        // Remove the border on the flat button
        button.FlatAppearance.BorderSize = 0;

        // Optional: Adjust padding if necessary for better alignment
        button.Padding = new Padding(0, 0, 20, 0); // 20 pixels padding on the right

        return button;
    }

    private Button CreateTabButton(string text, Point location, EventHandler eventHandler)
    {
        var button = new Button
        {
            Text = text,
            Size = new Size(this.Width / 4, 60),
            Location = location,
            FlatStyle = FlatStyle.Flat,
            ForeColor = ColorTranslator.FromHtml("#545f71"),
            Font = new Font("Arial", 12, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleLeft,
            ImageAlign = ContentAlignment.MiddleRight,
            Padding = new Padding(20, 0, 10, 0),
            TextImageRelation = TextImageRelation.TextBeforeImage
        };

        button.FlatAppearance.BorderSize = 0; // Remove border

        button.Paint += (sender, e) =>
        {
            ControlPaint.DrawBorder(e.Graphics, button.ClientRectangle,
                                Color.Transparent, 0, ButtonBorderStyle.None, // Left border
                                Color.Transparent, 0, ButtonBorderStyle.None, // Top border
                                ColorTranslator.FromHtml("#545f71"), 1, ButtonBorderStyle.Solid, // Right border
                                ColorTranslator.FromHtml("#545f71"), 1, ButtonBorderStyle.Solid); // Bottom border
        };

        // Load your arrow image
        try
        {
            Image arrowImage = Image.FromFile("./assets/chevron_icon.png"); // Replace with actual file path
            Size imageSize = new Size(16, 16); // Desired image size
            Bitmap resizedImage = new Bitmap(arrowImage, imageSize);
            button.Image = resizedImage;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error loading image: " + ex.Message);
        }

        button.Click += eventHandler;
        return button;
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
        ProfileManagementForm profileForm = new ProfileManagementForm(email, password, username);
        profileForm.Show();
    }

    private void BudgetButton_Click(object sender, EventArgs e)
    {
        BudgetForm budgetForm = new BudgetForm(email, DataChanged);
        budgetForm.Show();
        UpdateTotalAmount(email);
        cardText.Refresh();
    }

    private void CurrencyConversionButton_Click(object sender, EventArgs e)
    {
        CurrencyConverterForm conversionForm = new CurrencyConverterForm();
        conversionForm.Show();
    }

    private void AnalyticsButton_Click(object sender, EventArgs e)
    {
        AnalyticsForm analyticsForm = new AnalyticsForm(username);
        analyticsForm.Show();
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
                return;
            }

            transactionListContent.Controls.Clear();

            var topTransactions = user.Transactions.Take(5).ToList();
            int labelHeight = 50;

            for (int i = 0; i < topTransactions.Count; i++)
            {
                Label transactionLabel = new Label
                {
                    Text = (topTransactions[i] > 0 ? "Income: $" : "Expense: $") + Math.Abs(topTransactions[i]),
                    AutoSize = false,
                    Size = new Size(transactionListContent.Width, labelHeight),
                    Location = new Point(0, i * labelHeight),
                    Font = new Font("Arial", 10),
                    ForeColor = ColorTranslator.FromHtml("#545f71"),
                    TextAlign = ContentAlignment.MiddleLeft,
                    BackColor = Color.White
                };
                transactionListContent.Controls.Add(transactionLabel);

                // Add a border label below each transaction
                Label borderLabel = new Label
                {
                    Size = new Size(transactionListContent.Width, 1), // 1 pixel height for the border
                    Location = new Point(0, (i + 1) * labelHeight),
                    BackColor = ColorTranslator.FromHtml("#d3d3d3") // Light gray border
                };
                transactionListContent.Controls.Add(borderLabel);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("An error occurred while loading transactions: " + ex.Message);
            // Consider adding a debug message or breakpoint here
        }

        transactionListContent.Refresh();
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
                cardText.Text = "PKR 0";
                return;
            }

            // Calculate the sum of all transactions
            int totalAmount = user.Transactions.Sum();

            // Update the cardText label with the total amount
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
