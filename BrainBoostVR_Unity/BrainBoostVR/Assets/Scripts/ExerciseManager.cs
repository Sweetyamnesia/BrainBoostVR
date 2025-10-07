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
	public AudioSource audioSource;               // assigné via l’inspecteur
	public AudioClip instructionsAudio;           // assigné via l’inspecteur
	public List<ExerciseObject> exerciseObjects;  // liste des objets à placer

	private bool isExerciseRunning = false;
	private float elapsedTime = 0f;

	void Start()
	{
		// Ici, on peut accéder aux objets Unity en toute sécurité
		foreach (var obj in exerciseObjects)
		{
			Debug.Log("Objet prêt : " + obj.objectRef.name);
			obj.objectRef.SetActive(false); // cacher les objets au départ
		}
	}

	public void StartExercise()
	{
		// Jouer l’audio des instructions
		audioSource.clip = instructionsAudio;
		audioSource.Play();

		// Lancer coroutine qui attend la fin de l’audio
		StartCoroutine(PlayInstructionsAndStartTimer());
	}

	private IEnumerator PlayInstructionsAndStartTimer()
	{
		while (audioSource.isPlaying)
		{
    		yield return null;
		}
		isExerciseRunning = true;
		elapsedTime = 0f;
		Debug.Log("Chrono démarré !");


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
		}

		isExerciseRunning = false;
		Debug.Log("Exercice terminé !");
	}
}
