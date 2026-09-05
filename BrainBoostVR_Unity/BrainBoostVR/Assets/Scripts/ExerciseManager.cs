using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExerciseObject
{
    public GameObject objectRef;
    public Transform targetPosition;
    public bool isPlacedCorrectly; // mis à jour automatiquement par PlaceableObject
}

public class ExerciseManager : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip instructionsAudio;

    [Header("Exercise Objects")]
    public List<ExerciseObject> exerciseObjects = new List<ExerciseObject>();

    [Header("UI")]
    public TMPro.TextMeshProUGUI timerText;

    [Header("UI Panels")]
    public FinalGamePanel finalGamePanel;

    [Header("Score")]
    public ScoreManager scoreManager;

    [Header("Subtitles")]
    public SubtitleManager subtitleManager;

    private bool isExerciseRunning = false;
    private float timeRemaining = 0f;

    public float maxDuration = 300f;

    void Awake()
    {
        if (exerciseObjects.Count == 0)
            Debug.LogWarning("[EXERCISE] Aucun objet défini dans la liste.");

        foreach (var obj in exerciseObjects)
        {
            if (obj.objectRef != null)
                obj.objectRef.SetActive(false);
        }
    }

    public void StartExercise()
    {
        if (isExerciseRunning)
        {
            Debug.LogWarning("[EXERCISE] L'exercice est déjà en cours.");
            return;
        }

        // La session doit être active avant de commencer un exercice.
        if (scoreManager != null)
        {
            if (!scoreManager.sessionRunning)
            {
                scoreManager.StartSession();
            }

            scoreManager.StartExercise();
        }

        if (audioSource != null && instructionsAudio != null)
        {
            StartCoroutine(PlayInstructionsAndStartTimer());
        }
        else
        {
            ActivateExerciseObjects();
            StartTimer();
        }
    }

    private IEnumerator PlayInstructionsAndStartTimer()
    {
        audioSource.clip = instructionsAudio;

        if (subtitleManager != null)
            subtitleManager.PlaySubtitles(audioSource);

        audioSource.Play();

        yield return new WaitForSeconds(audioSource.clip.length);

        ActivateExerciseObjects();
        StartTimer();
    }

    private void StartTimer()
    {
        timeRemaining = maxDuration;
        isExerciseRunning = true;

        Debug.Log("[EXERCISE] Timer démarré.");
    }

    private void ActivateExerciseObjects()
    {
        foreach (var obj in exerciseObjects)
        {
            if (obj.objectRef != null)
                obj.objectRef.SetActive(true);
        }
    }

    void Update()
    {
        if (!isExerciseRunning)
            return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            isExerciseRunning = false;

            EndExercise();
        }
        else if (scoreManager != null)
        {
            scoreManager.UpdateSessionTime(maxDuration - timeRemaining);
        }

        UpdateTimerUI();
        CheckExerciseCompletion();
    }

    private void UpdateTimerUI()
    {
        if (timerText == null)
            return;

        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void CheckExerciseCompletion()
    {
        foreach (var obj in exerciseObjects)
        {
            if (!obj.isPlacedCorrectly)
                return;
        }

        isExerciseRunning = false;

        EndExercise();
    }

    private void EndExercise()
    {
        Debug.Log("[EXERCISE] Fin de l'exercice.");

        if (scoreManager != null)
            scoreManager.EndExercise();

        if (finalGamePanel != null && scoreManager != null)
        {
            int score = scoreManager.score;
            int errors = scoreManager.errors;
            float temps = scoreManager.sessionTime;

            finalGamePanel.DisplayEnd(score, errors, temps);
        }
    }

    public void ResetExercise()
    {
        foreach (var obj in exerciseObjects)
        {
            if (obj.objectRef != null)
            {
                obj.objectRef.transform.position = obj.targetPosition.position;
                obj.objectRef.SetActive(false);
                obj.isPlacedCorrectly = false;
            }
        }

        if (scoreManager != null)
            scoreManager.ResetScore();

        if (finalGamePanel != null)
            finalGamePanel.gameObject.SetActive(false);

        isExerciseRunning = false;
        timeRemaining = 0f;

        Debug.Log("[EXERCISE] Exercice réinitialisé.");
    }
}