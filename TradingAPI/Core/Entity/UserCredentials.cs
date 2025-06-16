using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingAPI.Core.Entity;

public class UserCredentials
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
