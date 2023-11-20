using System;

public class User
{
	public string Username { get; set; }
	public string PasswordHash { get; set; }

	public User(string username, string passwordHash)
	{
		Username = username;
		PasswordHash = passwordHash;
	}
}
