using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

namespace TradingAPI.Core.Entity;

public class Subscription
{

    public string Id { get; set; } = String.Empty;
    [FirestoreProperty("name")] public string Name { get; set; } = String.Empty;
    [FirestoreProperty("playlistid")] public Guid PlaylistId { get; set; }
}
