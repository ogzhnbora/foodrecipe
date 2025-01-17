using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;

public class FirebaseService
{
    public FirebaseService()
    {
        if (FirebaseApp.DefaultInstance == null)
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("firebase-service-account.json")
            });
                    Console.WriteLine("Firebase bağlantısı başarılı!");

        }
    }

    public async Task<string> RegisterUserAsync(string email, string password)
    {
        var userRecordArgs = new UserRecordArgs()
        {
            Email = email,
            Password = password
        };

        var userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(userRecordArgs);
        return userRecord.Uid;
    }

    public async Task UpdateUserProfileAsync(string userId, string firstName, string lastName)
    {
        var user = new UserRecordArgs()
        {
            Uid = userId,
            DisplayName = $"{firstName} {lastName}"
        };

        await FirebaseAuth.DefaultInstance.UpdateUserAsync(user);
    }

    public async Task<UserRecord> GetUserByEmailAsync(string email)
    {
        return await FirebaseAuth.DefaultInstance.GetUserByEmailAsync(email);
    }
}
