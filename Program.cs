using System;
using System.Windows.Forms;

public class Program
{
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new LoginForm());
        // Application.Run(new DashboardForm("AmmarKashif", "ammar@gmail.com", "Ammar_123"));
    }
}