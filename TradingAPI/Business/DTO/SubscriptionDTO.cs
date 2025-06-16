using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

namespace TradingAPI.Business.DTO;

public class SubscriptionReadDTO
{
    public string Id { get; set; } = String.Empty;
    public string SubscriptionId { get; set; } = String.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
}

[FirestoreData]
public class SubscriptionCreateDTO
{
    [FirestoreProperty("startdate")] public DateTime StartDate { get; set; }
    [FirestoreProperty("enddate")] public DateTime EndDate { get; set; }
    [FirestoreProperty("isactive")] public bool IsActive { get; set; }
    [FirestoreProperty("subscriptionid")] public string SubscriptionId { get; set; } = String.Empty;
}

public class SubscripDetailReadDTO
{
    public string SubscriptionId { get; set; } = string.Empty;
    public string SubscriptionName { get; set; } = string.Empty;
    public string PlaylistId { get; set; } = string.Empty;
}
public class SubscripDetailCreateDTO
{
    public string SubscriptionName { get; set; } = string.Empty;
    public string PlaylistId { get; set; } = string.Empty;
}
public class SubscripDetailUpdateDTO
{
    public string PlaylistId { get; set; } = string.Empty;
}
public class SubscripDetailDeleteDTO
{
    public string SubscriptionId { get; set; } = string.Empty;
}
