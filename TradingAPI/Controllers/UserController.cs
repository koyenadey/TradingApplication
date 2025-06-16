using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TradingAPI.Business;
using TradingAPI.Business.DTO;
using TradingAPI.Business.ServiceAbstract;
using TradingAPI.Business.ServiceAbstract.Services;
using TradingAPI.Business.ServiceImplementation;
using TradingAPI.Business.Shared;
using TradingAPI.Core.Entity;

namespace TradingAPI.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userServices;
        private readonly IHttpContextAccessor _httpAccessor;
        public UserController(IUserService userServices, IHttpContextAccessor httpAccessor)
        {
            _userServices = userServices;
            _httpAccessor = httpAccessor;
        }

        [Authorize(Roles = "user")]
        [HttpGet("{userId}")]
        public async Task<ActionResult<User>> GetUserAsync([FromRoute] string userId)
        {
            var userClaims = _httpAccessor.HttpContext?.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine("This is userClaims", userClaims);
            var foundUser = await _userServices.GetUserByIdAsync(userId);
            if (foundUser is not null) return Ok(foundUser);
            return NotFound("No user with the email exists");
        }

        [Authorize(Roles = "user")]
        [HttpGet("subscriptions/{userId}")]
        public async Task<ActionResult<SubscriptionReadDTO>> GetUserSubscriptionsAsync([FromRoute] string userId)
        {
            var userSubscriptions = await _userServices.GetUserSubscriptionsByIdAsync(userId);
            if (userSubscriptions is not null) return Ok(userSubscriptions);
            return NotFound("No user with the email exists");
        }


        [HttpPost("createuser")]
        public async Task<ActionResult<UserReadDTO>> CreateUserAsync([FromBody] UserCreateDTO userCreateDTO)
        {
            var createdUser = await _userServices.CreateUserByEmailAsync(userCreateDTO);
            if (createdUser is null) return BadRequest("User could not be created!");
            return Ok(createdUser);
        }

        [Authorize(Roles = "user")]
        [HttpPost("addsubscription/{userId}")]
        public async Task<ActionResult<List<SubscriptionReadDTO>>> AddSubscriptionAsync([FromBody] SubscriptionCreateDTO createDTO, [FromRoute] string userId)
        {
            var allSubscriptions = await _userServices.AddSubscriptionAsync(createDTO, userId);
            if (allSubscriptions is null) return BadRequest("The userId provided could not be found!");
            return allSubscriptions;
        }

        [Authorize(Roles = "admin")]
        [HttpPatch("role/{userid}")]
        public async Task<ActionResult<UserReadDTO>> UpdateUserRoleAsync([FromRoute] string userid, [FromBody] UpdateUserRoleDTO userRoleDto)
        {
            if (string.IsNullOrEmpty(userRoleDto.Role)) return BadRequest("The role you provided is empty!");
            var updatedUserRole = await _userServices.UpdateUserRoleAsync(userid, userRoleDto.Role);
            if (updatedUserRole is null) throw CustomException.InvalidResourceException("The data that you provided is incorrect or invalid");
            return Ok(updatedUserRole);
        }

        [Authorize(Roles = "user")]
        [HttpPatch("change-password/{email}")]
        public async Task<ActionResult<UserReadDTO>> UpdateUserPasswordAsync([FromRoute] string email, [FromBody] UpdateUserPasswordDTO newPasswordDTO)
        {
            var newPassword = newPasswordDTO.NewPassword;
            if (string.IsNullOrEmpty(newPassword)) return BadRequest("The password you provided is invalid");
            var updatedPassword = await _userServices.UpdateUserPasswordAsync(email, newPassword);
            if (updatedPassword is null) return BadRequest("The password could not be updated");
            return Ok(updatedPassword);
        }

        [Authorize(Roles = "admin")]
        [HttpPatch("subscriptions/{subid}")]
        public async Task<ActionResult<UserReadDTO>> UpdateUserCurrSubsAsync([FromRoute] string subid, [FromQuery] string userId)
        {
            var updatedUserData = await _userServices.UpdateUserCurrSubsAsync(userId, subid);
            if (updatedUserData is null) throw CustomException.InvalidResourceException("The data that you provided is incorrect or invalid");
            return Ok(updatedUserData);
        }




    }
}