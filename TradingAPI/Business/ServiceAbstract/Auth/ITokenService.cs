using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradingAPI.Core.Entity;

namespace TradingAPI.Business.ServiceAbstract.Auth;

public interface ITokenService
{
    public string GetToken(User user);
}
