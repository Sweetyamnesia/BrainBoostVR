using System;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // ---------------- Variables principales ----------------
    public int score = 0;
    public int maxScore = 5;
    public int errors = 0;

    public float sessionTime = 0f;

    public bool exerciseRunning = false;

    public List<SessionRecord> sessionHistory = new List<SessionRecord>();

    // ---------------- Events ----------------
    public event Action<int> OnScoreChanged;                 // déclenché à chaque changement de score
    public event Action<SessionRecord> OnExerciseFinished;   // déclenché à la fin d'une session

    // ---------------- Classe pour les sessions ----------------
    [Serializable]
    public class SessionRecord
    {
        public int score;
        public float timeSpent;
        public int errors;        // corrigé : on garde le nombre d'erreurs
        public string sessionId;  // optionnel : utile si tu veux un id unique
        public string timestamp;  // optionnel : ISO timestamp
    }

    // ---------------- Méthodes ----------------

    // Ajouter des points
    public void AddPoints(int points = 1)
    {
        if (!exerciseRunning) return;

        score += points;

        if (score > maxScore)
            score = maxScore;

        Debug.Log($"[SCORE] Objet correctement placé. Score actuel : {score} / {maxScore}");
        OnScoreChanged?.Invoke(score);

        if (score >= maxScore)
        {
            EndSession();
        }
    }

    public void RegisterError()
    {
        if (!exerciseRunning) return;

        errors++;
        Debug.Log($"Erreur enregistrée. Total erreurs : {errors}");
    }

    // Réinitialiser le score pour une nouvelle session
    public void ResetScore()
    {
        score = 0;
        errors = 0;
        sessionTime = 0f;
        exerciseRunning = false;

        Debug.Log("[SCORE] Score et erreurs réinitialisés pour nouvelle session.");
        OnScoreChanged?.Invoke(score);
    }

    // Démarrer une session
    public void StartSession()
    {
        exerciseRunning = true;
        score = 0;
        errors = 0;
        sessionTime = 0f;

        Debug.Log("[SCORE] Session commencée.");
        OnScoreChanged?.Invoke(score);
    }

    // Terminer une session
    public async void EndSession()
    {
        if (!exerciseRunning) return;

        exerciseRunning = false;

        SessionRecord record = new SessionRecord()
        {
            score = score,
            timeSpent = sessionTime,
            errors = errors,
            sessionId = System.Guid.NewGuid().ToString(),
            timestamp = System.DateTime.UtcNow.ToString("o") // ISO 8601 UTC
        };

        sessionHistory.Add(record);

        Debug.Log($"[SCORE] Session terminée. Score : {score} / {maxScore}, Temps : {sessionTime:F2} s, Erreurs : {errors}");
        
        // --- Envoi vers l'API (utilise FirebaseAnonymousAuth pour userId + token) ---
        try
        {
            await ApiClient.SendScoreAsync(FirebaseAnonymousAuth.UserId, FirebaseAnonymousAuth.IdToken, record);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[SCORE] Erreur lors de l'envoi au backend : {ex.Message}");
        }

        OnExerciseFinished?.Invoke(record);
    }

    // Mettre à jour le temps de session (appelé depuis ExerciseManager)
    public void UpdateSessionTime(float elapsedTime)
    {
        if (!exerciseRunning) return;

        sessionTime = elapsedTime;
    }
}
