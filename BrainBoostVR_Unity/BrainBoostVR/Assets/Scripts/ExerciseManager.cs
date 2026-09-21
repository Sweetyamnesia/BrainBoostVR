using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExerciseObject
{
    public GameObject objectRef;
    public Transform targetPosition;
    public bool isPlacedCorrectly;
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

    [Header("Timer")]
    public float maxDuration = 300f;

    private bool isExerciseRunning = false;
    private float timeRemaining = 0f;

    void Awake()
    {
        if (exerciseObjects.Count == 0)
        {
            Debug.LogWarning(
                "[EXERCISE] Aucun objet défini dans la liste."
            );
        }

        foreach (var obj in exerciseObjects)
        {
            if (obj.objectRef != null)
            {
                obj.objectRef.SetActive(false);
            }
        }
    }

    // ---------------- START EXERCISE ----------------

    public void StartExercise()
    {
        if (isExerciseRunning)
        {
            Debug.LogWarning(
                "[EXERCISE] L'exercice est déjà en cours."
            );

            return;
        }

        if (scoreManager == null)
        {
            Debug.LogError(
                "[EXERCISE] Aucun ScoreManager assigné."
            );

            return;
        }

        // Si aucune session n'existe encore,
        // on en démarre une.
        if (!scoreManager.sessionRunning)
        {
            scoreManager.StartSession();
        }

        // Le ScoreManager considère la session comme active
        // dès le lancement de StartSession().
        scoreManager.StartExercise();

        if (audioSource != null && instructionsAudio != null)
        {
            StartCoroutine(
                PlayInstructionsAndStartTimer()
            );
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
        {
            subtitleManager.PlaySubtitles(audioSource);
        }

        audioSource.Play();

        yield return new WaitForSeconds(
            audioSource.clip.length
        );

        // L'exercice commence réellement après les instructions.
        ActivateExerciseObjects();
        StartTimer();
    }

    private void StartTimer()
    {
        timeRemaining = maxDuration;
        isExerciseRunning = true;

        if (scoreManager != null)
        {
            scoreManager.UpdateSessionTime(0f);
        }

        Debug.Log(
            "[EXERCISE] Timer démarré."
        );
    }

    private void ActivateExerciseObjects()
    {
        foreach (var obj in exerciseObjects)
        {
            if (obj.objectRef != null)
            {
                obj.objectRef.SetActive(true);
            }
        }
    }

    // ---------------- UPDATE ----------------

    void Update()
    {
        if (!isExerciseRunning)
        {
            return;
        }

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;

            EndExercise();
        }
        else
        {
            if (scoreManager != null)
            {
                float elapsedTime =
                    maxDuration - timeRemaining;

                scoreManager.UpdateSessionTime(
                    elapsedTime
                );
            }
        }

        UpdateTimerUI();

        if (isExerciseRunning)
        {
            CheckExerciseCompletion();
        }
    }

    // ---------------- TIMER UI ----------------

    private void UpdateTimerUI()
    {
        if (timerText == null)
        {
            return;
        }

        int minutes =
            Mathf.FloorToInt(timeRemaining / 60f);

        int seconds =
            Mathf.FloorToInt(timeRemaining % 60f);

        timerText.text =
            $"{minutes:00}:{seconds:00}";
    }

    // ---------------- COMPLETION ----------------

    private void CheckExerciseCompletion()
    {
        foreach (var obj in exerciseObjects)
        {
            if (!obj.isPlacedCorrectly)
            {
                return;
            }
        }

        EndExercise();
    }

    // ---------------- END EXERCISE ----------------

    private async void EndExercise()
    {
        // Protection contre une double exécution.
        if (!isExerciseRunning)
        {
            return;
        }

        isExerciseRunning = false;

        Debug.Log(
            "[EXERCISE] Fin de l'exercice."
        );

        if (scoreManager != null)
        {
			await scoreManager.EndExerciseAsync();
			await scoreManager.EndSessionAsync();
        }

        if (finalGamePanel != null && scoreManager != null)
        {
            int score = scoreManager.score;
            int errors = scoreManager.errors;
            float temps = scoreManager.sessionTime;

            finalGamePanel.DisplayEnd(
                score,
                errors,
                temps
            );
        }
    }

    // ---------------- RESET EXERCISE ----------------

    public void ResetExercise()
    {
        foreach (var obj in exerciseObjects)
        {
            if (obj.objectRef != null)
            {
                if (obj.targetPosition != null)
                {
                    obj.objectRef.transform.position =
                        obj.targetPosition.position;
                }

                obj.objectRef.SetActive(false);
                obj.isPlacedCorrectly = false;
            }
        }

        if (scoreManager != null)
        {
            scoreManager.ResetScore();
        }

        if (finalGamePanel != null)
        {
            finalGamePanel.gameObject.SetActive(false);
        }

        isExerciseRunning = false;
        timeRemaining = 0f;

        UpdateTimerUI();

        Debug.Log(
            "[EXERCISE] Exercice réinitialisé."
        );
    }
}