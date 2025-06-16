using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TradingAPI.Business.DTO;
using TradingAPI.Business.ServiceAbstract.Services;

namespace TradingAPI.Controllers;

[ApiController]
[Route("api/subscription")]
public class SubscriptionController : ControllerBase
{
    private readonly ISubscriptionServices _subscriptionServices;
    public SubscriptionController(ISubscriptionServices subscriptionServices)
    {
        _subscriptionServices = subscriptionServices;
    }
    [HttpGet("subscription-details")]
    public async Task<ActionResult<List<SubscripDetailReadDTO>>> GetAllSubscriptions()
    {
        var allSubscriptions = await _subscriptionServices.GetAllSubscriptionsAsync();
        if (allSubscriptions is null) return NotFound("No subscription was found");
        return Ok(allSubscriptions);
    }

    [HttpPost]
    public async Task<ActionResult<SubscripDetailReadDTO>> CreateSubscriptionAsync([FromBody] SubscripDetailCreateDTO subscripDetailCreateDTO)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var createdSubscription = await _subscriptionServices.CreateSubscriptionAsync(subscripDetailCreateDTO);
        if (createdSubscription is null) return BadRequest("Subscription could not be created because of invalid input");
        return Ok(createdSubscription);
    }

    [HttpPatch("{subsId}")]
    public async Task<ActionResult<SubscripDetailReadDTO>> UpdateSubscriptionAsync([FromBody] SubscripDetailUpdateDTO subscripDetailUpdateDTO, [FromRoute] string subsId)
    {
        var playListId = subscripDetailUpdateDTO.PlaylistId;
        if (string.IsNullOrEmpty(playListId)) return BadRequest("Invalid Input!");
        var updatedSubcription = await _subscriptionServices.UpdateSubscriptionAsync(playListId, subsId);
        if (updatedSubcription is null) return StatusCode(500, "An unexpected error occurred.");
        return Ok(updatedSubcription);
    }

    [HttpDelete("{subsid}")]
    public async Task<ActionResult<SubscripDetailReadDTO>> DeleteSubscriptionAsync(string subsid)
    {
        if (string.IsNullOrEmpty(subsid)) return BadRequest("Invalid Input");
        var deletedSubscription = await _subscriptionServices.DeleteSubscriptionAsync(subsid);
        if (deletedSubscription is null) return NotFound("Resource could not be found!");
        return Ok(deletedSubscription);
    }
}
