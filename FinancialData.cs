using System;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;

public class UserInfo
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public List<TransactionInfo> Transactions { get; set; } 
    public decimal Budget { get; set; }
}

public class TransactionInfo
{
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}