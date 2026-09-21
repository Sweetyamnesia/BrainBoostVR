using System;
using System.Threading.Tasks;
using UnityEngine;

public class TestApi : MonoBehaviour
{
    private string currentSessionId = string.Empty;

    private async void Start()
    {
        await StartTestSession();
    }

    private async Task StartTestSession()
    {
        try
        {
            // Vérifie qu'un utilisateur Firebase est connecté
            var user = Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser;
            if (user == null)
            {
                Debug.LogError("[TestApi] Aucun utilisateur connecté Firebase !");
                return;
            }

            string firebaseUID = user.UserId;

            // Récupère le token Firebase ID
            string idToken = await user.TokenAsync(false);
            Debug.Log("[TestApi] Firebase ID Token récupéré : " + idToken);

            // Prépare le DTO pour créer une session
            var dto = new ApiClient.UnitySessionDto
            {
                FirebaseUID = firebaseUID,
                SessionUid = Guid.NewGuid().ToString(),
                StartTime = DateTime.UtcNow.ToString("o"),
                EndTime = DateTime.UtcNow.ToString("o"),
                DurationMinutes = 0f,
                Score = 0,
                Errors = 0
            };

            // Appelle l'API pour créer ou mettre à jour la session
            currentSessionId = await ApiClient.CreateOrUpdateSessionAsync(dto, idToken);

            if (!string.IsNullOrEmpty(currentSessionId))
            {
                Debug.Log("[TestApi] Session créée avec succès : " + currentSessionId);
            }
            else
            {
                Debug.LogError("[TestApi] Échec création session.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("[TestApi] Exception : " + ex.Message);
        }
    }
}
