namespace Scheduler.Cryptography;


public class SchedulerCryptography() : ISchedulerCryptography
{
    public string GetSaltedPasswordHash(string password, string salt)
    {
        var saltedPassword = GetSaltedPassword(password, salt);

        return HashPassword(saltedPassword);
    }

    private string HashPassword(string password) // using sha as learning to understand how to handle authentication - future implementations should use a more complex algorithm or implement rounds of salting
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var hashBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashBytes);
    }

    private string GetSaltedPassword(string password, string salt)
    { 
        return $"{salt}{password}";
    }

    public string GenerateSalt()
    {
        return Guid.NewGuid().ToString();
    }

}


