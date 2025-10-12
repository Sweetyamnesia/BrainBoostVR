using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
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

	[Header("UI")]
	public TMPro.TextMeshProUGUI timerText;

	[Header("Hooks")]
	public List<Hook> hookList;
	
	private bool isExerciseRunning = false;
	private float elapsedTime = 0f;

	public float maxDuration = 300f;

    void Awake()
	{
        if (exerciseObjects.Count == 0)
		{
			Debug.LogWarning("[EXERCISE] Aucun objet défini dans la liste.");
		}
		// Désactiver tous les objets avant même que la scène ne s'affiche
        foreach (var obj in exerciseObjects)
        {
			if (obj.objectRef != null)
			{
				obj.objectRef.SetActive(false);
			}
			
			// Désactive tous les hooks visuellement
			foreach (var hook in hookList)
			{
				if (hook != null)
					hook.gameObject.SetActive(true); //visible dans la scène mais non visible visuellement
			}
        }
    }

    public void StartExercise()
	{
		Debug.Log("[EXERCISE] Démarrage de l'exercice (lecture audio).");
		if (audioSource != null && instructionsAudio != null)
		{
			audioSource.clip = instructionsAudio;
			StartCoroutine(PlayInstructionsAndStartTimer());
		}
		else
		{
			Debug.LogWarning("[EXERCISE] Pas d'audio - activation immédiate des objets.");
			ActivateExerciseObjects();
			StartTimer();
		}
	}

	private IEnumerator PlayInstructionsAndStartTimer()
	{
		audioSource.Play();
		Debug.Log("[EXERCISE] Lecture de l'audio...");

		//attendre la fin de l'audio
		yield return new WaitForSeconds(audioSource.clip.length);

		Debug.Log("[EXERCISE] Audio terminé. Activation des objets et du chrono");
		ActivateExerciseObjects();
		StartTimer();
	}
	
	private void StartTimer()
	{
		elapsedTime = maxDuration;
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

		// décompte du chrono
		elapsedTime -= Time.deltaTime;
		if (elapsedTime <= 0f)
		{
			elapsedTime = 0f;
			isExerciseRunning = false;
			Debug.Log("Temps écoulé !");
		}

		UpdateTimerUI();
		CheckExerciseCompletion();
	}

	private void UpdateTimerUI()
	{
		if (timerText == null) return;
		int minutes = Mathf.FloorToInt(elapsedTime / 60f);
		int seconds = Mathf.FloorToInt(elapsedTime % 60f);
		timerText.text = $"{minutes:00}:{seconds:00}";
	}

	private void CheckExerciseCompletion()
	{
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
			Debug.Log($"[EXERCISE] Exercice terminé en {maxDuration - elapsedTime:F2} secondes !");
		}
	}
			
}

