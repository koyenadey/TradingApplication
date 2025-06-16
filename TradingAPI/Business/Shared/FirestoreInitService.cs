using System.Text.Json;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Google.Cloud.Firestore;

namespace TradingAPI.Business.Shared;

public class FirestoreInitService : IHostedService
{
    private readonly IAmazonSecretsManager _secretManager;
    private readonly GetSecretValueRequest _secretRequest;
    private readonly FirestoreAccessor _firestoreAccessor;
    public FirestoreInitService(IAmazonSecretsManager secretManager, GetSecretValueRequest secretRequest, FirestoreAccessor firestoreAccessor)
    {
        _secretManager = secretManager;
        _secretRequest = secretRequest;
        _firestoreAccessor = firestoreAccessor;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await InitAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task InitAsync()
    {
        try
        {
            var response = await _secretManager.GetSecretValueAsync(_secretRequest);

            string firebaseKeyJson = response.SecretString;

            //Console.WriteLine("Secret string retrieved: " + firebaseKeyJson);

            if (string.IsNullOrEmpty(firebaseKeyJson))
            {
                throw new Exception("Secret string is empty.");
            }

            // Parse the JSON to get the 'firebase-key.json' content
            var secretJson = JsonDocument.Parse(firebaseKeyJson);

            //Console.WriteLine("Secret JSON parsed successfully." + secretJson);

            if (!secretJson.RootElement.TryGetProperty("firebase-key.json", out var firebaseJsonElement))
            {
                throw new Exception("The 'firebase-key.json' key was not found in the secret.");
            }

            //Console.WriteLine("Firebase JSON parsed successfully." + firebaseJsonElement);

            // Now parse the JSON string inside 'firebase-key.json'
            var firebaseJsonstring = firebaseJsonElement.GetString();

            if (string.IsNullOrEmpty(firebaseJsonstring))
            {
                throw new Exception("The 'firebase-key.json' key contains an empty string.");
            }

            var firebaseJson = JsonDocument.Parse(firebaseJsonstring);
            if (!firebaseJson.RootElement.TryGetProperty("project_id", out var projectIdElement))
            {
                throw new Exception("The 'project_id' key was not found in the firebase-key.json.");
            }
            string projectId = projectIdElement.GetString();

            // 1. Write the credentials to a temporary file
            string tempFilePath = Path.GetTempFileName();
            File.WriteAllText(tempFilePath, firebaseJsonstring);
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", tempFilePath);


            // 2. Initialize FirestoreDb
            var Db = FirestoreDb.Create(projectId);
            _firestoreAccessor.SetDb(Db);

            Console.WriteLine("Firestore initialized successfully.");
        }
        catch (Exception ex)
        {
            // Handle exception
            Console.WriteLine($"From Init Service Error initializing Firestore: {ex}");
        }

    }
}
