using Google.Cloud.Firestore;
using TradingAPI.Business.DTO;
using TradingAPI.Core.Entity;
using TradingAPI.Business.Shared;
using TradingAPI.Business.Abstract.Auth;
using TradingAPI.Business.ServiceAbstract.Services;
using System.Threading.Tasks;

namespace TradingAPI.Business.ServiceImplementation;

public class UserServices : IUserService
{
    private readonly FirestoreDb _firestoreDb;
    private readonly IPasswordService _passwordService;
    public UserServices(FirestoreAccessor firestoreAccessor, IPasswordService passwordService)
    {
        //_firestoreAccessor = firestoreAccessor;
        _firestoreDb = firestoreAccessor.Db ??
    throw new InvalidOperationException("FirestoreDb has not been initialized.");
        _passwordService = passwordService;

    }

    public async Task<List<SubscriptionReadDTO>> GetUserSubscriptionsByIdAsync(string userId)
    {

        var user = await GetUserByIdAsync(userId);
        if (user is null) throw CustomException.NotFoundException("The user you are trying to find does not exist");

        CollectionReference subsRef = _firestoreDb.Collection("users").Document(user.UserId).Collection("subscriptions");

        var userSubsSnapshot = await subsRef.GetSnapshotAsync();
        List<SubscriptionReadDTO> subscriptions = new();
        foreach (var doc in userSubsSnapshot)
        {
            subscriptions.Add(new SubscriptionReadDTO
            {
                Id = doc.Id,
                SubscriptionId = doc.GetValue<string>("subscriptionid"),
                StartDate = doc.GetValue<DateTime>("startdate"),
                EndDate = doc.GetValue<DateTime>("enddate"),
                IsActive = doc.GetValue<bool>("isactive")
            });
        }

        return subscriptions;

    }

    public async Task<bool> CheckIfUserExists(string email)
    {
        var userRef = _firestoreDb.Collection("users").WhereEqualTo("email", email);
        var userData = await userRef.GetSnapshotAsync();
        if (userData.Count == 0) return false;
        return true;
    }

