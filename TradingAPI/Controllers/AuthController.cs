using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradingAPI.Business.DTO;
using TradingAPI.Business.ServiceAbstract;
using TradingAPI.Business.ServiceAbstract.Auth;
using TradingAPI.Business.ServiceAbstract.Services;
using TradingAPI.Core.Entity;

namespace TradingAPI.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService;
    public AuthController(IAuthService authService, IUserService userService)
    {
        _authService = authService;
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<AuthLoginDTO>> Login([FromBody] UserCredentials userCredentials)
    {
        var token = await _authService.LoginAsync(userCredentials);
        if (token is null) return BadRequest("Could not generate token!");
        return Ok(token);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<UserReadDTO>> Register([FromBody] UserCreateDTO userCreateDTO)
    {
        var userExists = await _userService.CheckIfUserExists(userCreateDTO.Email);
        if (userExists) return BadRequest("The user already exists. Please login!");
        var createdUser = await _userService.CreateUserByEmailAsync(userCreateDTO);
        if (createdUser is null) return StatusCode(500, "Internal Server Error!");
        return Ok(createdUser);
    }
}
