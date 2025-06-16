using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingAPI.Business.DTO;

public class UserReadDTO
{
    public string UserId { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
    public string CurrSubscriptionId { get; set; } = string.Empty;
}

// public class UserUpdateDTO
// {
//     public string? Role { get; set; } = string.Empty;
//     public string? CurrSubscriptionId { get; set; } = string.Empty;
//     public bool? SubsIsActive { get; set; }
// }

public class UserCreateDTO
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string CurrSubscriptionId { get; set; } = string.Empty;
    public string SubscriptionId { get; set; } = String.Empty;
}


// public class UserSubscriptionsReadDTO
// {
//     //public required UserReadDTO User { get; set; }
//     public required List<SubscriptionReadDTO> Subscriptions { get; set; }
// }

public class UpdateUserPasswordDTO
{
    public string NewPassword { get; set; } = string.Empty;
}
public class UpdateUserRoleDTO
{
    public string Role { get; set; } = string.Empty;
}


