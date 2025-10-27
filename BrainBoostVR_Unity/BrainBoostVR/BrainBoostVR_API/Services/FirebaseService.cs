using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;

namespace BrainBoostVR_API.Services
{
    public class FirebaseService
    {
        private readonly FirebaseApp _app;

        public FirebaseService()
        {
            // ✅ Empêche les doublons de FirebaseApp
            _app = FirebaseApp.DefaultInstance ?? FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile(
                    "/Users/angelarhin/Desktop/BrainBoostVR/BrainBoostVR_Unity/BrainBoostVR/BrainBoostVR_API/Config/brainboostvr-firebase-adminsdk-fbsvc-841d9189b7.json"
                )
            });
        }

        public async Task<string> VerifyTokenAsync(string token)
        {
            try
            {
                var decoded = await FirebaseAuth.GetAuth(_app).VerifyIdTokenAsync(token);
                return decoded.Uid;
            }
            catch
            {
                throw new UnauthorizedAccessException("Invalid or expired Firebase token.");
            }
        }
    }
}
