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

    [Header("Hooks")]
    public List<Hook> hookList;

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

        foreach (var hook in hookList)
        {
            if (hook != null && hook.hookRenderer != null)
                hook.hookRenderer.enabled = false; // hooks désactivés visuellement
        }
    }

    public void StartExercise()
    {
        Debug.Log("[EXERCISE] Démarrage de l'exercice (lecture audio).");

        if (audioSource != null && instructionsAudio != null)
            StartCoroutine(PlayInstructionsAndStartTimer());
        else
        {
            Debug.LogWarning("[EXERCISE] Pas d'audio - activation immédiate des objets.");
            ActivateExerciseObjects();
            StartTimer();
        }
    }

    private IEnumerator PlayInstructionsAndStartTimer()
    {
        audioSource.clip = instructionsAudio;
        audioSource.Play();
        Debug.Log("[EXERCISE] Lecture de l'audio...");

        yield return new WaitForSeconds(audioSource.clip.length);

        Debug.Log("[EXERCISE] Audio terminé. Activation des objets et du chrono");
        ActivateExerciseObjects();
        StartTimer();
    }

    private void StartTimer()
    {
        timeRemaining = maxDuration;
        isExerciseRunning = true;
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
        if (!isExerciseRunning) return;

        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            isExerciseRunning = false;
            Debug.Log("[EXERCISE] Temps écoulé !");
        }

        UpdateTimerUI();
        CheckExerciseCompletion();
    }

    private void UpdateTimerUI()
    {
        if (timerText == null) return;
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void CheckExerciseCompletion()
    {
        bool allPlaced = true;
        foreach (var obj in exerciseObjects)
        {
            if (!obj.isPlacedCorrectly)
            {
                allPlaced = false;
                break;
            }
        }

        if (allPlaced && isExerciseRunning)
        {
            isExerciseRunning = false;
            Debug.Log($"[EXERCISE] Exercice terminé en {maxDuration - timeRemaining:F2} secondes !");
        }
    }
}
