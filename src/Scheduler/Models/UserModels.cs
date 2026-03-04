public record class UserModel(Guid Id, string Username, string FirstName, string LastName, string Email, string Salt, string Password);

public record class UserLoginModel(string Username, string Password);

public class UserCreateModel(string username, string firstName, string lastName, string email, string password)
{
    public string Username { get; set; } = username;
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public string Email { get; set; } = email;
    public string Password { get; set; } = password;
}

public class UserUpdateModel(Guid id, string username, string firstName, string lastName, string email, string password)
{
    public Guid Id { get; set; } = id;
    public string Username { get; set; } = username;
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public string Email { get; set; } = email;
    public string Password { get; set; } = password;
}
