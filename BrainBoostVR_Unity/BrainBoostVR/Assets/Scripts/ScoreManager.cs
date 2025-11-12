using System;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    public int maxScore = 5;
    public int errors = 0;
    public float sessionTime = 0f;
    public bool exerciseRunning = false;

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
        sessionTime = time;
        sessionTimer = time;
    }

    // ---------------- START SESSION ----------------
    public async void StartSession()
    {
        if (exerciseRunning) return;

        exerciseRunning = true;
        score = 0;
        errors = 0;
        sessionTime = 0f;
        sessionTimer = 0f;
        OnScoreChanged?.Invoke(score);

        Debug.Log("[SESSION] Début de la session");

        try
        {
            var user = FirebaseAuth.DefaultInstance.CurrentUser;
            if (user == null)
            {
                Debug.LogError("[SESSION] Utilisateur Firebase non connecté !");
                exerciseRunning = false;
                return;
            }

            string firebaseUID = user.UserId;
            string idToken = await user.TokenAsync(false);

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

            currentSessionId = await ApiClient.CreateOrUpdateSessionAsync(dto, idToken);
            if (string.IsNullOrEmpty(currentSessionId))
            {
                Debug.LogError("[SESSION] Impossible de créer la session côté serveur !");
                exerciseRunning = false;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("[SESSION] Exception lors de StartSession : " + ex.Message);
            exerciseRunning = false;
        }
    }

    // ---------------- END SESSION ----------------
    public async void EndSession()
    {
        if (!exerciseRunning) return;

        exerciseRunning = false;

        var user = FirebaseAuth.DefaultInstance.CurrentUser;
        if (user == null)
        {
            Debug.LogError("[SESSION] Utilisateur Firebase non connecté !");
            return;
        }

        string firebaseUID = user.UserId;
        string idToken = await user.TokenAsync(false);

        SessionRecord record = new SessionRecord
        {
            score = score,
            timeSpent = sessionTime,
            errors = errors,
            sessionId = currentSessionId,
            timestamp = DateTime.UtcNow.ToString("o")
        };

        sessionHistory.Add(record);

        Debug.Log($"[SESSION] Fin de session : Score={score}, Temps={sessionTime:F2}s, Erreurs={errors}");

        // ✅ Crée un vrai DTO complet pour l'API
        var scoreDto = new ApiClient.UnityScoreDto
        {
            FirebaseUID = firebaseUID,
            Score = score,
            Errors = errors,
            TimeSpent = sessionTime,
            Timestamp = record.timestamp,
            SessionUid = currentSessionId
        };

        // ✅ Envoi du score
        try
        {
            await ApiClient.SendScoreAsync(firebaseUID, idToken, scoreDto);
            Debug.Log("[SESSION] Score envoyé avec succès ✅");
        }
        catch (Exception ex)
        {
            Debug.LogError("[SESSION] Erreur lors de l’envoi du score : " + ex.Message);
        }

        // ✅ Mise à jour de la session
        try
        {
            var updateDto = new ApiClient.UnitySessionDto
            {
                FirebaseUID = firebaseUID,
                SessionUid = currentSessionId,
                StartTime = DateTime.UtcNow.AddSeconds(-sessionTime).ToString("o"),
                EndTime = DateTime.UtcNow.ToString("o"),
                DurationMinutes = sessionTime / 60f,
                Score = score,
                Errors = errors
            };

            await ApiClient.CreateOrUpdateSessionAsync(updateDto, idToken);
            Debug.Log("[SESSION] Session mise à jour avec score et durée ✅");
        }
        catch (Exception ex)
        {
            Debug.LogError("[SESSION] Erreur lors de la mise à jour de la session : " + ex.Message);
        }

        OnExerciseFinished?.Invoke(record);
    }

    // ---------------- SCORE / ERREURS ----------------
    public void AddPoints(int points = 1)
    {
        if (!exerciseRunning) return;
        score += points;
        if (score > maxScore) score = maxScore;

        Debug.Log($"[SCORE] Score actuel : {score}/{maxScore}");
        OnScoreChanged?.Invoke(score);

        if (score >= maxScore) EndSession();
    }

    public void RegisterError()
    {
        if (!exerciseRunning) return;
        errors++;
        Debug.Log($"[SCORE] Erreurs : {errors}");
    }

    public void ResetScore()
    {
        score = 0;
        errors = 0;
        sessionTime = 0f;
        sessionTimer = 0f;
        exerciseRunning = false;
        currentSessionId = string.Empty;

        Debug.Log("[SCORE] Réinitialisation pour nouvelle session");
        OnScoreChanged?.Invoke(score);
    }
}
