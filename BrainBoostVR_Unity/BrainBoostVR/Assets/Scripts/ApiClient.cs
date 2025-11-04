using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class ApiClient
{
    private static readonly string baseUrl = "https://localhost:5286/api"; // ← mon endpoint réel

    [System.Serializable]
    private class ScorePayload
    {
        public string userId;
        public int score;
        public int errors;
        public float timeSpent;
        public string sessionId;
        public string timestamp;
    }

    public static async Task SendScoreAsync(string userId, string idToken, ScoreManager.SessionRecord record)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(idToken))
        {
            Debug.LogWarning("[API] UserId ou Token vide, envoi annulé.");
            return;
        }

        var payload = new ScorePayload
        {
            userId = userId,
            score = record.score,
            errors = record.errors,
            timeSpent = record.timeSpent,
            sessionId = record.sessionId,
            timestamp = record.timestamp
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
}
