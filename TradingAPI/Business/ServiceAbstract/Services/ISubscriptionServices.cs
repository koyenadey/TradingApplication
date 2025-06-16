using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TradingAPI.Business.DTO;

namespace TradingAPI.Business.ServiceAbstract.Services;

public interface ISubscriptionServices
{
    public Task<ActionResult<List<SubscripDetailReadDTO>>> GetAllSubscriptionsAsync();
    public Task<ActionResult<SubscripDetailReadDTO>> CreateSubscriptionAsync(SubscripDetailCreateDTO subscripDetailCreateDTO);
    public Task<ActionResult<SubscripDetailReadDTO>> UpdateSubscriptionAsync(string playListId, string subsId);
    public Task<ActionResult<SubscripDetailReadDTO>> DeleteSubscriptionAsync(string subscriptionId);

}
