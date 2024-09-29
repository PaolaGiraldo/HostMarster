namespace HostMaster.Frontend.Pages.Users;

public partial class Users
{
	// Create a list to store user data
	private List<User> users = new List<User>();

	// Method to create a new user
	private void CreateUser(User newUser)
	{
		users.Add(newUser);
	}

	// Method to read a user by ID
	private User ReadUser(int userId)
	{
		return users.FirstOrDefault(u => u.Id == userId);
	}

	// Method to update a user
	private void UpdateUser(User updatedUser)
	{
		var user = users.FirstOrDefault(u => u.Id == updatedUser.Id);
		if (user != null)
		{
			user.Name = updatedUser.Name;
			user.Email = updatedUser.Email;
			// Update other properties as needed
		}
	}

	// Method to delete a user
	private void DeleteUser(int userId)
	{
		var user = users.FirstOrDefault(u => u.Id == userId);
		if (user != null)
		{
			users.Remove(user);
		}
	}
}