using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class ApiClient
{
    public static string apiBaseUrl =
        "http://192.168.1.108:5286/api";

	// ============================================================
	// DTO SESSION
	// ============================================================
	[Serializable]
	public class UnitySessionDto
	{
		public int UserID;

		public string FirebaseUID;
		public string SessionUid;

		public float DurationMinutes;

		public string StartTime;
		public string EndTime;

		public int Score;
		public int Errors;
	}

	[Serializable]
	private class UnitySessionJsonDto
	{
		public int userID;
		public string firebaseUID;
		public string sessionUid;

		public float durationMinutes;

		public string startTime;
		public string endTime;

		public int score;
		public int errors;
	}
	
	[Serializable]
	private class UnitySessionJsonDtoArray
	{
		public UnitySessionJsonDto[] items;
	}

    // ============================================================
    // DTO SCORE
    // ============================================================
    [Serializable]
    public class UnityScoreDto
    {
        public int UserID;

        public string FirebaseUID;

        public int Score;
        public int Errors;

        public float TimeSpent;

        public string Timestamp;
        public string SessionUid;

        public int ExerciseID;
    }

    // ============================================================
    // DTO FIN DE SESSION
    // ============================================================
    [Serializable]
    public class UnitySessionCompleteDto
    {
        public int UserID;

        public string FirebaseUID;
        public string SessionUid;
    }

    // ============================================================
    // DTO CRÉATION DE PROFIL
    // ============================================================
    [Serializable]
    public class UnityCreateUserDto
    {
        public string FirebaseUID;
        public string Name;
    }

    // ============================================================
    // DTO LIAISON PROFIL EXISTANT
    // ============================================================
    [Serializable]
    public class UnityLinkProfileDto
    {
        public string FirebaseUID;
        public int UserID;
    }

    // ============================================================
    // RÉPONSE PROFIL UTILISATEUR
    // ============================================================
    [Serializable]
    public class UnityUserResponse
    {
        public string status;

        public int userID;

        public string firebaseUID;

        public string name;

        public string createdAt;

        public string message;
    }

    // ============================================================
    // CRÉATION D'UNE NOUVELLE SESSION
    // POST /api/sessions
    // ============================================================
    public static async Task<string> CreateOrUpdateSessionAsync(
        UnitySessionDto dto,
        string idToken)
    {
        using (UnityWebRequest www =
               new UnityWebRequest(
                   $"{apiBaseUrl}/sessions",
                   "POST"))
        {
            string jsonData =
                JsonUtility.ToJson(dto);

            byte[] bodyRaw =
                Encoding.UTF8.GetBytes(jsonData);

            www.uploadHandler =
                new UploadHandlerRaw(bodyRaw);

            www.downloadHandler =
                new DownloadHandlerBuffer();

            www.SetRequestHeader(
                "Content-Type",
                "application/json");

            www.SetRequestHeader(
                "Authorization",
                $"Bearer {idToken}");

            Debug.Log(
                $"[API] Envoi session : {jsonData}"
            );

            await www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(
                    "[API] Erreur envoi session : " +
                    www.error
                );

                Debug.LogError(
                    "[API] Réponse serveur : " +
                    www.downloadHandler.text
                );

                return string.Empty;
            }

            Debug.Log(
                "[API] Session créée avec succès ✅"
            );

            return dto.SessionUid;
        }
    }

    // ============================================================
    // TERMINER UNE SESSION EXISTANTE
    // POST /api/sessions/complete
    // ============================================================
    public static async Task<bool> CompleteSessionAsync(
        int userID,
        string firebaseUID,
        string sessionUid,
        string idToken)
    {
        using (UnityWebRequest www =
               new UnityWebRequest(
                   $"{apiBaseUrl}/sessions/complete",
                   "POST"))
        {
            var dto = new UnitySessionCompleteDto
            {
                UserID = userID,
                FirebaseUID = firebaseUID,
                SessionUid = sessionUid
            };

            string jsonData =
                JsonUtility.ToJson(dto);

            byte[] bodyRaw =
                Encoding.UTF8.GetBytes(jsonData);

            www.uploadHandler =
                new UploadHandlerRaw(bodyRaw);

            www.downloadHandler =
                new DownloadHandlerBuffer();

            www.SetRequestHeader(
                "Content-Type",
                "application/json");

            www.SetRequestHeader(
                "Authorization",
                $"Bearer {idToken}");

            Debug.Log(
                $"[API] Fin de session : {jsonData}"
            );

            await www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(
                    "[API] Erreur fin de session : " +
                    www.error
                );

                Debug.LogError(
                    "[API] Réponse serveur : " +
                    www.downloadHandler.text
                );

                return false;
            }

            Debug.Log(
                "[API] Session terminée avec succès ✅"
            );

            Debug.Log(
                "[API] Réponse serveur : " +
                www.downloadHandler.text
            );

            return true;
        }
    }

	// ============================================================
	// ENVOI DU SCORE
	// POST /api/scores
	// ============================================================
	public static async Task<bool> SendScoreAsync(
		string firebaseUID,
		string idToken,
		UnityScoreDto scoreDto)
	{
		using (UnityWebRequest www =
			   new UnityWebRequest(
				   $"{apiBaseUrl}/scores",
				   "POST"))
		{
			string jsonData =
				JsonUtility.ToJson(scoreDto);

			byte[] bodyRaw =
				Encoding.UTF8.GetBytes(jsonData);

			www.uploadHandler =
				new UploadHandlerRaw(bodyRaw);

			www.downloadHandler =
				new DownloadHandlerBuffer();

			www.SetRequestHeader(
				"Content-Type",
				"application/json");

			www.SetRequestHeader(
				"Authorization",
				$"Bearer {idToken}");

			Debug.Log(
				$"[API] Envoi du score : {jsonData}"
			);

			await www.SendWebRequest();

			if (www.result != UnityWebRequest.Result.Success)
			{
				Debug.LogError(
					"[API] Erreur envoi score : " +
					www.error
				);

				Debug.LogError(
					"[API] Réponse serveur : " +
					www.downloadHandler.text
				);

				return false;
			}

			Debug.Log(
				"[API] Score envoyé avec succès ✅"
			);

			Debug.Log(
				"[API] Réponse serveur : " +
				www.downloadHandler.text
			);

			return true;
		}
	}

    // ============================================================
    // RÉCUPÉRATION DE L'HISTORIQUE DES SESSIONS
    // GET /api/sessions/history/{userID}
    // ============================================================
    
	public static async Task<UnitySessionDto[]> GetSessionsAsync(int userID, string idToken)
	{
		using (UnityWebRequest www =
			UnityWebRequest.Get(
				$"{apiBaseUrl}/sessions/history/{userID}"))
		{
			www.downloadHandler =
				new DownloadHandlerBuffer();

			www.SetRequestHeader(
				"Authorization",
				$"Bearer {idToken}");

			Debug.Log(
				$"[API] Récupération de l'historique " +
				$"du profil UserID={userID}..."
			);

			await www.SendWebRequest();

			if (www.result != UnityWebRequest.Result.Success)
			{
				Debug.LogError(
					"[API] Erreur récupération sessions : " +
					www.error
				);

				Debug.LogError(
					"[API] Réponse serveur : " +
					www.downloadHandler.text
				);

				return new UnitySessionDto[0];
			}

			string json =
				www.downloadHandler.text;

			Debug.Log(
				"[API] Historique reçu : " + json
			);

			UnitySessionJsonDto[] jsonSessions =
				JsonUtility.FromJson<UnitySessionJsonDtoArray>(
					"{\"items\":" + json + "}"
				).items;

			if (jsonSessions == null)
				return new UnitySessionDto[0];

			UnitySessionDto[] sessions =
				new UnitySessionDto[jsonSessions.Length];

			for (int i = 0; i < jsonSessions.Length; i++)
			{
				sessions[i] = new UnitySessionDto
				{
					UserID = jsonSessions[i].userID,
					FirebaseUID = jsonSessions[i].firebaseUID,
					SessionUid = jsonSessions[i].sessionUid,
					DurationMinutes = jsonSessions[i].durationMinutes,
					StartTime = jsonSessions[i].startTime,
					EndTime = jsonSessions[i].endTime,
					Score = jsonSessions[i].score,
					Errors = jsonSessions[i].errors
				};
			}

			return sessions;
		}
	}
    
	[Serializable]
    private class UnitySessionDtoArray
    {
        public UnitySessionDto[] items;
    }

    // ============================================================
    // CRÉATION OU RÉCUPÉRATION DU PROFIL
    // POST /api/users/create-or-get
    // ============================================================
    public static async Task<UnityUserResponse> CreateOrGetUserAsync(
        string firebaseUID,
        string pseudo,
        string idToken)
    {
        using (UnityWebRequest www =
               new UnityWebRequest(
                   $"{apiBaseUrl}/users/create-or-get",
                   "POST"))
        {
            var dto = new UnityCreateUserDto
            {
                FirebaseUID = firebaseUID,
                Name = pseudo
            };

            string jsonData =
                JsonUtility.ToJson(dto);

            byte[] bodyRaw =
                Encoding.UTF8.GetBytes(jsonData);

            www.uploadHandler =
                new UploadHandlerRaw(bodyRaw);

            www.downloadHandler =
                new DownloadHandlerBuffer();

            www.SetRequestHeader(
                "Content-Type",
                "application/json");

            www.SetRequestHeader(
                "Authorization",
                $"Bearer {idToken}");

            Debug.Log(
                $"[API] Création/récupération du profil : " +
                $"{jsonData}"
            );

            await www.SendWebRequest();

            string responseJson =
                www.downloadHandler.text;

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(
                    "[API] Erreur profil utilisateur : " +
                    www.error
                );

                Debug.LogError(
                    "[API] Réponse serveur : " +
                    responseJson
                );

                // On essaie quand même de lire la réponse JSON,
                // notamment pour récupérer "pseudo_exists".
                if (!string.IsNullOrEmpty(responseJson))
                {
                    try
                    {
                        return JsonUtility.FromJson<UnityUserResponse>(
                            responseJson
                        );
                    }
                    catch
                    {
                        // Réponse non exploitable.
                    }
                }

                return null;
            }

            Debug.Log(
                "[API] Profil utilisateur reçu : " +
                responseJson
            );

            return JsonUtility.FromJson<UnityUserResponse>(
                responseJson
            );
        }
    }

    // ============================================================
    // ASSOCIER UN PROFIL EXISTANT
    // POST /api/users/link-profile
    // ============================================================
    public static async Task<UnityUserResponse> LinkProfileAsync(
        string firebaseUID,
        int userID,
        string idToken)
    {
        using (UnityWebRequest www =
               new UnityWebRequest(
                   $"{apiBaseUrl}/users/link-profile",
                   "POST"))
        {
            var dto = new UnityLinkProfileDto
            {
                FirebaseUID = firebaseUID,
                UserID = userID
            };

            string jsonData =
                JsonUtility.ToJson(dto);

            byte[] bodyRaw =
                Encoding.UTF8.GetBytes(jsonData);

            www.uploadHandler =
                new UploadHandlerRaw(bodyRaw);

            www.downloadHandler =
                new DownloadHandlerBuffer();

            www.SetRequestHeader(
                "Content-Type",
                "application/json");

            www.SetRequestHeader(
                "Authorization",
                $"Bearer {idToken}");

            Debug.Log(
                $"[API] Liaison du profil existant : " +
                $"{jsonData}"
            );

            await www.SendWebRequest();

            string responseJson =
                www.downloadHandler.text;

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(
                    "[API] Erreur liaison profil : " +
                    www.error
                );

                Debug.LogError(
                    "[API] Réponse serveur : " +
                    responseJson
                );

                if (!string.IsNullOrEmpty(responseJson))
                {
                    try
                    {
                        return JsonUtility.FromJson<UnityUserResponse>(
                            responseJson
                        );
                    }
                    catch
                    {
                        // Réponse non exploitable.
                    }
                }

                return null;
            }

            Debug.Log(
                "[API] Profil existant lié avec succès : " +
                responseJson
            );

            return JsonUtility.FromJson<UnityUserResponse>(
                responseJson
            );
        }
    }
}