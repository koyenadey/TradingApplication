using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using TradingAPI.Business.DTO;
using TradingAPI.Business.ServiceAbstract.Services;
using TradingAPI.Business.Shared;

namespace TradingAPI.Business.ServiceImplementation.Services;

public class SubscriptionServices : ISubscriptionServices
{
    private readonly FirestoreDb _firestoreDb;
    private readonly string COLLEC_NAME = "subscription";
    public SubscriptionServices(FirestoreAccessor firestoreAccessor)
    {
        _firestoreDb = firestoreAccessor.Db ?? throw new InvalidOperationException("FirestoreDb has not been initialized.");
    }
    public async Task<ActionResult<List<SubscripDetailReadDTO>>> GetAllSubscriptionsAsync()
    {
        var subscripDocRef = await _firestoreDb.Collection(COLLEC_NAME).GetSnapshotAsync();
        List<SubscripDetailReadDTO> allSubscriptions = new();
        foreach (var doc in subscripDocRef)
        {
            allSubscriptions.Add(new SubscripDetailReadDTO
            {
                SubscriptionId = doc.Id,
                SubscriptionName = doc.GetValue<string>("Name"),
                PlaylistId = doc.GetValue<string>("PlaylistId"),
            });
        }
        return allSubscriptions;
    }
    public async Task<ActionResult<SubscripDetailReadDTO>> CreateSubscriptionAsync(SubscripDetailCreateDTO subscripDetailCreateDTO)
    {
        var subscriptionRef = _firestoreDb.Collection(COLLEC_NAME);
        var subscripDocRef = await subscriptionRef.AddAsync(subscripDetailCreateDTO);
        var subscriptionDoc = await subscripDocRef.GetSnapshotAsync();
        return new SubscripDetailReadDTO
        {
            SubscriptionId = subscriptionRef.Id,
            SubscriptionName = subscriptionDoc.GetValue<string>("Name"),
            PlaylistId = subscriptionDoc.GetValue<string>("PlaylistId"),
        };
    }


    public async Task<ActionResult<SubscripDetailReadDTO>> UpdateSubscriptionAsync(string playListId, string subsId)
    {
        var subscripDocRef = _firestoreDb.Collection("COLLEC_NAME").Document(subsId);
        await subscripDocRef.UpdateAsync("PlaylistId", playListId);
        var updatedDataDoc = await subscripDocRef.GetSnapshotAsync();
        return new SubscripDetailReadDTO
        {
            SubscriptionId = subscripDocRef.Id,
            SubscriptionName = updatedDataDoc.GetValue<string>("Name"),
            PlaylistId = updatedDataDoc.GetValue<string>("PlaylistId"),
        };
    }
    public async Task<ActionResult<SubscripDetailReadDTO>> DeleteSubscriptionAsync(string subscriptionId)
    {
        var subscripDocRef = _firestoreDb.Collection("COLLEC_NAME").Document(subscriptionId);
        var toDeleteSubscripDoc = await subscripDocRef.GetSnapshotAsync();
        await subscripDocRef.DeleteAsync();
        return new SubscripDetailReadDTO
        {
            SubscriptionId = subscripDocRef.Id,
            SubscriptionName = toDeleteSubscripDoc.GetValue<string>("Name"),
            PlaylistId = toDeleteSubscripDoc.GetValue<string>("PlaylistId"),
        };
    }
}
