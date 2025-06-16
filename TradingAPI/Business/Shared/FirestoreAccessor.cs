using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

namespace TradingAPI.Business.Shared;

public class FirestoreAccessor
{
    public FirestoreDb? Db { get; private set; }
    public void SetDb(FirestoreDb db)
    {
        Db = db;
        Console.WriteLine("FirestoreDb set successfully.");
    }
}
