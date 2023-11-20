using System;

public class User
{
	public string Username { get; set; }
	public string Email { get; set; }
	public string PasswordHash { get; set; }

	public User(string username, string email, string passwordHash)
	{
		Username = username;
		Email = email;
		PasswordHash = passwordHash;
	}
}
