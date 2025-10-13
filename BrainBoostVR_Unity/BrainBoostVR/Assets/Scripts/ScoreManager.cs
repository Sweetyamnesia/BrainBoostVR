using System;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // Variables principales
    public int score = 0;
    public int maxScore = 5;
    public float sessionTime = 0f;
    public bool exerciseRunning = false;

    public List<SessionRecord> sessionHistory = new List<SessionRecord>();

    // Events
    public event Action<int> OnScoreChanged;                 // déclenché à chaque changement de score
    public event Action<SessionRecord> OnExerciseFinished;   // déclenché à la fin d'une session

    // Classe pour stocker les sessions
    [Serializable]
    public class SessionRecord
    {
        public int score;
        public float timeSpent;
        public int objectsPlaced;
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

    // Retirer des points (optionnel)
    public void SubtractPoints(int points = 1)
    {
        if (!exerciseRunning) return;

        score -= points;

        if (score < 0)
            score = 0;

        Debug.Log($"[SCORE] Points retirés. Score actuel : {score} / {maxScore}");
        OnScoreChanged?.Invoke(score);
    }

    // Réinitialiser le score pour une nouvelle session
    public void ResetScore()
    {
        score = 0;
        sessionTime = 0f;
        exerciseRunning = false;

        Debug.Log("[SCORE] Score réinitialisé pour nouvelle session.");
        OnScoreChanged?.Invoke(score);
    }

    // Démarrer une session
    public void StartSession()
    {
        exerciseRunning = true;
        score = 0;
        sessionTime = 0f;

        Debug.Log("[SCORE] Session commencée.");
        OnScoreChanged?.Invoke(score);
    }

    // Terminer une session
    public void EndSession()
    {
        if (!exerciseRunning) return;

        exerciseRunning = false;

        SessionRecord record = new SessionRecord()
        {
            score = score,
            timeSpent = sessionTime,
            objectsPlaced = score
        };

        sessionHistory.Add(record);

        Debug.Log($"[SCORE] Session terminée. Score : {score} / {maxScore}, Temps : {sessionTime:F2} s, Objets placés : {score}");
        OnExerciseFinished?.Invoke(record);
    }

    // Mettre à jour le temps de session (appelé depuis ExerciseManager)
    public void UpdateSessionTime(float deltaTime)
    {
        if (!exerciseRunning) return;

        sessionTime += deltaTime;

        // Vérification si le score a atteint le maximum
        if (score >= maxScore)
        {
            EndSession();
        }
    }

    // Mettre à jour l'UI ou panneau récapitulatif
    public void UpdateUI()
    {
        // À implémenter : mettre à jour score, temps, objets placés dans l'UI
    }

    // Préparer envoi vers API / Firebase (à implémenter plus tard)
    public void SyncWithAPI()
    {
        // À implémenter : préparer JSON et envoyer via POST
    }
}
