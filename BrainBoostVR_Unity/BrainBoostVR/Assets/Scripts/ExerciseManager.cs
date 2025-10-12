using System.Collections;
using System.Collections.Generic;
using System.Threading;
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

	public float maxDuration = 300f;
	
	[Header("UI")]
	public TMPro.TextMeshProUGUI timerText;

    void Awake()
	{
        if (exerciseObjects.Count == 0)
		{
			Debug.LogWarning("[EXERCISE] La liste exerciseObjects est vide ! Aucun objet ne sera activé.");
		}
		// Désactiver tous les objets avant même que la scène ne s'affiche
        foreach (var obj in exerciseObjects)
        {
            if (obj.objectRef != null)
            {
                obj.objectRef.SetActive(false);
                Debug.Log($"[EXERCISE] {obj.objectRef.name} désactivé au démarrage.");
            }
            else
            {
                Debug.LogWarning("[EXERCISE] Un objet de la liste n'est pas assigné !");
            }
        }
    }

    public void StartExercise()
	{
		Debug.Log("[EXERCISE] StartExercise() appelé");

		if (audioSource == null)
		{
			Debug.LogWarning("[EXERCISE] audioSource == null !");
		}
		if (instructionsAudio == null)
		{
			Debug.LogWarning("[EXERCISE] instructionsAudio == null !");
		}

		if (instructionsAudio != null && audioSource != null)
		{
			audioSource.clip = instructionsAudio;
			audioSource.Play();
			Debug.Log($"[EXERCISE] audioSource.Play() -> isPlaying = {audioSource.isPlaying}, clip length = {audioSource.clip.length}");
			StartCoroutine(PlayInstructionsAndStartTimer());
		}
		else
		{
			Debug.LogWarning("[EXERCISE] Pas d'audio, activation immédiate des objets");
			ActivateExerciseObjects();
			isExerciseRunning = true;
		}
	}

	private IEnumerator PlayInstructionsAndStartTimer()
	{
		Debug.Log("[EXERCISE] Coroutine démarrée, attente fin audio...");
		// Sécurité: si pour une raison audioSource.isPlaying ne devient jamais vrai, on utilise un fallback maxTime
		float fallbackMax = (audioSource.clip != null) ? audioSource.clip.length + 1f : 10f;
		float waited = 0f;

		// Attendre que l'audio commence à jouer (si possible)
		while (!audioSource.isPlaying && waited < 0.5f)
		{
			waited += Time.deltaTime;
			yield return null;
		}

		// Puis attendre la fin ou fallback
		waited = 0f;
		while (audioSource.isPlaying && waited < fallbackMax)
		{
			waited += Time.deltaTime;
			yield return null;
		}

		Debug.Log("[EXERCISE] Audio fini ou timeout, activation objets...");
		ActivateExerciseObjects();
		isExerciseRunning = true;
		elapsedTime = 0f;
		Debug.Log("[EXERCISE] Chrono démarré !");
	}


	private void ActivateExerciseObjects()
	{
    	foreach (var obj in exerciseObjects)
    	{
        	if (obj.objectRef == null)
        	{
            	Debug.LogWarning("[EXERCISE] objectRef null dans la liste !");
            	continue;
        	}

        	// Si c'est un prefab (non présent dans la scène)
        	if (obj.objectRef.scene.rootCount == 0)
        	{
            	Debug.Log($"[EXERCISE] Instantiation du prefab {obj.objectRef.name}");
            	GameObject inst = Instantiate(obj.objectRef); // on le laisse à sa position par défaut
            	obj.objectRef = inst;
        	}

        	// ⚠️ NE PAS le replacer sur targetPosition
        	// → Il reste où tu l’as positionné dans la scène
        	obj.objectRef.SetActive(true);

        	Debug.Log($"[EXERCISE] {obj.objectRef.name} activé à sa position d’origine.");
    	}
	}



	void Update()
	{
		if (isExerciseRunning)
		{
			elapsedTime += Time.deltaTime;

			if (timerText != null)
				timerText.text = $"Temps : {elapsedTime:F1}s";
			
			// Vérifier si tous les objets sont placés
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
				Debug.Log($"[EXERCISE] Exercice terminé en {elapsedTime:F2} secondes !");
			}

			if (elapsedTime >= maxDuration)
			{
				isExerciseRunning = false;
				Debug.Log($"[EXERCISE] Temsp écoulé ({maxDuration}s). Fin de l'exercice.");
			}
		}
	}
}
