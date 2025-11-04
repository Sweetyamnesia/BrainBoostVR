using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class ApiClient
{
    private static readonly string baseUrl = "https://localhost:5286/api"; // ton endpoint

    // ----------------- DTO -----------------
    [System.Serializable]
    private class UnityUserDto
    {
        public string FirebaseUID;
        public string Name;
    }

    [System.Serializable]
    private class UserResponse
    {
        public string FirebaseUID;
        public string Name;
        public string CreatedAt;
    }

    [System.Serializable]
    private class UnityScoreDto
    {
        public string FirebaseUID;
        public int Score;
        public int Errors;
        public float TimeSpent;
        public string SessionId;
        public string Timestamp;
    }

    [System.Serializable]
    public class UnitySessionDto
    {
        public string FirebaseUID;
        public string SessionId;
        public float DurationMinutes;
        public string StartTime; // string pour JsonUtility
        public string EndTime;
        public int Score;
        public int Errors;
    }

    [System.Serializable]
    private class UnitySessionDtoWrapper
    {
        public UnitySessionDto[] Sessions;
    }

    // ----------------- Méthodes -----------------

    // Créer ou récupérer un utilisateur via le pseudo
    public static async Task<string> CreateOrGetUserAsync(string pseudo, string idToken)
    {
        if (string.IsNullOrEmpty(pseudo) || string.IsNullOrEmpty(idToken))
        {
            Debug.LogWarning("[API] Pseudo ou token vide, action annulée.");
            return null;
        }

        var payload = new UnityUserDto
        {
            FirebaseUID = pseudo,
            Name = pseudo
        };

        string json = JsonUtility.ToJson(payload);
        string url = $"{baseUrl}/users/create-or-get";

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {idToken}");

            Debug.Log("[API] Envoi du pseudo : " + json);
            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"[API] Erreur création/récupération utilisateur : {request.error} - {request.downloadHandler.text}");
                return null;
            }
            else
            {
                var responseJson = request.downloadHandler.text;
                var user = JsonUtility.FromJson<UserResponse>(responseJson);
                Debug.Log($"[API] Utilisateur récupéré ou créé : {user.FirebaseUID}");
                return user.FirebaseUID;
            }
        }
    }

    // Envoyer un score à l'API
    public static async Task SendScoreAsync(string firebaseUID, string idToken, ScoreManager.SessionRecord record)
    {
        if (string.IsNullOrEmpty(firebaseUID) || string.IsNullOrEmpty(idToken))
        {
            Debug.LogWarning("[API] FirebaseUID ou Token vide, envoi annulé.");
            return;
        }

        var payload = new UnityScoreDto
        {
            FirebaseUID = firebaseUID,
            Score = record.score,
            Errors = record.errors,
            TimeSpent = record.timeSpent,
            SessionId = record.sessionId,
            Timestamp = record.timestamp
        };

        string json = JsonUtility.ToJson(payload);
        string url = $"{baseUrl}/scores";

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {idToken}");

            Debug.Log("[API] Envoi des données : " + json);
            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
                Debug.LogError($"[API] Erreur : {request.error} - {request.downloadHandler.text}");
            else
                Debug.Log("[API] Score envoyé avec succès !");
        }
    }

    // Récupérer les sessions d’un utilisateur
    public static async Task<UnitySessionDto[]> GetSessionsAsync(string firebaseUID, string idToken)
    {
        if (string.IsNullOrEmpty(firebaseUID) || string.IsNullOrEmpty(idToken))
        {
            Debug.LogWarning("[API] FirebaseUID ou Token vide, requête annulée.");
            return null;
        }

        string url = $"{baseUrl}/sessions?userId={firebaseUID}";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("Authorization", $"Bearer {idToken}");
            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"[API] Erreur récupération sessions : {request.error}");
                return null;
            }
            else
            {
                var responseJson = request.downloadHandler.text;
                var wrapper = JsonUtility.FromJson<UnitySessionDtoWrapper>(responseJson);
                return wrapper.Sessions;
            }
        }
    }
}
