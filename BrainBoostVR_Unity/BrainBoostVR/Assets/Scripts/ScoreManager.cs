using System;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;

public class ScoreManager : MonoBehaviour
{
    [Header("Score de l'exercice")]
    public int score = 0;
    public int maxScore = 5;
    public int errors = 0;

    [Header("Temps")]
    public float sessionTime = 0f;

    [Header("État")]
    public bool exerciseRunning = false;
    public bool sessionRunning = false;

    [Header("Session")]
    public string currentSessionId = string.Empty;
    public List<SessionRecord> sessionHistory = new List<SessionRecord>();

    public event Action<int> OnScoreChanged;
    public event Action<SessionRecord> OnExerciseFinished;

    [Serializable]
    public class SessionRecord
    {
        public int score;
        public float timeSpent;
        public int errors;
        public string sessionId;
        public string timestamp;
        public string sessionUid;
    }

    private float sessionTimer = 0f;

    void Update()
    {
        if (exerciseRunning)
        {
            sessionTimer += Time.deltaTime;
            sessionTime = sessionTimer;
        }
    }

    public void UpdateSessionTime(float time)
    {
        if (!exerciseRunning) return;

        sessionTime = time;
        sessionTimer = time;
    }

    // ---------------- START SESSION ----------------
    public async void StartSession()
    {
        // Une session ne doit être créée qu'une seule fois.
        if (sessionRunning)
        {
            Debug.Log("[SESSION] Une session est déjà en cours.");
            return;
        }

        sessionRunning = true;

        Debug.Log("[SESSION] Début de la session");

        try
        {
            var user = FirebaseAuth.DefaultInstance.CurrentUser;

            if (user == null)
            {
                Debug.LogError("[SESSION] Utilisateur Firebase non connecté !");
                sessionRunning = false;
                return;
            }

            string firebaseUID = user.UserId;
            string idToken = await user.TokenAsync(false);

            var dto = new ApiClient.UnitySessionDto
            {
                FirebaseUID = firebaseUID,
                SessionUid = Guid.NewGuid().ToString(),
                StartTime = DateTime.Now.ToString("o"),
                EndTime = DateTime.Now.ToString("o"),
                DurationMinutes = 0f,
                Score = 0,
                Errors = 0
            };

            currentSessionId = await ApiClient.CreateOrUpdateSessionAsync(dto, idToken);

            if (string.IsNullOrEmpty(currentSessionId))
            {
                Debug.LogError("[SESSION] Impossible de créer la session côté serveur !");
                sessionRunning = false;
                return;
            }

            Debug.Log($"[SESSION] Session créée : {currentSessionId}");
        }
        catch (Exception ex)
        {
            Debug.LogError("[SESSION] Exception lors de StartSession : " + ex.Message);
            sessionRunning = false;
        }
    }

    // ---------------- START EXERCISE ----------------
    public void StartExercise()
    {
        if (!sessionRunning)
        {
            Debug.LogWarning("[EXERCISE] Impossible de démarrer l'exercice : aucune session active.");
            return;
        }

        if (exerciseRunning)
        {
            Debug.LogWarning("[EXERCISE] Un exercice est déjà en cours.");
            return;
        }

        exerciseRunning = true;

        score = 0;
        errors = 0;
        sessionTime = 0f;
        sessionTimer = 0f;

        OnScoreChanged?.Invoke(score);

        Debug.Log("[EXERCISE] Début de l'exercice");
    }

    // ---------------- END EXERCISE ----------------
    public void EndExercise()
    {
        if (!exerciseRunning)
        {
            Debug.LogWarning("[EXERCISE] Aucun exercice en cours.");
            return;
        }

        exerciseRunning = false;

        SessionRecord record = new SessionRecord
        {
            score = score,
            timeSpent = sessionTime,
            errors = errors,
            sessionId = currentSessionId,
            sessionUid = currentSessionId,
            timestamp = DateTime.UtcNow.ToString("o")
        };

        sessionHistory.Add(record);

        Debug.Log(
            $"[EXERCISE] Fin de l'exercice : " +
            $"Score={score}, Temps={sessionTime:F2}s, Erreurs={errors}"
        );

        // L'exercice est terminé, MAIS LA SESSION RESTE ACTIVE.
        OnExerciseFinished?.Invoke(record);
    }

    // ---------------- END SESSION ----------------
    public async void EndSession()
	{
		if (!sessionRunning)
		{
			Debug.LogWarning("[SESSION] Aucune session active.");
			return;
		}

		var user = FirebaseAuth.DefaultInstance.CurrentUser;

		if (user == null)
		{
			Debug.LogError("[SESSION] Utilisateur Firebase non connecté !");
			return;
		}

		if (string.IsNullOrEmpty(currentSessionId))
		{
			Debug.LogError(
				"[SESSION] SessionUid vide. Impossible de terminer la session."
			);
			return;
		}

		try
		{
			string firebaseUID = user.UserId;
			string idToken = await user.TokenAsync(false);

			Debug.Log("[SESSION] Fin de la session...");

			bool success =
				await ApiClient.CompleteSessionAsync(
					firebaseUID,
					currentSessionId,
					idToken
				);

			if (success)
			{
				Debug.Log(
					"[SESSION] Session terminée côté serveur ✅"
				);

				sessionRunning = false;
				currentSessionId = string.Empty;
			}
			else
			{
				Debug.LogError(
					"[SESSION] Impossible de terminer la session côté serveur."
				);
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(
				"[SESSION] Erreur lors de la fin de session : " +
				ex.Message
			);
		}
	}

    // ---------------- SCORE / ERREURS ----------------
    public void AddPoints(int points = 1)
    {
        if (!exerciseRunning) return;

        score += points;

        if (score > maxScore)
            score = maxScore;

        Debug.Log($"[SCORE] Score actuel : {score}/{maxScore}");

        OnScoreChanged?.Invoke(score);

        // Atteindre le score maximum termine uniquement l'exercice.
        if (score >= maxScore)
        {
            EndExercise();
        }
    }

    public void RegisterError()
    {
        if (!exerciseRunning) return;

        errors++;

        Debug.Log($"[SCORE] Erreurs : {errors}");
    }

    // ---------------- RESET EXERCISE ----------------
    public void ResetScore()
    {
        score = 0;
        errors = 0;
        sessionTime = 0f;
        sessionTimer = 0f;
        exerciseRunning = false;

        Debug.Log("[EXERCISE] Réinitialisation pour un nouvel exercice");

        OnScoreChanged?.Invoke(score);
    }
}
