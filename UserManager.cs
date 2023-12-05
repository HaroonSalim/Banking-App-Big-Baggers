using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

public class UserManager
{
	private const string UserFilePath = "./database.json";

	// registration method
	public void RegisterUser(string username, string email, string password)
	{
		// verify username, email, and password are valid
		if (username.Length < 4 && username.Length > 20 && username != "")
		{
			MessageBox.Show("Username must be between 4 to 20 characters.");
			return;
		}
		if (!IsEmailValid(email))
		{
			MessageBox.Show("Invalid email format.");
			return;
		}
		if (!IsPasswordComplex(password))
		{
			MessageBox.Show("Password must have at least 8 characters, including one uppercase letter and one digit.");
			return;
		}

		// generate password hash
		string passwordHash = GeneratePasswordHash(password);

		// read existing user data and add new user
		string jsonData = File.ReadAllText(UserFilePath);
		List<UserInfo> users = JsonConvert.DeserializeObject<List<UserInfo>>(jsonData) ?? new List<UserInfo>();

		var newUser = new UserInfo
		{
			Username = username,
			Email = email,
			Password = passwordHash,
			Transactions = new List<TransactionInfo>() // Use the new TransactionInfo structure
		};

		users.Add(newUser);

		string updatedJsonData = JsonConvert.SerializeObject(users, Formatting.Indented);
		File.WriteAllText(UserFilePath, updatedJsonData);

		MessageBox.Show("Registration successful!");
	}

	// login method
	public bool Login(string username, string email, string password)
	{
		try
		{
			string jsonData = File.ReadAllText(UserFilePath);
			List<UserInfo> users = JsonConvert.DeserializeObject<List<UserInfo>>(jsonData) ?? new List<UserInfo>();

			var user = users.FirstOrDefault(u => u.Username == username && u.Email == email);
			if (user != null && VerifyPasswordHash(password, user.Password))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		catch (Exception ex)
		{
			HandleLoginException(ex);
			return false;
		}
	}

	private void HandleLoginException(Exception ex)
	{
		// Handle exceptions (e.g., file not found, invalid JSON)
		MessageBox.Show($"An error occurred while reading user data: {ex.Message}");
	}
	
	// email validation method
	private bool IsEmailValid(string email)
	{
		return Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
	}

	// password validation method
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

	// password hash generation method
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


	// verify password hash method
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
