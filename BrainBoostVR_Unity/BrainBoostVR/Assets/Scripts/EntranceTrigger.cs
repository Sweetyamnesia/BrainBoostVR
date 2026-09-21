using UnityEngine;
using Unity.XR.CoreUtils;
using NUnit.Framework;

public class EntranceTrigger : MonoBehaviour
{
	[Header("Exercise Manager")]
	public ExerciseManager exerciseManager; //Assigner via l'inspecteur
	private bool exerciseStarted = false; //Pour éviter de lancer plusieurs fois

	private void OnTriggerEnter(Collider other)
	{
		//Vérifie que c'est bien le XR Rig qui entre
		if (exerciseStarted) return;

		XROrigin xROrigin = other.GetComponent<XROrigin>();
		if (xROrigin != null)
		{
			if (exerciseManager != null)
			{
				exerciseManager.StartExercise();
				exerciseStarted = true;
				Debug.Log("[TRIGGER] Exercise démarré !");
			}
			else
			{
				Debug.LogWarning("[TRIGGER] Aucun ExerciseManager assigné !");
			}
		}	
	}

}
