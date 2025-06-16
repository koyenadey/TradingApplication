using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradingAPI.Business.DTO;
using TradingAPI.Core.Entity;

namespace TradingAPI.Business.ServiceAbstract.Auth;

public interface IAuthService
{
    public Task<User> GetUserByCredentialsAsync(string email);
    public Task<AuthLoginDTO> LoginAsync(UserCredentials userCredentials);
    public Task<User> Register(UserCreateDTO userCreateDTO);
    public Task<UserReadDTO> GetCurrentProfile(string userId);
}
