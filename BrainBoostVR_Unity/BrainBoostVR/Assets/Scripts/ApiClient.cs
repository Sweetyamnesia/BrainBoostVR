using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;


public static class ApiClient
{
    public static string apiBaseUrl = "http://192.168.1.108:5286/api";

    [Serializable]
    public class UnitySessionDto
    {
        public string FirebaseUID;
        public string SessionUid;
        public float DurationMinutes;
        public string StartTime;
        public string EndTime;
        public int Score;
        public int Errors;
    }

    [Serializable]
    public class UnityScoreDto
    {
        public string FirebaseUID;
        public int Score;
        public int Errors;
        public float TimeSpent;
        public string Timestamp;
        public string SessionUid;
    }

    // 🔹 Création ou mise à jour d’une session
    public static async Task<string> CreateOrUpdateSessionAsync(UnitySessionDto dto, string idToken)
    {
        using (UnityWebRequest www = new UnityWebRequest($"{apiBaseUrl}/sessions", "POST"))
        {
            string jsonData = JsonUtility.ToJson(dto);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", $"Bearer {idToken}");

            Debug.Log($"[API] Envoi session : {jsonData}");

            await www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("[API] Erreur envoi session : " + www.error);
                return string.Empty;
            }

            Debug.Log("[API] Session créée ou mise à jour ✅");
            return dto.SessionUid;
        }
    }

	// 🔹 Envoi du score
	public static async Task<bool> SendScoreAsync(string firebaseUID, string idToken, UnityScoreDto scoreDto)
	{
		using (UnityWebRequest www = new UnityWebRequest($"{apiBaseUrl}/scores", "POST"))
		{
			string jsonData = JsonUtility.ToJson(scoreDto);
			byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
			www.uploadHandler = new UploadHandlerRaw(bodyRaw);
			www.downloadHandler = new DownloadHandlerBuffer();
			www.SetRequestHeader("Content-Type", "application/json");
			www.SetRequestHeader("Authorization", $"Bearer {idToken}");

			Debug.Log($"[API] Envoi du score : {jsonData}");

			await www.SendWebRequest();

			if (www.result != UnityWebRequest.Result.Success)
			{
				Debug.LogError("[API] Erreur envoi score : " + www.error);
				return false;
			}

			Debug.Log("[API] Score envoyé avec succès ✅");
			return true;
		}
	}

	// 🔹 Récupération de l'historique des sessions
	public static async Task<UnitySessionDto[]> GetSessionsAsync(string firebaseUID, string idToken)
	{
		using (UnityWebRequest www = UnityWebRequest.Get(
			$"{apiBaseUrl}/sessions/history/{firebaseUID}"))
		{
			www.downloadHandler = new DownloadHandlerBuffer();
			www.SetRequestHeader("Authorization", $"Bearer {idToken}");

			Debug.Log("[API] Récupération de l'historique des sessions...");

			await www.SendWebRequest();

			if (www.result != UnityWebRequest.Result.Success)
			{
				Debug.LogError("[API] Erreur récupération sessions : " + www.error);
				return new UnitySessionDto[0];
			}

			string json = www.downloadHandler.text;

			Debug.Log("[API] Historique reçu : " + json);

			return JsonUtility.FromJson<UnitySessionDtoArray>(
				"{\"items\":" + json + "}"
			).items;
		}
	}

	[Serializable]
	private class UnitySessionDtoArray
	{
		public UnitySessionDto[] items;
	}
}
