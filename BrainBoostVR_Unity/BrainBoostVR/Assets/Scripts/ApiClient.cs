using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class ApiClient
{
    private static readonly string baseUrl = "https://localhost:5286/api"; // ton endpoint

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

    // Méthode pour créer ou récupérer un utilisateur via le pseudo
    public static async Task<string> CreateOrGetUserAsync(string pseudo, string idToken)
    {
        if (string.IsNullOrEmpty(pseudo) || string.IsNullOrEmpty(idToken))
        {
            Debug.LogWarning("[API] Pseudo ou token vide, action annulée.");
            return null;
        }

        var payload = new UnityUserDto
        {
            FirebaseUID = pseudo, // on utilise le pseudo comme identifiant temporaire côté API
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
}
