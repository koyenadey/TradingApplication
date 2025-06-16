using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingAPI.Business.DTO;

public class AuthLoginDTO
{
    public string RefreshToken { get; set; } = string.Empty;
}

