using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingAPI.Business.Abstract.Auth;

public interface IPasswordService
{
    string HashPassword(string password, out string salt);
    bool VerifyPassword(string password, string storedPassHash, string salt);
}
