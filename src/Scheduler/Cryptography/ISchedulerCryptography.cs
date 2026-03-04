namespace Scheduler.Cryptography;

public interface ISchedulerCryptography
{
    public string GetSaltedPasswordHash(string password, string salt);

    public string GenerateSalt();
}