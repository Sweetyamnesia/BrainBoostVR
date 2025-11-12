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
            // Crée FirebaseApp seulement si aucune instance par défaut
            if (FirebaseApp.DefaultInstance == null)
            {
                _app = FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromFile("Config/brainboostvr-firebase-adminsdk-fbsvc-841d9189b7.json")
                });
            }
            else
            {
                _app = FirebaseApp.DefaultInstance;
            }
        }

        public async Task<string> VerifyTokenAsync(string token)
        {
            try
            {
                var decoded = await FirebaseAuth.GetAuth(_app).VerifyIdTokenAsync(token);
                Console.WriteLine($"[Middleware] UID validé : {decoded.Uid}");
                return decoded.Uid;
            }
            catch (FirebaseAuthException ex)
            {
                Console.WriteLine($"[Middleware] Erreur Firebase : {ex.Message}");
                throw new UnauthorizedAccessException("Token invalide ou expiré.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Middleware] Erreur inattendue : {ex}");
                throw new UnauthorizedAccessException("Erreur lors de la validation du token.");
            }
        }
    }
}
