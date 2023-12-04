using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

public class CurrencyConverterForm : Form
{
    private TextBox amountTextBox;
    private Label fromCurrencyLabel;
    private Label toCurrencyLabel;
    private Label resultLabel;
    private Label amountLabel;
    private ComboBox fromCurrencyComboBox;
    private ComboBox toCurrencyComboBox;
    private Button convertButton;

    // Dictionary to store dummy conversion rates
    private Dictionary<string, Dictionary<string, decimal>> conversionRates;

    public CurrencyConverterForm()
    {
        InitializeComponents();
        InitializeConversionRates();
    }

    private void InitializeComponents()
    {
        this.Text = "Currency Converter";
        this.Size = new System.Drawing.Size(400, 200);
        this.StartPosition = FormStartPosition.CenterScreen;

       
        // Amount Label and TextBox
        amountLabel = new Label
        {
            Text = "Enter Amount:",
            Location = new System.Drawing.Point(20, 20),
            AutoSize = true,
        };

        amountTextBox = new TextBox
        {
            Location = new System.Drawing.Point(20, 50),
            Size = new System.Drawing.Size(100, 20),
        };

        // From Currency Label and ComboBox
        fromCurrencyLabel = new Label
        {
            Text = "From Currency:",
            Location = new System.Drawing.Point(140, 20),
            AutoSize = true,
        };

        fromCurrencyComboBox = new ComboBox
        {
            Location = new System.Drawing.Point(140, 50),
            Size = new System.Drawing.Size(80, 20),
        };

        // To Currency Label and ComboBox
        toCurrencyLabel = new Label
        {
            Text = "To Currency:",
            Location = new System.Drawing.Point(230, 20),
            AutoSize = true,
        };

        toCurrencyComboBox = new ComboBox
        {
            Location = new System.Drawing.Point(230, 50),
            Size = new System.Drawing.Size(80, 20),
        };

        convertButton = new Button
        {
            Text = "Convert",
            Location = new System.Drawing.Point(320, 50),
            Size = new System.Drawing.Size(80, 30),
        };
        convertButton.Click += async (sender, e) => await ConvertCurrency();

        resultLabel = new Label
        {
            Location = new System.Drawing.Point(20, 100),
            AutoSize = true,
        };

        // Add currency options to ComboBoxes
        string[] currencies = { "USD", "EUR", "GBP", "PKR" };

        fromCurrencyComboBox.Items.AddRange(currencies);
        toCurrencyComboBox.Items.AddRange(currencies);

        this.Controls.Add(amountLabel);
        this.Controls.Add(amountTextBox);
        this.Controls.Add(fromCurrencyLabel);
        this.Controls.Add(fromCurrencyComboBox);
        this.Controls.Add(toCurrencyLabel);
        this.Controls.Add(toCurrencyComboBox);
        this.Controls.Add(convertButton);
        this.Controls.Add(resultLabel);
    }

    private void InitializeConversionRates()
    {
        // Dummy conversion rates
        conversionRates = new Dictionary<string, Dictionary<string, decimal>>
        {
            { "USD", new Dictionary<string, decimal> { { "EUR", 0.85m }, { "GBP", 0.73m }, { "PKR", 275.0m } } },
            { "EUR", new Dictionary<string, decimal> { { "USD", 1.18m }, { "GBP", 0.86m }, { "PKR", 384.62m } } },
            { "GBP", new Dictionary<string, decimal> { { "USD", 1.37m }, { "EUR", 1.16m }, { "PKR", 446.81m } } },
            { "PKR", new Dictionary<string, decimal> { { "USD", 0.0036m }, { "EUR", 0.0026m }, { "GBP", 0.0022m } } }
        };
    }

    private async Task ConvertCurrency()
    {
        string fromCurrency = fromCurrencyComboBox.SelectedItem.ToString();
        string toCurrency = toCurrencyComboBox.SelectedItem.ToString();

        try
        {
            // Getting the conversion rate dynamically from the dictionary
            decimal conversionRate = conversionRates[fromCurrency][toCurrency];

            // Perform the conversion
            decimal amount = decimal.Parse(amountTextBox.Text);
            decimal convertedAmount = amount * conversionRate;

            resultLabel.Text = $"{amount} {fromCurrency} = {convertedAmount} {toCurrency}";
        }
        catch (Exception ex)
        {
            resultLabel.Text = $"An error occurred: {ex.Message}";
        }
    }
}
