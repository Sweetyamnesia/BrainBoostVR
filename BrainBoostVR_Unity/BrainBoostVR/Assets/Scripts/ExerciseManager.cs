using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // permet de voir chaque objet dans l’inspecteur Unity
public class ExerciseObject
{
    public GameObject objectRef;    // l’objet à manipuler
    public Transform targetPosition; // où il doit être placé
    public bool isPlacedCorrectly;   // suivi de l’état
}

public class ExerciseManager : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;               // assigné via l’inspecteur
    public AudioClip instructionsAudio;           // assigné via l’inspecteur
    
    [Header("Exercise Objects")]
    public List<ExerciseObject> exerciseObjects = new List<ExerciseObject>();  // liste des objets à placer

    private bool isExerciseRunning = false;
    private float elapsedTime = 0f;

    void Start()
    {
        // Cacher tous les objets au départ et vérifier qu'ils sont assignés
        foreach (var obj in exerciseObjects)
        {
            if (obj.objectRef != null)
            {
                Debug.Log($"[ExerciseManager] Objet prêt : {obj.objectRef.name}");
                obj.objectRef.SetActive(false);
            }
        }
    }

    public void StartExercise()
    {
        // Lancer la coroutine qui joue l'audio et active les objets
        StartCoroutine(PlayInstructionsAndActivateObjects());
    }

    private IEnumerator PlayInstructionsAndActivateObjects()
    {
        // Jouer l’audio si disponible
        if (audioSource != null && instructionsAudio != null)
        {
            audioSource.clip = instructionsAudio;
            audioSource.Play();

            while (audioSource.isPlaying)
            {
                yield return null;
            }
        }

        // Activer tous les objets à manipuler
        foreach (var obj in exerciseObjects)
        {
            if (obj.objectRef != null)
                obj.objectRef.SetActive(true);
        }

        // Démarrer le chrono
        isExerciseRunning = true;
        elapsedTime = 0f;

        Debug.Log("[ExerciseManager] Exercice lancé : objets activés et chrono démarré !");
    }

    void Update()
    {
        if (!isExerciseRunning)
            return;

        elapsedTime += Time.deltaTime;

        // Vérifier si tous les objets sont correctement placés
        bool allPlaced = true;
        foreach (var obj in exerciseObjects)
        {
            if (!obj.isPlacedCorrectly)
            {
                allPlaced = false;
                break;
            }
        }

        if (allPlaced)
        {
            isExerciseRunning = false;
            Debug.Log($"[ExerciseManager] Exercice terminé en {elapsedTime:F2} secondes !");
        }
    }
}
