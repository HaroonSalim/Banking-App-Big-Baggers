using System;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Windows.Forms;

public class UserManager
{
	private const string UserFilePath = "./userdataPG.txt";

	public void RegisterUser(string username, string password)
	{
		//------------------CHECKING FOR EMAIL FORMAT-------------------//
		if (!IsEmailValid(username))
		{
			MessageBox.Show("Invalid email format.");
			return;
		}

		//------------------CHECKING FOR PASSWORD COMPLEXITY-------------------//
		if (!IsPasswordComplex(password))
		{
			MessageBox.Show("Password must have at least 8 characters, including one uppercase letter and one digit.");
			return;
		}

		//------------------PASSWORD HASH GENERATION-------------------//
		string passwordHash = GeneratePasswordHash(password);

		var user = new User(username, passwordHash);
		string userData = $"{user.Username}:{user.PasswordHash}";

		File.AppendAllLines(UserFilePath, new[] { userData });

		MessageBox.Show("Registration complete.");
	}


	//-------------------ALL THE LOGIN PROCEDURE FILE CHECKING---------------------//
	public bool Login(string username, string password)
	{
		if (File.Exists(UserFilePath))
		{
			string[] lines = File.ReadAllLines(UserFilePath);
			foreach (var line in lines)
			{
				string[] parts = line.Split(':');
				if (parts.Length == 2)
				{
					string storedUsername = parts[0];
					string storedPasswordHash = parts[1];

					if (storedUsername == username && VerifyPasswordHash(password, storedPasswordHash))
					{
						return true;
					}
				}
			}
		}

		return false;
	}


	//-------------------VALID CHARACTER IN THE EMAIL---------------------//
	private bool IsEmailValid(string email)
	{
		return Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
	}


	//-------------------PASSWORD COMPLEXITY---------------------//

	private bool IsPasswordComplex(string password)
	{
		if (password.Length < 8)
		{
			return false; // Password is too short
		}

		bool hasDigit = false;
		bool hasLower = false;
		bool hasUpper = false;

		foreach (char c in password)
		{
			if (char.IsDigit(c))
			{
				hasDigit = true;
			}
			else if (char.IsLower(c))
			{
				hasLower = true;
			}
			else if (char.IsUpper(c))
			{
				hasUpper = true;
			}
		}

		return hasDigit && hasLower && hasUpper;
	}




	//----------------PASSWORD HASH---------------------//
	private string GeneratePasswordHash(string password)
	{
		byte[] salt;
		new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
		var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
		byte[] hash = pbkdf2.GetBytes(20);

		byte[] saltAndHash = new byte[36];
		Array.Copy(salt, 0, saltAndHash, 0, 16);
		Array.Copy(hash, 0, saltAndHash, 16, 20);

		return Convert.ToBase64String(saltAndHash);
	}


	//-------------------VERIFY PASSWORD HASH---------------------//
	private bool VerifyPasswordHash(string password, string storedHash)
	{
		byte[] hashBytes = Convert.FromBase64String(storedHash);
		byte[] salt = new byte[16];
		Array.Copy(hashBytes, 0, salt, 0, 16);
		var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);

		byte[] hash = pbkdf2.GetBytes(20);

		for (int i = 0; i < 20; i++)
		{
			if (hashBytes[i + 16] != hash[i])
			{
				return false;
			}
		}

		return true;
	}
}
