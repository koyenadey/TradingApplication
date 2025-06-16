using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

namespace TradingAPI.Core.Entity;

[FirestoreData]
public class User
{
    public string UserId { get; set; } = string.Empty;
    [FirestoreProperty("email")] public string Email { get; set; } = string.Empty;
    [FirestoreProperty("password")] public string Password { get; set; } = string.Empty;
    [FirestoreProperty("salt")] public string Salt { get; set; } = String.Empty;
    [FirestoreProperty("role")] public string Role { get; set; } = String.Empty;
    [FirestoreProperty("createdon")] public DateTime CreatedOn { get; set; }
    [FirestoreProperty("modifiedon")] public DateTime ModifiedOn { get; set; }
    [FirestoreProperty("currsubsid")] public string CurrSubscriptionId { get; set; } = string.Empty;
}
