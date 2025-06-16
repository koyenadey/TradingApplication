using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using TradingAPI.Business.Abstract.Auth;

namespace TradingAPI.Business.ServiceImplementation.Auth;

public class PasswordService : IPasswordService
{
    public string HashPassword(string password, out string salt)
    {
        byte[] saltBytes = RandomNumberGenerator.GetBytes(16);
        salt = Convert.ToBase64String(saltBytes);

        using var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 100_000, HashAlgorithmName.SHA256);
        byte[] hashBytes = pbkdf2.GetBytes(32);
        string hash = Convert.ToBase64String(hashBytes);

        return hash;
    }

    public bool VerifyPassword(string password, string storedPassHash, string salt)
    {
        byte[] saltBytes = Convert.FromBase64String(salt);
        using var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 100_000, HashAlgorithmName.SHA256);
        byte[] hashBytes = pbkdf2.GetBytes(32);
        string computedHash = Convert.ToBase64String(hashBytes);

        Console.WriteLine("Computed Hash: " + computedHash == storedPassHash);
        return computedHash == storedPassHash;
    }
}
