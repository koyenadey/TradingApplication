using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradingAPI.Business.DTO;
using TradingAPI.Core.Entity;

namespace TradingAPI.Business.ServiceAbstract.Services;

public interface IUserService
{
    public Task<User> GetUserByIdAsync(string userId);
    public Task<bool> CheckIfUserExists(string email);
    public Task<List<SubscriptionReadDTO>> GetUserSubscriptionsByIdAsync(string userId);
    public Task<UserReadDTO> CreateUserByEmailAsync(UserCreateDTO userCreateDTO);
    public Task<UserReadDTO> UpdateUserPasswordAsync(string userId, string newPassword);
    public Task<UserReadDTO> UpdateUserCurrSubsAsync(string userId, string newSubsId);
    public Task<UserReadDTO> UpdateUserRoleAsync(string userid, string role);
    public Task<List<SubscriptionReadDTO>> AddSubscriptionAsync(SubscriptionCreateDTO createDTO, string userId);
}
