using Google.Cloud.Firestore;
using TradingAPI.Business.Abstract;
using TradingAPI.Business.Abstract.Auth;
using TradingAPI.Business.DTO;
using TradingAPI.Business.ServiceAbstract;
using TradingAPI.Business.ServiceAbstract.Auth;
using TradingAPI.Business.ServiceAbstract.Services;
using TradingAPI.Business.Shared;
using TradingAPI.Core.Entity;

namespace TradingAPI.Business.ServiceImplementation.Auth;

public class AuthService : IAuthService
{
    private readonly IUserService _userservice;
    private readonly ITokenService _tokenService;
    private readonly IPasswordService _passwordService;
    private readonly FirestoreDb _firestoreDb;
    public AuthService(IUserService userService,
    ITokenService tokenService,
    IPasswordService passwordService, FirestoreDb firestoreDb)
    {
        //_firestoreDb = firestoreAccessor.Db ?? throw new InvalidOperationException("FirestoreDb has not been initialized.");
        _userservice = userService;
        _tokenService = tokenService;
        _passwordService = passwordService;
        _firestoreDb = firestoreDb;
    }
    public Task<UserReadDTO> GetCurrentProfile(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<AuthLoginDTO> LoginAsync(UserCredentials userCredentials)
    {

        //User service to check if the user exists in the db
        var foundUser = await GetUserByCredentialsAsync(userCredentials.Email);
        //If  no throw exception
        if (foundUser is null) throw CustomException.NotFoundException("The user does not exist!");
        //Now check the password
        // Console.WriteLine("Found User Creds: " + foundUser.Password + " " + foundUser.Salt);
        var isMatch = _passwordService.VerifyPassword(userCredentials.Password, foundUser.Password, foundUser.Salt);
        //If not throw exception
        if (!isMatch) throw CustomException.UnauthorizedException("Password Incorrect");
        //If yes create token
        var token = _tokenService.GetToken(foundUser);
        //Return Token
        return new AuthLoginDTO
        {
            RefreshToken = token
        };
    }

    public async Task<User> GetUserByCredentialsAsync(string email)
    {
        var userRef = _firestoreDb.Collection("users").WhereEqualTo("email", email);
        var userSnapshot = await userRef.GetSnapshotAsync();
        if (userSnapshot.Count == 0) throw CustomException.NotFoundException("The person you are trying to find does not exist");

        var document = userSnapshot.Documents[0];
        return new User
        {
            UserId = document.Id,
            Email = document.GetValue<string>("email"),
            Password = document.GetValue<string>("password"),
            Salt = document.GetValue<string>("salt"),
            Role = document.GetValue<string>("role"),
            CreatedOn = document.GetValue<DateTime>("createdon"),
            ModifiedOn = document.GetValue<DateTime>("modifiedon"),
            CurrSubscriptionId = document.GetValue<string>("currsubsid")
        };
    }
    public Task<User> Register(UserCreateDTO userCreateDTO)
    {
        throw new NotImplementedException();
    }
}
