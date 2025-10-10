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
		// Ici, on peut accéder aux objets Unity en toute sécurité
		foreach (var obj in exerciseObjects)
		{
			if (obj.objectRef != null)
			{
				Debug.Log("Objet prêt : " + obj.objectRef.name);
				obj.objectRef.SetActive(false); // cacher les objets au départ
			}
		}
	}

	public void StartExercise()
	{
		if (instructionsAudio != null && audioSource != null)
		{
			// Jouer l’audio des instructions
			audioSource.clip = instructionsAudio;
			audioSource.Play();

			// Lancer coroutine qui attend la fin de l’audio
			StartCoroutine(PlayInstructionsAndStartTimer());
		}
		else
		{
			Debug.LogWarning("AudioSource ou instructionsAudio manquant !");
			ActivateExerciseObjects();
			isExerciseRunning = true;
		}
	}

	private IEnumerator PlayInstructionsAndStartTimer()
	{
		while (audioSource.isPlaying)
		{
			yield return null;
		}
		ActivateExerciseObjects();
		isExerciseRunning = true;
		elapsedTime = 0f;
		Debug.Log("Chrono démarré !");
	}

	private void ActivateExerciseObjects()
	{
		// Afficher les objets à placer
		foreach (var obj in exerciseObjects)
		{
			obj.objectRef.SetActive(true);
		}
	}

	void Update()
	{
		if (isExerciseRunning)
		{
			elapsedTime += Time.deltaTime;

			//Si tous les objet sont bien placés, on termine l'exercice
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
				Debug.Log($"Exercice terminé en {elapsedTime:F2} secondes !");	
			}
		}

	}
}
