using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using TradingAPI.Business.ServiceAbstract;
using TradingAPI.Business.ServiceAbstract.Auth;
using TradingAPI.Core.Entity;

namespace TradingAPI.Business.ServiceImplementation.Auth;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string GetToken(User user)
    {
        //Create a claim using the data you want like email,bla bla
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Email,user.Email),
            new Claim("UserId",user.UserId),
            new Claim("Role",user.Role),
            new Claim("SubscriptionId",user.CurrSubscriptionId)
        };
        //Get the secret long key
        var jwtKey = _configuration["Secrets:JWTKey"];
        if (jwtKey is null) throw new ArgumentNullException("Jwt key is not found in appsettings.json");

        //Create the security key
        var securityKey = new SigningCredentials(
                           new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                           SecurityAlgorithms.HmacSha256Signature);
        // token handler
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Date.AddMonths(3).AddDays(1).AddTicks(-1),
            SigningCredentials = securityKey,
            Issuer = _configuration["Secrets:Issuer"]
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenVal = tokenHandler.WriteToken(token);
        Console.WriteLine("THIS IS YOUR JWT TOKEN -----> \n" + tokenVal);
        return tokenVal;
    }
}
