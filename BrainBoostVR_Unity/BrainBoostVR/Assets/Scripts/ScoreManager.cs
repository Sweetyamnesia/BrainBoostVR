using System.Collections.Generic;
using UnityEngine;
using System;

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
        // Logique à compléter : ajouter points, limiter à maxScore, déclencher OnScoreChanged
    }

    // Retirer des points (optionnel)
    public void SubtractPoints(int points = 1)
    {
        // Logique à compléter : retirer points, ne pas descendre en dessous de 0, déclencher OnScoreChanged
    }

    // Remettre le score à zéro pour une nouvelle session
    public void ResetScore()
    {
        // Logique à compléter : score = 0, sessionTime = 0, exerciseRunning = false, déclencher OnScoreChanged
    }

    // Démarrer une session
    public void StartSession()
    {
        // Logique à compléter : exerciseRunning = true, score = 0, sessionTime = 0, déclencher OnScoreChanged
    }

    // Terminer une session
    public void EndSession()
    {
        // Logique à compléter : exerciseRunning = false, créer SessionRecord, l'ajouter à l'historique, déclencher OnExerciseFinished
    }

    // Mettre à jour le temps de session (appelé depuis ExerciseManager)
    public void UpdateSessionTime(float deltaTime)
    {
        // Logique à compléter : incrémenter sessionTime si exerciseRunning == true, vérifier conditions d'arrêt
    }

    // Mettre à jour l'UI ou panneau récapitulatif
    public void UpdateUI()
    {
        // Logique à compléter : score, temps, objets placés
    }

    // Préparer envoi vers API / Firebase (à implémenter plus tard)
    public void SyncWithAPI()
    {
        // Logique à compléter : préparer JSON et POST
    }
}
