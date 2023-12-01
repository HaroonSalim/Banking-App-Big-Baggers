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
    public List<int> Transactions { get; set; }  // Assuming transactions is a list of mixed types

    public decimal Budget { get; set; }
}