    public async Task<UserReadDTO> CreateUserByEmailAsync(UserCreateDTO userCreateDTO)
    {
        var hashedPassword = _passwordService.HashPassword(userCreateDTO.Password, out string salt);
        var createUser = new User
        {
            Email = userCreateDTO.Email,
            Password = hashedPassword,
            Salt = salt,
            CreatedOn = DateTime.Now.ToUniversalTime(),
            ModifiedOn = DateTime.Now.ToUniversalTime(),
            Role = "user",
            CurrSubscriptionId = userCreateDTO.CurrSubscriptionId
        };

        //Creating a subscription 
        //Extract into a new method
        //Violating SRP
        var addUserSubscription = SubscriptionFactory(userCreateDTO.CurrSubscriptionId);

        UserReadDTO? user = null;
        try
        {

            await _firestoreDb.RunTransactionAsync(transaction =>
            {
                var userCollection = _firestoreDb.Collection("users");
                var newUserRef = userCollection.Document();
                transaction.Set(newUserRef, createUser);

                //AddSubscriptionToUser(transaction, newUserRef, createUser.CurrSubscriptionId);
                var subscriptionCollec = newUserRef.Collection("subscriptions");
                var newSubRef = subscriptionCollec.Document();
                transaction.Set(newSubRef, addUserSubscription);

                user = new()
                {
                    UserId = newUserRef.Id,
                    Email = createUser.Email,
                    Password = createUser.Password,
                    CreatedOn = createUser.CreatedOn,
                    ModifiedOn = createUser.ModifiedOn,
                    Role = createUser.Role,
                    CurrSubscriptionId = createUser.CurrSubscriptionId
                };
                return Task.CompletedTask;
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        return user;
    }

    public async Task<User> GetUserByIdAsync(string userId)
    {
        //var userRef = _firestoreDb.Collection("users").WhereEqualTo("email", email);
        var userRef = _firestoreDb.Collection("users").Document(userId);
        var userDoc = await userRef.GetSnapshotAsync();
        User? userRetrieved = null;

        if (userDoc == null)
            throw new Exception($"User with ID {userId} not found.");

        else
        {
            //fetch the subdocuments
            //var document = userSnapshot.;
            userRetrieved = new User
            {
                UserId = userDoc.Id,
                Email = userDoc.GetValue<string>("email"),
                Salt = userDoc.GetValue<string>("salt"),
                Password = userDoc.GetValue<string>("password"),
                CreatedOn = userDoc.GetValue<DateTime>("createdon"),
                ModifiedOn = userDoc.GetValue<DateTime>("modifiedon"),
                Role = userDoc.GetValue<string>("role"),
                CurrSubscriptionId = userDoc.GetValue<string>("currsubsid")
            };
        }
        return userRetrieved;

    }


    public async Task<UserReadDTO> UpdateUserPasswordAsync(string userId, string newPassword)
    {
        var foundUser = await GetUserByIdAsync(userId);
        if (foundUser is null) throw CustomException.NotFoundException("The user you are trying to find does not exist");
        var hashedPassword = _passwordService.HashPassword(newPassword, out string newSalt);
        foundUser.Password = hashedPassword;
        foundUser.Salt = newSalt;
        var userRef = _firestoreDb.Collection("users").Document(foundUser.UserId);
        Dictionary<string, object> passwordUpdates = new(){
            {"modifiedon",DateTime.Now.ToUniversalTime()},
            {"password",foundUser.Password},
            {"salt",foundUser.Salt}
        };

        await userRef.UpdateAsync(passwordUpdates);

        var userUpdatedDoc = await userRef.GetSnapshotAsync();

        return new UserReadDTO()
        {
            UserId = userRef.Id,
            Email = userUpdatedDoc.GetValue<string>("email"),
            Password = userUpdatedDoc.GetValue<string>("password"),
            CreatedOn = userUpdatedDoc.GetValue<DateTime>("createdon"),
            ModifiedOn = userUpdatedDoc.GetValue<DateTime>("modifiedon"),
            Role = userUpdatedDoc.GetValue<string>("role"),
            CurrSubscriptionId = userUpdatedDoc.GetValue<string>("currsubsid")
        };
    }

    public async Task<UserReadDTO> UpdateUserCurrSubsAsync(string userId, string newSubsId)
    {
        if (string.IsNullOrEmpty(userId)) throw CustomException.InvalidResourceException("The userId you provided is not valid");

        var userRef = _firestoreDb.Collection("users").Document(userId);
        var userData = await userRef.GetSnapshotAsync();

        await userRef.UpdateAsync("currsubsid", newSubsId);

        return new UserReadDTO()
        {
            UserId = userRef.Id,
            Password = userData.GetValue<string>("password"),
            ModifiedOn = userData.GetValue<DateTime>("modifiedon"),
            Role = userData.GetValue<string>("role"),
            CreatedOn = userData.GetValue<DateTime>("createdon"),
            CurrSubscriptionId = userData.GetValue<string>("currsubsid")
        };
    }

    public async Task<UserReadDTO> UpdateUserRoleAsync(string userid, string role)
    {
        var userRef = _firestoreDb.Collection("users").Document(userid);
        await userRef.UpdateAsync("role", role);
        var userData = await userRef.GetSnapshotAsync();


        return new UserReadDTO()
        {
            UserId = userData.Id,
            Email = userData.GetValue<string>("email"),
            Password = userData.GetValue<string>("password"),
            ModifiedOn = userData.GetValue<DateTime>("modifiedon"),
            Role = userData.GetValue<string>("role"),
            CreatedOn = userData.GetValue<DateTime>("createdon"),
            CurrSubscriptionId = userData.GetValue<string>("currsubsid")
        };
    }

    public async Task<List<SubscriptionReadDTO>> AddSubscriptionAsync(SubscriptionCreateDTO createDTO, string userId)
    {
        //Get the user row for which you want to add subscription
        var userRef = _firestoreDb.Collection("users").Document(userId);

        //Create the subscription
        var newSubscrip = SubscriptionFactory(createDTO.SubscriptionId);

        //Get the sub-sollection subscription
        var subscripCollecRef = userRef.Collection("subscriptions");

        //Fetch the subcollections data
        var allsubscripData = await subscripCollecRef.GetSnapshotAsync();

        //Start a batch
        var batch = subscripCollecRef.Database.StartBatch();

        //Also update the other subscriptions isActive Status as false
        foreach (var doc in allsubscripData)
        {
            Dictionary<string, object> newSubscripData = new()
            {
                {"isactive",false}
            };
            batch.Update(doc.Reference, newSubscripData);
        }
        await batch.CommitAsync();

        //Add the newly created subscription to the the sub-collection
        Dictionary<string, object> addSubscription = new()
        {
            {"startdate",createDTO.StartDate},
            {"enddate",createDTO.EndDate},
            {"isactive",createDTO.IsActive},
            {"subscriptionid",createDTO.EndDate}
        };

        await subscripCollecRef.AddAsync(addSubscription);

        //Update the current subscription
        await UpdateUserCurrSubsAsync(userRef.Id, newSubscrip.SubscriptionId);

        //Return all the subscriptions of the user 
        var allSubscrip = await GetUserSubscriptionsByIdAsync(userRef.Id);
        return allSubscrip;
    }

    private SubscriptionCreateDTO SubscriptionFactory(string currentSubsId)
    {
        if (string.IsNullOrEmpty(currentSubsId)) throw CustomException.NotFoundException();

        var startDate = DateTime.UtcNow.Date;

        return new SubscriptionCreateDTO
        {
            StartDate = startDate,
            EndDate = startDate.AddDays(90).AddTicks(-1),
            IsActive = true,
            SubscriptionId = currentSubsId
        };
    }
}
